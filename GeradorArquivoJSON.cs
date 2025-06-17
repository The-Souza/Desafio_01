using System.Text.Json;

namespace Desafio_01
{
    public class GeradorArquivoJSON
    {
        public void GerarArquivoJson()
        {
            GeradorListaAlfanumerica geradorListaAlfanumerica = new();
            Diretorio diretorio = new();
            List<ParametrosJSON> listaParametrosJSON = geradorListaAlfanumerica.GerarListaAlfanumerica();

            JsonSerializerOptions identacao = new() { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(listaParametrosJSON, identacao);

            string nomeArquivo = "listaAlfanumericos.json";
            string raiz = diretorio.GetRaiz();
            string caminhoCompleto = Path.Combine(raiz, nomeArquivo);

            if (File.Exists(caminhoCompleto))
            {
                File.Delete(caminhoCompleto);
                File.AppendAllText(caminhoCompleto, jsonString);
                Console.WriteLine("Serialização A do arquivo completa");
            }
            else
            {
                File.WriteAllText(caminhoCompleto, jsonString);
                Console.WriteLine("Serialização B do arquivo completa");
            }
        }
    }
}
