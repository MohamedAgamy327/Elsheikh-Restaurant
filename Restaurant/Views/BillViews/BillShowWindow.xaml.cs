using MahApps.Metro.Controls;
using Restaurant.ViewModels;

namespace Restaurant.Views.BillViews
{
    public partial class BillShowWindow : MetroWindow
    {
        public BillShowWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("BillShow");
        }
    }
}
