﻿using GalaSoft.MvvmLight.CommandWpf;
using Restaurant.Views.MainViews;
using Restaurant.Views.SafeViews;
using Restaurant.Views.SpendingViews;
using MahApps.Metro.Controls;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Forms;
using System;
using System.Data.Entity;
using System.Windows.Input;
using System.Data.SqlClient;
using Restaurant.Views.UserViews;
using Restaurant.Views.ShiftViews;
using DAL.BindableBaseService;
using BLL.UnitOfWorkService;
using DAL;
using DTO.MainDataModel;
using DAL.Entities;
using DAL.ConstString;
using Restaurant.Views.ItemViews;
using DTO.UserDataModel;
using System.IO;
using System.Data;
using DTO.ShiftDataModel;
using Restaurant.Views.BillViews;
using Restaurant.Views.CashierViews.BillItemsViews;

namespace Restaurant.ViewModels
{
    public class MainViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;
        public static bool IsSignOut { get; set; }

        private readonly BackupDialog backupDialog;
        private readonly RestoreBackupDialog restoreBackupDialog;
        private readonly Views.MainViews.LoginDialog loginDialog;
        private readonly StartShiftDialog startShiftDialog;

        public MainViewModel()
        {
            _isFocused = true;
            _loginModel = new LoginDataModel();
            currentWindow = System.Windows.Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            backupDialog = new BackupDialog();
            restoreBackupDialog = new RestoreBackupDialog();
            loginDialog = new Views.MainViews.LoginDialog();
            startShiftDialog = new StartShiftDialog();
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private LoginDataModel _loginModel;
        public LoginDataModel LoginModel
        {
            get { return _loginModel; }
            set { SetProperty(ref _loginModel, value); }
        }

        private StartShiftDataModel _newShiftModel;
        public StartShiftDataModel NewShiftModel
        {
            get { return _newShiftModel; }
            set { SetProperty(ref _newShiftModel, value); }
        }

        private BackupDataModel _backupModel;
        public BackupDataModel BackupModel
        {
            get { return _backupModel; }
            set { SetProperty(ref _backupModel, value); }
        }

        private RestoreBackupDataModel _restoreBackupModel;
        public RestoreBackupDataModel RestoreBackupModel
        {
            get { return _restoreBackupModel; }
            set { SetProperty(ref _restoreBackupModel, value); }
        }

        // Main

        private RelayCommand<string> _navigateToView;
        public RelayCommand<string> NavigateToView
        {
            get
            {
                return _navigateToView
                    ?? (_navigateToView = new RelayCommand<string>(NavigateToViewMethodAsync));
            }
        }
        public async void NavigateToViewMethodAsync(string destination)
        {
            try
            {
                switch (destination)
                {
                    case "Backup":
                        BackupModel = new BackupDataModel();
                        backupDialog.DataContext = this;
                        await currentWindow.ShowMetroDialogAsync(backupDialog);
                        break;

                    case "BackupRestore":
                        RestoreBackupModel = new RestoreBackupDataModel();
                        restoreBackupDialog.DataContext = this;
                        await currentWindow.ShowMetroDialogAsync(restoreBackupDialog);
                        break;

                    case "User":
                        currentWindow.Hide();
                        new UserWindow().ShowDialog();
                        currentWindow.Show();
                        break;

                    case "Shifts":
                        currentWindow.Hide();
                        new ShiftWindow().ShowDialog();
                        currentWindow.Show();
                        break;

                    case "SignOut":
                        LoginModel = new LoginDataModel();
                        loginDialog.DataContext = this;
                        await currentWindow.ShowMetroDialogAsync(loginDialog);
                        break;


                    case "Item":
                        currentWindow.Hide();
                        new ItemWindow().ShowDialog();
                        currentWindow.Show();
                        break;

                    case "Cashier":
                        currentWindow.Hide();
                        new BillItemsWindow().ShowDialog();
                        currentWindow.Show();
                        if(IsSignOut)
                        {
                            LoginModel = new LoginDataModel();
                            loginDialog.DataContext = this;
                            await currentWindow.ShowMetroDialogAsync(loginDialog);
                        }
                        break;

                    case "Bill":
                        currentWindow.Hide();
                        new BillWindow().ShowDialog();
                        currentWindow.Show();
                        break;

                    case "Spending":
                        currentWindow.Hide();
                        new SpendingWindow().ShowDialog();
                        currentWindow.Show();
                        break;

                    case "Safe":
                        currentWindow.Hide();
                        new SafeWindow().ShowDialog();
                        currentWindow.Show();
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

        // Login

        private RelayCommand _showLogin;
        public RelayCommand ShowLogin
        {
            get
            {
                return _showLogin ?? (_showLogin = new RelayCommand(
                    ExecuteShowLoginAsync));
            }
        }
        private async void ExecuteShowLoginAsync()
        {
            try
            {
                loginDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(loginDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private RelayCommand _signIn;
        public RelayCommand SignIn
        {
            get
            {
                return _signIn ?? (_signIn = new RelayCommand(
                    ExecuteSignInAsync,
                    CanExecuteSignIn));
            }
        }
        private async void ExecuteSignInAsync()
        {
            try
            {
                if (LoginModel.Name == null || LoginModel.Password == null)
                    return;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var user = unitOfWork.Users.SingleOrDefault(s => s.Name == _loginModel.Name && s.Password == _loginModel.Password && s.IsWorked == true);

                    if (user != null)
                    {
                        UserData.ID = user.ID;
                        UserData.Role = user.Role.Name;
                        UserData.Name = user.Name;
                        UserData.Password = user.Password;

                        if (UserData.Role == RoleText.Admin)
                        {
                            Mouse.OverrideCursor = null;
                            IsSignOut = false;
                            await currentWindow.HideMetroDialogAsync(loginDialog);
                        }
                        else
                        {
                            if (unitOfWork.Shifts.SingleOrDefault(s => s.UserID == user.ID && s.EndDate == null) != null)
                            {
                                Mouse.OverrideCursor = null;
                                await currentWindow.HideMetroDialogAsync(loginDialog);
                                IsSignOut = false;
                                NavigateToViewMethodAsync("Cashier");
                            }
                            else if (unitOfWork.Shifts.SingleOrDefault(s => s.UserID != user.ID && s.EndDate == null) != null)
                            {
                                Mouse.OverrideCursor = null;
                                await currentWindow.HideMetroDialogAsync(loginDialog);
                                await currentWindow.ShowMessageAsync("فشل الدخول", "يجب انهاء الشفت اولاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                                {
                                    AffirmativeButtonText = "موافق",
                                    DialogMessageFontSize = 25,
                                    DialogTitleFontSize = 30
                                });
                                await currentWindow.ShowMetroDialogAsync(loginDialog);
                            }
                            else
                            {
                                Mouse.OverrideCursor = null;
                                await currentWindow.HideMetroDialogAsync(loginDialog);
                                NewShiftModel = new StartShiftDataModel();
                                startShiftDialog.DataContext = this;
                                await currentWindow.ShowMetroDialogAsync(startShiftDialog);
                            }
                        }
                    }

                    else
                    {
                        Mouse.OverrideCursor = null;
                        await currentWindow.HideMetroDialogAsync(loginDialog);
                        await currentWindow.ShowMessageAsync("فشل الدخول", "يوجد خطأ فى الاسم أو الرقم السرى يرجى التأكد من البيانات", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                        await currentWindow.ShowMetroDialogAsync(loginDialog);
                    }
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
        private bool CanExecuteSignIn()
        {
            return !LoginModel.HasErrors;
        }

        // Start Shift

        private RelayCommand _startShift;
        public RelayCommand StartShift
        {
            get
            {
                return _startShift ?? (_startShift = new RelayCommand(
                    ExecuteStartShiftAsync,
                    CanExecuteStartShift));
            }
        }
        private async void ExecuteStartShiftAsync()
        {
            try
            {
                if (NewShiftModel.SafeStart == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    unitOfWork.Shifts.Add(new Shift
                    {
                        SafeStart = _newShiftModel.SafeStart,
                        StartDate = DateTime.Now,
                        UserID = UserData.ID
                    });
                    unitOfWork.Complete();
                }
                Mouse.OverrideCursor = null;
                await currentWindow.HideMetroDialogAsync(startShiftDialog);
                NavigateToViewMethodAsync("Cashier");
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
        private bool CanExecuteStartShift()
        {
            return !NewShiftModel.HasErrors;
        }

        // Backup

        private RelayCommand _browseFloder;
        public RelayCommand BrowseFloder
        {
            get
            {
                return _browseFloder ?? (_browseFloder = new RelayCommand(
                    ExecuteBrowseFloder));
            }
        }
        private void ExecuteBrowseFloder()
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowDialog();
                BackupModel.Path = fbd.SelectedPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _backup;
        public RelayCommand Backup
        {
            get
            {
                return _backup ?? (_backup = new RelayCommand(
                    ExecuteBackupAsync,
                    CanExecuteBackup));
            }
        }
        private async void ExecuteBackupAsync()
        {
            try
            {
                if (BackupModel.Path == null)
                    return;
                using (GeneralDBContext db = new GeneralDBContext())
                {
                    try
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        string fileName = BackupModel.Path + "\\RestaurantDB " + DateTime.Now.ToShortDateString().Replace('/', '-')
                                                + " - " + DateTime.Now.ToLongTimeString().Replace(':', '-');
                        string dbname = db.Database.Connection.Database;
                        string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'" + fileName + ".bak' WITH NOFORMAT, NOINIT,NAME = N'MyAir-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                        db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname));
                        BackupModel = new BackupDataModel();
                    }
                    catch
                    {
                        Mouse.OverrideCursor = null;
                        await currentWindow.HideMetroDialogAsync(backupDialog);
                        await currentWindow.ShowMessageAsync("فشل الحفظ", "يجب إختيار مكان آخر لحفظ النسخة الإحتياطية", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                        await currentWindow.ShowMetroDialogAsync(backupDialog);
                        return;
                    }
                    finally
                    {
                        Mouse.OverrideCursor = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteBackup()
        {
            return !BackupModel.HasErrors;
        }

        // Restore Backup

        private RelayCommand _browseFile;
        public RelayCommand BrowseFile
        {
            get
            {
                return _browseFile ?? (_browseFile = new RelayCommand(
                    ExecuteBrowseFile));
            }
        }
        private void ExecuteBrowseFile()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Title = "استرجاع نسخة احتياطية",
                    Filter = "Backup files(*.Bak)|*.Bak"
                };
                ofd.ShowDialog();
                RestoreBackupModel.Path = ofd.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _restore;
        public RelayCommand Restore
        {
            get
            {
                return _restore ?? (_restore = new RelayCommand(
                    ExecuteRestore,
                    CanExecuteRestore));
            }
        }
        private void ExecuteRestore()
        {
            try
            {
                if (RestoreBackupModel.Path == null)
                    return;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                SqlConnection sqlconnection = new SqlConnection(@"Server=.\sqlexpress; Database=master; Integrated Security=true");
                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlconnection
                };
                cmd.Parameters.Add("@fileName", SqlDbType.NChar).Value = RestoreBackupModel.Path;
                cmd.CommandText = "ALTER Database RestaurantDB SET OFFLINE WITH ROLLBACK IMMEDIATE; Restore Database RestaurantDB From Disk= @fileName WITH REPLACE";
                sqlconnection.Open();
                cmd.ExecuteNonQuery();
                string strQuery = "ALTER Database RestaurantDB SET ONLINE WITH ROLLBACK IMMEDIATE";
                cmd = new SqlCommand(strQuery, sqlconnection);
                cmd.ExecuteNonQuery();
                sqlconnection.Close();
                RestoreBackupModel = new RestoreBackupDataModel();
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
        private bool CanExecuteRestore()
        {
            return !RestoreBackupModel.HasErrors;
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
                    case "Backup":
                        await currentWindow.HideMetroDialogAsync(backupDialog);
                        break;
                    case "Restore":
                        await currentWindow.HideMetroDialogAsync(restoreBackupDialog);
                        break;
                    case "startShift":
                        await currentWindow.HideMetroDialogAsync(startShiftDialog);
                        currentWindow.Close();
                        break;
                    case "back":
                        await currentWindow.HideMetroDialogAsync(startShiftDialog);
                        LoginModel = new LoginDataModel();
                        loginDialog.DataContext = this;
                        await currentWindow.ShowMetroDialogAsync(loginDialog);
                        break;
                    case "Login":
                        await currentWindow.HideMetroDialogAsync(loginDialog);
                        currentWindow.Close();
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

        private RelayCommand _shutdown;
        public RelayCommand Shutdown
        {
            get
            {
                return _shutdown ?? (_shutdown = new RelayCommand(
                    ExecuteShutdown));
            }
        }
        public void ExecuteShutdown()
        {
            try
            {
                string path = "";
                bool exists;
                try
                {
                    path = @"D:\المطعم";
                    exists = Directory.Exists(path);
                    if (!exists)
                        Directory.CreateDirectory(path);
                }
                catch
                {

                }
                try
                {
                    path = @"E:\المطعم";
                    exists = Directory.Exists(path);
                    if (!exists)
                        Directory.CreateDirectory(path);
                }
                catch
                {
                    path = @"D:\المطعم";
                }
                using (GeneralDBContext db = new GeneralDBContext())
                {
                    try
                    {
                        string fileName = path + "\\RestaurantDB " + DateTime.Now.ToShortDateString().Replace('/', '-')
                                                + " - " + DateTime.Now.ToLongTimeString().Replace(':', '-');
                        string dbname = db.Database.Connection.Database;
                        string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'" + fileName + ".bak' WITH NOFORMAT, NOINIT,NAME = N'MyAir-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                        db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname));
                    }
                    catch
                    {
                    }
                }
                System.Windows.Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
