namespace ExampleAPI.DTOs.OrderDTOs;

public record OrderProductDto(int ProductId, string? ProductName, int Quantity, int? Price );