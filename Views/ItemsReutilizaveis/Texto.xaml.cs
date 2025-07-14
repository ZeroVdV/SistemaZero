using HandyControl.Controls;
using System.Windows;
using System.Windows.Controls;

namespace SistemaZero.Views.ItemsReutilizaveis
{
    public partial class Texto : UserControl
    {
        public string Titulo { get; private set; }
        public string Placeholder { get; private set; }

        public Texto(string titulo, string placeholder, bool importante)
        {
            InitializeComponent();
            Placeholder = placeholder;
            Titulo = titulo;

            InfoElement.SetTitle(texto, Titulo);
            InfoElement.SetPlaceholder(texto, Placeholder);
            InfoElement.SetNecessary(texto, importante);
        }

        public Texto(string titulo, string placeholder, string texto, bool editar, bool importante) : this(titulo, placeholder, importante) // Primeiro construtor
        {
            this.texto.Text = texto;
            this.texto.IsReadOnly = !editar;
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

        public bool ValidarTexto()
        {
            if (string.IsNullOrWhiteSpace(texto.Text))
            {
                ExibirAviso("Preencha esse campo!");
                return false;
            }

            LimparAviso();
            return true;
        }

        public string GetTexto()
        {
            return texto.Text;
        }

        public void limparTexto()
        {
            texto.Clear();
        }
    }
}
