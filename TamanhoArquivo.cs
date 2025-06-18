namespace Desafio_01
{
    public class TamanhoArquivo
    {
        public int GerarTamanhoArquivo()
        {
            Console.WriteLine("Digite o tamanho do arquivo desejado: ");
            var valorString = Console.ReadLine();

            bool n = true;
            while (n == true)
            {
                if (valorString != string.Empty)
                {
                    n = false;
                }
                else
                {
                    break;
                }
            }

            int valorInt = Convert.ToInt32(valorString);
            return valorInt;
        }
    }
}
