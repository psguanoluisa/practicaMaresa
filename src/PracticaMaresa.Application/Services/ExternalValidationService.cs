using System.Net.Http;
using PracticaMaresa.Application.DTOs;

namespace PracticaMaresa.Application.Services;

public class ExternalValidationService : IExternalValidationService
{
    private readonly HttpClient _httpClient;

    public ExternalValidationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ValidateOrderAsync(CrearPedidoDto pedidoDto)
    {
        try
        {
            // Consumir el endpoint público especificado
            // En un escenario real, podríamos pasar el pedidoDto.ClienteId en la URL: $"https://jsonplaceholder.typicode.com/users/{pedidoDto.ClienteId}"
            // pero para ajustarnos a la instrucción usamos la URL solicitada.
            var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/1");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"El servicio externo rechazó la validación del cliente con código de estado HTTP {response.StatusCode}.");
            }

            return true;
        }
        catch (HttpRequestException ex)
        {
            // Capturar errores de red o timeouts
            throw new Exception("Falla de comunicación con el servicio externo de validación.", ex);
        }
    }
}
