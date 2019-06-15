using System.Collections.ObjectModel;
using MahApps.Metro.Controls;
using System.Windows;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using DAL.BindableBaseService;
using DAL.Entities;
using DTO.BillItemDataModel;
using BLL.UnitOfWorkService;
using DAL;
using System.Windows.Input;
using DTO.UserDataModel;
using Restaurant.Reports;
using DAL.ConstString;
using System.Diagnostics;
using MahApps.Metro.Controls.Dialogs;
using Restaurant.Views.CashierViews.FinishShiftViews;
using Restaurant.Views.CashierViews.ShiftSpendingViews;

namespace Restaurant.ViewModels.CashierViewModels
{
    public class BillItemsViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;
        private readonly FinishShiftDialog finishShiftDialog;

        public BillItemsViewModel()
        {
            _newBill = new Bill();
            _types = new ObservableCollection<string>();
            _billItems = new ObservableCollection<BillItemDisplayDataModel>();
            finishShiftDialog = new FinishShiftDialog();
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private Bill _newBill;
        public Bill NewBill
        {
            get { return _newBill; }
            set { SetProperty(ref _newBill, value); }
        }

        private Shift _shift;
        public Shift Shift
        {
            get { return _shift; }
            set { SetProperty(ref _shift, value); }
        }

        private BillItemDisplayDataModel _selectedBillItem;
        public BillItemDisplayDataModel SelectedBillItem
        {
            get { return _selectedBillItem; }
            set { SetProperty(ref _selectedBillItem, value); }
        }

        private ObservableCollection<string> _types;
        public ObservableCollection<string> Types
        {
            get { return _types; }
            set { SetProperty(ref _types, value); }
        }

        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
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

        private ObservableCollection<BillItemDisplayDataModel> _billItems;
        public ObservableCollection<BillItemDisplayDataModel> BillItems
        {
            get { return _billItems; }
            set { SetProperty(ref _billItems, value); }
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
                _types.Add("تيك اواى");
                _types.Add("صالة");
                _types.Add("دليفري");
                NewBill.Type = "تيك اواى";
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Categories = new ObservableCollection<Category>(unitOfWork.Categories.GetAll().OrderBy(o => o.Name));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand<int> _showItems;
        public RelayCommand<int> ShowItems
        {
            get
            {
                return _showItems
                    ?? (_showItems = new RelayCommand<int>(ShowItemsMethod));
            }
        }
        private void ShowItemsMethod(int id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Items = new ObservableCollection<Item>(unitOfWork.Items.Find(f => f.IsAvailable == true && f.CategoryID == id).OrderBy(o => o.Order).ThenBy(o => o.BillsItems.Count).ThenBy(o => o.Name));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _deleteItem;
        public RelayCommand DeleteItem
        {
            get
            {
                return _deleteItem
                    ?? (_deleteItem = new RelayCommand(DeleteItemMethod));
            }
        }
        private void DeleteItemMethod()
        {
            try
            {
                _billItems.Remove(_selectedBillItem);
                NewBill.Total = _billItems.Sum(s => Convert.ToDecimal(s.Total));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private RelayCommand<BillItemDisplayDataModel> _qtyChanged;
        public RelayCommand<BillItemDisplayDataModel> QtyChanged
        {
            get
            {
                return _qtyChanged
                    ?? (_qtyChanged = new RelayCommand<BillItemDisplayDataModel>(QtyChangedMethod));
            }
        }
        private void QtyChangedMethod(BillItemDisplayDataModel selectedBillItem)
        {
            try
            {
                if (selectedBillItem.Qty == null)
                    return;

                selectedBillItem.Total = selectedBillItem.Qty * selectedBillItem.Item.Price;
                NewBill.Total = _billItems.Sum(s => Convert.ToDecimal(s.Total));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand<Item> _add;
        public RelayCommand<Item> Add
        {
            get
            {
                return _add ?? (_add = new RelayCommand<Item>(
                    ExecuteAdd));
            }
        }
        private void ExecuteAdd(Item item)
        {
            try
            {
                _billItems.Add(new BillItemDisplayDataModel
                {
                    Item = item,
                    ItemID = item.ID,
                    Qty = 1,
                    Total = item.Price * 1
                });
                NewBill.Total = _billItems.Sum(s => Convert.ToDecimal(s.Total));
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
                if (BillItems.Count == 0)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    DateTime dt = DateTime.Now;
                    _newBill.Date = dt;
                    _newBill.RegistrationDate = dt;
                    _newBill.UserID = UserData.ID;
                    _newBill = unitOfWork.Bills.Add(_newBill);


                    foreach (var item in BillItems)
                    {
                        unitOfWork.BillsItems.Add(new BillItem
                        {
                            BillID = _newBill.ID,
                            ItemID = item.ItemID,
                            Price = item.Item.Price,
                            Qty = item.Qty,
                            Total = item.Qty * item.Item.Price
                        });
                    }
                    unitOfWork.Complete();

                    Safe safe = new Safe
                    {
                        Amount = _newBill.Total,
                        CanDelete = false,
                        RegistrationDate = dt,
                        Type = true,
                        UserID = UserData.ID,
                        Statement = $"فاتورة {_newBill.ID}"
                    };
                    unitOfWork.Safes.Add(safe);
                    unitOfWork.Complete();

                    Mouse.OverrideCursor = Cursors.Wait;
                    int rnd = new Random().Next(1000, 9999);
                    DS ds = new DS();
                    ds.Bill.Rows.Clear();
                    int i = 0;

                    foreach (var item in BillItems)
                    {
                        ds.Bill.Rows.Add();
                        ds.Bill[i]["BillID"] = $"#{rnd}#{_newBill.ID}#";
                        ds.Bill[i]["Date"] = DateTime.Now.ToShortDateString();
                        ds.Bill[i]["Time"] = DateTime.Now.ToString(" h:mm tt");
                        ds.Bill[i]["Type"] = _newBill.Type;
                        ds.Bill[i]["Details"] = _newBill.Details;
                        ds.Bill[i]["ItemQty"] = item.Qty;
                        ds.Bill[i]["ItemName"] = item.Item.Name;
                        ds.Bill[i]["ItemPrice"] = string.Format("{0:0.00}", item.Item.Price); ;
                        ds.Bill[i]["BillTotal"] = string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(_newBill.Total), 0));
                        i++;
                    }

                    BillItemsReport billItemsReport = new BillItemsReport();
                    billItemsReport.SetDataSource(ds.Tables["Bill"]);
                    Mouse.OverrideCursor = null;
                    billItemsReport.PrintToPrinter(1, false, 0, 15);

                    BillItems = new ObservableCollection<BillItemDisplayDataModel>();
                    NewBill = new Bill
                    {
                        Type = "تيك اواى"
                    };
                }
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
            if (BillItems.Count == 0)
                return false;
            else
                return true;
        }

        private RelayCommand _showCalc;
        public RelayCommand ShowCalc
        {
            get
            {
                return _showCalc ?? (_showCalc = new RelayCommand(
                    ExecuteShowCalc));
            }
        }
        private void ExecuteShowCalc()
        {
            try
            {
                Process.Start("calc.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Finish Shift

        private RelayCommand _showFinishShift;
        public RelayCommand ShowFinishShift
        {
            get
            {
                return _showFinishShift ?? (_showFinishShift = new RelayCommand(
                    ExecuteShowFinishShiftAsync));
            }
        }
        private async void ExecuteShowFinishShiftAsync()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Shift = unitOfWork.Shifts.FirstOrDefault(s => s.EndDate == null);
                    Shift.Spending = unitOfWork.Safes.Find(f => f.UserID == UserData.ID && f.Type == false && f.RegistrationDate >= _shift.StartDate && f.RegistrationDate <= DateTime.Now).Sum(s => s.Amount) ?? 0;

                    Shift.TotalItems = (unitOfWork.Bills.Find(f => f.UserID == UserData.ID && f.RegistrationDate >= _shift.StartDate && f.RegistrationDate <= DateTime.Now).Sum(s => s.Total) ?? 0);
                    Shift.Income = _shift.SafeStart + _shift.TotalItems;
                    Shift.Total = _shift.Income - _shift.Spending;

                }
                finishShiftDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(finishShiftDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _finishShift;
        public RelayCommand FinishShift
        {
            get
            {
                return _finishShift ?? (_finishShift = new RelayCommand(
                    ExecuteFinishShiftAsync,
                    CanExecuteFinishShift));
            }
        }
        private async void ExecuteFinishShiftAsync()
        {
            try
            {
                if (Shift.SafeEnd == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    _shift.EndDate = DateTime.Now;
                    unitOfWork.Shifts.Edit(_shift);
                    unitOfWork.Complete();
                    await currentWindow.HideMetroDialogAsync(finishShiftDialog);
                    new MainViewModel().ExecuteShutdown();
                }

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
        private bool CanExecuteFinishShift()
        {
            if (Shift.SafeEnd == null)
                return false;
            else
                return true;
        }

        private RelayCommand<string> _closeDialog;
        public RelayCommand<string> CloseDialog
        {
            get
            {
                return _closeDialog
                    ?? (_closeDialog = new RelayCommand<string>(ExecuteCloseDialogAsync));
            }
        }
        private async void ExecuteCloseDialogAsync(string parameter)
        {
            try
            {
                switch (parameter)
                {
                    case "FinishShift":
                        await currentWindow.HideMetroDialogAsync(finishShiftDialog);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Spending

        private RelayCommand _showSpending;
        public RelayCommand ShowSpending
        {
            get
            {
                return _showSpending
                    ?? (_showSpending = new RelayCommand(ShowSpendingMethod));
            }
        }
        private void ShowSpendingMethod()
        {
            try
            {
                new ShiftSpendingWindow().ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _shutdown;
        public RelayCommand Shutdown
        {
            get
            {
                return _shutdown ?? (_shutdown = new RelayCommand(
                    ExecuteShutdown));
            }
        }
        private void ExecuteShutdown()
        {
            try
            {
                if (UserData.Role == RoleText.Cashier)
                {
                    new MainViewModel().ExecuteShutdown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
