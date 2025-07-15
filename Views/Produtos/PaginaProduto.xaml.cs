using SistemaZero.Controller;
using SistemaZero.Helpers;
using SistemaZero.Model.Entity;
using SistemaZero.Views.ItemsReutilizaveis;
using SistemaZero.Views.ItemsUnicos;
using HandyControl.Controls;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SistemaZero.Views.Produtos
{
    public partial class PaginaProduto : UserControl
    {
        private Texto campoDescricao;
        private Texto campoCodigoProduto;
        private ComboBoxEscalavel campoCodigoAdicional;
        private ControleEstoque campoEstoques;
        private ControleCategorias campoCategorias;
        private SeletorDeImagemUnica seletorImg;
        private Produto produto = new();
        private readonly ProdutoController PController = new();
        private readonly UserController userController = new();
        private bool permissao = false;

        public PaginaProduto()
        {
            permissao = userController.VerificarPermissao();
            InitializeComponent();
            inicializarCampos();
            inicializar();
        }

        internal PaginaProduto(Produto produto)
        {
            this.produto = produto;
            permissao = userController.VerificarPermissao();

            InitializeComponent();
            inicializarCamposComProduto();
            inicializar();
        }

        private void inicializarCampos()
        {
            campoDescricao = new Texto("Descrição do Produto", "Produto Exemplo", true);
            campoCodigoProduto = new Texto("Codigo do Produto", "0000-0000-0000", true);
            campoCodigoAdicional = new ComboBoxEscalavel("Codigos Adicionais do Produto", "0000-0000-0000", true);
            campoEstoques = new ControleEstoque();
            campoCategorias = new ControleCategorias(permissao);
            seletorImg = new SeletorDeImagemUnica();
            btnSalvar.Visibility = Visibility.Visible;
            btnAtualizar.Visibility = Visibility.Collapsed;
        }

        private void inicializarCamposComProduto()
        {
            btnSalvar.Visibility = Visibility.Collapsed;
            btnAtualizar.Visibility = Visibility.Visible;
            campoDescricao = new Texto("Descrição do Produto", "Produto Exemplo", produto.Descricao!, permissao, true);
            campoCodigoProduto = new Texto("Codigo do Produto", "0000-0000-0000", produto.CodigoProduto!, permissao, true);

            var codigosAdicionais = new ObservableCollection<ComboBoxEscalavel.Item>();
            if (produto.Codigos_Adicionais != null)
            {
                foreach (var cod in produto.Codigos_Adicionais)
                {
                    codigosAdicionais.Add(new ComboBoxEscalavel.Item(cod.Id, cod.Codigo, false));
                }
            }
            campoCodigoAdicional = new ComboBoxEscalavel("Codigos Adicionais do Produto", "0000-0000-0000", codigosAdicionais, permissao, true);
            campoEstoques = new ControleEstoque(produto.Estoques ?? new List<Estoque>(), permissao);
            campoCategorias = new ControleCategorias(produto.Categoria!, permissao);
            try
            {
                string? url = produto.Imagem_Estoque?.Url;
                string? caminho = !string.IsNullOrEmpty(url) ? PController.GetImagem(url) : null;

                seletorImg = new SeletorDeImagemUnica(!string.IsNullOrEmpty(caminho) ? caminho : null, permissao);
            }
            catch (Exception ex)
            {
                Growl.Error($"Erro ao carregar imagem: {PController.ObterMensagemErro(ex)}", "MessageTk");
                seletorImg = new SeletorDeImagemUnica(null, permissao);
            }
        }

        private void inicializar()
        {
            gdDescricao.Children.Add(campoDescricao);
            gdCodigoProduto.Children.Add(campoCodigoProduto);
            gdCodigoAdicional.Children.Add(campoCodigoAdicional);
            gdEstoques.Children.Add(campoEstoques);
            gdCategorias.Children.Add(campoCategorias);
            gdImgSelector.Children.Add(seletorImg);
        }

        private bool ValidarImagem()
        {
            string? url = seletorImg.GetUrl();

            if ((!string.IsNullOrWhiteSpace(url) && File.Exists(url)) || !permissao)
                return true;

            var confirmacao = new ConfirmacaoSimNao("Deseja enviar sem Imagem?");
            confirmacao.ShowDialog();

            if (confirmacao.Confirmado)
                return true;

            Growl.Error("Escolha uma Imagem.", "MessageTk");
            return false;
        }

        private bool verificarForms()
        {
            return campoDescricao.ValidarTexto()
                && campoCodigoProduto.ValidarTexto()
                && campoCategorias.ValidarCategoria()
                && ValidarImagem();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            SalvarProduto(true);
        }

        private void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            SalvarProduto(false);
        }

        private void SalvarProduto(bool novo)
        {
            Growl.Clear("MessageTk");

            if (!verificarForms())
            {
                Growl.Error("Preencha todos os campos obrigatórios.", "MessageTk");
                return;
            }

            try
            {
                bool? resultadoSenha = ConfirmacaoDeSenha.SolicitarConfirmacao();

                if (resultadoSenha == null)
                {
                    Growl.Info("Operação cancelada pelo usuário.", "MessageTk");
                    return;
                }
                else if (resultadoSenha == false)
                {
                    Growl.Warning("Senha incorreta. Operação cancelada.", "MessageTk");
                    return;
                }

                string? caminhoImagem = seletorImg.GetUrl();
                string? nomeImagem = !string.IsNullOrWhiteSpace(caminhoImagem) ? Path.GetFileName(caminhoImagem) : null;

                var novaImagem = new Imagem
                {
                    Id = null,
                    Url = nomeImagem
                };

                if (novo)
                    produto = new Produto();

                produto.Descricao = campoDescricao.GetTexto();
                produto.CodigoProduto = campoCodigoProduto.GetTexto();
                produto.Categoria = campoCategorias.GetCategoria();
                produto.Estoques = campoEstoques.GetEstoques();
                produto.Codigos_Adicionais = campoCodigoAdicional.GetCodigosAdicionais();

                string? antigaImagem = produto.Imagem_Estoque?.Url;
                bool imagemAlterada = novo || antigaImagem != novaImagem.Url;

                if (imagemAlterada)
                    produto.Imagem_Estoque = novaImagem;

                campoEstoques.AtualizarLocaisEstoques();

                if (novo)
                {
                    if (!string.IsNullOrWhiteSpace(caminhoImagem))
                    {
                        string nomeImagemSalva = PController.SalvarImagem(caminhoImagem);
                        produto.Imagem_Estoque!.Url = nomeImagemSalva;
                    }

                    PController.AdicionarProduto(produto, userController.GetIdUsuario());
                    Growl.Success("Produto salvo com sucesso!", "MessageTk");
                    limparCampos();
                }
                else
                {
                    if (imagemAlterada && !string.IsNullOrWhiteSpace(caminhoImagem))
                    {
                        string nomeImagemSalva = PController.SalvarImagem(caminhoImagem);
                        produto.Imagem_Estoque!.Url = nomeImagemSalva;

                        if (!string.IsNullOrWhiteSpace(antigaImagem))
                            PController.DeletarImagem(antigaImagem);
                    }

                    PController.EditarProduto(produto, userController.GetIdUsuario());

                    //apagar codigos adicionais
                    List<int>? codApagados = campoCodigoAdicional.GetErasedItems();
                    if (codApagados != null)
                        foreach (int cod in codApagados)
                        {
                            PController.DeletarCodigoAdicional(cod);
                        }

                    //apagar o estoques
                    campoEstoques.apagarEstoques();

                    if (Application.Current.MainWindow is MainWindow main)
                    {
                        main.Voltar(false);
                        Growl.Success("Produto atualizado com sucesso!", "MessageTk");
                    }
                }
            }
            catch (Exception ex)
            {
                string mensagem = PController.ObterMensagemErro(ex);
                Growl.Error($"Erro ao salvar o produto: {mensagem}", "MessageTk");
            }
        }


        private void limparCampos()
        {
            campoDescricao.limparTexto();
            campoCodigoProduto.limparTexto();
            campoCategorias.limpar();
            campoCodigoAdicional.limpar();
            campoEstoques.limparEstoques();
            seletorImg.ClearImg();
            produto = new();
        }
    }
}
