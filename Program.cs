using System;
using System.IO;

namespace Desafio_01
{
    public class Program
    {
        private const string NomeArquivo = "listaAlfanumericos.json";

        public static void Main(string[] args)
        {
            if (!ValidarArgumentos(args, out string diretorio, out double tamanhoMb))
            {
                Console.WriteLine("Uso: --output=<caminho> --size=<tamanho_em_MB>");
                return;
            }

            ExibirInformacoes(diretorio, tamanhoMb);

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

        private static bool ValidarArgumentos(string[] args, out string diretorio, out double tamanhoMb)
        {
            diretorio = string.Empty;
            tamanhoMb = 0;

            if (args == null || args.Length < 2)
                return false;

            diretorio = ObterValorParametro(args[0], "--output=");
            var tamanhoStr = ObterValorParametro(args[1], "--size=");

            if (string.IsNullOrWhiteSpace(diretorio) || string.IsNullOrWhiteSpace(tamanhoStr) || !double.TryParse(tamanhoStr, out tamanhoMb))
                return false;

            return true;
        }

        private static string ObterValorParametro(string argumento, string prefixo)
        {
            if (argumento.StartsWith(prefixo, StringComparison.OrdinalIgnoreCase))
                return argumento.Substring(prefixo.Length);
            return string.Empty;
        }

        private static void ExibirInformacoes(string diretorio, double tamanhoMb)
        {
            Console.WriteLine($"\nLocal de destino: {diretorio}");
            Console.WriteLine(Directory.Exists(diretorio) ? "O diretório existe." : "O diretório não existe.");
            Console.WriteLine($"Tamanho escolhido: {(tamanhoMb < 1000 ? $"{tamanhoMb} MB" : $"{tamanhoMb / 1000:F2} GB")}");
        }
    }
}
