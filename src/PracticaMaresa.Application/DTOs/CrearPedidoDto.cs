namespace PracticaMaresa.Application.DTOs;

public class CrearPedidoDto
{
    public int ClienteId { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public List<CrearPedidoItemDto> Items { get; set; } = new List<CrearPedidoItemDto>();
}
