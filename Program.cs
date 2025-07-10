namespace Desafio_01
{
    public class Program
    {
        private const string nomeArquivo = "listaAlfanumericos.json";

        private bool TryObterDiretorio(string definirDiretorio, string argumento, out string caminho)
        {
            caminho = string.Empty;
            if (argumento.StartsWith(definirDiretorio + "="))
            {
                caminho = argumento[(definirDiretorio.Length + 1)..];
                return true;
            }
            Console.WriteLine("\nArgumento de diretório inválido. Use --output=<caminho>.");
            return false;
        }

        private bool TryObterTamanhoArquivo(string definirTamanhoArquivo, string argumento, out double tamanho)
        {
            tamanho = 0;
            if (argumento.StartsWith(definirTamanhoArquivo + "="))
            {
                string valor = argumento[(definirTamanhoArquivo.Length + 1)..];
                if (double.TryParse(valor, out double resultado))
                {
                    tamanho = resultado;
                    return true;
                }
                Console.WriteLine("\nTamanho inválido. Use um valor numérico para --size.");
                return false;
            }
            Console.WriteLine("\nArgumento de tamanho inválido. Use --size=<valor em MB>.");
            return false;
        }

        private void MostrarTamanhoArquivo(double tamanho)
        {
            string tamanhoFormatado = tamanho < 1000 ? $"{tamanho}MB" : $"{Math.Round(tamanho / 1000, 2)}GB";
            Console.WriteLine($"\nTamanho escolhido: {tamanhoFormatado}");
        }

        private void MostrarDiretorio(string caminho)
        {
            Console.WriteLine($"\nLocal de destino: {caminho}");
            Console.WriteLine(Directory.Exists(caminho) ? "\nO diretório existe." : "\nO diretório não existe.");
        }

        public static void Main(string[] args)
        {
            try
            {
                Program program = new();

                string[] argumentos = Environment.GetCommandLineArgs();
                string definirDiretorio = "--output";
                string definirTamanho = "--size";

                if (argumentos.Length < 3)
                {
                    Console.WriteLine("\nUso correto: Desafio_01.exe --output=<caminho> --size=<tamanho_em_MB>");
                    return;
                }

                string argumento1 = argumentos[1];
                string argumento2 = argumentos[2];

                if (!program.TryObterDiretorio(definirDiretorio, argumento1, out string caminho)) return;
                if (!program.TryObterTamanhoArquivo(definirTamanho, argumento2, out double tamanho)) return;

                program.MostrarDiretorio(caminho);
                program.MostrarTamanhoArquivo(tamanho);

                string caminhoCompleto = Path.Combine(caminho, nomeArquivo);

                VerTempoGasto verTempoGasto = new();
                int tamanhoInt = Convert.ToInt32(tamanho);
                verTempoGasto.Cronometro(caminhoCompleto, tamanhoInt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro inesperado: {ex.Message}");
            }
        }
    }
}
