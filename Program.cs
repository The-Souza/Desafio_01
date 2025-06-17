using System.Collections.Generic;

namespace Desafio_01
{
    public class Program
    {
        public static void Main()
        {
            try 
            {
                GeradorArquivoJSON geradorArquivoJSON = new();
                GeradorListaAlfanumerica geradorListaAlfanumerica = new();

                List<ParametrosJSON> listaParametrosJSON = geradorListaAlfanumerica.GerarListaAlfanumerica();

                //foreach (ParametrosJSON parametrosJSON in listaParametrosJSON)
                //{
                //    Console.WriteLine($"Parametros {parametrosJSON.letras}: {parametrosJSON.parametros}");
                //}

                geradorArquivoJSON.GerarArquivoJson(listaParametrosJSON)
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