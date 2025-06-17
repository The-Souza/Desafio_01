namespace Desafio_01
{
    public class GeradorListaAlfanumerica
    {
        public List<ParametrosJSON> GerarListaAlfanumerica()
        {
            GeradorStringAlfanumerico geradorStringAlfanumerico = new();
            string listaParametroA = geradorStringAlfanumerico.GetAlfanumericoAleatoriaA();
            string listaParametroB = geradorStringAlfanumerico.GetAlfanumericoAleatoriaB();
            string listaParametroC = geradorStringAlfanumerico.GetAlfanumericoAleatoriaC();
            string listaParametroD = geradorStringAlfanumerico.GetAlfanumericoAleatoriaD();

            ParametrosJSON parametrosJsons = new(listaParametroA, listaParametroB, listaParametroC, listaParametroD);

            List<ParametrosJSON> parametrosJSON = new List<ParametrosJSON> { parametrosJsons };

            return parametrosJSON;
        }
    }
}