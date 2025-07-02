using System.Diagnostics;

namespace Desafio_01
{
    public class VerTempoGasto
    {
        public async Task Conometro(string pastaRaiz, int valorTamanhoArquivo)
        {
            GeradorArquivoJSON geradorArquivoJSON = new();
            Stopwatch stopwatch = new();

            stopwatch.Start();

            await geradorArquivoJSON.GerarArquivoJson(pastaRaiz, valorTamanhoArquivo);

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

