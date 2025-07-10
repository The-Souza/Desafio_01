using System.Diagnostics;

namespace Desafio_01
{
    public class VerTempoGasto
    {
        public void Cronometro(string pastaRaiz, int valorTamanhoArquivo)
        {
            GeradorArquivoJSON gerador = new();
            Stopwatch stopwatch = new();

            stopwatch.Start();

            gerador.GerarArquivoJson(pastaRaiz, valorTamanhoArquivo);

            stopwatch.Stop();

            TimeSpan tempoGasto = stopwatch.Elapsed;
            string tempoFormatado = tempoGasto.ToString("hh\\:mm\\:ss\\.fff");

            if (stopwatch.ElapsedMilliseconds > 1000)
            {
                Console.WriteLine($"\nTempo gasto: {tempoFormatado}");
            }
        }
    }
}

