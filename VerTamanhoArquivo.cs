namespace Desafio_01
{
    public class VerTamanhoArquivo
    {
        public void LimiteMaximoEmMB(string pasta, int tamanho)
        {
            double limiteMaximoEmMB = 400.00;
            double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);

            if (File.Exists(pasta) && tamanho < limiteComTolerancia)
            {
                FileInfo fileInfo = new(pasta);
                long jsonEmBytes = fileInfo.Length;
                double jsonEmMB = (double)jsonEmBytes / (1024 * 1024);
                double jsonEmMbFormatado = Math.Round(jsonEmMB, 2);

                Console.WriteLine($"\nTamanho do arquivo JSON: {jsonEmMbFormatado}MB");
            }
            else if (File.Exists(pasta) && tamanho > limiteComTolerancia)
            {
                Console.WriteLine($"\nO arquivo JSON excede o limite de {limiteComTolerancia}MB.");
            }
        }
    }
}
