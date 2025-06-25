using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        public void GerarArquivoJson(string pastaDestino, int tamanhoArquivoDesejado)
        {
            JsonSerializerOptions identacao = new() { WriteIndented = true };
            GeradorListaAlfanumerica geradorListaAlfanumerica = new();
            List<ParametrosJSON> listaParametrosJSON = geradorListaAlfanumerica.GerarListaAlfanumerica(tamanhoArquivoDesejado);

            double limiteMaximoEmMB = 400.00;
            double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);

            if (File.Exists(pastaDestino) && tamanhoArquivoDesejado < limiteComTolerancia)
            {
                File.Delete(pastaDestino);
                using (FileStream fileStream = new(pastaDestino, FileMode.Create))
                {
                    using (StreamWriter streamWriter = new(fileStream))
                    {
                        for (int i = 0; i < listaParametrosJSON.Count; i++)
                        {
                            string jsonString = JsonSerializer.Serialize(listaParametrosJSON[i], identacao);
                            streamWriter.WriteLine(jsonString + (i < listaParametrosJSON.Count - 1 ? "," : ""));
                        }
                    }
                }
                Console.WriteLine("\nArquivo JSON atualizado.");
            }
            else if (tamanhoArquivoDesejado < limiteComTolerancia)
            {
                using (FileStream fileStream = new(pastaDestino, FileMode.Create))
                {
                    using (StreamWriter streamWriter = new(fileStream))
                    {
                        for (int i = 0; i < listaParametrosJSON.Count; i++)
                        {
                            string jsonString = JsonSerializer.Serialize(listaParametrosJSON[i], identacao);
                            streamWriter.WriteLine(jsonString + (i < listaParametrosJSON.Count - 1 ? "," : ""));
                        }
                    }
                }
                Console.WriteLine("\nArquivo JSON criado.");
            }
        }
    }
}

