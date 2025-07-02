using System.Formats.Asn1;
using System.Text;
using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        private async Task EscreverJsonAsync(string pastaDestino, Func<int, object> provedorParametros, int numParametros)
        {
            using var stream = new FileStream(pastaDestino, FileMode.Create, FileAccess.Write, FileShare.None, 65536, useAsync: true);
            using var writer = new StreamWriter(stream);
            await writer.WriteAsync("[");

            for (int i = 0; i < numParametros; i++)
            {
                if (i > 0)
                {
                    await writer.WriteAsync(",");
                }

                var parametro = provedorParametros(i);
                var jsonString = JsonSerializer.Serialize(parametro);
                await writer.WriteAsync(jsonString);
            }

            await writer.WriteAsync("]");
            await writer.FlushAsync();
        }

        public async Task GetEscreverJsonAsync(string pastaDestino, Func<int, object> provedorParametros, int numParametros)
        {
            await Task.Run(() => EscreverJsonAsync(pastaDestino, provedorParametros, numParametros));
        }

        public async Task GerarArquivoJson(string pastaDestino, int tamanhoArquivoDesejado)
        {
            GeradorStringAlfanumerico geradorStringAlfanumerico = new();

            var linha = new ParametrosJSON
            {
                A = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                B = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                C = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                D = geradorStringAlfanumerico.GetAlfanumericoAleatoria()
            };

            int loopEquivalente1MB = 13500;
            int numParametros = 2; // tamanhoArquivoDesejado * loopEquivalente1MB;

            Func<int, object> provedorParametros = (index) => new { Id = index, A = linha };

            Console.WriteLine("\nIniciando escrita do arquivo JSON...");
            await GetEscreverJsonAsync(pastaDestino, provedorParametros, numParametros);
            Console.WriteLine("\nEscrita do arquivo JSON concluída.");
        }
    }
}
