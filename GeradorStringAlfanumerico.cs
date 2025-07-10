using System.Text;

namespace Desafio_01
{
    public class GeradorStringAlfanumerico
    {
        private const int Comprimento = 6;
        private const string Caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random Random = new();

        public string GerarString()
        {
            var builder = new StringBuilder(Comprimento);
            for (int i = 0; i < Comprimento; i++)
            {
                int indice = Random.Next(Caracteres.Length);
                builder.Append(Caracteres[indice]);
            }
            return builder.ToString();
        }
    }
}
