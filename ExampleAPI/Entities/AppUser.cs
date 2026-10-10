using System;
using System.Collections.Generic;

namespace ExampleAPI.Entities;

public partial class AppUser
{
    public int ID { get; set; }

    public string Login { get; set; } = null!;

    public string? FullName { get; set; }

    public string PassHash { get; set; }

    public int? RoleID { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }
}
