using Microsoft.EntityFrameworkCore;
using sro.Models;

namespace sro.Configuration
{
    public class SROContext : DbContext
    {
        public DbSet<Documento>? Documentos { get; set; } = null!;
        //public DbSet<SeguradoParticipante>? SeguradosParticipantes { get; set; } = null!;
        //public DbSet<Beneficiario>? Beneficiarios { get; set; } = null!;
        //public DbSet<Intermediario>? Intermediarios { get; set; } = null!;
        //public DbSet<CoberturaRiscoSeguro>? CoberturasRiscoSeguro { get; set; } = null!;
        //public DbSet<Carencia>? Carencias { get; set; } = null!;

        public SROContext(DbContextOptions<SROContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*// Documento
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

            // Configurar o nome das tabelas para coincidir com o banco
            modelBuilder.Entity<Documento>().ToTable("Documento", schema: "dbo");
            modelBuilder.Entity<SeguradoParticipante>().ToTable("SeguradoParticipante", schema: "dbo");
            modelBuilder.Entity<Beneficiario>().ToTable("Beneficiario", schema: "dbo");
            modelBuilder.Entity<Intermediario>().ToTable("Intermediario", schema: "dbo");
            modelBuilder.Entity<CoberturaRiscoSeguro>().ToTable("CoberturaRiscoSeguro", schema: "dbo");
            modelBuilder.Entity<Carencia>().ToTable("Carencia", schema: "dbo");*/
        }
    }
}