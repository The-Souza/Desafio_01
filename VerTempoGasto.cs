using System.Diagnostics;

namespace Desafio_01
{
    public class VerTempoGasto
    {
        public void Conometro(string pastaRaiz, int tamanhoArquivoMB)
        {
            GeradorArquivoJSON geradorArquivoJSON = new();
            Stopwatch stopwatch = new();

            stopwatch.Start();

            geradorArquivoJSON.GerarArquivoJson(pastaRaiz, tamanhoArquivoMB);

            stopwatch.Stop();

            TimeSpan tempoGasto = stopwatch.Elapsed;
            string tempoFormatado = tempoGasto.ToString("hh\\:mm\\:ss\\.fff");
            Console.WriteLine($"\nTempo gasto: {tempoFormatado}");
        }
    }
}
