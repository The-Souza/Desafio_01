using System.Diagnostics;

namespace Gerador_Arquivo_Json
{
    public class VerTempoGasto
    {
        public void Cronometro(string diretorio, int tamanhoMb)
        {
            var gerador = new GeradorArquivoJSON();
            var stopwatch = Stopwatch.StartNew();

            gerador.GerarArquivoJson(diretorio, tamanhoMb);

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