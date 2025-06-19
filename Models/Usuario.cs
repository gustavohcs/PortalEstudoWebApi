public class Usuario
{
    public int Id { get; set; }
    public string UsuarioLogin { get; set; }
    public string Senha { get; set; }
    public string? Salt { get; set; }
    public string Role { get; set; }
    public string Nome { get; set; }
    public int AnoLetivo { get; set; }
    public DateTime? CreationDate { get; set; }
    public DateTime? UpdateDate { get; set; }
}
