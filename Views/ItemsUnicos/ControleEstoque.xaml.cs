using SistemaZero.Controller;
using SistemaZero.Model.Entity;
using SistemaZero.Views.ItemsReutilizaveis;
using HandyControl.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using static SistemaZero.Views.ItemsReutilizaveis.ComboBoxEscalavel;

namespace SistemaZero.Views.ItemsUnicos
{
    public partial class ControleEstoque : UserControl
    {
        public ObservableCollection<Estoque> Estoques { get; set; } = new();
        public List<int> idApagados = new();

        public ComboBoxEscalavel campoNome { get; set; }
        public RadioEsquerdaDireita campoLado { get; set; }
        public Numero campoRua { get; set; }
        public Numero campoPredio { get; set; }
        public Numero campoNivel { get; set; }
        public Numero campoQuantidade { get; set; }

        private readonly ProdutoController controller = new();

        private bool editar = false;
        private bool permissao = true;
        private Estoque? estoqueEmEdicao = null;
        private Estoque? estoqueSelecionado = null;

        public ControleEstoque() : this(new List<Estoque>(), true) { }

        public ControleEstoque(List<Estoque> estoques, bool edit)
        {
            permissao = edit;
            Estoques = new ObservableCollection<Estoque>(estoques);
            InitializeComponent();
            InicializarFormulario();
            dgEstoques.ItemsSource = Estoques;
            AtualizarVisibilidadeEditarRemover();
        }

        private void AtualizarVisibilidadeEditarRemover()
        {
            var vis = permissao ? Visibility.Visible : Visibility.Collapsed;
            dgEstoques.Columns[5].Visibility = vis;
            dgEstoques.Columns[6].Visibility = vis;
            btnNovoEst.Visibility = vis;
        }

        private void InicializarFormulario()
        {
            var locais = controller.ListarEstoques();

            var itens = new ObservableCollection<Item>(
                locais.Where(e => e.Locais?.Nome != null)
                      .Select(e => new Item(e.Locais!.ID, e.Locais.Nome, false))
            );

            campoNome = new ComboBoxEscalavel("Lugares", "Novo Estoque", itens, edit: true, false);
            campoLado = new();
            campoRua = new("Rua", 0, 99);
            campoPredio = new("Predio", 0, 99);
            campoNivel = new("Nivel", 0, 99);
            campoQuantidade = new("Quantia", 0, 999);

            itemCmbNomes.Children.Add(campoNome);
            itemLado.Children.Add(campoLado);
            itemNumRua.Children.Add(campoRua);
            itemNumPredio.Children.Add(campoPredio);
            itemNumNivel.Children.Add(campoNivel);
            itemQuantidade.Children.Add(campoQuantidade);

            AtualizarTotal();
        }

        private void BtnAdicionarLinha_Click(object sender, RoutedEventArgs e)
        {
            if (!permissao) return;
            MostrarFormulario();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (!permissao) return;

            if (editar && estoqueEmEdicao != null)
            {
                AtualizarEstoqueComCampos(estoqueEmEdicao);
            }
            else
            {
                Estoques.Add(CriarEstoqueNovo());
            }

            AtualizarGrid();
            FinalizarEdicao();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) => FinalizarEdicao();

        private Estoque CriarEstoqueNovo() => new Estoque
        {
            Locais = new LocaisEstoque
            {
                ID = ObterIdEstoqueSelecionado(),
                Nome = campoNome.ObterTexto()
            },
            Rua = campoRua.ObterValor(),
            Predio = campoPredio.ObterValor(),
            Nivel = campoNivel.ObterValor(),
            Lado = campoLado.getEscolha(),
            Quantidade = 0
        };

        private void AtualizarEstoqueComCampos(Estoque estoque)
        {
            estoque.Locais!.Nome = campoNome.ObterTexto();
            estoque.Rua = campoRua.ObterValor();
            estoque.Predio = campoPredio.ObterValor();
            estoque.Nivel = campoNivel.ObterValor();
            estoque.Lado = campoLado.getEscolha();
        }

        private void FinalizarEdicao()
        {
            limparCampos();
            editar = false;
            estoqueEmEdicao = null;
            OcultarFormulario();
            AtualizarTotal();
        }

        private void AtualizarGrid() => dgEstoques.Items.Refresh();

        public void apagarEstoques()
        {
            try
            {
                foreach (var id in idApagados)
                {
                    var estoque = Estoques.FirstOrDefault(e => e.ID == id);
                    if (estoque != null) controller.DeletarEstoque(estoque);
                }
            }
            catch
            {
                Growl.Error("Erro ao apagar o Estoque.", "MessageTk");
            }
        }

        public void AtualizarLocaisEstoques()
        {
            try
            {
                AtualizarOuCriarLocais();
                AtualizarIdsLocaisEstoque();
            }
            catch
            {
                Growl.Error("Erro ao atualizar os locais de Estoque.", "MessageTk");
            }
        }

        private void AtualizarOuCriarLocais()
        {
            var todosItens = campoNome.GetItems();

            foreach (var item in todosItens)
            {
                if (item.ID == null)
                    item.ID = controller.AdicionarLocalEstoque(item.Text ?? string.Empty);
                else if (item.Erased)
                    controller.EditarNomeEstoque(item.ID.Value, item.Text ?? string.Empty);
            }
        }

        private void AtualizarIdsLocaisEstoque()
        {
            var todosItens = campoNome.GetItems();

            foreach (var estoque in Estoques)
            {
                if (estoque.Locais == null || string.IsNullOrWhiteSpace(estoque.Locais.Nome)) continue;

                var item = todosItens.FirstOrDefault(i =>
                    string.Equals(i.Text?.Trim(), estoque.Locais.Nome?.Trim(), StringComparison.OrdinalIgnoreCase));

                if (item != null && (estoque.Locais.ID == null || estoque.Locais.ID != item.ID))
                    estoque.Locais.ID = item.ID;
            }
        }

        public List<Estoque> GetEstoques() =>
            Estoques.Where(est => est.ID == null || !idApagados.Contains(est.ID.Value)).ToList();

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (!permissao) return;

            if (sender is Button btn && btn.Tag is Estoque estoque)
            {
                editar = true;
                estoqueEmEdicao = estoque;

                campoNome.Selecionar(estoque.Locais!.Nome);
                campoRua.SetNumero(estoque.Rua);
                campoPredio.SetNumero(estoque.Predio);
                campoNivel.SetNumero(estoque.Nivel);
                campoLado.setLado(estoque.Lado);

                MostrarFormulario();
            }
        }

        private void BtnApagar_Click(object sender, RoutedEventArgs e)
        {
            if (!permissao) return;

            if (sender is Button btn && btn.Tag is Estoque estoque)
            {
                if (estoque.ID != null) AlternarIdRemocao((int)estoque.ID);
                else Estoques.Remove(estoque);

                dgEstoques.SelectedItem = null;
                erro.Visibility = Visibility.Collapsed;
                AtualizarTotal();
                AtualizarGrid();
            }
        }

        private void AlternarIdRemocao(int id)
        {
            if (idApagados.Contains(id)) idApagados.Remove(id);
            else idApagados.Add(id);
            AtualizarGrid();
        }

        private int? ObterIdEstoqueSelecionado()
        {
            string texto = campoNome.ObterTexto()?.Trim() ?? "";

            return campoNome.GetItems()
                .FirstOrDefault(i => string.Equals(i.Text?.Trim(), texto, StringComparison.OrdinalIgnoreCase))
                ?.ID;
        }

        private void BtnRemover_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Estoque estoque)
            {
                btn.Content = (estoque.ID != null && idApagados.Contains((int)estoque.ID)) ? "🔄️" : "🗑️";
            }
        }

        private void dgEstoques_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEstoques.SelectedItem is Estoque selecionado)
            {
                estoqueSelecionado = selecionado;
                campoQuantidade.SetNumero(0);
                gdCalculos.Visibility = Visibility.Visible;
                erro.Visibility = Visibility.Collapsed;
            }
            else
            {
                estoqueSelecionado = null;
                gdCalculos.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnAumentar_Click(object sender, RoutedEventArgs e)
        {
            if (estoqueSelecionado == null) return;

            estoqueSelecionado.Quantidade += campoQuantidade.ObterValor();
            erro.Visibility = Visibility.Collapsed;
            AtualizarGrid();
            AtualizarTotal();
        }

        private void BtnDiminuir_Click(object sender, RoutedEventArgs e)
        {
            if (estoqueSelecionado == null) return;

            int valor = campoQuantidade.ObterValor();

            if (estoqueSelecionado.Quantidade - valor < 0)
            {
                erro.Text = "Quantidade não pode ser negativa.";
                erro.Visibility = Visibility.Visible;
            }
            else
            {
                estoqueSelecionado.Quantidade -= valor;
                erro.Visibility = Visibility.Collapsed;
                AtualizarGrid();
                AtualizarTotal();
            }
        }

        private void AtualizarTotal()
        {
            int total = Estoques
                .Where(e => e.ID == null || !idApagados.Contains((int)e.ID))
                .Sum(e => e.Quantidade);

            txtTotal.Text = $"Total: {total}";
        }

        private void limparCampos()
        {
            campoNome.limpar();
            campoNivel.limpar();
            campoPredio.limpar();
            campoRua.limpar();
            // lado não precisa limpar
        }

        private void MostrarFormulario()
        {
            gdForms.Visibility = Visibility.Visible;
            dgEstoques.SelectedItem = null;
            dgEstoques.Visibility = Visibility.Collapsed;
            btnNovoEst.Visibility = Visibility.Collapsed;
            erro.Visibility = Visibility.Collapsed;
        }

        private void OcultarFormulario()
        {
            gdForms.Visibility = Visibility.Collapsed;
            dgEstoques.Visibility = Visibility.Visible;
            btnNovoEst.Visibility = Visibility.Visible;
        }

        public void limparEstoques()
        {
            Estoques.Clear();
            AtualizarTotal();
            limparCampos();
            estoqueEmEdicao = null;
            estoqueSelecionado = null;
            editar = false;
        }
    }

}
