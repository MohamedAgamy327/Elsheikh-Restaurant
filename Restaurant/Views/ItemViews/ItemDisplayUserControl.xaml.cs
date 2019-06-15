using Restaurant.ViewModels;
using System.Windows.Controls;

namespace Restaurant.Views.ItemViews
{
    public partial class ItemUserControl : UserControl
    {
        public ItemUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ItemDisplay");
        }
    }
}
