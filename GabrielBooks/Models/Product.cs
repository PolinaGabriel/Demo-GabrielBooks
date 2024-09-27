using System;
using System.Collections.Generic;

namespace GabrielBooks.Models;

public partial class Product
{
    public int IdPro { get; set; }

    public string NamePro { get; set; } = null!;

    public string? Description { get; set; }

    public string Mainphoto { get; set; } = null!;

    public int Manufacturerid { get; set; }

    public bool Enabled { get; set; }

    public float Price { get; set; }

    public int? Attachedquantity { get; set; }

    public virtual ICollection<Attached> AttachedMainproducts { get; set; } = new List<Attached>();

    public virtual ICollection<Attached> AttachedSecondproducts { get; set; } = new List<Attached>();

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<Productphoto> Productphotos { get; set; } = new List<Productphoto>();

    public virtual ICollection<Saleproduct> Saleproducts { get; set; } = new List<Saleproduct>();
}
