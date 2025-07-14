namespace SistemaZero.Model.Entity
{
    public class User
    {
        public int? ID { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public Cargo Cargo { get; set; }
        public bool Ativo { get; set; }
        public DateOnly Registro { get; set; }

        public string RegistroFormatado => Registro.ToString("dd/MM/yyyy");
    }
}
