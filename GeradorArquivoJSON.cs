using System.Text.Json;

namespace Desafio_01
{
    internal class GeradorArquivoJSON
    {
        var identacao = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(parametrosJSON, identacao);
        Diretorio diretorio = new();

        string nomeArquivo = "parametrosAlfanumericos.json";
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
