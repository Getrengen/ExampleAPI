namespace ExampleAPI.DTOs;

public record GetOrderProduct(string ProductName, int? Quantity, int?  ProductPrice);
public record GetOrder(DateTime? CreatedDate, List<GetOrderProduct> OrderProducts);