using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        private List<ParametrosJSON> GerarListaAlfanumerica(GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop)
        {
            List<ParametrosJSON> parametrosJSONs = [];

            Console.Write($"Gerando lista...\n");

            int quantidadeObjetos = quantidadeLoop * 4;
            for (int i = 0; i < quantidadeLoop; i++)
            {
                parametrosJSONs.Add(new ParametrosJSON
                {
                    A = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                    B = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                    C = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                    D = geradorStringAlfanumerico.GetAlfanumericoAleatoria()
                });
                BarraDeProgresso(i, quantidadeLoop, out int porcentagem, out string barraDeProgresso);
            }
            Console.Write($"Objetos criados: {quantidadeObjetos}\n");
            return parametrosJSONs;
        }

        private void BarraDeProgresso(int iteracoes, int total, out int porcentagem, out string barraDeProgresso)
        {
            porcentagem = (int)(((double)iteracoes / total) * 100);
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

        private void ApagarArquivosTemporarios(string caminhoPastaArquivosTemporarios)
        {
            try
            {
                if (Directory.Exists(caminhoPastaArquivosTemporarios))
                {
                    string[] arquivos = Directory.GetFiles(caminhoPastaArquivosTemporarios);
                    foreach (string arquivo in arquivos)
                    {
                        try
                        {
                            if (File.Exists(arquivo))
                            {
                                File.Delete(arquivo);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao excluir {arquivo}: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"\nA pasta '{caminhoPastaArquivosTemporarios}' não existe.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nOcorreu um erro ao excluir os arquivos: {ex.Message}");
            }
        }

        private void ValidarTamanhoArquivoParaCombinar(string pastaDestino, JsonSerializerOptions options, List<string> arquivosTemporarios, double limiteComTolerancia)
        {
            string caminhoPastaArquivosTemporarios = @"C:\Users\guilherme2000925\Desktop\PastaDestino\ArquivosTemporarios";
            string extensaoPermitida = ".json" ;

            long tamanhoTotal = CalcularTamanhoArquivoPasta(caminhoPastaArquivosTemporarios, extensaoPermitida);
            double tamanhoTotalFormatado = Math.Round((double)tamanhoTotal / (1024 * 1024), 2);

            try
            {
                if (tamanhoTotalFormatado <= limiteComTolerancia)
                {
                    List<ParametrosJSON> dadosCombinados = CombinarArquivosTemporarios(arquivosTemporarios);

                    string jsonStringFinal = JsonSerializer.Serialize(dadosCombinados, options);
                    File.WriteAllText(pastaDestino, jsonStringFinal);

                    VerTamanhoArquivoAposFechar(pastaDestino);
                }
                else
                {
                    ApagarArquivosTemporarios(caminhoPastaArquivosTemporarios);
                    Console.WriteLine($"Tamanho excede o limite. Arquivo não gerado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar o arquivo combinado: {ex.Message}");
            }
        }

        private long CalcularTamanhoArquivoPasta(string caminhoPasta, string extensaoPermitida = null!)
        {
            long tamanhoTotal = 0;
            DirectoryInfo diretorio = new(caminhoPasta);

            if (!diretorio.Exists)
            {
                Console.WriteLine("O caminho especificado não existe.");
                return 0;
            }

            FileInfo[] arquivos = extensaoPermitida == null ? diretorio.GetFiles() : diretorio.GetFiles().Where(f => extensaoPermitida.Contains(f.Extension.ToLower())).ToArray();
            foreach (FileInfo arquivo in arquivos)
            {
                tamanhoTotal += arquivo.Length;
            }
            return tamanhoTotal;
        }

        private List<ParametrosJSON> CombinarArquivosTemporarios(List<string> arquivosTemporarios)
        {
            int iteracoesCombinarArquivos = 0;

            List<ParametrosJSON> dadosCombinados = [];
            foreach (string arquivo in arquivosTemporarios)
            {
                try
                {
                    string jsonString = File.ReadAllText(arquivo);
                    List<ParametrosJSON>? parteDadosCombinados = JsonSerializer.Deserialize<List<ParametrosJSON>>(jsonString)!;
                    dadosCombinados.AddRange(parteDadosCombinados);
                    File.Delete(arquivo);

                    iteracoesCombinarArquivos++;

                    BarraDeProgresso(iteracoesCombinarArquivos, arquivosTemporarios.Count, out int porcentagem, out string barraDeProgresso);
                    Console.Write($"\rCombinando arquivos temporários | {barraDeProgresso} {porcentagem}%");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nErro ao combinar arquivos: {ex.Message}");
                }
            }
            return dadosCombinados;
        }

        private Action<int> GerarArquivosTemporarios(List<ParametrosJSON> parametrosJSONs, int numThreads, int tamanhoParte, List<string> arquivosTemporarios, JsonSerializerOptions options, object lockObject)
        {
            return (indiceParte) =>
            {
                int inicio = indiceParte * tamanhoParte;
                int fim = (indiceParte == numThreads - 1) ? parametrosJSONs.Count : (indiceParte + 1) * tamanhoParte;
                List<ParametrosJSON> dadosTemporarios = parametrosJSONs.GetRange(inicio, fim - inicio);

                string pastaDestinoTemporario = @$"C:\Users\guilherme2000925\Desktop\PastaDestino\ArquivosTemporarios\dadosTemporários_{indiceParte}.json";
                string jsonString = JsonSerializer.Serialize(dadosTemporarios, options);

                lock (lockObject)
                {
                    arquivosTemporarios.Add(pastaDestinoTemporario);
                }
                File.WriteAllText(pastaDestinoTemporario, jsonString);
            };
        }

        private void EscreverEmPartes(List<ParametrosJSON> parametrosJSONs, int numThreads, int tamanhoParte, JsonSerializerOptions options, List<string> arquivosTemporarios)
        {
            int iteracoesEscreverEmPartes = 0;
            object lockObject = new();

            Action<int> escreverParte = GerarArquivosTemporarios(parametrosJSONs, numThreads, tamanhoParte, arquivosTemporarios, options, lockObject);
            Parallel.For(0, numThreads, i =>
            {
                escreverParte(i);
                lock (lockObject)
                {
                    iteracoesEscreverEmPartes++;
                    BarraDeProgresso(iteracoesEscreverEmPartes, numThreads, out int porcentagem, out string barraDeProgresso);
                    Console.Write($"\rGerando arquivos temporários | {barraDeProgresso} {porcentagem}%");
                }
            });
            Console.WriteLine();
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
                JsonSerializerOptions options = new() { WriteIndented = true };

                List<string> arquivosTemporarios = [];
                EscreverEmPartes(parametrosJSONs, numThreads, tamanhoParte, options, arquivosTemporarios);
                ValidarTamanhoArquivoParaCombinar(pastaDestino, options, arquivosTemporarios, limiteComTolerancia);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nOperação concluída!");
                Console.ResetColor();
                Console.WriteLine(separador);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao escrever arquivo JSON: {ex.Message}");
            }
        }

        public void GerarArquivoJson(string pastaDestino, int tamanhoArquivoDesejado)
        {
            GeradorStringAlfanumerico geradorStringAlfanumerico = new();
            double limiteMaximoEmMB = 400.00;
            double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);
            int loopEquivalente1MB = 11655;
            int quantidadeLoop = tamanhoArquivoDesejado * loopEquivalente1MB;

            if (tamanhoArquivoDesejado <= limiteComTolerancia)
            {
                EscreverArquivoComThreads(pastaDestino, geradorStringAlfanumerico, quantidadeLoop, limiteComTolerancia);
                Console.WriteLine("\nArquivo JSON criado.");
            }
            else
            {
                Console.WriteLine($"\nArquivo não gerado, passou do limite de {limiteComTolerancia}MB.");
            }
        }
    }
}
