using PracticaMaresa.Application.DTOs;
using PracticaMaresa.Application.Interfaces;
using PracticaMaresa.Domain.Entities;

namespace PracticaMaresa.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IAuditoriaRepository _auditoriaRepository;
    private readonly IExternalValidationService _externalValidationService;

    public PedidoService(
        IUnitOfWork unitOfWork,
        IPedidoRepository pedidoRepository,
        IAuditoriaRepository auditoriaRepository,
        IExternalValidationService externalValidationService)
    {
        _unitOfWork = unitOfWork;
        _pedidoRepository = pedidoRepository;
        _auditoriaRepository = auditoriaRepository;
        _externalValidationService = externalValidationService;
    }

    public async Task<PedidoCabecera> RegistrarPedidoAsync(CrearPedidoDto pedidoDto)
    {
        if (pedidoDto == null || pedidoDto.Items == null || !pedidoDto.Items.Any())
        {
            throw new ArgumentException("Datos del pedido inválidos.");
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // 1. Log inicio
            var logInicio = new LogAuditoria
            {
                Fecha = DateTime.UtcNow,
                Evento = "INICIO_REGISTRO_PEDIDO",
                Descripcion = $"Iniciando registro de pedido para Cliente {pedidoDto.ClienteId}"
            };
            await _auditoriaRepository.AddLogAsync(logInicio);

            // 2. Crear PedidoCabecera y Detalles
            var pedidoCabecera = new PedidoCabecera
            {
                ClienteId = pedidoDto.ClienteId,
                Usuario = pedidoDto.Usuario,
                Fecha = DateTime.UtcNow,
                Total = pedidoDto.Items.Sum(i => i.Cantidad * i.Precio)
            };

            foreach (var item in pedidoDto.Items)
            {
                if (item.Cantidad <= 0 || item.Precio < 0)
                {
                    throw new ArgumentException("Cantidades o precios inválidos en los detalles.");
                }

                pedidoCabecera.Detalles.Add(new PedidoDetalle
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    Precio = item.Precio
                });
            }

            await _pedidoRepository.AddPedidoAsync(pedidoCabecera);
            await _unitOfWork.SaveChangesAsync(); // Guardar para obtener Ids si es necesario o antes de validar

            // 3. Validar con servicio externo
            await _externalValidationService.ValidateOrderAsync(pedidoDto);

            // 4. Log éxito
            var logExito = new LogAuditoria
            {
                Fecha = DateTime.UtcNow,
                Evento = "REGISTRO_PEDIDO_EXITOSO",
                Descripcion = $"Pedido {pedidoCabecera.Id} registrado exitosamente para Cliente {pedidoDto.ClienteId}"
            };
            await _auditoriaRepository.AddLogAsync(logExito);

            // Guardar logs finales y confirmar transacción
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return pedidoCabecera;
        }
        catch (Exception ex)
        {
            // En caso de cualquier error, hacer rollback
            await _unitOfWork.RollbackTransactionAsync();

            // Guardar log de error de forma independiente (fuera de la transacción revertida)
            var logError = new LogAuditoria
            {
                Fecha = DateTime.UtcNow,
                Evento = "ERROR_REGISTRO_PEDIDO",
                Descripcion = $"Error al registrar pedido para Cliente {pedidoDto.ClienteId}. Detalles: {ex.Message}"
            };
            await _auditoriaRepository.AddLogIndependentAsync(logError);

            throw; // Relanzar la excepción para que sea manejada por el Middleware o el Controlador
        }
    }
}
