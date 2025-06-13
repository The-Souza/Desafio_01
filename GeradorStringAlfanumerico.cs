using System.Text;

namespace Desafio_01
{
    public class GeradorStringAlfanumerico
    {
        public string GerarStringAlfanmerico(int comprimento)
        {
            string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new();
            StringBuilder alfanumerico = new StringBuilder(comprimento);


            for (int i = 0; i < comprimento; i++)
            {
                int indiceAleatorio = random.Next(0, caracteres.Length);
                alfanumerico.Append(caracteres[indiceAleatorio]);
            }
            return alfanumerico.ToString();
        }
    }
}
