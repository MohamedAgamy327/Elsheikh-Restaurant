using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Restaurant.Views.ItemViews;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DAL.BindableBaseService;
using Utilities.Paging;
using BLL.UnitOfWorkService;
using DAL;
using DTO.CategoryDataModel;

namespace Restaurant.ViewModels.ItemViewModels
{
    public class CategoryDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;
        private readonly CategoryAddDialog categoryAddDialog;
        private readonly CategoryUpdateDialog categoryUpdateDialog;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Categories.GetRecordsNumber(_key);
                Paging.GetFirst();
                Categories = new ObservableCollection<CategoryDisplayDataModel>(unitOfWork.Categories.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public CategoryDisplayViewModel()
        {
            _key = "";
            _isFocused = true;
            _paging = new PagingWPF();
            categoryAddDialog = new CategoryAddDialog();
            categoryUpdateDialog = new CategoryUpdateDialog();
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

        private CategoryDisplayDataModel _selectedCategory;
        public CategoryDisplayDataModel SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }

        private CategoryAddDataModel _newCategory;
        public CategoryAddDataModel NewCategory
        {
            get { return _newCategory; }
            set { SetProperty(ref _newCategory, value); }
        }

        private CategoryUpdateDataModel _categoryUpdate;
        public CategoryUpdateDataModel CategoryUpdate
        {
            get { return _categoryUpdate; }
            set { SetProperty(ref _categoryUpdate, value); }
        }

        private ObservableCollection<CategoryDisplayDataModel> _categories;
        public ObservableCollection<CategoryDisplayDataModel> Categories
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
                    Categories = new ObservableCollection<CategoryDisplayDataModel>(unitOfWork.Categories.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Categories = new ObservableCollection<CategoryDisplayDataModel>(unitOfWork.Categories.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _delete;
        public RelayCommand Delete
        {
            get
            {
                return _delete
                    ?? (_delete = new RelayCommand(DeleteMethodAsync));
            }
        }
        private async void DeleteMethodAsync()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    MessageDialogResult result = await currentWindow.ShowMessageAsync("تأكيد الحذف", "هل تـريــد حــذف هـذا النوع؟", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "موافق",
                        NegativeButtonText = "الغاء",
                        DialogMessageFontSize = 25,
                        DialogTitleFontSize = 30
                    });
                    if (result == MessageDialogResult.Affirmative)
                    {
                        unitOfWork.Categories.Remove(_selectedCategory.Category);
                        unitOfWork.Complete();
                        Load();
                    }
                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Add

        private RelayCommand _showAdd;
        public RelayCommand ShowAdd
        {
            get
            {
                return _showAdd
                    ?? (_showAdd = new RelayCommand(ShowAddMethod));
            }
        }
        private async void ShowAddMethod()
        {
            try
            {
                NewCategory = new CategoryAddDataModel();
                categoryAddDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(categoryAddDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _save;
        public RelayCommand Save
        {
            get
            {
                return _save ?? (_save = new RelayCommand(
                    ExecuteSaveAsync,
                    CanExecuteSave));
            }
        }
        private async void ExecuteSaveAsync()
        {
            try
            {
                if (NewCategory.Name == null )
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var category = unitOfWork.Categories.SingleOrDefault(s => s.Name == _newCategory.Name);

                    if (category != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذاالنوع موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else
                    {
                        unitOfWork.Categories.Add(new DAL.Entities.Category
                        {
                            Name = _newCategory.Name
                        });
                        unitOfWork.Complete();
                        NewCategory = new CategoryAddDataModel();
                        Load();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteSave()
        {
            if (NewCategory.HasErrors)
                return false;
            else
                return true;
        }

        // Update

        private RelayCommand _showUpdate;
        public RelayCommand ShowUpdate
        {
            get
            {
                return _showUpdate
                    ?? (_showUpdate = new RelayCommand(ShowUpdateMethod));
            }
        }
        private async void ShowUpdateMethod()
        {
            try
            {
                categoryUpdateDialog.DataContext = this;
                CategoryUpdate = new CategoryUpdateDataModel
                {
                    Name = _selectedCategory.Category.Name,
                    ID = _selectedCategory.Category.ID
                };

                await currentWindow.ShowMetroDialogAsync(categoryUpdateDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private RelayCommand _update;
        public RelayCommand Update
        {
            get
            {
                return _update ?? (_update = new RelayCommand(
                    ExecuteUpdateAsync,
                    CanExecuteUpdate));
            }
        }
        private async void ExecuteUpdateAsync()
        {
            try
            {
                if (CategoryUpdate.Name == null )
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var category = unitOfWork.Categories.SingleOrDefault(s => s.Name == CategoryUpdate.Name && s.ID != CategoryUpdate.ID);
                    if (category != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذاالنوع موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else
                    {
                        SelectedCategory.Category.Name = CategoryUpdate.Name;
                        unitOfWork.Categories.Edit(_selectedCategory.Category);
                        unitOfWork.Complete();
                        await currentWindow.HideMetroDialogAsync(categoryUpdateDialog);
                        Load();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteUpdate()
        {
            try
            {
                return !CategoryUpdate.HasErrors;
            }
            catch
            {
                return false;
            }
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
                    case "Add":
                        await currentWindow.HideMetroDialogAsync(categoryAddDialog);
                        break;
                    case "Update":
                        await currentWindow.HideMetroDialogAsync(categoryUpdateDialog);
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
    }
}
