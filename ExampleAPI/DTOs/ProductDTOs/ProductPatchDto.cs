using System.ComponentModel.DataAnnotations;

namespace ExampleAPI.DTOs.ProductDTOs;

public record ProductPatchDto(string? Name, int? Price, string? Description);