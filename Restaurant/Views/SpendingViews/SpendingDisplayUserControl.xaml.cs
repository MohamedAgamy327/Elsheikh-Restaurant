using Restaurant.ViewModels;
using System.Windows.Controls;

namespace Restaurant.Views.SpendingViews
{
    public partial class SpendingDisplayUserControl : UserControl
    {
        public SpendingDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SpendingDisplay");
        }
    }
}
