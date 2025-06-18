using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        public void GerarArquivoJson(string pastaDestino, int tamanhoArquivoDesejado)
        {
            GeradorListaAlfanumerica geradorListaAlfanumerica = new();
            List<ParametrosJSON> listaParametrosJSON = geradorListaAlfanumerica.GerarListaAlfanumerica(tamanhoArquivoDesejado);

            JsonSerializerOptions identacao = new() { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(listaParametrosJSON, identacao);

            long jsonEmBytes = System.Text.Encoding.UTF8.GetByteCount(jsonString);
            double jsonEmMB = (double)jsonEmBytes / (1024 * 1024);
            double jsonEmMbFormatado = Math.Round(jsonEmMB, 2);

            double limiteMaximoEmMB = 400.00;
            double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);
            if (jsonEmMbFormatado < limiteComTolerancia)
            {
                Console.WriteLine($"\nTamanho do arquivo JSON: {jsonEmMbFormatado}MB");
                if (File.Exists(pastaDestino))
                {
                    StreamWriter arquivoJsonAtualizado = new(pastaDestino, false);
                    arquivoJsonAtualizado.Write(jsonString);
                    arquivoJsonAtualizado.Close();
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
            else
            {
                Console.WriteLine($"\nO arquivo JSON excede o limite de {limiteComTolerancia}MB.");
                Console.WriteLine($"\nTamanho do arquivo JSON: {jsonEmMbFormatado}MB");
            }

        }
    }
}
