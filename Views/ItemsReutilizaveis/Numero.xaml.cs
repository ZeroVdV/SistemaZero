using HandyControl.Controls;
using System.Windows;
using System.Windows.Controls;

namespace SistemaZero.Views.ItemsReutilizaveis
{
    public partial class Numero : UserControl
    {
        public string Titulo { get; private set; }

        public Numero(string titulo, int? min = null, int? max = null)
        {
            InitializeComponent();
            Titulo = titulo;
            InfoElement.SetTitle(numero, Titulo);
            Configurar(min, max);
        }

        public Numero(string titulo, int valor, bool editar = true, int? min = null, int? max = null)
            : this(titulo, min, max)
        {
            numero.Value = valor;
            numero.IsEnabled = editar;
        }

        public void Configurar(int? min = null, int? max = null)
        {
            if (min.HasValue)
                numero.Minimum = min.Value;

            if (max.HasValue)
                numero.Maximum = max.Value;
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

        public bool ValidarNumero()
        {
            if ((int)numero.Value == 0)
            {
                ExibirAviso("Insira um valor!");
                return false;
            }

            LimparAviso();
            return true;
        }

        public void limpar()
        {
            numero.Value = 0;
        }

        public int ObterValor()
        {
            return (int)numero.Value;
        }

        public void SetNumero(int valor)
        {
            numero.Value = valor;
        }
    }
}
