using DAL.BindableBaseService;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using Utilities.Paging;
using System.Collections.ObjectModel;
using DTO.BillDataModel;
using BLL.UnitOfWorkService;
using DAL;
using System.Windows;
using System.Linq;
using MahApps.Metro.Controls;
using Restaurant.Views.BillViews;

namespace Restaurant.ViewModels.BillViewModels
{
    public class BillDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Bills.GetRecordsNumber(_key, _dateFrom, _dateTo);
                Paging.GetFirst();
                Bills = new ObservableCollection<BillDisplayDataModel>(unitOfWork.Bills.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public BillDisplayViewModel()
        {
            _key = "";
            _dateTo = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            _dateFrom = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            _paging = new PagingWPF();
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        private DateTime _dateTo;
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }

        private DateTime _dateFrom;
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }

        private PagingWPF _paging;
        public PagingWPF Paging
        {
            get { return _paging; }
            set { SetProperty(ref _paging, value); }
        }

        private BillDisplayDataModel _selectedBill;
        public BillDisplayDataModel SelectedBill
        {
            get { return _selectedBill; }
            set { SetProperty(ref _selectedBill, value); }
        }

        private ObservableCollection<BillDisplayDataModel> _bills;
        public ObservableCollection<BillDisplayDataModel> Bills
        {
            get { return _bills; }
            set { SetProperty(ref _bills, value); }
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
                Load();
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

        private RelayCommand _next;
        public RelayCommand Next
        {
            get
            {
                return _next
                    ?? (_next = new RelayCommand(NextMethod));
            }
        }
        private void NextMethod()
        {
            try
            {
                Paging.Next();
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Bills = new ObservableCollection<BillDisplayDataModel>(unitOfWork.Bills.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _previous;
        public RelayCommand Previous
        {
            get
            {
                return _previous
                    ?? (_previous = new RelayCommand(PreviousMethod));
            }
        }
        private void PreviousMethod()
        {
            try
            {
                Paging.Previous();
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Bills = new ObservableCollection<BillDisplayDataModel>(unitOfWork.Bills.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _show;
        public RelayCommand Show
        {
            get
            {
                return _show
                    ?? (_show = new RelayCommand(ShowMethod));
            }
        }
        private void ShowMethod()
        {
            try
            {
                BillShowViewModel.BillID = _selectedBill.Bill.ID;
                currentWindow.Hide();
                new BillShowWindow().ShowDialog();
                Load();
                currentWindow.ShowDialog();
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
