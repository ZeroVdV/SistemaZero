using HandyControl.Controls;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SistemaZero.Views.ItemsReutilizaveis
{
    public partial class Email : UserControl
    {
        public string Titulo { get; private set; }
        public string Placeholder { get; private set; }

        public Email(string titulo, string placeholder)
        {
            InitializeComponent();
            Placeholder = placeholder;
            Titulo = titulo;

            InfoElement.SetTitle(email, Titulo);
            InfoElement.SetPlaceholder(email, Placeholder);
        }

        public Email(string titulo, string placeholder, string email) : this(titulo, placeholder) {
            this.email.Text = email;
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

        public bool ValidarEmail()
        {
            if (string.IsNullOrWhiteSpace(email.Text))
            {
                ExibirAviso("Preencha esse campo!");
                return false;
            }

            if (!Regex.IsMatch(email.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            {
                ExibirAviso("Insira um email válido");
                return false;
            }

            LimparAviso();
            return true;
        }

        public string GetEmail()
        {
            return email.Text;
        }

        public void limparEmail()
        {
            email.Clear();
            LimparAviso();
        }
    }
}
