using Restaurant.ViewModels;
using System.Windows.Controls;

namespace Restaurant.Views.SafeViews
{
    public partial class SafeDisplayUserControl : UserControl
    {
        public SafeDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SafeDisplay");
        }
    }
}
