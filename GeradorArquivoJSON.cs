using System.Text;
using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        public void GerarArquivoJson(string pastaDestino, int tamanhoArquivoDesejado)
        {
            GeradorStringAlfanumerico geradorStringAlfanumerico = new();

            double limiteMaximoEmMB = 20000.00;
            double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);
            int loopEquivalente1MB = 13500;
            int quantidadeLoop = tamanhoArquivoDesejado * loopEquivalente1MB;

            if (File.Exists(pastaDestino) && tamanhoArquivoDesejado < limiteComTolerancia)
            {
                File.Delete(pastaDestino);
                EscreverArquivo(pastaDestino, geradorStringAlfanumerico, quantidadeLoop);
                Console.WriteLine("\nArquivo JSON atualizado.");
            }
            else if (tamanhoArquivoDesejado < limiteComTolerancia)
            {
                EscreverArquivo(pastaDestino, geradorStringAlfanumerico, quantidadeLoop);
                Console.WriteLine("\nArquivo JSON criado.");
            }
        }

        private void EscreverArquivo(string pastaDestino, GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop)
        {
            try
            {
                using StreamWriter writer = new(pastaDestino);
                GerarLinha(geradorStringAlfanumerico);
                int quantidadeObjetos = 0;
                long tamanhoBytes = 0;

                Console.WriteLine("\nIniciando operação...");
                for (int i = 0; i < quantidadeLoop; i++)
                {
                    writer.WriteLine(GerarLinha(geradorStringAlfanumerico) + (i < quantidadeLoop - 1 ? "," : ""));
                    tamanhoBytes += Encoding.UTF8.GetByteCount(GerarLinha(geradorStringAlfanumerico) + (i < quantidadeLoop - 1 ? "," : ""));

                    int porcetagem = (int)(((double)i / quantidadeLoop) * 100);
                    string barraDeProgresso = "[" + new string('#', porcetagem / 2) + new string('-', 50 - porcetagem / 2) + "]";

                    quantidadeObjetos += 4;

                    double jsonEmMB = (double)tamanhoBytes / (1024 * 1024);
                    double jsonEmMbFormatado = Math.Round(jsonEmMB, 2);

                    Console.Write($"\r{barraDeProgresso} {porcetagem}% | Objetos criados: {quantidadeObjetos} | Tamanho do arquivo JSON: {jsonEmMbFormatado}MB");
                }
                Console.WriteLine("\nOperação concluída!");

                tamanhoBytes = new FileInfo(pastaDestino).Length;
                double jsonEmMbAposFechar = (double)tamanhoBytes / (1024 * 1024);
                double jsonEmMbFormatadoAposFechar = Math.Round(jsonEmMbAposFechar, 2);
                Console.WriteLine($"\nTamanho do arquivo (após fechar): {jsonEmMbFormatadoAposFechar}MB");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nException: {ex.Message}");
            }
        }

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
    }
}
