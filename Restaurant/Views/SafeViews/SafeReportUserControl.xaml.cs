using Restaurant.ViewModels;
using System.Windows.Controls;

namespace Restaurant.Views.SafeViews
{
    public partial class SafeReportUserControl : UserControl
    {
        public SafeReportUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SafeReport");
        }
    }
}
