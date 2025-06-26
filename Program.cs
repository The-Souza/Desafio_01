namespace Desafio_01
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                //Pasta Destino => C:\Users\guilherme2000925\Desktop\Pasta Destino        
                Diretorio diretorio = new();
                VerTempoGasto verTempoGasto = new();
                DefinirTamanhoArquivo definirTamanhoArquivo = new();
                VerTamanhoArquivo verTamanhoArquivo = new();

                string pastaDiretorio = diretorio.CaminhoDeSaida();
                int tamanhoArquivo = definirTamanhoArquivo.GerarTamanhoArquivo();
                string nomeArquivo = "listaAlfanumericos.json";
                string caminhoCompleto = Path.Combine(pastaDiretorio, nomeArquivo);

                verTempoGasto.Conometro(caminhoCompleto, tamanhoArquivo);
                verTamanhoArquivo.LimiteMaximoEmMB(caminhoCompleto, tamanhoArquivo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nException: {ex.Message}");
            }
        }
    }
}
