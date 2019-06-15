using Restaurant.ViewModels;
using MahApps.Metro.Controls;

namespace Restaurant.Views.CashierViews.ShiftSpendingViews
{
    public partial class ShiftSpendingWindow : MetroWindow
    {
        public ShiftSpendingWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("ShiftSpending");
        }
    }
}
