namespace Desafio_01
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                Diretorio diretorio = new();
                GeradorArquivoJSON geradorArquivoJSON = new();
                TamanhoArquivo valorTamanho = new();

                int tamanhoArquivo = valorTamanho.GerarTamanhoArquivo();
                string nomeArquivo = "listaAlfanumericos.json";
                string pastaRaiz = diretorio.GetRaiz();
                string caminhoCompleto = Path.Combine(pastaRaiz, nomeArquivo);

                geradorArquivoJSON.GerarArquivoJson(caminhoCompleto, tamanhoArquivo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}