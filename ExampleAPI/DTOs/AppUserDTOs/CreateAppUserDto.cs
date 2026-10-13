using System.ComponentModel.DataAnnotations;

namespace ExampleAPI.DTOs.AppUserDTOs;

public class CreateAppUserDto
{
    [Required]
    public string Login { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    // Клиент отправляет пароль, а не хеш. Сервис хеширует его перед сохранением.
    [Required]
    public string Password { get; set; } = string.Empty;
    
}
