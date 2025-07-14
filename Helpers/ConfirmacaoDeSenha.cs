using SistemaZero.Views.ItemsReutilizaveis;

namespace SistemaZero.Helpers
{
    public static class ConfirmacaoDeSenha
    {
        public static bool? SolicitarConfirmacao()
        {
            var janela = new ConfirmarComSenha();
            bool? resultado = null;

            try
            {
                janela.ShowDialog();
                resultado = janela.SenhaCorreta;
            }
            catch
            {
                throw;
            }

            return resultado;
        }
    }
}
