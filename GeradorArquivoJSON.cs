using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        public void GerarArquivoJson(IEnumerable<ParametrosJSON> parametrosJSON, int tamanhoArquivoDesejado, string pastaDestino)
        {
            double limiteMaximoEmMB = 400.00;
            double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);
            int quantidadeObjetos = 0;

            if (File.Exists(pastaDestino) && tamanhoArquivoDesejado < limiteComTolerancia)
            {
                File.Delete(pastaDestino);
                using (FileStream fileStream = new(pastaDestino, FileMode.Create))
                {
                    using var writer = new Utf8JsonWriter(fileStream, new JsonWriterOptions { Indented = true });

                    writer.WriteStartArray();
                    foreach (var parametros in parametrosJSON)
                    {
                        writer.WriteStartObject();
                        writer.WriteString("A", parametros.A);
                        writer.WriteString("B", parametros.B);
                        writer.WriteString("C", parametros.C);
                        writer.WriteString("D", parametros.D);
                        writer.WriteEndObject();
                        quantidadeObjetos += 4;
                    }
                    writer.WriteEndArray();
                }
                Console.WriteLine("\nArquivo JSON atualizado.");
                Console.WriteLine($"\nQuatidade de objetos criados: {quantidadeObjetos}");
            }
            else if (tamanhoArquivoDesejado < limiteComTolerancia)
            {
                using (FileStream fileStream = new(pastaDestino, FileMode.Create))
                {
                    using var writer = new Utf8JsonWriter(fileStream, new JsonWriterOptions { Indented = true });

                    writer.WriteStartArray();
                    foreach (var parametros in parametrosJSON)
                    {
                        writer.WriteStartObject();
                        writer.WriteString("A", parametros.A);
                        writer.WriteString("B", parametros.B);
                        writer.WriteString("C", parametros.C);
                        writer.WriteString("D", parametros.D);
                        writer.WriteEndObject();
                        quantidadeObjetos += 4;
                    }
                    writer.WriteEndArray();
                }
                Console.WriteLine("\nArquivo JSON criado.");
                Console.WriteLine($"\nQuatidade de objetos criados: {quantidadeObjetos}");
            }
        }
    }
}

