using Microsoft.EntityFrameworkCore;
using sro.Models;

namespace sro.Configuration
{
    public class SROContext : DbContext
    {
        public DbSet<Documento> Documento { get; set; } = null!;
        public DbSet<Intermediario>? Intermediario { get; set; }
        public DbSet<CoberturaRiscoSeguro>? CoberturaRiscoSeguro { get; set; }
        public DbSet<SeguradoParticipante>? SeguradoParticipante { get; set; }
        public DbSet<Beneficiario>? Beneficiario { get; set; }
        public DbSet<LogProcessamento>? LogProcessamento { get; set; }

        public SROContext(DbContextOptions<SROContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar o nome da tabela para coincidir com o banco (singular)
            modelBuilder.Entity<Documento>().ToTable("Documento");
            modelBuilder.Entity<Intermediario>().ToTable("Intermediario");
            modelBuilder.Entity<CoberturaRiscoSeguro>().ToTable("CoberturaRiscoSeguro");
            modelBuilder.Entity<SeguradoParticipante>().ToTable("SeguradoParticipante");
            modelBuilder.Entity<Beneficiario>().ToTable("Beneficiario");
            modelBuilder.Entity<LogProcessamento>().ToTable("LogProcessamento");

            modelBuilder.Entity<Documento>()
                .HasMany(d => d.Intermediarios)
                .WithOne(i => i.Documento)
                .HasForeignKey(i => i.DocumentoId);

            // Configurações de precisão para propriedades decimal
            modelBuilder.Entity<Intermediario>()
                .Property(i => i.ValorComissaoReal)
                .HasPrecision(18, 2);

            // Configurações de precisão para Documento
            modelBuilder.Entity<Documento>()
                .Property(d => d.LimiteMaximoGarantiaReal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Documento>()
                .Property(d => d.PercentualRetido)
                .HasPrecision(7, 4);

            modelBuilder.Entity<Documento>()
                .Property(d => d.ValorTotalReal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Documento>()
                .Property(d => d.AdicionalFracionamento)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Documento>()
                .Property(d => d.ValorCarregamentoTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Documento>()
                .Property(d => d.Iof)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Documento>()
                .Property(d => d.ValorSegurado)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Documento>()
                .Property(d => d.ValorEstipulante)
                .HasPrecision(18, 2);

            // Configurações de precisão para CoberturaRiscoSeguro
            modelBuilder.Entity<CoberturaRiscoSeguro>()
                .Property(c => c.Iof)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CoberturaRiscoSeguro>()
                .Property(c => c.LimiteMaximoIndenizacaoReal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CoberturaRiscoSeguro>()
                .Property(c => c.ValorPremioReal)
                .HasPrecision(18, 2);

            // Relacionamentos
            modelBuilder.Entity<Documento>()
                .HasMany(d => d.CoberturasRiscoSeguro)
                .WithOne(c => c.Documento)
                .HasForeignKey(c => c.DocumentoId);
                
            modelBuilder.Entity<Documento>()
                .HasMany(d => d.SeguradosParticipantes)
                .WithOne(sp => sp.Documento)
                .HasForeignKey(sp => sp.DocumentoId);

            modelBuilder.Entity<Documento>()
                .HasMany(d => d.Beneficiarios)
                .WithOne(b => b.Documento)
                .HasForeignKey(b => b.DocumentoId);
        }
    }
}
