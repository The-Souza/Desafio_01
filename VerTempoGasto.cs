using System.Diagnostics;

namespace Desafio_01
{
    public class VerTempoGasto
    {
        public void Cronometro(string caminhoArquivo, int tamanhoMb)
        {
            var gerador = new GeradorArquivoJSON();
            var stopwatch = Stopwatch.StartNew();

            gerador.GerarArquivoJson(caminhoArquivo, tamanhoMb);

            stopwatch.Stop();
            ExibirTempoGasto(stopwatch.Elapsed);
        }

        private void ExibirTempoGasto(TimeSpan tempo)
        {
            if (tempo.TotalSeconds < 1)
            {
                return;
            }
            string tempoFormatado = tempo.ToString(@"hh\:mm\:ss\.fff");
            Console.WriteLine($"\nTempo gasto: {tempoFormatado}");
        }
    }
}