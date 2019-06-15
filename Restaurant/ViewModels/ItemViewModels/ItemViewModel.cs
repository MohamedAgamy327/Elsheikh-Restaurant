using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;
using DAL.BindableBaseService;

namespace Restaurant.ViewModels.ItemViewModels
{
    public class ItemViewModel : ValidatableBindableBase
    {
        static string Destination { get; set; }

        public ItemViewModel()
        {
            Destination = "CategoryDisplay";
            _currentViewModel = new CategoryDisplayViewModel();
        }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        private RelayCommand<string> _navigateToView;
        public RelayCommand<string> NavigateToView
        {
            get
            {
                return _navigateToView
                    ?? (_navigateToView = new RelayCommand<string>(NavigateToViewMethod));
            }
        }
        private void NavigateToViewMethod(string destination)
        {
            try
            {
                if (Destination != destination)
                {
                    Destination = destination;
                    switch (destination)
                    {
                        case "CategoryDisplay":
                            CurrentViewModel = new CategoryDisplayViewModel();
                            break;
                        case "ItemDisplay":
                            CurrentViewModel = new ItemDisplayViewModel();
                            break;
                        case "ItemOrder":
                            CurrentViewModel = new ItemOrderViewModel();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
