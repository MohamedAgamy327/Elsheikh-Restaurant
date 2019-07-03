using BLL.UnitOfWorkService;
using DAL;
using DAL.BindableBaseService;
using DAL.Entities;
using DTO.BillItemDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using Restaurant.Reports;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels.ShiftViewModels
{
    public class BillsCategoriesViewModel : ValidatableBindableBase
    {
        public static int ShiftID { get; set; }

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                if (_selectedShift.EndDate == null)
                    Items = new ObservableCollection<BillsCategoriesDataModel>(unitOfWork.BillsItems.GetBillsCategories(_selectedCategory.ID, _selectedShift.StartDate));
                else
                    Items = new ObservableCollection<BillsCategoriesDataModel>(unitOfWork.BillsItems.GetBillsCategories(_selectedCategory.ID, _selectedShift.StartDate, Convert.ToDateTime(_selectedShift.EndDate)));
                OnPropertyChanged("ItemsSum");
            }
        }

        public BillsCategoriesViewModel()
        {
            _key = "";
            _isFocused = true;
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        public decimal ItemsSum
        {
            get
            {
                if (Items != null && Items.Count > 0)
                    return Items.Sum(s => Convert.ToDecimal(s.Total));
                else
                    return 0;
            }
        }

        private decimal _checkedSum;
        public decimal CheckedSum
        {
            get { return _checkedSum; }
            set { SetProperty(ref _checkedSum, value); }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }

        private Shift _selectedShift;
        public Shift SelectedShift
        {
            get { return _selectedShift; }
            set { SetProperty(ref _selectedShift, value); }
        }

        private ObservableCollection<BillsCategoriesDataModel> _items;
        public ObservableCollection<BillsCategoriesDataModel> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        // Display

        private RelayCommand _loaded;
        public RelayCommand Loaded
        {
            get
            {
                return _loaded
                    ?? (_loaded = new RelayCommand(LoadedMethod));
            }
        }
        private void LoadedMethod()
        {
            try
            {

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Categories = new ObservableCollection<Category>(unitOfWork.Categories.GetAll().OrderBy(o => o.Name));
                    SelectedCategory = _categories[0];
                    _selectedShift = unitOfWork.Shifts.Get(ShiftID);
                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _search;
        public RelayCommand Search
        {
            get
            {
                return _search
                    ?? (_search = new RelayCommand(SearchMethod));
            }
        }
        private void SearchMethod()
        {
            try
            {
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _check;
        public RelayCommand Check
        {
            get
            {
                return _check
                    ?? (_check = new RelayCommand(CheckMethod));
            }
        }
        private void CheckMethod()
        {
            try
            {
                CheckedSum = Items.Where(w => w.Checked == true).Sum(s => Convert.ToDecimal(s.Total));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _print;
        public RelayCommand Print
        {
            get
            {
                return _print
                    ?? (_print = new RelayCommand(PrintMethod, CanExecutePrint));
            }
        }
        private void PrintMethod()
        {
            try
            {
                if (_items.Where(w => w.Checked == true).Count() == 0)
                    return;

                Mouse.OverrideCursor = Cursors.Wait;
                DS ds = new DS();
                ds.BillsCategories.Rows.Clear();
                int i = 0;
                foreach (var item in _items.Where(w => w.Checked == true))
                {
                    ds.BillsCategories.Rows.Add();
                    ds.BillsCategories[i]["Qty"] = item.Qty;
                    ds.BillsCategories[i]["Item"] = item.Item.Name;
                    ds.BillsCategories[i]["Price"] = string.Format("{0:0.00}", item.Total); ;
                    ds.BillsCategories[i]["Total"] = string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(_checkedSum), 0));
                    i++;
                }
                //  ReportWindow rpt = new ReportWindow();
                ItemsOnlyReport itemsOnlyReport = new ItemsOnlyReport();
                itemsOnlyReport.SetDataSource(ds.Tables["BillsCategories"]);
                Mouse.OverrideCursor = null;
                itemsOnlyReport.PrintToPrinter(1, false, 0, 15);
                // rpt.crv.ViewerCore.ReportSource = itemsOnlyReport;
                //rpt.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        private bool CanExecutePrint()
        {
            if (Items == null || Items.Where(w => w.Checked).Count() == 0)
                return false;
            else
                return true;
        }
    }
}
