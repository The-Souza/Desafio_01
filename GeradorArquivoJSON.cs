using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        public void GerarArquivoJson(string pastaDestino, int tamanhoArquivoDesejado)
        {
            VerTamanhoArquivo verTamanhoArquivo = new();
            GeradorListaAlfanumerica geradorListaAlfanumerica = new();
            List<ParametrosJSON> listaParametrosJSON = geradorListaAlfanumerica.GerarListaAlfanumerica(tamanhoArquivoDesejado);

            JsonSerializerOptions identacao = new() { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(listaParametrosJSON, identacao);
            verTamanhoArquivo.LimiteMaximoEmMB(jsonString);

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
    }
}
