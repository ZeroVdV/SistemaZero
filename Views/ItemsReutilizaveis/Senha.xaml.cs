using HandyControl.Controls;
using System.Windows;
using System.Windows.Controls;

namespace SistemaZero.Views.ItemsReutilizaveis
{
    /// <summary>
    /// Interação lógica para Senha.xam
    /// </summary>
    public partial class Senha : UserControl
    {
        public string Titulo { get; private set; }
        public string Placeholder { get; private set; }
        public Senha(string titulo, string placeholder)
        {
            InitializeComponent();
            Placeholder = placeholder;
            Titulo = titulo;

            InfoElement.SetTitle(senha, Titulo);
            InfoElement.SetPlaceholder(senha, Placeholder);
        }

        private void ExibirAviso(string mensagem)
        {
            erro.Text = mensagem;
            erro.Visibility = Visibility.Visible;
        }

        private void LimparAviso()
        {
            erro.Visibility = Visibility.Collapsed;
        }

        public bool ValidarSenha()
        {
            if (string.IsNullOrWhiteSpace(senha.Password))
            {
                ExibirAviso("Preencha esse campo!");
                return false;
            }
            LimparAviso();
            return true;
        }

        public string GetSenha()
        {
            return senha.Password;
        }

        public void limparSenha()
        {
            senha.Clear();
            LimparAviso();
        }
    }
}
