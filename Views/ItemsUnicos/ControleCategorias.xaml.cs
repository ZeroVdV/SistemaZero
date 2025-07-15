using SistemaZero.Controller;
using SistemaZero.Model.Entity;
using HandyControl.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SistemaZero.Views.ItemsUnicos
{
    public partial class ControleCategorias : UserControl
    {
        private readonly ProdutoController controller = new();
        private ObservableCollection<Categoria> Items;

        public ControleCategorias(bool permissao)
        {
            InitializeComponent();
            PreencherCategorias();
            cmBoxLinhas.ItemsSource = Linha.Todas;
            cmBoxLinhas.DisplayMemberPath = "Nome";
            cmBoxLinhas.SelectedValuePath = "Id";
            btn_Add.Visibility = permissao ? Visibility.Visible : Visibility.Collapsed;
        }

        public ControleCategorias(Categoria cat, bool edit) : this(edit)
        {
            cmBoxCategorias.Visibility = edit ? Visibility.Visible : Visibility.Collapsed;
            txtCategoria.Visibility = edit ? Visibility.Collapsed : Visibility.Visible;
            SelecionarCategoria(cat);
        }

        public void PreencherCategorias()
        {
            var categorias = controller.ListarCategorias();
            Items = new ObservableCollection<Categoria>(categorias);
            cmBoxCategorias.ItemsSource = Items;
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            AlternarVisibilidade(true);
        }

        private void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(texto.Text))
                return;

            if (cmBoxLinhas.SelectedItem == null)
            {
                ExibirAviso("Selecione a linha do Produto");
                return;
            }

            if (Items.Any(i => string.Equals(i.Nome, texto.Text, StringComparison.OrdinalIgnoreCase)))
            {
                ExibirAviso("Item já existe");
                return;
            }

            var novoItem = new Categoria
            {
                Id = null,
                Nome = texto.Text,
                Linha = ((Linha)cmBoxLinhas.SelectedItem).Nome
            };

            Items.Add(novoItem);
            cmBoxCategorias.SelectedItem = novoItem;

            limpar();
            AlternarVisibilidade(false);
            LimparAviso();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            limpar();
            AlternarVisibilidade(false);
            LimparAviso();
        }

        private void AlternarVisibilidade(bool edicao)
        {
            var fadeIn = (Storyboard)FindResource("FadeIn");
            var fadeOut = (Storyboard)FindResource("FadeOut");

            if (edicao)
            {
                fadeOut.Begin(gd_cmb);
                fadeIn.Begin(gd_tb);

                gd_cmb.Visibility = Visibility.Collapsed;
                gd_tb.Visibility = Visibility.Visible;
                texto.Focus();
            }
            else
            {
                fadeOut.Begin(gd_tb);
                fadeIn.Begin(gd_cmb);

                gd_tb.Visibility = Visibility.Collapsed;
                gd_cmb.Visibility = Visibility.Visible;
            }
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

        public bool ValidarCategoria()
        {
            return cmBoxCategorias.SelectedItem != null;
        }

        internal Categoria GetCategoria()
        {
            return (Categoria)cmBoxCategorias.SelectedItem;
        }

        public void Selecionar(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                cmBoxCategorias.SelectedItem = null;
                return;
            }

            var item = Items.FirstOrDefault(i =>
                string.Equals(i.Nome, texto.Trim(), StringComparison.OrdinalIgnoreCase));

            if (item != null)
            {
                cmBoxCategorias.SelectedItem = item;
                txtCategoria.Text = item.Nome;
            }
        }

        private void SelecionarCategoria(Categoria cat)
        {
            if (cat == null) return;

            var item = Items.FirstOrDefault(i =>
                (cat.Id != null && i.Id == cat.Id) ||
                string.Equals(i.Nome, cat.Nome, StringComparison.OrdinalIgnoreCase));

            if (item != null)
            {
                cmBoxCategorias.SelectedItem = item;
                txtCategoria.Text = item.Nome;
            }
        }

        public void limpar()
        {
            texto.Text = string.Empty;
            cmBoxCategorias.SelectedItem = null;
        }
    }
}
