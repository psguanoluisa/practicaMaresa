using PracticaMaresa.Application.DTOs;

namespace PracticaMaresa.Application.Services;

public class ExternalValidationService : IExternalValidationService
{
    public async Task<bool> ValidateOrderAsync(CrearPedidoDto pedidoDto)
    {
        // Simular llamada de red
        await Task.Delay(500);

        // Simular validación: si clienteId es 9999, simular una falla externa
        if (pedidoDto.ClienteId == 9999)
        {
            throw new Exception("El servicio externo rechazó la validación del pedido.");
        }

        return true; // Validación exitosa
    }
}
