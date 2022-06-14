namespace Catalog.Api
{
    public class Matematicas : IMatematicas
    {
        public int Multiplicar(int a, int b)
        {
            return a * b;
        }

        public int Sumar(int a, int b)
        {
            return a + b;
        }
    }
}
