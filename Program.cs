namespace Desafio_01
{
    public class Program
    {
        public static void Main()
        {
            try 
            {
                GeradorArquivoJSON geradorArquivoJSON = new();
                geradorArquivoJSON.GerarArquivoJson();
            } 
            catch (Exception ex) 
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally 
            {
                Console.WriteLine("\nCorreu tudo bem!");
            }
        }
    }
}