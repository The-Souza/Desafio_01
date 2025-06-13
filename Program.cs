using System.Text.Json;

namespace Desafio_01
{
    public class Program
    {
        public static void Main(string[] args) 
        {
            var geradorAlfanumerico = new GeradorStringAlfanumerico();
            int comprimentoDesejado = 6;
            string alfanumericoAleatoriaA = geradorAlfanumerico.GerarStringAlfanmerico(comprimentoDesejado);
            string alfanumericoAleatoriaB = geradorAlfanumerico.GerarStringAlfanmerico(comprimentoDesejado);
            string alfanumericoAleatoriaC = geradorAlfanumerico.GerarStringAlfanmerico(comprimentoDesejado);
            string alfanumericoAleatoriaD = geradorAlfanumerico.GerarStringAlfanmerico(comprimentoDesejado);

            var parametrosJSON = new ParametrosJSON
            {
                A = alfanumericoAleatoriaA,
                B = alfanumericoAleatoriaB,
                C = alfanumericoAleatoriaC,
                D = alfanumericoAleatoriaD
            };

            string jsonString = JsonSerializer.Serialize<ParametrosJSON>(parametrosJSON);

            Console.WriteLine(jsonString);

        }
    }
}