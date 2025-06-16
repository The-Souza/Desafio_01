using System.Text.Json;
using System.IO;

namespace Desafio_01
{
    public class Program
    {
        public static void Main()
        {
            int comprimentoDesejado = 6;
            string alfanumericoAleatoriaA = GeradorStringAlfanumerico.GerarStringAlfanmerico(comprimentoDesejado);
            string alfanumericoAleatoriaB = GeradorStringAlfanumerico.GerarStringAlfanmerico(comprimentoDesejado);
            string alfanumericoAleatoriaC = GeradorStringAlfanumerico.GerarStringAlfanmerico(comprimentoDesejado);
            string alfanumericoAleatoriaD = GeradorStringAlfanumerico.GerarStringAlfanmerico(comprimentoDesejado);

            ParametrosJSON parametrosJSON1 = new ParametrosJSON { A = alfanumericoAleatoriaA};
            ParametrosJSON parametrosJSON2 = new ParametrosJSON { B = alfanumericoAleatoriaB };
            ParametrosJSON parametrosJSON3 = new ParametrosJSON { C = alfanumericoAleatoriaC };
            ParametrosJSON parametrosJSON4 = new ParametrosJSON { D = alfanumericoAleatoriaD };

            List<ParametrosJSON> parametrosJSON = new List<ParametrosJSON>();
            
            parametrosJSON.Add(parametrosJSON1);
            parametrosJSON.Add(parametrosJSON2);
            parametrosJSON.Add(parametrosJSON3);
            parametrosJSON.Add(parametrosJSON4);

            

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
            else {
                File.WriteAllText(caminhoCompleto, jsonString);
                Console.WriteLine("Serialização B do arquivo completa");
            }
        }
    }
}