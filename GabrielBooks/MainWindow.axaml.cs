using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Castle.Core.Resource;
using GabrielBooks;
using GabrielBooks.Models;
using HarfBuzzSharp;
using Metsys.Bson;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GabrielBooks
{
    public partial class MainWindow : Window
    {
        public bool start;
        public List<Photo> photos = Helper.Database.Photos.ToList();
        public List<Product> forAttachment = Helper.Database.Products.ToList(); 
        
        public MainWindow()
        {
            InitializeComponent();
            start = false;
            errorBox.IsVisible = false;
            sortBox.SelectedIndex = 0;
            searchBox.Text = "";
            List<Manufacturer> manufacturers = [new Manufacturer(){ IdMan = -1, NameMan = "Âñå ïðîèçâîäèòåëè" }, .. Helper.Database.Manufacturers];
            filterBox.ItemsSource = manufacturers.ToList().Select(x => new
            {
                x.IdMan,
                x.NameMan
            });
            filterBox.SelectedIndex = 0;
            start = true;
            Fill();
        }

        public void Fill()
        {
            List<Product> all = Helper.Database.Products.Include(x => x.Productphotos).Include(x => x.Manufacturer).ToList();
            all = ForSort(ForSearch(ForFilter(all)));
            productList.ItemsSource = all.ToList().Select(x => new
            {
                x.IdPro,
                NamePhoto = new Bitmap(x.Mainphoto),
                NameAndAttached = x.NamePro + " (" + x.Attachedquantity + ")",
                x.Price,
                x.Enabled
            });
            numberBox.Text = all.Count() + " / " + Helper.Database.Products.Count();
        }

        public List<Product> ForSort(List<Product> source)
        {
            switch (sortBox.SelectedIndex)
            {
                default:
                    return source;

                case 1:
                    return source.OrderBy(x => x.Price).ToList();

                case 2:
                    return source.OrderByDescending(x => x.Price).ToList();
            }
        }

        public List<Product> ForSearch(List<Product> source)
        {
            switch (searchBox.Text)
            {
                case "":
                    return source;

                default:
                    IEnumerable<Product> search =
                    from x in source
                    where x.NamePro.ToLower().Contains(searchBox.Text.ToLower()) ||
                          x.Description.ToLower().Contains(searchBox.Text.ToLower())
                    select x;
                    return search.ToList();
            }
        }

        public List<Product> ForFilter(List<Product> source)
        {
            switch (filterBox.SelectedIndex)
            {
                case 0:
                    return source;

                default:
                    IEnumerable<Product> filter =
                    from x in source
                    where x.Manufacturerid == filterBox.SelectedIndex
                    select x;
                    return filter.ToList();
            }
        }
       
        public void Sort(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (start == true)
            {
                Fill();
            }
        }

        public void Search(object sender, KeyEventArgs keyEventArgs)
        {
            if (start == true)
            {
                Fill();
            }
        }

        public void Filter(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (start == true)
            {
                Fill();
            }
        }

        public void EditProduct(object sender, RoutedEventArgs routedEventArgs)
        {
            ProductPage productPage = new ProductPage();
            productPage.Title = "Ðåäàêòèðîâàíèå òîâàðà";
            productPage.forAttachment = forAttachment.ToList();
            productPage.idBox.Text = Convert.ToString(Helper.Database.Products.FirstOrDefault(x => x.IdPro == (int)(sender as Button).Tag).IdPro);
            productPage.photo = Helper.Database.Products.FirstOrDefault(x => x.IdPro == (int)(sender as Button).Tag).Mainphoto;
            productPage.photoBox.Source = new Bitmap(productPage.photo);
            productPage.nameBox.Text = Helper.Database.Products.FirstOrDefault(x => x.IdPro == (int)(sender as Button).Tag).NamePro;
            productPage.priceBox.Text = Convert.ToString(Helper.Database.Products.FirstOrDefault(x => x.IdPro == (int)(sender as Button).Tag).Price);
            productPage.manufacturerBox.SelectedIndex = Helper.Database.Products.FirstOrDefault(x => x.IdPro == (int)(sender as Button).Tag).Manufacturerid;
            productPage.enabledBox.IsChecked = Helper.Database.Products.FirstOrDefault(x => x.IdPro == (int)(sender as Button).Tag).Enabled;
            productPage.descriptionBox.Text = Helper.Database.Products.FirstOrDefault(x => x.IdPro == (int)(sender as Button).Tag).Description;


            for (int i = 0; i < forAttachment.Count(); i++)
            {
                if (forAttachment[i].IdPro == Helper.Database.Products.FirstOrDefault(x => x.IdPro == (int)(sender as Button).Tag).IdPro
                    || forAttachment[i].Enabled == false)
                {
                    forAttachment.Remove(forAttachment[i]);
                }
            }
            productPage.attachedList.ItemsSource = forAttachment.ToList();
            List<Attached> attachedProducts = Helper.Database.Attacheds.Include(x => x.Mainproduct).Where(x => x.Mainproductid == Convert.ToInt32(productPage.idBox.Text)).ToList();
            foreach (Product p in forAttachment)
            {
                foreach (Attached a in attachedProducts)
                {
                    if (p.IdPro == a.Secondproductid)
                    {
                        productPage.attachedList.SelectedItems.Add(p);
                    }
                }
            }


            productPage.Show();
            this.Close();
        }

        public void DeleteProduct(object sender, RoutedEventArgs routedEventArgs)
        {
            IEnumerable<Saleproduct> remSales =
            from x in Helper.Database.Saleproducts
            where x.Productid == (int)(sender as Button).Tag
            select x;
            if (remSales.Count() == 0)
            {
                errorBox.IsVisible = false;
                Helper.Database.Products.Remove(Helper.Database.Products.FirstOrDefault(x => x.IdPro == (int)(sender as Button).Tag));
                IEnumerable<Attached> remAttached =
                from x in Helper.Database.Attacheds
                where x.Mainproductid == (int)(sender as Button).Tag
                select x;
                foreach (Attached a in remAttached)
                {
                    Helper.Database.Attacheds.Remove(a);
                }
                Helper.Database.SaveChanges();
                Fill();
            }
            else
            {
                errorBox.IsVisible = true;
            }
        }

        public void NewProduct(object sender, RoutedEventArgs routedEventArgs)
        {
            ProductPage productPage = new ProductPage();
            productPage.Title = "Íîâûé òîâàð";
            productPage.history.IsVisible = false;
            productPage.forAttachment = forAttachment.ToList();
            productPage.idBox.Text = "new";
            productPage.photoBox.Source = new Bitmap("Asset/default.png");
            productPage.nameBox.Text = "";
            productPage.priceBox.Text = "";
            productPage.manufacturerBox.SelectedIndex = 0;
            productPage.descriptionBox.Text = "";
            productPage.enabledBox.IsChecked = false;
            productPage.attachedList.ItemsSource = forAttachment.ToList();
            productPage.Show();
            this.Close();
        }
    }
}
