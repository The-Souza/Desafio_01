namespace Desafio_01
{
    public class DefinirTamanhoArquivo
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

            int valorIntTamanhoArquivo = Convert.ToInt32(valorStringTamanhoArquivo);

            if (valorIntTamanhoArquivo < 1000)
            {
                Console.WriteLine($"Tamanho escolhido: {valorStringTamanhoArquivo}MB");
            }
            else if (valorIntTamanhoArquivo >= 1000)
            {
                int valorIntGB = valorIntTamanhoArquivo / 1000;
                Console.WriteLine($"Tamanho escolhido: {valorIntGB}GB");
            }
            return valorIntTamanhoArquivo;
        }
    }
}
