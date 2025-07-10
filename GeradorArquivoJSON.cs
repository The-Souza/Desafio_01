using System.Text;
using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        private const double LimiteMb = 400.0;
        private const double Tolerancia = 0.01;
        private const int IteracoesPorMb = 13500;

        public void GerarArquivoJson(string pastaDestino, int tamanhoDesejadoEmMb)
        {
            GeradorStringAlfanumerico gerador = new();
            int quantidadeDeIteracoes = tamanhoDesejadoEmMb * IteracoesPorMb;
            double limiteMbComTolerancia = LimiteMb + (LimiteMb * Tolerancia);

            if (tamanhoDesejadoEmMb > limiteMbComTolerancia)
            {
                Console.WriteLine($"\nArquivo não gerado, passou do limite de {limiteMbComTolerancia}MB.");
                return;
            }
            EscreverArquivo(pastaDestino, gerador, quantidadeDeIteracoes, limiteMbComTolerancia);
            Console.WriteLine("\nArquivo JSON criado.");
        }

        private void EscreverArquivo(string pastaDestino, GeradorStringAlfanumerico gerador, int quantidadeDeIteracoes, double limiteMbComTolerancia)
        {
            try
            {
                int numThreads = Environment.ProcessorCount;
                int tamanhoParte = quantidadeDeIteracoes / numThreads;
                List<List<string>> blocos = [];

                Parallel.For(0, numThreads, i =>
                {
                    List<string> bloco = [];
                    for (int j = 0; j < tamanhoParte; j++)
                    {
                        int indexGlobal = i * tamanhoParte + j;
                        bloco.Add(GerarLinha(gerador));
                    }
                    lock (blocos)
                    {
                        blocos.Add(bloco);
                    }
                });

                List<string> todasLinhas = [.. blocos.SelectMany(b => b)];
                if (todasLinhas.Count < quantidadeDeIteracoes)
                {
                    for (int i = todasLinhas.Count; i < quantidadeDeIteracoes; i++)
                    {
                        todasLinhas.Add(GerarLinha(gerador));
                    }
                }

                using StreamWriter writer = new(pastaDestino);
                long bytes = 0;
                int quantidadeObjetos = 0;
                string separador = "\n------------------------------------------------------------------------------------------------------------------------";

                Console.WriteLine(separador);
                Console.WriteLine("\nIniciando operação...\n");

                writer.WriteLine("[");
                for (int i = 0; i < todasLinhas.Count; i++)
                {
                    string linha = todasLinhas[i];
                    double tamanhoAtual = TamanhoArquivoDuranteLoop(linha, quantidadeDeIteracoes, ref bytes, i);

                    if (tamanhoAtual <= limiteMbComTolerancia)
                    {
                        writer.WriteLine(linha + (i < todasLinhas.Count - 1 ? "," : ""));
                        BarraDeProgresso(i, quantidadeDeIteracoes, out int porcentagem, out string barraDeProgresso);
                        quantidadeObjetos += 4;
                        Console.Write($"\r{barraDeProgresso} {porcentagem}% | Objetos criados: {quantidadeObjetos} | Tamanho arquivo JSON: {tamanhoAtual}MB");
                    }
                    else
                    {
                        Console.WriteLine($"\n\nArquivo passou do limite de {limiteMbComTolerancia}MB, fechando arquivo.");
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
        
        private string GerarLinha(GeradorStringAlfanumerico gerador)
        {
            JsonSerializerOptions identacao = new() { WriteIndented = true };
            var parametros = new ParametrosJSON
            {
                A = gerador.GerarString(),
                B = gerador.GerarString(),
                C = gerador.GerarString(),
                D = gerador.GerarString()
            };
            return JsonSerializer.Serialize(parametros, identacao);
        }

        private void BarraDeProgresso(int atual, int total, out int porcentagem, out string barraDeProgresso)
        {
            porcentagem = (int)(((double)atual / total) * 100);
            barraDeProgresso = "[" + new string('#', porcentagem / 2) + new string('-', 50 - porcentagem / 2) + "]";
        }

        private double TamanhoArquivoDuranteLoop(string linha, int quantidadeDeIteracoes, ref long tamanhoBytes, int i)
        {
            tamanhoBytes += Encoding.UTF8.GetByteCount(linha + (i < quantidadeDeIteracoes - 1 ? "," : ""));
            return Math.Round((double)tamanhoBytes / (1024 * 1024), 2);
        }

        private long VerTamanhoArquivoAposFechar(string pastaDestino)
        {
            long bytes = new FileInfo(pastaDestino).Length;
            double tamanhoMB = Math.Round((double)bytes / (1024 * 1024), 2);

            string tamanhoFormatado = tamanhoMB < 1000 ? $"{tamanhoMB}MB" : $"{Math.Round(tamanhoMB / 1000, 2)}GB";
            Console.WriteLine($"\nTamanho do arquivo (após fechamento): {tamanhoFormatado}");
            return bytes;
        }
    }
}
