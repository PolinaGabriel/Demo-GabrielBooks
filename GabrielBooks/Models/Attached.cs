using System;
using System.Collections.Generic;

namespace GabrielBooks.Models;

public partial class Attached
{
    public int Id { get; set; }

    public int Mainproductid { get; set; }

    public int Secondproductid { get; set; }

    public virtual Product Mainproduct { get; set; } = null!;

    public virtual Product Secondproduct { get; set; } = null!;
}
