using System;
using System.Collections.Generic;

namespace GabrielBooks.Models;

public partial class Manufacturer
{
    public int IdMan { get; set; }

    public string NameMan { get; set; } = null!;

    public DateOnly Startdate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
