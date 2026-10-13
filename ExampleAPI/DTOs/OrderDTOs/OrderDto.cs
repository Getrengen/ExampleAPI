namespace ExampleAPI.DTOs.OrderDTOs;

public record OrderDto(int ID, int UserID, DateTime? CreatedDate, List<OrderProductDto> Products);