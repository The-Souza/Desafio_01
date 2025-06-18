using System.Text;

namespace Desafio_01
{
    public class GeradorStringAlfanumerico
    {
        public string GerarStringAlfanumerico()
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

        public string GetAlfanumericoAleatoria()
        {
            string alfanumericoAleatoria = GerarStringAlfanumerico();
            return alfanumericoAleatoria;
        }
    }
}
