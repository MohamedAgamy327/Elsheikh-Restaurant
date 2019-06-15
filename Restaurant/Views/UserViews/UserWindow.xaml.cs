using Restaurant.ViewModels;
using MahApps.Metro.Controls;

namespace Restaurant.Views.UserViews
{
    public partial class UserWindow : MetroWindow
    {
        public UserWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("User");
        }
    }
}
