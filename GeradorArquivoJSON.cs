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
            string pastaRaiz = diretorio.GetRaiz();
            string caminhoCompleto = Path.Combine(pastaRaiz, nomeArquivo);

            if (File.Exists(caminhoCompleto))
            {
                StreamWriter streamWriterFalse = new(caminhoCompleto, false);
                streamWriterFalse.Write(jsonString);
                streamWriterFalse.Close();
            }
            else
            {
                StreamWriter streamWriterTrue = new(caminhoCompleto, true); 
                streamWriterTrue.Write(jsonString);
                streamWriterTrue.Close();
            }
        }
    }
}
