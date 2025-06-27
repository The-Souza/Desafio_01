namespace Desafio_01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // cd C:\Users\guilherme2000925\source\repos\Desafio_01\bin\Debug\net8.0
            // Desafio_01.exe --output=C:\Users\guilherme2000925\Desktop\PastaDestino --size=400

            try
            {
                ArgumentNullException.ThrowIfNull(args);
                string[] argumentos = Environment.GetCommandLineArgs();
                string definirDiretorio = "--output";
                string definirTamanhoArquivo = "--size";
                string caminhoPadrao = @"C:\Users\guilherme2000925\Desktop\PastaDestino";
                int tamanhoPadrao = 100;

                if (argumentos.Length >= 2)
                {
                    string argumento1 = argumentos[1];
                    string argumento2 = argumentos[2];

                    caminhoPadrao = GetDiretorio(definirDiretorio, caminhoPadrao, argumento1);
                    tamanhoPadrao = GetTamanhoArquivo(definirTamanhoArquivo, tamanhoPadrao, argumento2);

                    VerTempoGasto verTempoGasto = new();
                    VerTamanhoArquivo verTamanhoArquivo = new();

                    string nomeArquivo = "listaAlfanumericos.json";
                    string caminhoCompleto = Path.Combine(caminhoPadrao, nomeArquivo);

                    MostrarDiretorio(caminhoPadrao);
                    MostarTamanhoArquivo(tamanhoPadrao);
                    verTempoGasto.Conometro(caminhoCompleto, tamanhoPadrao);
                    verTamanhoArquivo.LimiteMaximoEmMB(caminhoCompleto, tamanhoPadrao);
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
        private static string GetDiretorio(string definirDiretorio, string caminhoPadrao, string argumento1)
        {
            if (argumento1.StartsWith(definirDiretorio + "="))
            {
                string valorStringdefinirDiretorio = argumento1.Substring(definirDiretorio.Length + 1);
                caminhoPadrao = valorStringdefinirDiretorio;
            }
            else
            {
                Console.WriteLine($"Valor inválido: {definirDiretorio}. Usando valor padrão.");
            }
            return caminhoPadrao;
        }

        private static int GetTamanhoArquivo(string tamanhoArquivo, int tamanhoPadrao, string argumento2)
        {
            if (argumento2.StartsWith(tamanhoArquivo + "="))
            {
                string valorStringTamanhoArquivo = argumento2.Substring(tamanhoArquivo.Length + 1);
                if (int.TryParse(valorStringTamanhoArquivo, out int resultado))
                {
                    tamanhoPadrao = (tamanhoPadrao - tamanhoPadrao) + resultado;
                }
                else
                {
                    Console.WriteLine($"Valor inválido: {valorStringTamanhoArquivo}. Usando valor padrão.");
                }
            }
            return tamanhoPadrao;
        }

        private static void MostarTamanhoArquivo(int tamanhoPadrao)
        {
            if (tamanhoPadrao < 1000)
            {
                Console.WriteLine($"\nTamanho escolhido: {tamanhoPadrao}MB");
            }
            else if (tamanhoPadrao >= 1000)
            {
                int valorIntGB = tamanhoPadrao / 1000;
                Console.WriteLine($"Tamanho escolhido: {valorIntGB}GB");
            }
        }

        private static void MostrarDiretorio(string caminhoPadrao)
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

    }
}