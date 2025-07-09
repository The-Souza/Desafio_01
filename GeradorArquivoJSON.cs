//using System.Text.Json;

//namespace Desafio_01
//{
//    public class GeradorArquivoJSON
//    {
//        private List<ParametrosJSON> GerarListaAlfanumerica(GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop)
//        {
//            List<ParametrosJSON> parametrosJSONs = [];

//            Console.Write($"Gerando lista...\n");

//            int quantidadeObjetos = quantidadeLoop * 4;
//            for (int i = 0; i < quantidadeLoop; i++)
//            {
//                parametrosJSONs.Add(new ParametrosJSON
//                {
//                    A = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
//                    B = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
//                    C = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
//                    D = geradorStringAlfanumerico.GetAlfanumericoAleatoria()
//                });
//                BarraDeProgresso(i, quantidadeLoop, out int porcentagem, out string barraDeProgresso);
//            }
//            Console.Write($"Objetos criados: {quantidadeObjetos}\n");
//            return parametrosJSONs;
//        }

//        private void BarraDeProgresso(int iteracoes, int total, out int porcentagem, out string barraDeProgresso)
//        {
//            porcentagem = (int)(((double)iteracoes / total) * 100);
//            barraDeProgresso = "[" + new string('#', porcentagem / 2) + new string('-', 50 - porcentagem / 2) + "]";
//        }

//        private long VerTamanhoArquivoAposFechar(string pastaDestino)
//        {
//            long tamanhoBytes = new FileInfo(pastaDestino).Length;
//            double jsonEmMb = Math.Round((double)tamanhoBytes / (1024 * 1024), 2);

//            if (jsonEmMb < 1000.00)
//            {
//                Console.WriteLine($"\nTamanho do arquivo (Após fechar): {jsonEmMb}MB");
//            }
//            else
//            {
//                Console.WriteLine($"\nTamanho do arquivo (Após fechar): {Math.Round(jsonEmMb / 1000, 2)}GB");
//            }
//            return tamanhoBytes;
//        }

//        private void ApagarArquivosTemporarios(string caminhoPastaArquivosTemporarios)
//        {
//            try
//            {
//                if (Directory.Exists(caminhoPastaArquivosTemporarios))
//                {
//                    string[] arquivos = Directory.GetFiles(caminhoPastaArquivosTemporarios);
//                    foreach (string arquivo in arquivos)
//                    {
//                        try
//                        {
//                            if (File.Exists(arquivo))
//                            {
//                                File.Delete(arquivo);
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            Console.WriteLine($"Erro ao excluir {arquivo}: {ex.Message}");
//                        }
//                    }
//                }
//                else
//                {
//                    Console.WriteLine($"\nA pasta '{caminhoPastaArquivosTemporarios}' não existe.");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"\nOcorreu um erro ao excluir os arquivos: {ex.Message}");
//            }
//        }

//        private void ValidarTamanhoArquivoParaCombinar(string pastaDestino, JsonSerializerOptions options, List<string> arquivosTemporarios, double limiteComTolerancia)
//        {
//            string caminhoPastaArquivosTemporarios = @"C:\Users\guilherme2000925\Desktop\PastaDestino\ArquivosTemporarios";
//            string extensaoPermitida = ".json" ;

//            long tamanhoTotal = CalcularTamanhoArquivoPasta(caminhoPastaArquivosTemporarios, extensaoPermitida);
//            double tamanhoTotalFormatado = Math.Round((double)tamanhoTotal / (1024 * 1024), 2);

//            try
//            {
//                if (tamanhoTotalFormatado <= limiteComTolerancia)
//                {
//                    List<ParametrosJSON> dadosCombinados = CombinarArquivosTemporarios(arquivosTemporarios);

//                    string jsonStringFinal = JsonSerializer.Serialize(dadosCombinados, options);
//                    File.WriteAllText(pastaDestino, jsonStringFinal);

//                    VerTamanhoArquivoAposFechar(pastaDestino);
//                }
//                else
//                {
//                    ApagarArquivosTemporarios(caminhoPastaArquivosTemporarios);
//                    Console.WriteLine($"Tamanho excede o limite. Arquivo não gerado.");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Erro ao salvar o arquivo combinado: {ex.Message}");
//            }
//        }

//        private long CalcularTamanhoArquivoPasta(string caminhoPasta, string extensaoPermitida = null!)
//        {
//            long tamanhoTotal = 0;
//            DirectoryInfo diretorio = new(caminhoPasta);

//            if (!diretorio.Exists)
//            {
//                Console.WriteLine("O caminho especificado não existe.");
//                return 0;
//            }

//            FileInfo[] arquivos = extensaoPermitida == null ? diretorio.GetFiles() : diretorio.GetFiles().Where(f => extensaoPermitida.Contains(f.Extension.ToLower())).ToArray();
//            foreach (FileInfo arquivo in arquivos)
//            {
//                tamanhoTotal += arquivo.Length;
//            }
//            return tamanhoTotal;
//        }

//        private List<ParametrosJSON> CombinarArquivosTemporarios(List<string> arquivosTemporarios)
//        {
//            int iteracoesCombinarArquivos = 0;

//            List<ParametrosJSON> dadosCombinados = [];
//            foreach (string arquivo in arquivosTemporarios)
//            {
//                try
//                {
//                    string jsonString = File.ReadAllText(arquivo);
//                    List<ParametrosJSON>? parteDadosCombinados = JsonSerializer.Deserialize<List<ParametrosJSON>>(jsonString)!;
//                    dadosCombinados.AddRange(parteDadosCombinados);
//                    File.Delete(arquivo);

//                    iteracoesCombinarArquivos++;

//                    BarraDeProgresso(iteracoesCombinarArquivos, arquivosTemporarios.Count, out int porcentagem, out string barraDeProgresso);
//                    Console.Write($"\rCombinando arquivos temporários | {barraDeProgresso} {porcentagem}%");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"\nErro ao combinar arquivos: {ex.Message}");
//                }
//            }
//            return dadosCombinados;
//        }

//        private Action<int> GerarArquivosTemporarios(List<ParametrosJSON> parametrosJSONs, int numThreads, int tamanhoParte, List<string> arquivosTemporarios, JsonSerializerOptions options, object lockObject)
//        {
//            return (indiceParte) =>
//            {
//                int inicio = indiceParte * tamanhoParte;
//                int fim = (indiceParte == numThreads - 1) ? parametrosJSONs.Count : (indiceParte + 1) * tamanhoParte;
//                List<ParametrosJSON> dadosTemporarios = parametrosJSONs.GetRange(inicio, fim - inicio);

//                string pastaDestinoTemporario = @$"C:\Users\guilherme2000925\Desktop\PastaDestino\ArquivosTemporarios\dadosTemporários_{indiceParte}.json";
//                string jsonString = JsonSerializer.Serialize(dadosTemporarios, options);

//                lock (lockObject)
//                {
//                    arquivosTemporarios.Add(pastaDestinoTemporario);
//                }
//                File.WriteAllText(pastaDestinoTemporario, jsonString);
//            };
//        }

//        private void EscreverEmParalelo(List<ParametrosJSON> parametrosJSONs, int numThreads, int tamanhoParte, JsonSerializerOptions options, List<string> arquivosTemporarios)
//        {
//            int iteracoesEscreverEmParalelo = 0;
//            object lockObject = new();

//            Action<int> escreverParte = GerarArquivosTemporarios(parametrosJSONs, numThreads, tamanhoParte, arquivosTemporarios, options, lockObject);
//            Parallel.For(0, numThreads, i =>
//            {
//                escreverParte(i);
//                lock (lockObject)
//                {
//                    iteracoesEscreverEmParalelo++;
//                    BarraDeProgresso(iteracoesEscreverEmParalelo, numThreads, out int porcentagem, out string barraDeProgresso);
//                    Console.Write($"\rGerando arquivos temporários | {barraDeProgresso} {porcentagem}%");
//                }
//            });
//            Console.WriteLine();
//        }

//        private void EscreverArquivoComThreads(string pastaDestino, GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop, double limiteComTolerancia)
//        {
//            try
//            {
//                string separador = "\n------------------------------------------------------------------------------------------------------------------------";
//                Console.WriteLine(separador);
//                Console.WriteLine("\nIniciando operação...\n");

//                List<ParametrosJSON> parametrosJSONs = GerarListaAlfanumerica(geradorStringAlfanumerico, quantidadeLoop);

//                int numThreads = Environment.ProcessorCount;
//                int tamanhoParte = parametrosJSONs.Count / numThreads;
//                JsonSerializerOptions options = new() { WriteIndented = true };

//                List<string> arquivosTemporarios = [];

//                EscreverEmParalelo(parametrosJSONs, numThreads, tamanhoParte, options, arquivosTemporarios);
//                ValidarTamanhoArquivoParaCombinar(pastaDestino, options, arquivosTemporarios, limiteComTolerancia);

//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("\nOperação concluída!");
//                Console.ResetColor();
//                Console.WriteLine(separador);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"\nErro ao escrever arquivo JSON: {ex.Message}");
//            }
//        }

//        public void GerarArquivoJson(string pastaDestino, int tamanhoArquivoDesejado)
//        {
//            GeradorStringAlfanumerico geradorStringAlfanumerico = new();
//            double limiteMaximoEmMB = 400.00;
//            double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);
//            int loopEquivalente1MB = 11655;
//            int quantidadeLoop = tamanhoArquivoDesejado * loopEquivalente1MB;

//            if (tamanhoArquivoDesejado <= limiteComTolerancia)
//            {
//                EscreverArquivoComThreads(pastaDestino, geradorStringAlfanumerico, quantidadeLoop, limiteComTolerancia);
//                Console.WriteLine("\nArquivo JSON criado.");
//            }
//            else
//            {
//                Console.WriteLine($"\nArquivo não gerado, passou do limite de {limiteComTolerancia}MB.");
//            }
//        }
//    }
//}

using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        private const string CaminhoPadraoArquivosTemporarios = @"C:\\Users\\guilherme2000925\\Desktop\\PastaDestino\\ArquivosTemporarios";

        private List<ParametrosJSON> CriarListaDeParametrosJson(GeradorStringAlfanumerico gerador, int quantidadeDeIteracoes)
        {
            List<ParametrosJSON> parametros = [];
            Console.WriteLine("Gerando lista...");

            int totalObjetos = quantidadeDeIteracoes * 4;
            for (int i = 0; i < quantidadeDeIteracoes; i++)
            {
                parametros.Add(new ParametrosJSON
                {
                    A = gerador.GetAlfanumericoAleatoria(),
                    B = gerador.GetAlfanumericoAleatoria(),
                    C = gerador.GetAlfanumericoAleatoria(),
                    D = gerador.GetAlfanumericoAleatoria()
                });
                AtualizarProgresso("Progresso da geração", i, quantidadeDeIteracoes);
            }

            Console.WriteLine($"Objetos criados: {totalObjetos}");
            return parametros;
        }

        private void AtualizarProgresso(string mensagem, int atual, int total)
        {
            int porcentagem = (int)(((double)atual / total) * 100);
            string barra = "[" + new string('#', porcentagem / 2) + new string('-', 50 - porcentagem / 2) + "]";
            Console.Write($"\r{mensagem} | {barra} {porcentagem}%");
        }

        private long ObterTamanhoArquivo(string caminhoArquivo)
        {
            long bytes = new FileInfo(caminhoArquivo).Length;
            double tamanhoMB = Math.Round((double)bytes / (1024 * 1024), 2);

            string tamanhoFormatado = tamanhoMB < 1000 ? $"{tamanhoMB}MB" : $"{Math.Round(tamanhoMB / 1000, 2)}GB";
            Console.WriteLine($"\nTamanho do arquivo (após fechamento): {tamanhoFormatado}");
            return bytes;
        }

        private void ApagarTemporarios(string caminho)
        {
            if (!Directory.Exists(caminho))
            {
                Console.WriteLine($"\nA pasta '{caminho}' não existe.");
                return;
            }

            foreach (string arquivo in Directory.GetFiles(caminho))
            {
                try 
                { 
                    if (File.Exists(arquivo)) File.Delete(arquivo); 
                }
                catch (Exception ex) 
                { 
                    Console.WriteLine($"Erro ao excluir {arquivo}: {ex.Message}"); 
                }
            }
        }

        private void ValidarETentarCombinar(string destinoFinal, JsonSerializerOptions opcoesDeSerializacao, List<string> arquivosTemporarios, double limiteMbComTolerancia)
        {
            long tamanhoTotalBytes = CalcularTamanhoArquivos(CaminhoPadraoArquivosTemporarios, ".json");
            double tamanhoTotalMb = Math.Round((double)tamanhoTotalBytes / (1024 * 1024), 2);

            if (tamanhoTotalMb > limiteMbComTolerancia)
            {
                ApagarTemporarios(CaminhoPadraoArquivosTemporarios);
                Console.WriteLine("Tamanho excede o limite. Arquivo não gerado.");
                return;
            }

            try
            {
                var dadosCombinados = CombinarArquivosTemporarios(arquivosTemporarios);
                string jsonFinal = JsonSerializer.Serialize(dadosCombinados, opcoesDeSerializacao);
                File.WriteAllText(destinoFinal, jsonFinal);
                ObterTamanhoArquivo(destinoFinal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar o arquivo combinado: {ex.Message}");
            }
        }

        private long CalcularTamanhoArquivos(string caminho, string extensao)
        {
            if (!Directory.Exists(caminho))
            {
                Console.WriteLine("O caminho especificado não existe.");
                return 0;
            }

            return new DirectoryInfo(caminho)
                .GetFiles()
                .Where(f => extensao.Contains(f.Extension.ToLower()))
                .Sum(f => f.Length);
        }

        private List<ParametrosJSON> CombinarArquivosTemporarios(List<string> arquivosTemporarios)
        {
            var dadosCombinados = new List<ParametrosJSON>();

            for (int i = 0; i < arquivosTemporarios.Count; i++)
            {
                try
                {
                    string conteudo = File.ReadAllText(arquivosTemporarios[i]);
                    var dados = JsonSerializer.Deserialize<List<ParametrosJSON>>(conteudo);
                    if (dados != null) dadosCombinados.AddRange(dados);
                    File.Delete(arquivosTemporarios[i]);

                    AtualizarProgresso("Combinando arquivos temporários", i + 1, arquivosTemporarios.Count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nErro ao combinar arquivos: {ex.Message}");
                }
            }
            Console.WriteLine();
            return dadosCombinados;
        }

        private Action<int> CriarGeradorTemporario(List<ParametrosJSON> parametros, int numThreads, int tamanhoDivisao, List<string> arquivos, JsonSerializerOptions opcoes, object trava)
        {
            return i =>
            {
                int inicio = i * tamanhoDivisao;
                int fim = (i == numThreads - 1) ? parametros.Count : (i + 1) * tamanhoDivisao;
                var parte = parametros.GetRange(inicio, fim - inicio);

                string caminho = $"{CaminhoPadraoArquivosTemporarios}\\dadosTemporarios_{i}.json";
                string json = JsonSerializer.Serialize(parte, opcoes);

                lock (trava) arquivos.Add(caminho);
                File.WriteAllText(caminho, json);
            };
        }

        private void EscreverEmParalelo(List<ParametrosJSON> parametros, int numThreads, int tamanhoDivisao, JsonSerializerOptions opcoes, List<string> arquivos)
        {
            object trava = new();
            var gerador = CriarGeradorTemporario(parametros, numThreads, tamanhoDivisao, arquivos, opcoes, trava);

            Parallel.For(0, numThreads, i =>
            {
                gerador(i);
                lock (trava)
                    AtualizarProgresso("Gerando arquivos temporários", i + 1, numThreads);
            });
            Console.WriteLine();
        }

        private void EscreverArquivoJsonFinal(string destinoFinal, GeradorStringAlfanumerico gerador, int quantidadeDeIteracoes, double limiteMbComTolerancia)
        {
            try
            {
                Console.WriteLine("\n---------------- INICIO ----------------");
                Console.WriteLine("Iniciando operação...\n");

                var parametros = CriarListaDeParametrosJson(gerador, quantidadeDeIteracoes);
                int numThreads = Environment.ProcessorCount;
                int tamanhoDivisao = parametros.Count / numThreads;
                var opcoes = new JsonSerializerOptions { WriteIndented = true };
                List<string> arquivosTemporarios = [];

                EscreverEmParalelo(parametros, numThreads, tamanhoDivisao, opcoes, arquivosTemporarios);
                ValidarETentarCombinar(destinoFinal, opcoes, arquivosTemporarios, limiteMbComTolerancia);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nOperação concluída!");
                Console.ResetColor();
                Console.WriteLine("--------------- FIM ----------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro na operação: {ex.Message}");
            }
        }

        public void GerarArquivoJson(string destinoFinal, int tamanhoDesejadoEmMb)
        {
            var gerador = new GeradorStringAlfanumerico();
            double limiteMb = 400.0;
            double limiteMbComTolerancia = limiteMb + (limiteMb * 0.01);
            int iteracoesPorMb = 11655;
            int quantidadeDeIteracoes = tamanhoDesejadoEmMb * iteracoesPorMb;

            if (tamanhoDesejadoEmMb > limiteMbComTolerancia)
            {
                Console.WriteLine($"\nArquivo não gerado, passou do limite de {limiteMbComTolerancia}MB.");
                return;
            }

            EscreverArquivoJsonFinal(destinoFinal, gerador, quantidadeDeIteracoes, limiteMbComTolerancia);
            Console.WriteLine("\nArquivo JSON criado.");
        }
    }
}
