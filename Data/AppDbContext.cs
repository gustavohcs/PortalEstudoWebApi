using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Apostila> Apostilas { get; set; }
    public DbSet<Questao> Questoes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<ComentarioQuestao> ComentariosXQuestoes { get; set; }
    public DbSet<ComentarioApostila> ComentariosXApostilas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Apostila>().ToTable("apostilas");
        modelBuilder.Entity<Questao>().ToTable("questoes");
        modelBuilder.Entity<Usuario>().ToTable("usuario");
        modelBuilder.Entity<ComentarioQuestao>().ToTable("comentariosxquestoes");
        modelBuilder.Entity<ComentarioApostila>().ToTable("comentariosxapostilas");
    }
}


