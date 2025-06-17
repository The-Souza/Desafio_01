using System.Text;

namespace Desafio_01
{
    public class GeradorStringAlfanumerico
    {
        private string? AlfanumericoAleatoriaA { get; set; }
        private string? AlfanumericoAleatoriaB { get; set; }
        private string? AlfanumericoAleatoriaC { get; set; }
        private string? AlfanumericoAleatoriaD { get; set; }

        public string GerarStringAlfanmerico()
        {
            int comprimento = 6;
            string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new();
            StringBuilder alfanumerico = new(comprimento);

            for (int i = 0; i < comprimento; i++)
            {
                int indiceAleatorio = random.Next(0, caracteres.Length);
                alfanumerico.Append(caracteres[indiceAleatorio]);
            }
            return alfanumerico.ToString();
        }

        public string GetAlfanumericoAleatoriaA()
        {
            string alfanumericoAleatoriaA = GerarStringAlfanmerico();
            return alfanumericoAleatoriaA;
        }

        public string GetAlfanumericoAleatoriaB()
        {
            string alfanumericoAleatoriaB = GerarStringAlfanmerico();
            return alfanumericoAleatoriaB;
        }

        public string GetAlfanumericoAleatoriaC()
        {
            string alfanumericoAleatoriaC = GerarStringAlfanmerico();
            return alfanumericoAleatoriaC;
        }

        public string GetAlfanumericoAleatoriaD()
        {
            string alfanumericoAleatoriaD = GerarStringAlfanmerico();
            return alfanumericoAleatoriaD;
        }
    }
}
