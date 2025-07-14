namespace SistemaZero.Model.Entity
{
    public class Log_Produto
    {
        public int ID { get; set; }
        public string? CodigoProduto { get; set; }
        public string? EmailUser { get; set; }
        public string? ContextoEscrito { get; set; }
        public DateOnly Registro { get; set; }

        public string RegistroFormatado => Registro.ToString("dd/MM/yyyy");
    }
}
