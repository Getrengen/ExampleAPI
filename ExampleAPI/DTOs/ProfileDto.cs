using ExampleAPI.DTOs.AppUserDTOs;
using ExampleAPI.DTOs.OrderDTOs;

namespace ExampleAPI.DTOs;

public record ProfileDto(AppUserDto User, List<OrderDto> Orders);