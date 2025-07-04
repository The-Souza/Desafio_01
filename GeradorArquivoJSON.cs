using System.Text;
using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        private static List<ParametrosJSON> GerarListaAlfanumerica(GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop)
        {
            List<ParametrosJSON> parametrosJSONs = [];
            for (int i = 0; i < quantidadeLoop; i++)
            {
                parametrosJSONs.Add(new ParametrosJSON
                {
                    A = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                    B = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                    C = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                    D = geradorStringAlfanumerico.GetAlfanumericoAleatoria()
                });
            }
            return parametrosJSONs;
        }

        private void BarraDeProgresso(int itensIterados, out int porcentagem, out string barraDeProgresso)
        {
            porcentagem = (int)(((double)itensIterados) * 100);
            barraDeProgresso = "[" + new string('#', porcentagem / 2) + new string('-', 50 - porcentagem / 2) + "]";
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

        private Action<int> GerarArquivosTemporarios(List<ParametrosJSON> parametrosJSONs, int numThreads, int tamanhoParte, List<string> arquivosTemporarios, JsonSerializerOptions options)
        {
            return (indiceParte) =>
            {
                int inicio = indiceParte * tamanhoParte;
                int fim = (indiceParte == numThreads - 1) ? parametrosJSONs.Count : (indiceParte + 1) * tamanhoParte;
                List<ParametrosJSON> dadosTemporarios = parametrosJSONs.GetRange(inicio, fim - inicio);

                string pastaDestinoTemporario = $"dadosTemporários_{indiceParte}.json";
                string jsonString = JsonSerializer.Serialize(dadosTemporarios, options);

                arquivosTemporarios.Add(pastaDestinoTemporario);

                File.WriteAllText(pastaDestinoTemporario, jsonString);
                //Console.WriteLine($"\nThread {indiceParte}: Arquivo {pastaDestinoTemporario} escrito.");
            };
        }

        private List<ParametrosJSON> CombinarArquivosTemporarios(List<string> arquivosTemporarios)
        {
            List<ParametrosJSON> dadosCombinados = [];
            //int totalItems = arquivosTemporarios.Count;
            //int itensIterados = 0;
            foreach (string arquivo in arquivosTemporarios)
            {
                string jsonString = File.ReadAllText(arquivo);
                List<ParametrosJSON>? parteDadosCombinados = JsonSerializer.Deserialize<List<ParametrosJSON>>(jsonString);
                dadosCombinados.AddRange(parteDadosCombinados);
                //itensIterados++;
                
                //BarraDeProgresso(itensIterados, out int porcentagem, out string barraDeProgresso);
                //Console.Write($"\r{barraDeProgresso} {porcentagem}%");

                File.Delete(arquivo);
            }
            return dadosCombinados;
        }

        private void EscreverArquivoComThreads(string pastaDestino, GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop, double limiteComTolerancia)
        {
            try
            {
                string separador = "\n------------------------------------------------------------------------------------------------------------------------";
                Console.WriteLine(separador);
                Console.WriteLine("\nIniciando operação...\n");

                List<ParametrosJSON> parametrosJSONs = GerarListaAlfanumerica(geradorStringAlfanumerico, quantidadeLoop);

                int numThreads = Environment.ProcessorCount;
                int tamanhoParte = parametrosJSONs.Count / numThreads;

                List<string> arquivosTemporarios = [];
                JsonSerializerOptions options = new() { WriteIndented = true };

                Action<int> escreverParte = GerarArquivosTemporarios(parametrosJSONs, numThreads, tamanhoParte, arquivosTemporarios, options);
                Parallel.For(0, numThreads, escreverParte);

                List<ParametrosJSON> dadosCombinados = CombinarArquivosTemporarios(arquivosTemporarios);

                string jsonStringFinal = JsonSerializer.Serialize(dadosCombinados, options);
                File.WriteAllText(pastaDestino, jsonStringFinal);

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
            int loopEquivalente1MB = 11655;
            int quantidadeLoop = tamanhoArquivoDesejado * loopEquivalente1MB;

            if (tamanhoArquivoDesejado < limiteComTolerancia)
            {
                EscreverArquivoComThreads(pastaDestino, geradorStringAlfanumerico, quantidadeLoop, limiteComTolerancia);
                Console.WriteLine(File.Exists(pastaDestino) ? "\nArquivo JSON atualizado." : "\nArquivo JSON criado.");
            }
            else
            {
                Console.WriteLine($"\nArquivo não gerado, passou do limite de {limiteComTolerancia}MB.");
            }
        }
    }
}