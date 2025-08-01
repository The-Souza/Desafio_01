using System.Text.Json;

namespace Gerador_Arquivo_Json
{
    public class GeradorArquivoJSON
    {
        private const string NomeArquivo = "listaAlfanumericos.json";
        private const string PastaDestino = "PastaDestino";
        private const string PastaTemporaria = "ArquivosTemporarios";
        private const double LimiteMb = 400.0;
        private const double Tolerancia = 0.01;
        private const int IteracoesPorMb = 11655;

        public void GerarArquivoJson(string diretorio, int tamanhoDesejadoMb)
        {
            if (tamanhoDesejadoMb > LimiteMb + (LimiteMb * Tolerancia))
            {
                Console.WriteLine($"\nArquivo não gerado: tamanho ultrapassa limite de {LimiteMb}MB.");
                return;
            }

            string caminhoPastaDestino = Path.Combine(diretorio, PastaDestino);
            string caminhoDestino = Path.Combine(caminhoPastaDestino, NomeArquivo);
            string caminhoPastaTemporaria = Path.Combine(diretorio, PastaTemporaria);

            Directory.CreateDirectory(caminhoPastaTemporaria);
            Directory.CreateDirectory(caminhoPastaDestino);

            LimparPastaTemporaria(caminhoPastaTemporaria);

            string separador = "\n------------------------------------------------------------------------------------------------------------------------";
            Console.WriteLine(separador);

            int quantidadeIteracoes = tamanhoDesejadoMb * IteracoesPorMb;
            GeradorStringAlfanumerico geradorStrings = new();

            Console.WriteLine("\nIniciando geração dos dados...");

            var dados = CriarListaDeParametrosJson(geradorStrings, quantidadeIteracoes);

            int numThreads = Environment.ProcessorCount;
            int divisao = dados.Count / numThreads;
            var arquivosTemporarios = new List<string>();
            var opcoesJson = new JsonSerializerOptions { WriteIndented = true };

            EscreverArquivosTemporarios(caminhoPastaTemporaria, dados, numThreads, divisao, arquivosTemporarios, opcoesJson);

            if (!CombinarArquivosTemporarios(caminhoDestino, arquivosTemporarios, opcoesJson))
            {
                Console.WriteLine("Falha ao gerar arquivo final devido ao tamanho excedido.");
                return;
            }

            Console.WriteLine(separador);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nArquivo JSON criado com sucesso!");
            Console.ResetColor();
        }


        private void LimparPastaTemporaria(string caminhoPastaTemporaria)
        {
            foreach (var arquivo in Directory.GetFiles(caminhoPastaTemporaria))
            {
                try
                {
                    File.Delete(arquivo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao apagar arquivo temporário {arquivo}: {ex.Message}");
                }
            }
        }

        private List<ParametrosJSON> CriarListaDeParametrosJson(GeradorStringAlfanumerico gerador, int quantidadeDeIteracoes)
        {
            var lista = new List<ParametrosJSON>(quantidadeDeIteracoes);

            int totalObjetos = quantidadeDeIteracoes * 4;
            for (int i = 0; i < quantidadeDeIteracoes; i++)
            {
                lista.Add(new ParametrosJSON
                {
                    A = gerador.GerarString(),
                    B = gerador.GerarString(),
                    C = gerador.GerarString(),
                    D = gerador.GerarString()
                });
            }

            Console.WriteLine($"Objetos criados: {totalObjetos}");
            return lista;
        }

        private void EscreverArquivosTemporarios(string caminhoPastaTemporaria, List<ParametrosJSON> dados, int numThreads, int divisao, List<string> arquivos, JsonSerializerOptions opcoes)
        {
            object trava = new();

            Parallel.For(0, numThreads, i =>
            {
                int inicio = i * divisao;
                int fim = (i == numThreads - 1) ? dados.Count : (i + 1) * divisao;

                var parte = dados.GetRange(inicio, fim - inicio);
                string caminho = Path.Combine(caminhoPastaTemporaria, $"dadosTemp_{i}.json");

                string json = JsonSerializer.Serialize(parte, opcoes);
                File.WriteAllText(caminho, json);

                lock (trava)
                {
                    arquivos.Add(caminho);
                    AtualizarProgresso("Gerando arquivos temporários", arquivos.Count, numThreads);
                }
            });
            Console.WriteLine();
        }

        private bool CombinarArquivosTemporarios(string caminhoDestino, List<string> arquivosTemporarios, JsonSerializerOptions opcoes)
        {
            double limiteComTolerancia = LimiteMb + (LimiteMb * Tolerancia);

            long tamanhoAtualBytes = arquivosTemporarios.Sum(arquivo => new FileInfo(arquivo).Length);
            double tamanhoAtualMb = tamanhoAtualBytes / (1024.0 * 1024.0);

            if (tamanhoAtualMb > limiteComTolerancia)
            {
                Console.WriteLine("Tamanho dos arquivos temporários excede o limite permitido.");
                return false;
            }

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
                    Thread.Sleep(10);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao combinar arquivo temporário {arquivosTemporarios[i]}: {ex.Message}");
                    return false;
                }
            }
            Console.WriteLine();

            try
            {
                if (File.Exists(caminhoDestino))
                {
                    File.Delete(caminhoDestino);
                }

                string jsonFinal = JsonSerializer.Serialize(dadosCombinados, opcoes);
                File.WriteAllText(caminhoDestino, jsonFinal);
                Console.WriteLine($"Tamanho final do arquivo: {ObterTamanhoFormatado(caminhoDestino)}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar arquivo final: {ex.Message}");
                return false;
            }
        }

        private void AtualizarProgresso(string mensagem, int atual, int total)
        {
            int percentual = (int)((double)atual / total * 100);
            string barra = "[" + new string('#', percentual / 2) + new string('-', 50 - percentual / 2) + "]";
            Console.Write($"\r{mensagem} {barra} {percentual}%");
        }

        private string ObterTamanhoFormatado(string caminhoArquivo)
        {
            long tamanhoBytes = new FileInfo(caminhoArquivo).Length;
            double tamanhoMb = tamanhoBytes / (1024.0 * 1024.0);
            return tamanhoMb < 1000 ? $"{tamanhoMb:F2}MB" : $"{tamanhoMb / 1000:F2}GB";
        }
    }
}
