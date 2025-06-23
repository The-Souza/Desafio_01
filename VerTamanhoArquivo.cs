namespace Desafio_01
{
    public class VerTamanhoArquivo
    {
        public void LimiteMaximoEmMB(string arquivoJSON)
        {
            long jsonEmBytes = System.Text.Encoding.UTF8.GetByteCount(arquivoJSON);
            double jsonEmMB = (double)jsonEmBytes / (1024 * 1024);
            double jsonEmMbFormatado = Math.Round(jsonEmMB, 2);

            double limiteMaximoEmMB = 400.00;
            double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);

            if (jsonEmMbFormatado < limiteComTolerancia)
            {
                Console.WriteLine($"\nTamanho do arquivo JSON: {jsonEmMbFormatado}MB");
            }
            else
            {
                Console.WriteLine($"\nO arquivo JSON excede o limite de {limiteComTolerancia}MB.");
                Console.WriteLine($"\nTamanho do arquivo JSON: {jsonEmMbFormatado}MB");
            }
        }
    }
}
