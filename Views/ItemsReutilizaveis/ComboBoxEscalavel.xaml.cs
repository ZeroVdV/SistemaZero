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

        private readonly bool PodeApagar;
        private bool IsEditingMode => !PodeApagar;

        private ObservableCollection<Item> Items = new();
        public string Titulo { get; private set; }
        public string Placeholder { get; private set; }
        public bool Edit = true;

        public ComboBoxEscalavel(string titulo, string placeholder, bool podeApagar)
        {
            InitializeComponent();
            Placeholder = placeholder;
            Titulo = titulo;
            PodeApagar = podeApagar;
            InicializarItems(new ObservableCollection<Item>());
        }

        public ComboBoxEscalavel(string titulo, string placeholder, ObservableCollection<Item> items, bool edit, bool podeApagar)
        {
            InitializeComponent();
            Placeholder = placeholder;
            Titulo = titulo;
            Edit = edit;
            PodeApagar = podeApagar;
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
            cmBox.SelectedItem = null;
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

            if (IsEditingMode && cmBox.SelectedItem is Item itemSelecionado && itemSelecionado.ID != null)
            {
                itemSelecionado.Text = texto.Text.Trim();
                itemSelecionado.Erased = true; //no caso aqui estou reutilizando o erased como edição
                cmBox.ItemsSource = null;
                cmBox.ItemsSource = Items;
                cmBox.SelectedItem = itemSelecionado;
            }
            else
            {
                var novoItem = new Item(null, texto.Text.Trim(), false);
                Items.Add(novoItem);
                cmBox.SelectedItem = novoItem;
            }

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
                btn_Restaurar.Content = IsEditingMode ? "✏️" : " ↩️ ";
                if (IsEditingMode && item.ID != null)
                {
                    btn_Delete.Visibility = Visibility.Collapsed;
                    btn_Restaurar.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Delete.Visibility = item.Erased ? Visibility.Collapsed : Visibility.Visible;
                    btn_Restaurar.Visibility = item.Erased ? Visibility.Visible : Visibility.Collapsed;
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
                if (IsEditingMode)
                {
                    texto.Text = item.Text;
                    AlternarVisibilidade(edicao: true);
                }
                else if (item.ID != null && item.Erased)
                {
                    item.Erased = false;
                    btn_Delete.Visibility = Visibility.Visible;
                    btn_Restaurar.Visibility = Visibility.Collapsed;
                    cmBox.Items.Refresh();
                }
            }
        }


        public ObservableCollection<Item> GetItems()
        {
            return Items;
        }

        public List<int> GetErasedItems()
        {
            return Items
                .Where(item => item.Erased && item.ID != null)
                .Select(item => item.ID!.Value)
                .ToList();
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

        public List<(int? Id, string Nome)> GetItensEditados()
        {
            return Items
                .Where(item => item.Erased && item.ID != null)
                .Select(item => (item.ID, item.Text ?? string.Empty))
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
            Items.Clear();
        }
    }
}
