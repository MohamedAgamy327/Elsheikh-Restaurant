using Restaurant.ViewModels;
using MahApps.Metro.Controls;

namespace Restaurant.Views.BillViews
{
    public partial class BillWindow :  MetroWindow
    {
        public BillWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Bill");
        }
    }
}
