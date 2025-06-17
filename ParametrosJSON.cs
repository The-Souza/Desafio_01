namespace Desafio_01
{
    public class ParametrosJSON
    {
        public string? letras { get; set; }
        public string? parametros { get; set; }
        
        public ParametrosJSON(string letras, string parametros) 
        {
            this.letras = letras;
            this.parametros = parametros;
        }
    }
}
