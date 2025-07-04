namespace Desafio_01
{
    public class Program
    {
        private string GetDiretorio(string definirDiretorio, string caminhoPadrao, string argumento1)
        {
            if (argumento1.StartsWith(definirDiretorio + "="))
            {
                string valorStringDefinirDiretorio = argumento1.Substring(definirDiretorio.Length + 1);
                caminhoPadrao = valorStringDefinirDiretorio;
            }
            else
            {
                Console.WriteLine($"\nValor inválido.");
            }
            return caminhoPadrao;
        }

        private double GetTamanhoArquivo(string definirTamanhoArquivo, double tamanhoPadrao, string argumento2)
        {
            if (argumento2.StartsWith(definirTamanhoArquivo + "="))
            {
                string valorStringDefinirTamanhoArquivo = argumento2.Substring(definirTamanhoArquivo.Length + 1);
                if (double.TryParse(valorStringDefinirTamanhoArquivo, out double resultado))
                {
                    tamanhoPadrao = resultado;
                }
                else
                {
                    Console.WriteLine($"\nValor inválido.");
                }
            }
            return tamanhoPadrao;
        }

        private void MostarTamanhoArquivo(double tamanhoPadrao)
        {
            if (tamanhoPadrao < 1000.00)
            {
                Console.WriteLine($"\nTamanho escolhido: {tamanhoPadrao}MB");
            }
            else if (tamanhoPadrao >= 1000.00)
            {
                double valorDoubleGB = Math.Round((tamanhoPadrao / 1000), 2);
                Console.WriteLine($"\nTamanho escolhido: {valorDoubleGB}GB");
            }
        }

        private void MostrarDiretorio(string caminhoPadrao)
        {
            Console.WriteLine($"\nLocal de destino: {caminhoPadrao}");

            if (Directory.Exists(caminhoPadrao))
            {
                Console.WriteLine("\nO diretório existe.");
            }
            else
            {
                Console.WriteLine($"\nO diretório não existe.");
            }
        }

        public static void Main(string[] args)
        {
            // cd C:\Users\guilherme2000925\source\repos\Desafio_01\bin\Debug\net8.0
            // Desafio_01.exe --output=C:\Users\guilherme2000925\Desktop\PastaDestino --size=400

            try
            {
                ArgumentNullException.ThrowIfNull(args);
                Program program = new();

                string[] argumentos = Environment.GetCommandLineArgs();
                string definirDiretorio = "--output";
                string definirTamanhoArquivo = "--size";
                string caminhoPadrao = "";
                double tamanhoPadrao = 0;

                if (argumentos.Length >= 2)
                {
                    string argumento1 = argumentos[1];
                    string argumento2 = argumentos[2];

                    caminhoPadrao = program.GetDiretorio(definirDiretorio, caminhoPadrao, argumento1);
                    tamanhoPadrao = program.GetTamanhoArquivo(definirTamanhoArquivo, tamanhoPadrao, argumento2);

                    VerTempoGasto verTempoGasto = new();

                    string nomeArquivo = "listaAlfanumericos.json";
                    string caminhoCompleto = Path.Combine(caminhoPadrao, nomeArquivo);

                    program.MostrarDiretorio(caminhoPadrao);
                    program.MostarTamanhoArquivo(tamanhoPadrao);

                    int valorIntTamanhoPadrao = Convert.ToInt32(tamanhoPadrao);
                    verTempoGasto.Conometro(caminhoCompleto, valorIntTamanhoPadrao);
                }
                else
                {
                    Console.WriteLine("Por favor, forneça dois argumentos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nException: {ex.Message}");
            }
        }
    }
}