using System.Text;
using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        private List<string> GerarLinhaEmParalelo(GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop, int numThreads, int tamanhoParte)
        {
            JsonSerializerOptions identacao = new() { WriteIndented = true };
            var parametros = new ParametrosJSON
            {
                A = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                B = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                C = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                D = geradorStringAlfanumerico.GetAlfanumericoAleatoria()
            };
            var linha = JsonSerializer.Serialize(parametros, identacao);

            List<List<string>> blocos = [];

            Parallel.For(0, numThreads, i =>
            {
                List<string> bloco = new();
                for (int j = 0; j < tamanhoParte; j++)
                {
                    int indexGlobal = i * tamanhoParte + j;
                    bloco.Add(linha);
                }
                lock (blocos)
                {
                    blocos.Add(bloco);
                }
            });

            List<string> todasLinhas = [.. blocos.SelectMany(b => b)];
            if (todasLinhas.Count < quantidadeLoop)
            {
                for (int i = todasLinhas.Count; i < quantidadeLoop; i++)
                    todasLinhas.Add(linha);
            }
            return todasLinhas;
        }

        private void BarraDeProgresso(int quantidadeLoop, int i, out int porcentagem, out string barraDeProgresso)
        {
            porcentagem = (int)(((double)i / quantidadeLoop) * 100);
            barraDeProgresso = "[" + new string('#', porcentagem / 2) + new string('-', 50 - porcentagem / 2) + "]";
        }

        private double TamanhoArquivoDuranteLoop(string linha, int quantidadeLoop, ref long tamanhoBytes, int i)
        {
            tamanhoBytes += Encoding.UTF8.GetByteCount(linha + (i < quantidadeLoop - 1 ? "," : ""));
            return Math.Round((double)tamanhoBytes / (1024 * 1024), 2);
        }

        private long VerTamanhoArquivoAposFechar(string pastaDestino)
        {
            long tamanhoBytes = new FileInfo(pastaDestino).Length;
            double jsonEmMb = Math.Round((double)tamanhoBytes / (1024 * 1024), 2);

            if (jsonEmMb < 1000.00)
            {
                Console.WriteLine($"\nTamanho do arquivo (Após fechar): {jsonEmMb}MB");
            }
            else
            {
                Console.WriteLine($"\nTamanho do arquivo (Após fechar): {Math.Round(jsonEmMb / 1000, 2)}GB");
            }
            return tamanhoBytes;
        }

        private void EscreverArquivoEmParalelo(string pastaDestino, GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop, double limiteComTolerancia)
        {
            try
            {
                int numThreads = Environment.ProcessorCount;
                int tamanhoParte = quantidadeLoop / numThreads;
                Thread[] threads = new Thread[numThreads];
                List<string> todasLinhas = GerarLinhaEmParalelo(geradorStringAlfanumerico, quantidadeLoop, numThreads, tamanhoParte);

                using StreamWriter writer = new(pastaDestino);
                long tamanhoBytes = 0;
                int quantidadeObjetos = 0;
                string separador = "\n------------------------------------------------------------------------------------------------------------------------";

                Console.WriteLine(separador);
                Console.WriteLine("\nIniciando operação...\n");

                writer.WriteLine("[");
                for (int i = 0; i < numThreads; i++)
                {
                    int startIndex = i * (tamanhoParte / numThreads);
                    int endIndex = (i + 1) * (tamanhoParte / numThreads);
                    if (i == numThreads - 1)
                    {
                        endIndex = tamanhoParte;
                    }

                    int threadIndex = i;
                    threads[i] = new Thread(() =>
                    {
                        for (int j = startIndex; j < endIndex; j++)
                        {
                            EscreverEmJSON(limiteComTolerancia, todasLinhas, tamanhoParte, writer, ref tamanhoBytes, ref quantidadeObjetos);
                        }
                    });
                    threads[i].Start();
                }
                foreach (Thread thread in threads)
                {
                    thread.Join();
                }
                writer.WriteLine("]");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\nOperação concluída!");
                Console.ResetColor();
                Console.WriteLine(separador);

                VerTamanhoArquivoAposFechar(pastaDestino);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nException: {ex.Message}");
            }
        }

        private void EscreverEmJSON(double limiteComTolerancia, List<string> todasLinhas, int tamanhoParte, StreamWriter writer, ref long tamanhoBytes, ref int quantidadeObjetos)
        {
            for (int i = 0; i < todasLinhas.Count; i++)
            {
                string linha = todasLinhas[i];
                double tamanhoAtual = TamanhoArquivoDuranteLoop(linha, tamanhoParte, ref tamanhoBytes, i);

                if (tamanhoAtual <= limiteComTolerancia)
                {
                    writer.WriteLine(linha + (i < todasLinhas.Count - 1 ? "," : ""));
                    BarraDeProgresso(tamanhoParte, i, out int porcentagem, out string barraDeProgresso);

                    quantidadeObjetos += 4;

                    Console.Write($"\r{barraDeProgresso} {porcentagem}% | Objetos criados: {quantidadeObjetos} | Tamanho arquivo JSON: {tamanhoAtual}MB");
                }
                else
                {
                    Console.WriteLine($"\n\nArquivo passou do limite de {limiteComTolerancia}MB, fechando arquivo.");
                    break;
                }
            }
        }

        public void GerarArquivoJson(string pastaDestino, int tamanhoArquivoDesejado)
        {
            GeradorStringAlfanumerico geradorStringAlfanumerico = new();
            double limiteMaximoEmMB = 400.00;
            double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);
            int loopEquivalente1MB = 1350;
            int quantidadeLoop = tamanhoArquivoDesejado * loopEquivalente1MB;

            if (tamanhoArquivoDesejado < limiteComTolerancia)
            {
                EscreverArquivoEmParalelo(pastaDestino, geradorStringAlfanumerico, quantidadeLoop, limiteComTolerancia);
                Console.WriteLine(File.Exists(pastaDestino) ? "\nArquivo JSON atualizado." : "\nArquivo JSON criado.");
            }
            else
            {
                Console.WriteLine($"\nArquivo não gerado, passou do limite de {limiteComTolerancia}MB.");
            }
        }
    }
}