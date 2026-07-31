using PracticaMaresa.Application.Interfaces;
using PracticaMaresa.Domain.Entities;

namespace PracticaMaresa.Infrastructure.Data;

public class AuditoriaRepository : IAuditoriaRepository
{
    private readonly ApplicationDbContext _context;

    public AuditoriaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddLogAsync(LogAuditoria log)
    {
        await _context.LogAuditorias.AddAsync(log);
    }

    public async Task AddLogIndependentAsync(LogAuditoria log)
    {
        // Limpiar el ChangeTracker para evitar intentar guardar entidades 
        // de la transacción que hizo rollback
        _context.ChangeTracker.Clear();
        
        await _context.LogAuditorias.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}
