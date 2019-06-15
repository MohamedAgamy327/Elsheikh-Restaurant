using Restaurant.ViewModels;
using System.Windows.Controls;

namespace Restaurant.Views.ShiftViews
{ 
    public partial class ShiftDisplayUserControl : UserControl
    {
        public ShiftDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ShiftDisplay");
        }
    }
}
