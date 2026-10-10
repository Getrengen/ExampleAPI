using System;
using System.Collections.Generic;

namespace ExampleAPI.Entities;

public partial class Order
{
    public int ID { get; set; }

    public int? UserID { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual AppUser? User { get; set; }
}
