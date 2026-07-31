using Microsoft.EntityFrameworkCore;
using PracticaMaresa.Domain.Entities;

namespace PracticaMaresa.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<PedidoCabecera> PedidoCabeceras { get; set; } = null!;
    public DbSet<PedidoDetalle> PedidoDetalles { get; set; } = null!;
    public DbSet<LogAuditoria> LogAuditorias { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PedidoCabecera>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Usuario).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<PedidoDetalle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Precio).HasColumnType("decimal(18,2)");
            
            entity.HasOne(d => d.PedidoCabecera)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<LogAuditoria>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Evento).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descripcion).IsRequired();
        });
    }
}
