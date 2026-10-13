using System.ComponentModel.DataAnnotations;

namespace ExampleAPI.DTOs.ProductDTOs;

public record ProductCreateDto(
    [Required]
    string Name,
    int Price,
    string? Description);