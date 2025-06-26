namespace Desafio_01
{
    public class GeradorListaAlfanumerica
    {
        public List<ParametrosJSON> GerarListaAlfanumerica(int tamanhoMB)
        {
            GeradorStringAlfanumerico geradorStringAlfanumerico = new();

            var parametrosJsonLoop = new List<ParametrosJSON>();
            int loopEquivalente1MB = 11655;
            int quantidadeLoop = tamanhoMB * loopEquivalente1MB;
            for (int i = 0; i < quantidadeLoop; i++)
            {
                parametrosJsonLoop.Add(new ParametrosJSON
                {
                    A = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                    B = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                    C = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
                    D = geradorStringAlfanumerico.GetAlfanumericoAleatoria()
                });
            }
            return parametrosJsonLoop;
        }
    }
}
