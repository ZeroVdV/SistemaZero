namespace SistemaZero.Model.Entity
{
    public class Linha
    {
        public int Id { get; set; }
        public string? Nome { get; set; }

        public static List<Linha> Todas => new List<Linha>
    {
        new() { Id = 0, Nome = "Linha de Produção 1" },
        new() { Id = 1, Nome = "Linha de Produção 2" },
        new() { Id = 2, Nome = "Linha de Produção 3" }
    };
    }
}
