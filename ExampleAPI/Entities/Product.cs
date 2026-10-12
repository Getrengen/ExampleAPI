using System;
using System.Collections.Generic;

namespace ExampleAPI.Entities;

public partial class Product
{
    public int ID { get; set; }

    public string Name { get; set; }

    public int Price { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
