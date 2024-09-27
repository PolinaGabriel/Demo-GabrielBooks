using System;
using System.Collections.Generic;

namespace GabrielBooks.Models;

public partial class Saleproduct
{
    public int Id { get; set; }

    public int Saleid { get; set; }

    public int Productid { get; set; }

    public int Productquantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
