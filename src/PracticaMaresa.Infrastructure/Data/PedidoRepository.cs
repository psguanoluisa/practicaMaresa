using PracticaMaresa.Application.Interfaces;
using PracticaMaresa.Domain.Entities;

namespace PracticaMaresa.Infrastructure.Data;

public class PedidoRepository : IPedidoRepository
{
    private readonly ApplicationDbContext _context;

    public PedidoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddPedidoAsync(PedidoCabecera pedido)
    {
        await _context.PedidoCabeceras.AddAsync(pedido);
    }
}
