using PracticaMaresa.Application.DTOs;
using PracticaMaresa.Domain.Entities;

namespace PracticaMaresa.Application.Services;

public interface IPedidoService
{
    Task<PedidoCabecera> RegistrarPedidoAsync(CrearPedidoDto pedidoDto);
}
