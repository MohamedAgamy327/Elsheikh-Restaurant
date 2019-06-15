using Restaurant.ViewModels;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace Restaurant.Views.CashierViews.BillItemsViews
{
    public partial class BillItemsWindow : MetroWindow
    {
        public BillItemsWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("BillItems");

        }

    }
}
