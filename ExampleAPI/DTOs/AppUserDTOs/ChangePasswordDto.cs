namespace ExampleAPI.DTOs.AppUserDTOs;

public record ChangePasswordDto(string OldPassword, string NewPassword);