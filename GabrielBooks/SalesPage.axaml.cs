using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using GabrielBooks.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GabrielBooks;

public partial class SalesPage : Window
{
    public bool start;

    public SalesPage()
    {
        InitializeComponent();
    }

    public void Fill()
    {
        List<Saleproduct> all = Helper.Database.Saleproducts.Include(x => x.Sale).Include(x => x.Product).ToList();
        all = ForFilter(all);
        saleList.ItemsSource = all.ToList().Select(x => new
        {
            CurrentDate = x.Sale.Date,
            CurrentTime = x.Sale.Time,
            x.Productquantity
        });
    }

    public List<Saleproduct> ForFilter(List<Saleproduct> source)
    {
        switch (productBox.SelectedIndex + 1)
        {
            case 1:
                return source;

            default:
                IEnumerable<Saleproduct> filter =
                from x in source
                where x.Productid == productBox.SelectedIndex - 1
                select x;
                return filter.ToList();
        }
    }

    public void Filter(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
    {
        if (start == true)
        {
            Fill();
        }
    }

    public void Cancel(object sender, RoutedEventArgs routedEventArgs)
    {
        this.Close();
    }
}