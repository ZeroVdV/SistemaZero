namespace SistemaZero.Model.Entity
{
    public class Estoque
    {
        public int? ID { get; set; } 
        public LocaisEstoque? Locais { get; set; }
        public int Rua { get; set; }
        public int Predio { get; set; }
        public int Nivel { get; set; }
        public bool Lado { get; set; }
        public int Quantidade { get; set; }

        public string RuaLadoFormatado => $"{Rua}-{(Lado ? "D" : "E")}";
    }
}
