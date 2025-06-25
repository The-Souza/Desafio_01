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

            Console.WriteLine($"Tamanho escolhido: {valorStringTamanhoArquivo}MB");

            int valorIntTamanhoArquivo = Convert.ToInt32(valorStringTamanhoArquivo);

            return valorIntTamanhoArquivo;
        }
    }
}
