using System;
using System.Collections.Generic;

namespace GabrielBooks.Models;

public partial class Sale
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public virtual ICollection<Saleproduct> Saleproducts { get; set; } = new List<Saleproduct>();
}
