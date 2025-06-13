using System.Text.Json;

namespace Desafio_01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int comprimentoDesejado = 6;
            string alfanumericoAleatoriaA = GeradorStringAlfanumerico.GerarStringAlfanmerico(comprimentoDesejado);
            string alfanumericoAleatoriaB = GeradorStringAlfanumerico.GerarStringAlfanmerico(comprimentoDesejado);
            string alfanumericoAleatoriaC = GeradorStringAlfanumerico.GerarStringAlfanmerico(comprimentoDesejado);
            string alfanumericoAleatoriaD = GeradorStringAlfanumerico.GerarStringAlfanmerico(comprimentoDesejado);

            var parametrosJSON = new ParametrosJSON
            {
                A = alfanumericoAleatoriaA,
                B = alfanumericoAleatoriaB,
                C = alfanumericoAleatoriaC,
                D = alfanumericoAleatoriaD
            };

            string jsonString = JsonSerializer.Serialize(parametrosJSON);

            string caminhoCompleto = Path.Combine(diretorio, nomeArquivo);
            File.WriteAllText(caminhoCompleto, jsonString);

            Console.WriteLine($"Arquivo JSON criado em: {caminhoCompleto}");
        }
    }
}