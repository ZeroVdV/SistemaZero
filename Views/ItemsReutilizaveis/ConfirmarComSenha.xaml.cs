using SistemaZero.Model.Entity;
using SistemaZero.Controller;
using System.Windows;

namespace SistemaZero.Views.ItemsReutilizaveis
{
    public partial class ConfirmarComSenha : Window
    {
        private Senha campoSenha;
        private readonly UserController controller = new UserController();

        public bool? SenhaCorreta { get; private set; } = null;

        public ConfirmarComSenha()
        {
            InitializeComponent();
            campoSenha = new Senha("Digite sua senha:", "Senha de confirmação");
            SenhaContainer.Content = campoSenha;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (!campoSenha.ValidarSenha())
                return;

            int id = UserSession.atual?.ID ?? -1;
            if (id == -1)
            {
                Close();
                return;
            }

            try
            {
                if (controller.ConfirmarSenha(id, campoSenha.GetSenha()))
                {
                    SenhaCorreta = true;
                }
                else
                {
                    SenhaCorreta = false;
                }
            }
            catch
            {
                throw;
            }

            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            SenhaCorreta = null;
            Close();
        }
    }
}
