using System.ComponentModel.DataAnnotations.Schema;

[Table("apostilas")]
public class Apostila
{
    public int Id { get; set; }
    public int Capitulo { get; set; }
    public string Topico { get; set; }
    public string Materia { get; set; }
    public int Ano { get; set; }
    public string Conteudo { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }
}
