using SistemaZero.Controller;
using SistemaZero.Model.Entity;
using HandyControl.Controls;
using System.Windows;
using System.Windows.Controls;

namespace SistemaZero.Views.Produtos
{
    public partial class LogProduto : UserControl
    {
        private readonly ProdutoController controller = new();
        private List<Log_Produto> listaLogs = new();
        private int ultimoId = 0;

        public LogProduto()
        {
            InitializeComponent();
            cbQuantidade.SelectedIndex = 0;
            cbTipo.SelectedIndex = 0;
            BuscarLogs();
        }

        private void btnContinuar_Click(object sender, RoutedEventArgs e)
        {
            BuscarLogs();
        }

        private void BuscarLogs()
        {
            int quantidade = ObterQuantidadeSelecionada();
            int? tipo = ObterTipoSelecionado();
            string termo = barraPesquisa.Text.Trim();

            int qtdConsulta = quantidade > 0 ? quantidade + 1 : -1;
            var novosLogs = controller.BuscarLogsProduto(ultimoId, qtdConsulta, termo, tipo);

            if (novosLogs.Count == 0)
            {
                Growl.Info("Nenhum novo log encontrado.", "MessageTk");
                return;
            }

            bool temMais = quantidade > 0 && novosLogs.Count > quantidade;

            if (temMais)
                novosLogs.RemoveAt(novosLogs.Count - 1);

            listaLogs.AddRange(novosLogs);
            dgLogs.ItemsSource = null;
            dgLogs.ItemsSource = listaLogs;

            ultimoId = listaLogs[^1].ID;
            btnContinuar.Visibility = temMais ? Visibility.Visible : Visibility.Collapsed;
        }


        private int ObterQuantidadeSelecionada()
        {
            string? content = (cbQuantidade.SelectedItem as ComboBoxItem)?.Content.ToString();
            return content == "Todos" ? -1 : int.Parse(content ?? "10");
        }

        private int? ObterTipoSelecionado()
        {
            ComboBoxItem? item = cbTipo.SelectedItem as ComboBoxItem;
            if (item?.Tag?.ToString() == "-1")
                return null;

            return int.Parse(item?.Tag?.ToString() ?? "0");
        }

        private void barraPesquisa_SearchStarted(object sender, HandyControl.Data.FunctionEventArgs<string> e)
        {
            ultimoId = 0;
            listaLogs.Clear();
            BuscarLogs();
        }
    }
}
