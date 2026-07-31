using PracticaMaresa.Domain.Entities;

namespace PracticaMaresa.Application.Interfaces;

public interface IPedidoRepository
{
    Task AddPedidoAsync(PedidoCabecera pedido);
}
