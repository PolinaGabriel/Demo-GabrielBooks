using System;
using System.Collections.Generic;
using GabrielBooks.Models;
using Microsoft.EntityFrameworkCore;

namespace GabrielBooks.Context;

public partial class User723Context : DbContext
{
    public User723Context()
    {
    }

    public User723Context(DbContextOptions<User723Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Attached> Attacheds { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productphoto> Productphotos { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Saleproduct> Saleproducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=192.168.2.159;Database=user723;Username=user723;Password=53344");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attached>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("attached_pk");

            entity.ToTable("attached", "books");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.Mainproductid).HasColumnName("mainproductid");
            entity.Property(e => e.Secondproductid).HasColumnName("secondproductid");

            entity.HasOne(d => d.Mainproduct).WithMany(p => p.AttachedMainproducts)
                .HasForeignKey(d => d.Mainproductid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("attached_product_fk");

            entity.HasOne(d => d.Secondproduct).WithMany(p => p.AttachedSecondproducts)
                .HasForeignKey(d => d.Secondproductid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("attached_product_fk_1");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.IdMan).HasName("manufacturer_pk");

            entity.ToTable("manufacturer", "books");

            entity.Property(e => e.IdMan)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("id_man");
            entity.Property(e => e.NameMan)
                .HasColumnType("character varying")
                .HasColumnName("Name_man");
            entity.Property(e => e.Startdate).HasColumnName("startdate");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.IdPho).HasName("photo_pk");

            entity.ToTable("photo", "books");

            entity.Property(e => e.IdPho)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("id_pho");
            entity.Property(e => e.NamePho)
                .HasColumnType("character varying")
                .HasColumnName("Name_pho");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdPro).HasName("product_pk");

            entity.ToTable("product", "books");

            entity.Property(e => e.IdPro)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("id_pro");
            entity.Property(e => e.Attachedquantity).HasColumnName("attachedquantity");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Enabled).HasColumnName("enabled");
            entity.Property(e => e.Mainphoto)
                .HasColumnType("character varying")
                .HasColumnName("mainphoto");
            entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");
            entity.Property(e => e.NamePro)
                .HasColumnType("character varying")
                .HasColumnName("Name_pro");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.Manufacturerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_manufacturer_fk");
        });

        modelBuilder.Entity<Productphoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productphoto_pk");

            entity.ToTable("productphoto", "books");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.Photoid).HasColumnName("photoid");
            entity.Property(e => e.Productid).HasColumnName("productid");

            entity.HasOne(d => d.Photo).WithMany(p => p.Productphotos)
                .HasForeignKey(d => d.Photoid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productphoto_photo_fk");

            entity.HasOne(d => d.Product).WithMany(p => p.Productphotos)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productphoto_product_fk");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sale_pk");

            entity.ToTable("sale", "books");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("id");
        });

        modelBuilder.Entity<Saleproduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("saleproduct_pk");

            entity.ToTable("saleproduct", "books");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Productquantity).HasColumnName("productquantity");
            entity.Property(e => e.Saleid).HasColumnName("saleid");

            entity.HasOne(d => d.Product).WithMany(p => p.Saleproducts)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saleproduct_product_fk");

            entity.HasOne(d => d.Sale).WithMany(p => p.Saleproducts)
                .HasForeignKey(d => d.Saleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saleproduct_sale_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
