namespace Desafio_01
{
    public class TamanhoArquivo
    {
        public int GerarTamanhoArquivo()
        {
            string valorStringTamanhoArquivo;
            do
            {
                Console.WriteLine("Digite o tamanho do arquivo desejado");
                valorStringTamanhoArquivo = Console.ReadLine() ?? string.Empty;
                Console.Clear();
            } while (string.IsNullOrEmpty(valorStringTamanhoArquivo));

            Console.WriteLine("Digite o tamanho do arquivo desejado");
            Console.WriteLine(valorStringTamanhoArquivo);

            bool n = true;
            while (n == true)
            {
                if (valorStringTamanhoArquivo != string.Empty)
                {
                    n = false;
                }
                else
                {
                    break;
                }
            }

            int valorIntTamanhoArquivo = Convert.ToInt32(valorStringTamanhoArquivo);
            return valorIntTamanhoArquivo;
        }
    }
}
