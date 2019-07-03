using GalaSoft.MvvmLight.Ioc;
using Restaurant.ViewModels.SafeViewModels;
using Restaurant.ViewModels.ItemViewModels;
using Restaurant.ViewModels.SpendingViewModels;
using Restaurant.ViewModels.UserViewModels;
using Restaurant.ViewModels.CashierViewModels;
using Restaurant.ViewModels.ShiftViewModels;
using Restaurant.ViewModels.BillViewModels;
using CommonServiceLocator;

namespace Restaurant.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<UserViewModel>();
            SimpleIoc.Default.Register<UserDisplayViewModel>();
            SimpleIoc.Default.Register<ShiftViewModel>();
            SimpleIoc.Default.Register<ShiftDisplayViewModel>();
            SimpleIoc.Default.Register<ItemViewModel>();
            SimpleIoc.Default.Register<ItemDisplayViewModel>();
            SimpleIoc.Default.Register<ItemOrderViewModel>();
            SimpleIoc.Default.Register<SpendingViewModel>();
            SimpleIoc.Default.Register<SpendingDisplayViewModel>();
            SimpleIoc.Default.Register<SpendingReportViewModel>();
            SimpleIoc.Default.Register<SafeViewModel>();
            SimpleIoc.Default.Register<SafeDisplayViewModel>();
            SimpleIoc.Default.Register<SafeReportViewModel>();
            SimpleIoc.Default.Register<BillItemsViewModel>();
            SimpleIoc.Default.Register<ShiftSpendingViewModel>();
            SimpleIoc.Default.Register<BillViewModel>();
            SimpleIoc.Default.Register<BillDisplayViewModel>();
            SimpleIoc.Default.Register<BillShowViewModel>();
            SimpleIoc.Default.Register<CategoryDisplayViewModel>();
            SimpleIoc.Default.Register<BillsCategoriesViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public UserViewModel User
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserViewModel>();
            }
        }

        public UserDisplayViewModel UserDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserDisplayViewModel>();
            }
        }

        public ShiftViewModel Shift
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShiftViewModel>();
            }
        }

        public ShiftDisplayViewModel ShiftDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShiftDisplayViewModel>();
            }
        }

        public ItemViewModel Item
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ItemViewModel>();
            }
        }

        public ItemDisplayViewModel ItemDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ItemDisplayViewModel>();
            }
        }

        public SpendingViewModel Spending
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SpendingViewModel>();
            }
        }

        public SpendingReportViewModel SpendingReport
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SpendingReportViewModel>();
            }
        }

        public SpendingDisplayViewModel SpendingDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SpendingDisplayViewModel>();
            }
        }

        public SafeViewModel Safe
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SafeViewModel>();
            }
        }

        public SafeDisplayViewModel SafeDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SafeDisplayViewModel>();
            }
        }

        public SafeReportViewModel SafeReport
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SafeReportViewModel>();
            }
        }

        public ShiftSpendingViewModel ShiftSpending
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShiftSpendingViewModel>();
            }
        }

        public BillItemsViewModel BillItems
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BillItemsViewModel>();
            }
        }

        public BillViewModel Bill
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BillViewModel>();
            }
        }

        public BillDisplayViewModel BillDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BillDisplayViewModel>();
            }
        }

        public BillShowViewModel BillShow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BillShowViewModel>();
            }
        }

        public CategoryDisplayViewModel CategoryDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CategoryDisplayViewModel>();
            }
        }

        public ItemOrderViewModel ItemOrder
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ItemOrderViewModel>();
            }
        }

        public BillsCategoriesViewModel BillsCategories
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BillsCategoriesViewModel>();
            }
        }

        public static void Cleanup(string viewModel)
        {
            switch (viewModel)
            {
                case "Main":
                    SimpleIoc.Default.Unregister<MainViewModel>();
                    break;

                case "User":
                    SimpleIoc.Default.Unregister<UserViewModel>();
                    SimpleIoc.Default.Register<UserViewModel>();
                    break;

                case "UserDisplay":
                    SimpleIoc.Default.Unregister<UserDisplayViewModel>();
                    SimpleIoc.Default.Register<UserDisplayViewModel>();
                    break;

                case "Shift":
                    SimpleIoc.Default.Unregister<ShiftViewModel>();
                    SimpleIoc.Default.Register<ShiftViewModel>();
                    break;

                case "ShiftDisplay":
                    SimpleIoc.Default.Unregister<ShiftDisplayViewModel>();
                    SimpleIoc.Default.Register<ShiftDisplayViewModel>();
                    break;

                case "Item":
                    SimpleIoc.Default.Unregister<ItemViewModel>();
                    SimpleIoc.Default.Register<ItemViewModel>();
                    break;

                case "ItemDisplay":
                    SimpleIoc.Default.Unregister<ItemDisplayViewModel>();
                    SimpleIoc.Default.Register<ItemDisplayViewModel>();
                    break;

                case "Spending":
                    SimpleIoc.Default.Unregister<SpendingViewModel>();
                    SimpleIoc.Default.Register<SpendingViewModel>();
                    break;

                case "SpendingDisplay":
                    SimpleIoc.Default.Unregister<SpendingDisplayViewModel>();
                    SimpleIoc.Default.Register<SpendingDisplayViewModel>();
                    break;

                case "SpendingReport":
                    SimpleIoc.Default.Unregister<SpendingReportViewModel>();
                    SimpleIoc.Default.Register<SpendingReportViewModel>();
                    break;

                case "Safe":
                    SimpleIoc.Default.Unregister<SafeViewModel>();
                    SimpleIoc.Default.Register<SafeViewModel>();
                    break;

                case "SafeDisplay":
                    SimpleIoc.Default.Unregister<SafeDisplayViewModel>();
                    SimpleIoc.Default.Register<SafeDisplayViewModel>();
                    break;

                case "SafeReport":
                    SimpleIoc.Default.Unregister<SafeReportViewModel>();
                    SimpleIoc.Default.Register<SafeReportViewModel>();
                    break;

                case "ShiftSpending":
                    SimpleIoc.Default.Unregister<ShiftSpendingViewModel>();
                    SimpleIoc.Default.Register<ShiftSpendingViewModel>();
                    break;

                case "BillItems":
                    SimpleIoc.Default.Unregister<BillItemsViewModel>();
                    SimpleIoc.Default.Register<BillItemsViewModel>();
                    break;

                case "Bill":
                    SimpleIoc.Default.Unregister<BillViewModel>();
                    SimpleIoc.Default.Register<BillViewModel>();
                    break;

                case "BillDisplay":
                    SimpleIoc.Default.Unregister<BillDisplayViewModel>();
                    SimpleIoc.Default.Register<BillDisplayViewModel>();
                    break;

                case "BillShow":
                    SimpleIoc.Default.Unregister<BillShowViewModel>();
                    SimpleIoc.Default.Register<BillShowViewModel>();
                    break;

                case "CategoryDisplay":
                    SimpleIoc.Default.Unregister<CategoryDisplayViewModel>();
                    SimpleIoc.Default.Register<CategoryDisplayViewModel>();
                    break;

                case "ItemOrder":
                    SimpleIoc.Default.Unregister<ItemOrderViewModel>();
                    SimpleIoc.Default.Register<ItemOrderViewModel>();
                    break;

                case "BillsCategories":
                    SimpleIoc.Default.Unregister<BillsCategoriesViewModel>();
                    SimpleIoc.Default.Register<BillsCategoriesViewModel>();
                    break;

                default:
                    break;
            }


        }
    }
}