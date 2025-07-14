using System.Windows.Controls;

namespace SistemaZero.Views.ItemsUnicos
{
    public partial class RadioEsquerdaDireita : UserControl
    {
        public RadioEsquerdaDireita()
        {
            InitializeComponent();
        }

        public bool getEscolha()
        {
            if (rbEsquerda.IsChecked == true) return false;
            else return true;
        }

        public void setLado(bool lado)
        {
            if (lado)
            {
                rbDireita.IsChecked = true;
            }else
                rbEsquerda.IsChecked = true;
        }
    }
}
