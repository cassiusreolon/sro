using Microsoft.EntityFrameworkCore;
using sro.Models;

namespace sro.Configuration
{
    public class SROContext : DbContext
    {
        public DbSet<Documento>? Documento { get; set; } = null!;
        //public DbSet<SeguradoParticipante>? SeguradoParticipante { get; set; } = null!;
        //public DbSet<Beneficiario>? Beneficiario { get; set; } = null!;
        //public DbSet<Intermediario>? Intermediario { get; set; } = null!;
        //public DbSet<CoberturaRiscoSeguro>? CoberturaRiscoSeguro { get; set; } = null!;
        //public DbSet<Carencia>? Carencia { get; set; } = null!;

        public SROContext(DbContextOptions<SROContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar o nome da tabela para coincidir com o banco (singular)
            modelBuilder.Entity<Documento>().ToTable("Documento");
            
            /* Relacionamentos comentados - descomente conforme necessário
            // Documento
            modelBuilder.Entity<Documento>()
                .HasMany(d => d.SeguradosParticipantes)
                .WithOne(sp => sp.Documento)
                .HasForeignKey(sp => sp.DocumentoId);

            modelBuilder.Entity<Documento>()
                .HasMany(d => d.Beneficiarios)
                .WithOne(b => b.Documento)
                .HasForeignKey(b => b.DocumentoId);

            modelBuilder.Entity<Documento>()
                .HasMany(d => d.Intermediarios)
                .WithOne(i => i.Documento)
                .HasForeignKey(i => i.DocumentoId);

            modelBuilder.Entity<Documento>()
                .HasMany(d => d.CoberturasRiscoSeguro)
                .WithOne(c => c.Documento)
                .HasForeignKey(c => c.DocumentoId);

            // CoberturaRiscoSeguro
            modelBuilder.Entity<CoberturaRiscoSeguro>()
                .HasMany(c => c.Carencias)
                .WithOne(car => car.CoberturaRiscoSeguro)
                .HasForeignKey(car => car.CoberturaRiscoSeguroId);

            // Outras tabelas
            modelBuilder.Entity<SeguradoParticipante>().ToTable("SeguradoParticipante");
            modelBuilder.Entity<Beneficiario>().ToTable("Beneficiario");
            modelBuilder.Entity<Intermediario>().ToTable("Intermediario");
            modelBuilder.Entity<CoberturaRiscoSeguro>().ToTable("CoberturaRiscoSeguro");
            modelBuilder.Entity<Carencia>().ToTable("Carencia");
            */
        }
    }
}