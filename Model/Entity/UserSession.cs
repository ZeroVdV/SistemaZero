namespace SistemaZero.Model.Entity
{
    class UserSession
    {
        public static User? atual;

        public static void Iniciar(User? usuario)
        {
            atual = usuario;
        }
        public static void Encerrar()
        {
            atual = null;
        }
    }
}
