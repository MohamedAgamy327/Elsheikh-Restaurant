using Restaurant.ViewModels;
using System.Windows.Controls;

namespace Restaurant.Views.ItemViews
{
    public partial class ItemOrderUserControl : UserControl
    {
        public ItemOrderUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ItemOrder");
        }
    }
}
