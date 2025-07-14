using SistemaZero.Model.Entity;
using HandyControl.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SistemaZero.Views.ItemsReutilizaveis
{
    public partial class ComboBoxEscalavel : UserControl
    {
        public class Item
        {
            public int? ID { get; set; }
            public string? Text { get; set; }
            public bool Erased { get; set; }

            public Item(int? id, string? text, bool erased)
            {
                ID = id;
                Text = text;
                Erased = erased;
            }

            public override string ToString() => Text ?? string.Empty;
        }

        private ObservableCollection<Item> Items = new();
        public string Titulo { get; private set; }
        public string Placeholder { get; private set; }
        public bool Edit { get; private set; }

        public ComboBoxEscalavel(string titulo, string placeholder)
        {
            InitializeComponent();
            Placeholder = placeholder;
            Titulo = titulo;
            InicializarItems(new ObservableCollection<Item>());
        }

        public ComboBoxEscalavel(string titulo, string placeholder, ObservableCollection<Item> items, bool edit)
        {
            InitializeComponent();
            Placeholder = placeholder;
            Titulo = titulo;
            Edit = edit;
            btn_Add.Visibility = edit ? Visibility.Visible : Visibility.Collapsed;
            InicializarItems(items);
        }

        private void InicializarItems(ObservableCollection<Item> items)
        {
            InfoElement.SetTitle(cmBox, Titulo);
            InfoElement.SetTitle(texto, Titulo);
            InfoElement.SetPlaceholder(texto, Placeholder);

            this.Items = items ?? new ObservableCollection<Item>();
            cmBox.ItemsSource = this.Items;
            if (this.Items.Count > 0)
                cmBox.SelectedIndex = 0;
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            AlternarVisibilidade(edicao: true);
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (cmBox.SelectedItem is Item item)
            {
                if (item.ID == null)
                    Items.Remove(item);
                else {
                    item.Erased = true;
                    btn_Delete.Visibility = Visibility.Collapsed;
                    btn_Restaurar.Visibility = Visibility.Visible;
                }
                
                cmBox.Items.Refresh();
            }
        }

        private void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(texto.Text))
                return;

            if (Items.Any(i => string.Equals(i.Text, texto.Text, StringComparison.OrdinalIgnoreCase)))
            {
                ExibirAviso("Item já existe");
                return;
            }

            var novoItem = new Item(null, texto.Text.Trim(), false);
            Items.Add(novoItem);
            cmBox.SelectedItem = novoItem;
            texto.Text = string.Empty;
            AlternarVisibilidade(edicao: false);
            LimparAviso();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            texto.Text = string.Empty;
            AlternarVisibilidade(edicao: false);
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

        private void cmBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Edit) return;
            if (cmBox.SelectedItem is Item item)
            {
                if (item.Erased)
                {
                    btn_Delete.Visibility = Visibility.Collapsed;
                    btn_Restaurar.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Delete.Visibility = Visibility.Visible;
                    btn_Restaurar.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                btn_Delete.Visibility = Visibility.Collapsed;
                btn_Restaurar.Visibility = Visibility.Collapsed;
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

        private void btn_Restaurar_Click(object sender, RoutedEventArgs e)
        {
            if (cmBox.SelectedItem is Item item)
            {
                if (item.ID != null && item.Erased)
                    item.Erased = false;
                btn_Delete.Visibility = Visibility.Visible;
                btn_Restaurar.Visibility = Visibility.Collapsed;
                cmBox.Items.Refresh();
            }
        }

        public ObservableCollection<Item> GetItems()
        {
            return Items;
        }

        public List<Codigo_Adicional> GetCodigosAdicionais()
        {
            return Items
                .Where(item => !item.Erased)
                .Select(item => new Codigo_Adicional
                {
                    Id = item.ID,
                    Codigo = item.Text
                })
                .ToList();
        }

        public string ObterTexto()
        {
            return cmBox.SelectedItem is Item item ? item.Text ?? string.Empty : string.Empty;
        }

        public void Selecionar(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                cmBox.SelectedItem = null;
                return;
            }

            var item = Items.FirstOrDefault(i =>
                string.Equals(i.Text, texto, StringComparison.OrdinalIgnoreCase));

            if (item != null)
                cmBox.SelectedItem = item;
        }

        public void limpar()
        {
            cmBox.SelectedItem = null;
        }
    }
}
