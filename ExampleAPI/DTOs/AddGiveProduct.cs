using ExampleAPI.Entities;

namespace ExampleAPI.DTOs;

public record AddGiveProduct(string Name, int Price, string Description);
public record PatchProductDTO(string? Name, int? Price, string? Description);