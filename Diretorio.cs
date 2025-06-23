namespace Desafio_01
{
    public class Diretorio
    {
        public string CaminhoDeSaida()
        {
            string valorStringDiretorio;
            do { 
                Console.WriteLine("Escolha o local onde o arquivo será criado:");
                valorStringDiretorio = Console.ReadLine() ?? string.Empty;
                Console.Clear();
            } while (string.IsNullOrEmpty(valorStringDiretorio));

            Console.WriteLine($"Local de destino: {valorStringDiretorio}");

            if (Directory.Exists(valorStringDiretorio))
            {
                Console.WriteLine($"\nO diretório '{valorStringDiretorio}' existe.\n");
            }
            else
            {
                Console.WriteLine($"\nO diretório '{valorStringDiretorio}' não existe.\n");
            }
            return valorStringDiretorio;
        }
    }
}
