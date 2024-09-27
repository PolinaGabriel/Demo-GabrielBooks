using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Castle.Core.Resource;
using GabrielBooks.Models;
using HarfBuzzSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GabrielBooks;

public partial class ProductPage : Window
{
    public List<Product> forAttachment = new List<Product>();
    public string photo;

    public ProductPage()
    {
        InitializeComponent();
        errorBox.IsVisible = false;
        List<Manufacturer> manufacturers = [new Manufacturer() { IdMan = -1, NameMan = "Производитель" }, .. Helper.Database.Manufacturers];
        manufacturerBox.ItemsSource = manufacturers.ToList().Select(x => new
        {
            x.IdMan,
            x.NameMan
        });
        manufacturerBox.SelectedIndex = 0;
    }

    public void ShowSales(object sender, RoutedEventArgs routedEventArgs)
    {
        SalesPage salesPage = new SalesPage();
        salesPage.start = false;
        List<Product> products = [new Product() { IdPro = -1, NamePro = "Все продукты" }, .. Helper.Database.Products.OrderBy(x => x.IdPro)];
        salesPage.productBox.ItemsSource = products.ToList().Select(x => new
        {
            x.IdPro,
            x.NamePro
        });
        salesPage.productBox.SelectedIndex = Convert.ToInt32(idBox.Text) + 1;
        salesPage.start = true;
        salesPage.Fill();
        salesPage.Show();
    }

    private readonly FileDialogFilter fileFilter = new()
    {
        Extensions = new List<string>() { "png", "jpg", "jpeg" }
    };

    private async void SelectImage(object sender, RoutedEventArgs e)
    {
        try
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(fileFilter);
            var result = await dialog.ShowAsync(this);
            string file = String.Join("", result);
            long length = new System.IO.FileInfo(file).Length;
            if (length <= 20000) //размер!!
            {
                errorBox.IsVisible = false;
                Random random = new Random();
                photo = "Asset/photo" + random.Next(1, 10000) + ".jpg";
                System.IO.File.Copy(file, photo);
                photoBox.Source = new Bitmap(photo);
            }
            else
            {
                errorBox.IsVisible = true;
            }
        }
        catch { }
    }

    public void Editing()
    {
        Helper.Database.Products.FirstOrDefault(x => x.IdPro == Convert.ToInt32(idBox.Text)).Mainphoto = photo;
        Helper.Database.Products.FirstOrDefault(x => x.IdPro == Convert.ToInt32(idBox.Text)).NamePro = nameBox.Text;
        Helper.Database.Products.FirstOrDefault(x => x.IdPro == Convert.ToInt32(idBox.Text)).Price = float.Parse(priceBox.Text);
        Helper.Database.Products.FirstOrDefault(x => x.IdPro == Convert.ToInt32(idBox.Text)).Manufacturerid = manufacturerBox.SelectedIndex;
        if (enabledBox.IsChecked == true)
        {
            Helper.Database.Products.FirstOrDefault(x => x.IdPro == Convert.ToInt32(idBox.Text)).Enabled = true;
        }
        else
        {
            Helper.Database.Products.FirstOrDefault(x => x.IdPro == Convert.ToInt32(idBox.Text)).Enabled = false;
        }
        Helper.Database.Products.FirstOrDefault(x => x.IdPro == Convert.ToInt32(idBox.Text)).Description = descriptionBox.Text;
    }

    public Product Adding(Product product)
    {
        product.Mainphoto = photo;
        product.NamePro = nameBox.Text;
        product.Price = float.Parse(priceBox.Text);
        product.Manufacturerid = manufacturerBox.SelectedIndex;
        if (enabledBox.IsChecked == true)
        {
            product.Enabled = true;
        }
        else
        {
            product.Enabled = false;
        }
        product.Description = descriptionBox.Text;
        return product;
    }

    public void RemoveingAttached(Product product) 
    {
        product.Attachedquantity = 0;
        foreach (Attached attached in Helper.Database.Attacheds)
        {
            if (attached.Mainproductid == Convert.ToInt32(idBox.Text))
            {
                Helper.Database.Attacheds.Remove(attached);
            }
        }
        Helper.Database.SaveChanges();
    }

    public void AddingAttached(Product product0)
    {
        foreach (Product product1 in attachedList.SelectedItems)
        {
            Attached attached = new Attached()
            {
                Mainproductid = product0.IdPro,
                Secondproductid = product1.IdPro
            };
            Helper.Database.Attacheds.Add(attached);
            product0.Attachedquantity++;
        }
        Helper.Database.SaveChanges();
    }

    public void Save(object sender, RoutedEventArgs routedEventArgs)
    {
        if (float.Parse(priceBox.Text) > 0)
        {
            if (this.Title == "Редактирование товара")
            {
                Editing();
                RemoveingAttached(Helper.Database.Products.FirstOrDefault(x => x.IdPro == Convert.ToInt32(idBox.Text)));
                AddingAttached(Helper.Database.Products.FirstOrDefault(x => x.IdPro == Convert.ToInt32(idBox.Text)));
            }
            else
            {
                Product product = new Product();
                Helper.Database.Products.Add(Adding(product));
                AddingAttached(product);
            }
            Helper.Database.SaveChanges();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        else
        {
            priceBox.BorderBrush = Brushes.Red;
        }
    }

    public void Cancel(object sender, RoutedEventArgs routedEventArgs)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }
}