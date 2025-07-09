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

        private void BarraDeProgresso(int total, int i, out int porcentagem, out string barraDeProgresso)
        {
            porcentagem = (int)(((double)i / total) * 100);
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

        private void EscreverArquivo(string pastaDestino, GeradorStringAlfanumerico gerador, int quantidadeDeIteracoes, double limiteMbComTolerancia)
        {
            try
            {
                using StreamWriter writer = new(pastaDestino);
                int totalObjetos = 0;
                long bytes = 0;
                double tamanhoArquivoDuranteLoop = 0;
                string separador = "\n------------------------------------------------------------------------------------------------------------------------";

                Console.WriteLine(separador);
                Console.WriteLine("\nIniciando operação...\n");

                writer.WriteLine("[");
                for (int i = 0; i < quantidadeDeIteracoes; i++)
                {
                    if(tamanhoArquivoDuranteLoop <= limiteMbComTolerancia)
                    {
                        writer.WriteLine(GerarLinha(gerador) + (i < quantidadeDeIteracoes - 1 ? "," : ""));
                        BarraDeProgresso(i, quantidadeDeIteracoes, out int porcentagem, out string barraDeProgresso);
                        tamanhoArquivoDuranteLoop = TamanhoArquivoDuranteLoop(gerador, quantidadeDeIteracoes, ref bytes, i);

                        totalObjetos += 4;

                        Console.Write($"\r{barraDeProgresso} {porcentagem}% | Objetos criados: {totalObjetos} | Tamanho arquivo JSON: {tamanhoArquivoDuranteLoop}MB");
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

        public void GerarArquivoJson(string pastaDestino, int tamanhoDesejadoEmMb)
        {
            GeradorStringAlfanumerico gerador = new();
            double limiteMb = 400.0;
            double limiteMbComTolerancia = limiteMb + (limiteMb * 0.01);
            int iteracoesPorMb = 13500;
            int quantidadeDeIteracoes = tamanhoDesejadoEmMb * iteracoesPorMb;

            if (tamanhoDesejadoEmMb > limiteMbComTolerancia)
            {
                Console.WriteLine($"\nArquivo não gerado, passou do limite de {limiteMbComTolerancia}MB.");
                return;
            }
            EscreverArquivo(pastaDestino, gerador, quantidadeDeIteracoes, limiteMbComTolerancia);
            Console.WriteLine("\nArquivo JSON criado.");
        }
    }
}
