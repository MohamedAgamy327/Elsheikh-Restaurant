using BLL.UnitOfWorkService;
using DAL;
using DAL.BindableBaseService;
using DAL.Entities;
using DTO.ItemDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Utilities.Paging;

namespace Restaurant.ViewModels.ItemViewModels
{
    public class ItemOrderViewModel : ValidatableBindableBase
    {
        readonly MetroWindow currentWindow;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Items.GetRecordsNumber(_selectedCategory.ID);
                Items = new ObservableCollection<ItemOrderDataModel>(unitOfWork.Items.Search(_selectedCategory.ID));
                //int i = 1;
                //foreach (var item in Items)
                //{
                //    item.Item.Order = i;
                //    unitOfWork.Items.Edit(item.Item);
                //    i++;
                //}
                //unitOfWork.Complete();
            }
        }

        public ItemOrderViewModel()
        {
            _key = "";
            _isFocused = true;
            _paging = new PagingWPF();
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
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

        private PagingWPF _paging;
        public PagingWPF Paging
        {
            get { return _paging; }
            set { SetProperty(ref _paging, value); }
        }

        private ItemOrderDataModel _selectedItem;
        public ItemOrderDataModel SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }

        private ObservableCollection<ItemOrderDataModel> _items;
        public ObservableCollection<ItemOrderDataModel> Items
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

        private RelayCommand _moveUp;
        public RelayCommand MoveUp
        {
            get
            {
                return _moveUp
                    ?? (_moveUp = new RelayCommand(MoveUpMethod));
            }
        }
        private void MoveUpMethod()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Item previousItem = unitOfWork.Items.FirstOrDefault(f => f.Order == _selectedItem.Item.Order-1 && f.CategoryID==_selectedCategory.ID);
                    previousItem.Order += 1;
                    unitOfWork.Items.Edit(previousItem);
                    _selectedItem.Item.Order -= 1;
                    unitOfWork.Items.Edit(_selectedItem.Item);
                    unitOfWork.Complete();
                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _moveDown;
        public RelayCommand MoveDown
        {
            get
            {
                return _moveDown
                    ?? (_moveDown = new RelayCommand(MoveDownMethod));
            }
        }
        private void MoveDownMethod()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Item nextItem = unitOfWork.Items.FirstOrDefault(f => f.Order == _selectedItem.Item.Order + 1 && f.CategoryID == _selectedCategory.ID);
                    nextItem.Order -= 1;
                    unitOfWork.Items.Edit(nextItem);
                    _selectedItem.Item.Order += 1;
                    unitOfWork.Items.Edit(_selectedItem.Item);
                    unitOfWork.Complete();
                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
