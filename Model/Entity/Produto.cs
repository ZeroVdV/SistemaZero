namespace SistemaZero.Model.Entity
{
    public class Produto
    {
        public int ID { get; set; }
        public string? Descricao { get; set; }
        public string? CodigoProduto { get; set; }
        public bool Ativo { get; set; }
        public Categoria? Categoria { get; set; }
        public Imagem? Imagem_Estoque { get; set; } //em funções de acessar apenas a loja, é so colocar como nulo
        public List<Imagens_Loja>? Imagens_Loja { get; set; } //em funções de acessar apenas o estoque, é só colocar como nulo
        public List<Estoque>? Estoques { get; set; } 
        public List<Codigo_Adicional>? Codigos_Adicionais { get; set; }

        //viewModel
        //concatenação dos estoques:
        public string Estoques_Locais =>
            string.Join("\n", Estoques?.Select(e => e.Locais?.Nome) ?? Enumerable.Empty<string>());

        public string Estoques_Ruas =>
            string.Join("\n", Estoques?.Select(e => e.RuaLadoFormatado) ?? Enumerable.Empty<string>());

        public string Estoques_Predios =>
            string.Join("\n", Estoques?.Select(e => e.Predio.ToString()) ?? Enumerable.Empty<string>());

        public string Estoques_Niveis =>
            string.Join("\n", Estoques?.Select(e => e.Nivel.ToString()) ?? Enumerable.Empty<string>());

        //concatenação dos codigos_adicionais
        public string Codigos_Adicionais_Concatenados =>
            string.Join("\n", Codigos_Adicionais?.Select(e => e.Codigo) ?? Enumerable.Empty<string>());
    }
}
