namespace Desafio_01
{
    public class Diretorio{
        private const string raiz = @"C:\Users\guilherme2000925\source\repos\Desafio_01";

        public string GetRaiz() {
            return raiz; 
        }
    }

    public class ParametrosJSON
    {
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
        public string? D { get; set; }
    }
}
