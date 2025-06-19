public class ComentarioApostilaDto
{
    public int Id { get; set; }
    public int IdApostila { get; set; }
    public int IdUsuario { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public int? RespostaParaIdComentario { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }
}
