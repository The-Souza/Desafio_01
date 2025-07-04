using System.Text;
using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        private string GerarLinha(GeradorStringAlfanumerico geradorStringAlfanumerico)
        {
            JsonSerializerOptions identacao = new() { WriteIndented = true };
            var parametros = new ParametrosJSON
            {
                A = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                B = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                C = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                D = geradorStringAlfanumerico.GetAlfanumericoAleatoria()
            };
            return JsonSerializer.Serialize(parametros, identacao);
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

        private void EscreverArquivo(string pastaDestino, GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop, double limiteComTolerancia)
        {
            try
            {
                int numThreads = Environment.ProcessorCount;
                int tamanhoParte = quantidadeLoop / numThreads;
                List<List<string>> blocos = new();

                Parallel.For(0, numThreads, i =>
                {
                    List<string> bloco = new();
                    for (int j = 0; j < tamanhoParte; j++)
                    {
                        int indexGlobal = i * tamanhoParte + j;
                        bloco.Add(GerarLinha(geradorStringAlfanumerico));
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
                        todasLinhas.Add(GerarLinha(geradorStringAlfanumerico));
                }

                using StreamWriter writer = new(pastaDestino);
                long tamanhoBytes = 0;
                int quantidadeObjetos = 0;
                string separador = "\n------------------------------------------------------------------------------------------------------------------------";

                Console.WriteLine(separador);
                Console.WriteLine("\nIniciando operação...\n");

                writer.WriteLine("[");
                for (int i = 0; i < todasLinhas.Count; i++)
                {
                    string linha = todasLinhas[i];
                    double tamanhoAtual = TamanhoArquivoDuranteLoop(linha, quantidadeLoop, ref tamanhoBytes, i);

                    if (tamanhoAtual <= limiteComTolerancia)
                    {
                        writer.WriteLine(linha + (i < todasLinhas.Count - 1 ? "," : ""));
                        BarraDeProgresso(quantidadeLoop, i, out int porcentagem, out string barraDeProgresso);
                        quantidadeObjetos += 4;
                        Console.Write($"\r{barraDeProgresso} {porcentagem}% | Objetos criados: {quantidadeObjetos} | Tamanho arquivo JSON: {tamanhoAtual}MB");
                    }
                    else
                    {
                        Console.WriteLine($"\n\nArquivo passou do limite de {limiteComTolerancia}MB, fechando arquivo.");
                        break;
                    }
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

        public void GerarArquivoJson(string pastaDestino, int tamanhoArquivoDesejado)
        {
            GeradorStringAlfanumerico geradorStringAlfanumerico = new();
            double limiteMaximoEmMB = 400.00;
            double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);
            int loopEquivalente1MB = 13500;
            int quantidadeLoop = tamanhoArquivoDesejado * loopEquivalente1MB;

            if (tamanhoArquivoDesejado < limiteComTolerancia)
            {
                EscreverArquivo(pastaDestino, geradorStringAlfanumerico, quantidadeLoop, limiteComTolerancia);
                Console.WriteLine(File.Exists(pastaDestino) ? "\nArquivo JSON atualizado." : "\nArquivo JSON criado.");
            }
            else
            {
                Console.WriteLine($"\nArquivo não gerado, passou do limite de {limiteComTolerancia}MB.");
            }
        }
    }
}
