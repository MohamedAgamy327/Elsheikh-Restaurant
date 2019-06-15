using MahApps.Metro.Controls;
using Restaurant.ViewModels;

namespace Restaurant.Views.SpendingViews
{
    public partial class SpendingWindow : MetroWindow
    {
        public SpendingWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Spnending");
        }
    }
}
