using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        public void GerarArquivoJson(string pastaDestino)
        {
            GeradorListaAlfanumerica geradorListaAlfanumerica = new();
            List<ParametrosJSON> listaParametrosJSON = geradorListaAlfanumerica.GerarListaAlfanumerica();
            JsonSerializerOptions identacao = new() { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(listaParametrosJSON, identacao);

            long jsonEmBytes = System.Text.Encoding.UTF8.GetByteCount(jsonString);
            double jsonEmMB = (double)jsonEmBytes / (1024 * 1024);
            double jsonEmMbFormatado = Math.Round(jsonEmMB, 2);

            double limiteMaximoEmMB = 400.00;
            double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);
            if (jsonEmMbFormatado > limiteComTolerancia)
            {
                Console.WriteLine($"O arquivo JSON excede o limite de {limiteComTolerancia}MB.");
            }
            else
            {
                Console.WriteLine($"Tamanho do arquivo JSON: {jsonEmMbFormatado} MB");
                if (File.Exists(pastaDestino))
                {
                    StreamWriter arquivoJsonReescrito = new(pastaDestino, false);
                    arquivoJsonReescrito.Write(jsonString);
                    arquivoJsonReescrito.Close();
                    Console.WriteLine("\nArquivo JSON atualizado.");
                }
                else
                {
                    StreamWriter arquivoJson = new(pastaDestino, true); 
                    arquivoJson.Write(jsonString);
                    arquivoJson.Close();
                    Console.WriteLine("\nArquivo JSON criado.");
                }
            }

        }
    }
}
