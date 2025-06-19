public class ComentarioQuestao
{
    public int Id { get; set; }
    public int IdQuestao { get; set; }
    public int IdUsuario { get; set; }
    public int? IdComentario { get; set; }
    public int? RespostaParaIdComentario { get; set; }
    public string Mensagem { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }
}
