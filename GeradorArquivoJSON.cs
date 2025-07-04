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
            var linha = JsonSerializer.Serialize(parametros, identacao);
            return linha;
        }

        private void BarraDeProgresso(int quantidadeLoop, int i, out int porcentagem, out string barraDeProgresso)
        {
            porcentagem = (int)(((double)i / quantidadeLoop) * 100);
            barraDeProgresso = "[" + new string('#', porcentagem / 2) + new string('-', 50 - porcentagem / 2) + "]";
        }

        private double TamanhoArquivoDuranteLoop(GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop, ref long tamanhoBytes, int i)
        {
            tamanhoBytes += Encoding.UTF8.GetByteCount(GerarLinha(geradorStringAlfanumerico) + (i < quantidadeLoop - 1 ? "," : ""));
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

                using StreamWriter writer = new(pastaDestino);
                long tamanhoBytes = 0;
                int quantidadeObjetos = 0;
                double tamanhoArquivoDuranteLoop = 0;
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
                            EscreverArquivo(geradorStringAlfanumerico, tamanhoParte, limiteComTolerancia, tamanhoArquivoDuranteLoop, writer);
                            BarraDeProgresso(tamanhoParte, i, out int porcentagem, out string barraDeProgresso);
                            tamanhoArquivoDuranteLoop = TamanhoArquivoDuranteLoop(geradorStringAlfanumerico, tamanhoParte, ref tamanhoBytes, i);

                            quantidadeObjetos += 4;

                            Console.Write($"\r{barraDeProgresso} {porcentagem}% | Objetos criados: {quantidadeObjetos} | Tamanho arquivo JSON: {tamanhoArquivoDuranteLoop}MB");
                        }
                    });
                    threads[i].Start();
                }

                foreach (Thread thread in threads)
                {
                    thread.Join();
                }

                Console.WriteLine("Loop completo.");

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

        private void EscreverArquivo(GeradorStringAlfanumerico geradorStringAlfanumerico, int tamanhoParte, double limiteComTolerancia, double tamanhoArquivoDuranteLoop, StreamWriter writer)
        {
            for (int i = 0; i < tamanhoParte; i++)
            {
                if (tamanhoArquivoDuranteLoop <= limiteComTolerancia)
                {
                    writer.WriteLine(GerarLinha(geradorStringAlfanumerico) + (i < tamanhoParte - 1 ? "," : ""));
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
            int loopEquivalente1MB = 13500;
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
