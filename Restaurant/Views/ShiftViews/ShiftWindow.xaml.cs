using Restaurant.ViewModels;
using MahApps.Metro.Controls;

namespace Restaurant.Views.ShiftViews
{
    public partial class ShiftWindow : MetroWindow
    {
        public ShiftWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Shift");
        }
    }
}
