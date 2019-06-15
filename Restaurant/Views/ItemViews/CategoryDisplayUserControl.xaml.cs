using Restaurant.ViewModels;
using System.Windows.Controls;

namespace Restaurant.Views.ItemViews
{
    public partial class CategoryUserControl : UserControl
    {
        public CategoryUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("CategoryDisplay");
        }
    }
}
