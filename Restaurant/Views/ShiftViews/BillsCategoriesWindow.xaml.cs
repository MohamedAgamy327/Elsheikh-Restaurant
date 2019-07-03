using MahApps.Metro.Controls;
using Restaurant.ViewModels;

namespace Restaurant.Views.ShiftViews
{
    public partial class BillsCategoriesWindow : MetroWindow
    {
        public BillsCategoriesWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("BillsCategories");
        }
    }
}
