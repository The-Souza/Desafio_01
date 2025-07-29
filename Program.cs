namespace Gerador_Arquivo_Json
{
    public class Program
    {
        private const string NomeArquivo = "listaAlfanumericos.json";

        public static void Main(string[] args)
        {
            Program program = new();

            if (!program.ValidarArgumentos(args, out string diretorio, out double tamanhoMb))
            {
                Console.WriteLine("Uso: --output=<caminho> --size=<tamanho_em_MB>");
                return;
            }

            program.ExibirInformacoes(diretorio, tamanhoMb);

            var caminhoCompleto = Path.Combine(diretorio, NomeArquivo);
            var cronometro = new VerTempoGasto();

            try
            {
                cronometro.Cronometro(caminhoCompleto, (int)tamanhoMb);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }
        }

        private bool ValidarArgumentos(string[] args, out string diretorio, out double tamanhoMb)
        {
            diretorio = string.Empty;
            tamanhoMb = 0;

            if (args == null || args.Length < 2)
                return false;

            diretorio = ObterValorParametro(args[0], "--output=");
            var tamanhoStr = ObterValorParametro(args[1], "--size=");

            if (string.IsNullOrWhiteSpace(diretorio) || string.IsNullOrWhiteSpace(tamanhoStr) || !double.TryParse(tamanhoStr, out tamanhoMb))
            {
                return false;
            }
            return true;
        }

        private string ObterValorParametro(string argumento, string prefixo)
        {
            if (argumento.StartsWith(prefixo, StringComparison.OrdinalIgnoreCase))
            {
                return argumento[prefixo.Length..];
            }
            return string.Empty;
        }

        private void ExibirInformacoes(string diretorio, double tamanhoMb)
        {
            Console.WriteLine($"\nLocal de destino: {diretorio}");
            Console.WriteLine(Directory.Exists(diretorio) ? "\nO diretório existe." : "\nO diretório não existe.");
            Console.WriteLine($"\nTamanho escolhido: {(tamanhoMb < 1000 ? $"{tamanhoMb}MB" : $"{tamanhoMb / 1000:F2}GB")}");
        }
    }
}