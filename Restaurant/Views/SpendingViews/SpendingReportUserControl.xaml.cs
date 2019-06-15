using Restaurant.ViewModels;
using System.Windows.Controls;

namespace Restaurant.Views.SpendingViews
{
    public partial class SpendingReportUserControl : UserControl
    {
        public SpendingReportUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SpendingReport");
        }
    }
}
