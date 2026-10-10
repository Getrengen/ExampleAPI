using System;
using System.Collections.Generic;

namespace ExampleAPI.Entities;

public partial class Role
{
    public int ID { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>();
}
