using Restaurant.ViewModels;
using System.Windows.Controls;

namespace Restaurant.Views.UserViews
{
    public partial class UserDisplayUserControl : UserControl
    {
        public UserDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("UserDisplay");
        }
    }
}
