using PracticaMaresa.Domain.Entities;

namespace PracticaMaresa.Application.Interfaces;

public interface IAuditoriaRepository
{
    Task AddLogAsync(LogAuditoria log);
    Task AddLogIndependentAsync(LogAuditoria log); // Para guardar logs incluso si hay rollback
}
