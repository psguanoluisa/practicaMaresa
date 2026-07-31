using Microsoft.AspNetCore.Mvc;
using PracticaMaresa.Application.DTOs;
using PracticaMaresa.Application.Services;

namespace PracticaMaresa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarPedido([FromBody] CrearPedidoDto pedidoDto)
    {
        var pedido = await _pedidoService.RegistrarPedidoAsync(pedidoDto);

        // Devolvemos Created (201) con el id generado (y podríamos retornar la URL para obtener el pedido si existiera un GET)
        return Created("", new { mensaje = "Pedido registrado exitosamente.", pedidoId = pedido.Id, total = pedido.Total });
    }
}
