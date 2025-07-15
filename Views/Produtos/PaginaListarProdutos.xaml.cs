using SistemaZero.Controller;
using SistemaZero.Model.Entity;
using HandyControl.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SistemaZero.Views.Produtos
{
    public partial class PaginaListarProdutos : UserControl
    {
        private bool _isLoaded = false;
        private List<Produto>? produtos = new();
        private readonly ProdutoController controller = new();

        private string? tipoPesquisaAtual;
        private string? termoAtual;
        private int? categoriaAtual;
        private int? estoqueAtual;
        private int? ruaAtual;
        private int? predioAtual;
        private int? nivelAtual;
        private bool? ladoAtual;

        private int ultimoId = 0;
        private int qtdRetorno => ObterQtdRetorno();

        public PaginaListarProdutos()
        {
            InitializeComponent();
            dgProdutos.RowHeight = double.NaN;
            this.IsVisibleChanged += PaginaListagemProdutos_IsVisibleChanged;
        }

        private void PaginaListarProdutos_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isLoaded) return;

            tipoPesquisaAtual = "Todos";
            var categorias = controller.ListarCategorias();
            cmbCategorias.ItemsSource = categorias;
            cmbCategorias.DisplayMemberPath = "Nome";
            cmbCategorias.SelectedValuePath = "Id";

            var estoques = controller.ListarEstoques();
            EstLocal.ItemsSource = estoques;
            EstLocal.DisplayMemberPath = "Locais.Nome";
            EstLocal.SelectedValuePath = "Locais.ID";

            _isLoaded = true;
            AtualizarDataGrid(ExecutarPesquisa(ultimoId, qtdRetorno), limparLista: true);
        }

        private void PaginaListagemProdutos_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && _isLoaded)
            {
                AtualizarItems();
            }
        }

        private MainWindow? ObterMainWindow() => Application.Current.MainWindow as MainWindow;

        private void PagAddProd_Click(object sender, RoutedEventArgs e)
        {
            ObterMainWindow()?.NavegarPara(new PaginaProduto(), "Adicionar Produto");
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Produto produto)
                ObterMainWindow()?.NavegarPara(new PaginaProduto(produto), "Editar Produto");
        }

        private void cmbPesquisa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLoaded || cmbPesquisa.SelectedItem is not ComboBoxItem selectedItem)
                return;

            string? selected = selectedItem.Content?.ToString();
            if (selected == null) return;

            sbPesquisa.Visibility = selected == "Categoria" || selected == "Estoque" || selected == "Todos" ? Visibility.Collapsed : Visibility.Visible;
            btnPesquisar.Visibility = selected == "Todos" ? Visibility.Visible : Visibility.Collapsed;
            gdCategorias.Visibility = selected == "Categoria" ? Visibility.Visible : Visibility.Collapsed;
            gdEstoques.Visibility = selected == "Estoque" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void EstEsq_Checked(object sender, RoutedEventArgs e)
        {
            if (EstDir.IsChecked == true)
                EstDir.IsChecked = false;
        }

        private void EstDir_Checked(object sender, RoutedEventArgs e)
        {
            if (EstEsq.IsChecked == true)
                EstEsq.IsChecked = false;
        }

        private void Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            ultimoId = 0;
            tipoPesquisaAtual = (cmbPesquisa.SelectedItem as ComboBoxItem)?.Content?.ToString();
            termoAtual = sbPesquisa.Text?.Trim();
            categoriaAtual = cmbCategorias.SelectedValue as int?;
            estoqueAtual = ObterIdEstoqueSelecionado();
            ruaAtual = int.TryParse(EstRua.Text, out var r) ? r : null;
            predioAtual = int.TryParse(EstPredio.Text, out var p) ? p : null;
            nivelAtual = int.TryParse(EstNivel.Text, out var n) ? n : null;
            //esquerda = false | direita = true | nenhum = null;
            ladoAtual = EstEsq.IsChecked == true ? false : EstDir.IsChecked == true ? true : null;

            produtos = ExecutarPesquisa(ultimoId, qtdRetorno);
            AtualizarDataGrid(produtos, limparLista: true);
        }

        private void GrowlMessage(string txt)
        {
            Growl.Clear("MessageTk");
            Growl.Info(txt, "MessageTk");
        }

        private void AtualizarDataGrid(List<Produto>? novos, bool limparLista = false)
        {
            if (novos == null || novos.Count == 0)
            {
                GrowlMessage("Nenhum produto encontrado.");
                btnContinuar.IsEnabled = false;
                return;
            }

            var exibidos = qtdRetorno > 0 ? novos.Take(qtdRetorno).ToList() : novos;

            if (limparLista)
                produtos = new List<Produto>();

            produtos ??= new List<Produto>();
            produtos.AddRange(exibidos);

            dgProdutos.ItemsSource = null;
            dgProdutos.ItemsSource = produtos;

            // Atualiza o ultimoId apenas com base nos itens exibidos
            ultimoId = produtos?.Any() == true ? produtos.Max(p => p.ID) : 0;

            // Ativa e desativa os botões auxiliares da pesquisa
            btnContinuar.IsEnabled = novos.Count > qtdRetorno && qtdRetorno != -1;
            btnVoltar.IsEnabled = produtos != null && produtos.Count > qtdRetorno && qtdRetorno != -1;
        }

        private List<Produto>? ExecutarPesquisa(int idAtual, int qtdRetorno)
        {
            qtdRetorno++;
            return tipoPesquisaAtual switch
            {
                "Descricao" => controller.PesquisarPorDescricao(idAtual, qtdRetorno, termoAtual!),
                "Categoria" => categoriaAtual.HasValue ? controller.PesquisarPorCategoria(idAtual, qtdRetorno, categoriaAtual.Value) : new(),
                "Código" => controller.PesquisarPorCodigoProduto(idAtual, qtdRetorno, termoAtual!),
                "Código Adicional" => controller.PesquisarPorCodigoAdicional(idAtual, qtdRetorno, termoAtual!),
                "Estoque" => controller.PesquisarPorLocal(idAtual, qtdRetorno, estoqueAtual, ruaAtual, predioAtual, nivelAtual, ladoAtual),
                "Todos" => controller.GetAllProdutos(idAtual, qtdRetorno),
                _ => new()
            };
        }

        private int ObterQtdRetorno()
        {
            string? valor = (cmbQtdSaidas.SelectedItem as ComboBoxItem)?.Content?.ToString()?.ToLower();
            return valor switch
            {
                "1" => 1,
                "5" => 5,
                "10" => 10,
                "20" => 20,
                "50" => 50,
                "Todos" => -1,
                _ => 20
            };
        }

        private void SomenteNumeros_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private int? ObterIdEstoqueSelecionado()
        {
            //É feita uma pesquisa por texto, caso o usuario não queira pesquisar pelo estoque. É melhor do que colocar uma seleção nula no combobox
            string textoDigitado = EstLocal.Text?.Trim() ?? "";
            var itemCorrespondente = EstLocal.Items
                .OfType<Estoque>()
                .FirstOrDefault(e => e.Locais?.Nome?.Equals(textoDigitado, StringComparison.OrdinalIgnoreCase) == true);
            return itemCorrespondente?.Locais?.ID;
        }

        private void Att_Click(object sender, RoutedEventArgs e)
        {
            AtualizarItems();
            Growl.Clear("MessageTk");
            Growl.Info("Produtos Atualizados!", "MessageTk");
        }

        private void AtualizarItems()
        {
            if (tipoPesquisaAtual == null) tipoPesquisaAtual = "Todos";
            int qtd = produtos?.Count ?? 0;
            var atualizados = ExecutarPesquisa(0, qtd);
            produtos = atualizados?.Where(p => p.ID <= ultimoId).ToList() ?? new List<Produto>();
            dgProdutos.ItemsSource = null;
            dgProdutos.ItemsSource = produtos;
        }

        private void btnContinuar_Click(object sender, RoutedEventArgs e)
        {
            if (tipoPesquisaAtual == null)
            {
                GrowlMessage("Nenhuma pesquisa foi realizada ainda.");
                return;
            }

            var novos = ExecutarPesquisa(ultimoId, qtdRetorno);
            AtualizarDataGrid(novos);
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            if (qtdRetorno == 0) return;
            if (produtos == null || produtos.Count <= qtdRetorno) return;

            //remove os ultimos produtos da lista
            produtos = produtos.Take(produtos.Count - qtdRetorno).ToList();

            dgProdutos.ItemsSource = null;
            dgProdutos.ItemsSource = produtos;

            if (produtos.Count > 0)
                ultimoId = produtos.Max(p => p.ID);
            else
                ultimoId = 0;

            btnContinuar.IsEnabled = true;
            btnVoltar.IsEnabled = produtos != null && produtos.Count > qtdRetorno && qtdRetorno != -1;
        }

        private void cmbQtdSaidas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLoaded) return;

            btnVoltar.IsEnabled = produtos != null && produtos.Count > qtdRetorno && qtdRetorno != -1;
        }
    }
}