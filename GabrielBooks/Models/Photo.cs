using System;
using System.Collections.Generic;

namespace GabrielBooks.Models;

public partial class Photo
{
    public int IdPho { get; set; }

    public string NamePho { get; set; } = null!;

    public virtual ICollection<Productphoto> Productphotos { get; set; } = new List<Productphoto>();
}
