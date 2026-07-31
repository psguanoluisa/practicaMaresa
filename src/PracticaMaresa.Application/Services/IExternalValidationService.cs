using PracticaMaresa.Application.DTOs;

namespace PracticaMaresa.Application.Services;

public interface IExternalValidationService
{
    Task<bool> ValidateOrderAsync(CrearPedidoDto pedidoDto);
}
