using BLL.UnitOfWorkService;
using DAL;
using DAL.BindableBaseService;
using DAL.Entities;
using DTO.BillItemDataModel;
using DTO.UserDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Restaurant.Reports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels.BillViewModels
{
    public class BillShowViewModel : ValidatableBindableBase
    {
        public static int BillID { get; set; }

        readonly MetroWindow currentWindow;

        public BillShowViewModel()
        {
            _types = new ObservableCollection<string>();
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private Bill _selectedBill;
        public Bill SelectedBill
        {
            get { return _selectedBill; }
            set { SetProperty(ref _selectedBill, value); }
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
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Categories = new ObservableCollection<Category>(unitOfWork.Categories.GetAll().OrderBy(o => o.Name));
                    SelectedBill = unitOfWork.Bills.Get(BillID);
                    BillItems = new ObservableCollection<BillItemDisplayDataModel>(unitOfWork.BillsItems.Find(f => f.BillID == BillID).Select(s => new BillItemDisplayDataModel
                    {
                        Item = s.Item,
                        ItemID = s.ItemID,
                        Qty = s.Qty,
                        Total = s.Total
                    }));
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
                    Items = new ObservableCollection<Item>(unitOfWork.Items.Find(f => f.IsAvailable == true && f.CategoryID == id).OrderBy(o => o.Order));
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
                SelectedBill.Total = _billItems.Sum(s => Convert.ToDecimal(s.Total));
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
                SelectedBill.Total = _billItems.Sum(s => Convert.ToDecimal(s.Total));
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
                SelectedBill.Total = _billItems.Sum(s => Convert.ToDecimal(s.Total));
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
                    ?? (_print = new RelayCommand(PrintMethod));
            }
        }
        private void PrintMethod()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    unitOfWork.Bills.Edit(_selectedBill);
                    unitOfWork.Safes.Remove(d => d.RegistrationDate == _selectedBill.RegistrationDate);
                    Safe safe = new Safe
                    {
                        Amount = _selectedBill.Total,
                        CanDelete = false,
                        RegistrationDate = _selectedBill.RegistrationDate,
                        Type = true,
                        Statement = $"فاتورة {_selectedBill.ID}",
                        UserID=(int)_selectedBill.UserID
                    };
                    unitOfWork.Safes.Add(safe);
                    unitOfWork.BillsItems.Remove(s => s.BillID == BillID);
                    foreach (var item in BillItems)
                    {
                        unitOfWork.BillsItems.Add(new BillItem
                        {
                            BillID = BillID,
                            ItemID = item.ItemID,
                            Price = item.Item.Price,
                            Qty = item.Qty,
                            Total = item.Qty * item.Item.Price
                        });
                    }
                    unitOfWork.Complete();

                    Mouse.OverrideCursor = Cursors.Wait;
                    int rnd = new Random().Next(1000, 9999);

                    List<int?> categoriesId = _billItems.Select(s => s.Item.CategoryID).Distinct().ToList();

                    foreach (var categoryId in categoriesId)
                    {
                        DS ds = new DS();
                        ds.Bill.Rows.Clear();
                        int i = 0;

                        foreach (var item in BillItems.Where(w => w.Item.CategoryID == categoryId))
                        {
                            ds.Bill.Rows.Add();
                            ds.Bill[i]["BillID"] = $"#{rnd}#{_selectedBill.ID}#";
                            ds.Bill[i]["Date"] = DateTime.Now.ToShortDateString();
                            ds.Bill[i]["Time"] = DateTime.Now.ToString(" h:mm tt");
                            ds.Bill[i]["Type"] = _selectedBill.Type;
                            ds.Bill[i]["Details"] = _selectedBill.Details;
                            ds.Bill[i]["ItemQty"] = item.Qty;
                            ds.Bill[i]["ItemName"] = item.Item.Name;
                            ds.Bill[i]["ItemPrice"] = string.Format("{0:0.00}", item.Item.Price); ;
                            ds.Bill[i]["BillTotal"] = string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(BillItems.Where(w => w.Item.CategoryID == categoryId).Sum(s => s.Total)), 0));
                            i++;
                        }

                        BillItemsReport billItemsReport = new BillItemsReport();
                        billItemsReport.SetDataSource(ds.Tables["Bill"]);
                        Mouse.OverrideCursor = null;
                        billItemsReport.PrintToPrinter(1, false, 0, 15);
                    }



                    currentWindow.Close();
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
    }
}
