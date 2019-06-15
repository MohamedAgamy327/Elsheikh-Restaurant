using MahApps.Metro.Controls;
using Restaurant.ViewModels;

namespace Restaurant.Views.SafeViews
{
    public partial class SafeWindow : MetroWindow
    {
        public SafeWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Safe");
        }
    }
}
