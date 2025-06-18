namespace Desafio_01
{
    public class GeradorListaAlfanumerica
    {
        public List<ParametrosJSON> GerarListaAlfanumerica()
        {
            GeradorStringAlfanumerico geradorStringAlfanumerico = new();
            List<ParametrosJSON> parametrosJSON = new();

            for (int i = 0; i < 4670000; i++)
            {
                string listaParametroA = geradorStringAlfanumerico.GetAlfanumericoAleatoriaA();
                string listaParametroB = geradorStringAlfanumerico.GetAlfanumericoAleatoriaB();
                string listaParametroC = geradorStringAlfanumerico.GetAlfanumericoAleatoriaC();
                string listaParametroD = geradorStringAlfanumerico.GetAlfanumericoAleatoriaD();

                ParametrosJSON parametrosJsons = new(listaParametroA, listaParametroB, listaParametroC, listaParametroD);

                parametrosJSON.Add(parametrosJsons);
            }
            
            return parametrosJSON;
        }
    }
}