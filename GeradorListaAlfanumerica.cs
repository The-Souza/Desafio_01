namespace Desafio_01
{
    public class GeradorListaAlfanumerica
    {
        public List<ParametrosJSON> GerarListaAlfanumerica(int tamanhoMB)
        {
            GeradorStringAlfanumerico geradorStringAlfanumerico = new();
            List<ParametrosJSON> parametrosJSON = new();

            int loopEquivalente1MB = 11675;
            int quantidadeLoop = tamanhoMB * loopEquivalente1MB;
            for (int i = 0; i < quantidadeLoop; i++)
            {
                string listaParametroA = geradorStringAlfanumerico.GetAlfanumericoAleatoria();
                string listaParametroB = geradorStringAlfanumerico.GetAlfanumericoAleatoria();
                string listaParametroC = geradorStringAlfanumerico.GetAlfanumericoAleatoria();
                string listaParametroD = geradorStringAlfanumerico.GetAlfanumericoAleatoria();

                ParametrosJSON parametrosJsons = new(listaParametroA, listaParametroB, listaParametroC, listaParametroD);

                parametrosJSON.Add(parametrosJsons);
            }
            return parametrosJSON;
        }
    }
}