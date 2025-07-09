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
            }
            Console.WriteLine($"Objetos criados: {totalObjetos}");
            return parametros;
        }

        private void AtualizarProgresso(string mensagem, int atual, int total)
        {
            int porcentagem = (int)(((double)atual / total) * 100);
            string barraDeProgresso = "[" + new string('#', porcentagem / 2) + new string('-', 50 - porcentagem / 2) + "]";
            Console.Write($"\r{mensagem} | {barraDeProgresso} {porcentagem}%");
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

        private void ValidarETentarCombinar(string pastaDestino, JsonSerializerOptions opcoes, List<string> arquivosTemporarios, double limiteMbComTolerancia)
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
                string jsonFinal = JsonSerializer.Serialize(dadosCombinados, opcoes);
                File.WriteAllText(pastaDestino, jsonFinal);
                ObterTamanhoArquivo(pastaDestino);
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
                .Where(f => extensao.Contains(f.Extension, StringComparison.CurrentCultureIgnoreCase))
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
                    if (dados != null) 
                    {
                        dadosCombinados.AddRange(dados);
                    }
                    File.Delete(arquivosTemporarios[i]);

                    AtualizarProgresso("Combinando arquivos temporários", i + 1, arquivosTemporarios.Count);
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nErro ao combinar arquivos: {ex.Message}");
                }
            }
            Console.WriteLine();
            return dadosCombinados;
        }

        private Action<int> GerarArquivosTemporarios(List<ParametrosJSON> parametros, int numThreads, int tamanhoDivisao, List<string> arquivos, JsonSerializerOptions opcoes, object trava)
        {
            return i =>
            {
                int inicio = i * tamanhoDivisao;
                int fim = (i == numThreads - 1) ? parametros.Count : (i + 1) * tamanhoDivisao;
                var parte = parametros.GetRange(inicio, fim - inicio);

                string caminho = $"{CaminhoPadraoArquivosTemporarios}\\dadosTemporarios_{i}.json";
                string json = JsonSerializer.Serialize(parte, opcoes);

                lock (trava)
                { 
                    arquivos.Add(caminho);
                }
                File.WriteAllText(caminho, json);
            };
        }

        private void EscreverEmParalelo(List<ParametrosJSON> parametros, int numThreads, int tamanhoDivisao, JsonSerializerOptions opcoes, List<string> arquivos)
        {
            object trava = new();
            var gerador = GerarArquivosTemporarios(parametros, numThreads, tamanhoDivisao, arquivos, opcoes, trava);

            Parallel.For(0, numThreads, i =>
            {
                gerador(i);
                lock (trava)
                {
                    AtualizarProgresso("Gerando arquivos temporários", i + 1, numThreads);
                    Thread.Sleep(100);
                }
            });
            Console.WriteLine();
        }

        private void EscreverArquivoJsonFinal(string pastaDestino, GeradorStringAlfanumerico gerador, int quantidadeDeIteracoes, double limiteMbComTolerancia)
        {
            try
            {
                string separador = "\n------------------------------------------------------------------------------------------------------------------------";
                Console.WriteLine(separador);
                Console.WriteLine("Iniciando operação...\n");

                var parametros = CriarListaDeParametrosJson(gerador, quantidadeDeIteracoes);
                int numThreads = Environment.ProcessorCount;
                int tamanhoDivisao = parametros.Count / numThreads;
                var opcoes = new JsonSerializerOptions { WriteIndented = true };
                List<string> arquivosTemporarios = [];

                EscreverEmParalelo(parametros, numThreads, tamanhoDivisao, opcoes, arquivosTemporarios);
                ValidarETentarCombinar(pastaDestino, opcoes, arquivosTemporarios, limiteMbComTolerancia);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nOperação concluída!");
                Console.ResetColor();
                Console.WriteLine(separador);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro na operação: {ex.Message}");
            }
        }

        public void GerarArquivoJson(string pastaDestino, int tamanhoDesejadoEmMb)
        {
            GeradorStringAlfanumerico gerador = new();
            double limiteMb = 400.0;
            double limiteMbComTolerancia = limiteMb + (limiteMb * 0.01);
            int iteracoesPorMb = 11655;
            int quantidadeDeIteracoes = tamanhoDesejadoEmMb * iteracoesPorMb;

            if (tamanhoDesejadoEmMb < limiteMbComTolerancia)
            {
                EscreverArquivoJsonFinal(pastaDestino, gerador, quantidadeDeIteracoes, limiteMbComTolerancia);
                Console.WriteLine(File.Exists(pastaDestino) ? "\nArquivo JSON atualizado." : "\nArquivo JSON criado.");
            }
            else
            {
                Console.WriteLine($"\nArquivo não gerado, passou do limite de {limiteMbComTolerancia}MB.");
            }
        }
    }
}
