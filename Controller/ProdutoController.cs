﻿using SistemaZero.Model;
using SistemaZero.Model.Entity;
using SistemaZero.Views.Produtos;

namespace SistemaZero.Controller
{
    class ProdutoController
    {
        private ConexaoBD conexao = new();
        private ServidorDeArquivos Serv_arquivos = new();
        public string ObterMensagemErro(Exception ex)
        {
            // Tenta pegar a exceção mais específica
            while (ex.InnerException != null)
                ex = ex.InnerException;

            return ex.Message;
        }

        public void AdicionarProduto(Produto produto, int userID)
        {
            if (produto.Categoria!.Id == null)
                produto.Categoria.Id = conexao.adicionarCategoria(produto.Categoria);

            produto.ID = conexao.adicionarProduto(produto);
            conexao.LogNovoProduto(userID, produto);
        }

        public int AdicionarLocalEstoque(string local)
        {
            return conexao.adicionarLocalEstoque(local);
        }

        public string SalvarImagem(string caminho)
        {
            return Serv_arquivos.SalvarImagemNoServidor(caminho);
        }

        public string GetImagem(string nome)
        {
            return Serv_arquivos.CarregarImagemDoServidor(nome);
        }

        public void DeletarImagem(string nome)
        {
            Serv_arquivos.deletarImg(nome);
        }

        public Produto? GetProduto(int id)
        {
            return conexao.obterProduto(id);
        }

        public List<Produto>? GetAllProdutos(int id_atual, int qtd_retorno)
        {
            return conexao.obterProdutos(id_atual, qtd_retorno);
        }

        public List<Produto>? PesquisarPorDescricao(int id_atual, int qtd_retorno, string descricao)
        {
            return conexao.PesquisarPorDescricao(id_atual, qtd_retorno, descricao);
        }

        public List<Produto>? PesquisarPorCategoria(int id_atual, int qtd_retorno, int categoriaId)
        {
            return conexao.PesquisarPorCategoria(id_atual, qtd_retorno, categoriaId);
        }

        public List<Produto>? PesquisarPorCodigoProduto(int id_atual, int qtd_retorno, string codigo)
        {
            return conexao.PesquisarPorCodigoProduto(id_atual, qtd_retorno, codigo);
        }

        public List<Produto>? PesquisarPorCodigoAdicional(int id_atual, int qtd_retorno, string codigoAdicional)
        {
            return conexao.PesquisarPorCodigoAdicional(id_atual, qtd_retorno, codigoAdicional);
        }

        public List<Produto>? PesquisarPorLocal(int id_atual, int qtd_retorno, int? estoqueId = null, int? rua = null, int? predio = null, int? nivel = null, bool? lado = null)
        {
            return conexao.pesquisarProdutoLocal(id_atual, qtd_retorno, estoqueId, rua, predio, nivel, lado);
        }

        public List<Categoria> ListarCategorias()
        {
            return conexao.listarCategorias();
        }

        public List<Estoque> ListarEstoques()
        {
            return conexao.listarEstoques();
        }

        public void DeletarCodigoAdicional(int cod)
        {
            conexao.deletarCodigo(cod);
        }

        public void DeletarEstoque(Estoque est)
        {
            conexao.deletarEstoque(est);
        }

        public void EditarProduto(Produto produto, int userID)
        {
            if (produto.Categoria!.Id == null)
                produto.Categoria.Id = conexao.adicionarCategoria(produto.Categoria);

            conexao.editarProduto(produto);
            conexao.LogEditarProduto(userID, produto);
        }

        public void EditarNomeEstoque(int id, string novoNome)
        {
            conexao.editarNomeEstoque(id, novoNome);
        }

        public List<Log_Produto> BuscarLogsProduto(DateTime? ultimaData, int ultimoId, int qtd_retorno, string? termo, int? tipo)
        {
            return conexao.BuscarLogsProdutoPaginado(ultimaData, ultimoId, qtd_retorno, termo, tipo);
        }

    }
}
