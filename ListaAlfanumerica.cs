namespace Desafio_01
{
    public class GeradorListaAlfanumerica
    {
        public GeradorListaAlfanumerica()
        {
            GeradorStringAlfanumerico geradorStringAlfanumerico = new();
            string listaParametroA = geradorStringAlfanumerico.GetAlfanumericoAleatoriaA();
            string listaParametroB = geradorStringAlfanumerico.GetAlfanumericoAleatoriaB();
            string listaParametroC = geradorStringAlfanumerico.GetAlfanumericoAleatoriaC();
            string listaParametroD = geradorStringAlfanumerico.GetAlfanumericoAleatoriaD();

            ParametrosJSON parametrosJsonA = new() { A = listaParametroA };
            ParametrosJSON parametrosJsonB = new() { B = listaParametroB };
            ParametrosJSON parametrosJsonC = new() { C = listaParametroC };
            ParametrosJSON parametrosJsonD = new() { D = listaParametroD };

            List<ParametrosJSON> parametrosJSON =
            [
                parametrosJsonA,
                parametrosJsonB,
                parametrosJsonC,
                parametrosJsonD,
            ];
        }
    }
}
