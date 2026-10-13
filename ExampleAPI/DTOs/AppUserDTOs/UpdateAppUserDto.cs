using System.ComponentModel.DataAnnotations;

namespace ExampleAPI.DTOs.AppUserDTOs;

public class UpdateAppUserDto
{
    [Required]
    public string Login { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int RoleID { get; set; }
}