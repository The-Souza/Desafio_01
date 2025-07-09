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

        private void BarraDeProgresso(int i, int quantidadeLoop, out int porcentagem, out string barraDeProgresso)
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
            long bytes = new FileInfo(pastaDestino).Length;
            double tamanhoMB = Math.Round((double)bytes / (1024 * 1024), 2);

            string tamanhoFormatado = tamanhoMB < 1000 ? $"{tamanhoMB}MB" : $"{Math.Round(tamanhoMB / 1000, 2)}GB";
            Console.WriteLine($"\nTamanho do arquivo (após fechamento): {tamanhoFormatado}");
            return bytes;
        }

        private void EscreverArquivo(string pastaDestino, GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop, double limiteComTolerancia)
        {
            try
            {
                using StreamWriter writer = new(pastaDestino);
                GerarLinha(geradorStringAlfanumerico);
                int quantidadeObjetos = 0;
                long tamanhoBytes = 0;
                double tamanhoArquivoDuranteLoop = 0;
                string separador = "\n------------------------------------------------------------------------------------------------------------------------";

                Console.WriteLine(separador);
                Console.WriteLine("\nIniciando operação...\n");

                writer.WriteLine("[");
                for (int i = 0; i < quantidadeLoop; i++)
                {
                    if(tamanhoArquivoDuranteLoop <= limiteComTolerancia)
                    {
                        writer.WriteLine(GerarLinha(geradorStringAlfanumerico) + (i < quantidadeLoop - 1 ? "," : ""));
                        BarraDeProgresso(i, quantidadeLoop, out int porcentagem, out string barraDeProgresso);
                        tamanhoArquivoDuranteLoop = TamanhoArquivoDuranteLoop(geradorStringAlfanumerico, quantidadeLoop, ref tamanhoBytes, i);

                        quantidadeObjetos += 4;

                        Console.Write($"\r{barraDeProgresso} {porcentagem}% | Objetos criados: {quantidadeObjetos} | Tamanho arquivo JSON: {tamanhoArquivoDuranteLoop}MB");
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

            if (File.Exists(pastaDestino) && tamanhoArquivoDesejado < limiteComTolerancia)
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
