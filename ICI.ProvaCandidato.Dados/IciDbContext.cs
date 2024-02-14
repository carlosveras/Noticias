using ICI.ProvaCandidato.Dados.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICI.ProvaCandidato.Dados
{
    public class IciDbContext : DbContext
    {
    
        public IciDbContext(DbContextOptions<IciDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Chaves
            modelBuilder.Entity<Noticia>()
                        .HasKey(n => n.Id);

            modelBuilder.Entity<NoticiaTag>()
                        .HasKey(nt => nt.Id);

            // Relacionamentos
            modelBuilder.Entity<NoticiaTag>()
                        .HasOne(nt => nt.Noticia)
                        .WithMany(n => n.NoticiaTags)
                        .HasForeignKey(nt => nt.NoticiaId);

            modelBuilder.Entity<NoticiaTag>()
                        .HasOne(nt => nt.Tag)
                        .WithMany()
                        .HasForeignKey(nt => nt.TagId);


            // Previne exclusao
            modelBuilder.Entity<NoticiaTag>()
                        .HasOne(nt => nt.Tag)
                        .WithMany()
                        .HasForeignKey(nt => nt.TagId)
                        .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<NoticiaTag> NoticiaTags { get; set; }
        public DbSet<Noticia> Noticias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
