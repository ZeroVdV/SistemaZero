using System.Windows;

namespace SistemaZero.Views.ItemsReutilizaveis
{
    public partial class ConfirmacaoSimNao : HandyControl.Controls.Window
    {
        public bool Confirmado { get; private set; }

        public ConfirmacaoSimNao(string txt)
        {
            InitializeComponent();
            txtConfirmacao.Text = txt;
        }

        private void Sim_Click(object sender, RoutedEventArgs e)
        {
            Confirmado = true;
            Close();
        }

        private void Nao_Click(object sender, RoutedEventArgs e)
        {
            Confirmado = false;
            Close();
        }
    }
}
