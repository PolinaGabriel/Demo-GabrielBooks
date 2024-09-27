using System;
using System.Collections.Generic;

namespace GabrielBooks.Models;

public partial class Productphoto
{
    public int Id { get; set; }

    public int Productid { get; set; }

    public int Photoid { get; set; }

    public virtual Photo Photo { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
