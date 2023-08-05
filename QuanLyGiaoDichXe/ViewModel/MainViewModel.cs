using QuanLyGiaoDichXe.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyGiaoDichXe.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<TonKho> _TonkhoList;
        public ObservableCollection<TonKho> TonkhoList { get => _TonkhoList; set { _TonkhoList = value; OnPropertyChanged(); } }
        private DateTime _StartDate;
        public DateTime StartDate { get => _StartDate; set { _StartDate = value; OnPropertyChanged(); } }
        private DateTime _EndDate;
        public DateTime EndDate { get => _EndDate; set { _EndDate = value; OnPropertyChanged(); } }
        private int _numberOfCars;
        public int NumberOfCars
        {
            get { return _numberOfCars; }
            set
            {
                _numberOfCars = value;
                OnPropertyChanged();
            }
        }

        private bool _isUserCreated;
        public bool IsUserCreated
        {
            get { return _isUserCreated; }
            set
            {
                _isUserCreated = value;
                OnPropertyChanged();
            }
        }

        //Mọi thứ xử lý sẽ nămg trong này
        public bool Isloaded = false;
        public string UserName { get; set; }
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand UnitCommand { get; set; }
        public ICommand SuplierCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand ObjectCommand { get; set; }
        public ICommand UserCommand { get; set; }
        public int IsAdmin { get; set; }
        public int NoAdmin { get; set; }
        public ICommand InputCommand { get; set; }
        public ICommand OutputCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand LogoutCommand { get; set; }


        public MainViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                LoadMainWindow(p);
            }
            );
            UnitCommand = new RelayCommand<object>((p) => {
                return true;
            }, (p) =>
            {
                if (NoAdmin == 3)
                {
                    UnitWindow wd = new UnitWindow();
                    wd.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Bạn không có quyền");
                    return;
                }
            }
            );
            SuplierCommand = new RelayCommand<object>((p) => {
                return true;
            }, (p) => {
                if (NoAdmin == 3)
                {
                    SuplierWindow slwd = new SuplierWindow();
                    slwd.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Bạn không có quyền");
                    return;
                }
            }
            );
            CustomerCommand = new RelayCommand<object>((p) => {
                return true;
            }, (p) => {
                if (NoAdmin == 2)
                {
                    CustomerWindow slwd = new CustomerWindow();
                    slwd.ShowDialog();
                }
                else if (NoAdmin == 2)
                {
                }
                else
                {
                    MessageBox.Show("Bạn không có quyền");
                    return;
                }
            }
            );
            ObjectCommand = new RelayCommand<object>((p) => {
                return true;
            }, (p) => {
                if (NoAdmin == 3 || NoAdmin == 2)
                {
                    ObjectWindow slwd = new ObjectWindow();
                    slwd.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Bạn không có quyền");
                    return;
                }
            }
            );
            UserCommand = new RelayCommand<object>((p) => {
                return true;
            }, (p) => {
                if (IsAdmin == 1)
                {
                    UserWindow slwd = new UserWindow();
                    slwd.ShowDialog();
                }
                else if (NoAdmin == 2 || NoAdmin == 3)
                {
                    UserWindow slwd = new UserWindow();
                    slwd.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Bạn không có quyền");
                    return;
                }
            });
            InputCommand = new RelayCommand<object>((p) => {
                return true;
            }, (p) => {
                if (NoAdmin == 3)
                {
                    InputWindow slwd = new InputWindow();
                    slwd.ShowDialog();
                }
            }
            );
            OutputCommand = new RelayCommand<object>((p) => {
                return true;
            }, (p) => {
                if (NoAdmin == 3 || NoAdmin == 2)
                {
                    if (NoAdmin == 2)
                    {
                        OutputWindow outputWindow = new OutputWindow();
                        outputWindow.ShowDialog();
                    }
                    else if (NoAdmin == 3)
                    {
                        OutputWindow outputWindow = new OutputWindow();
                        outputWindow.ShowDialog();
                    }
                }

            }
            );
            FilterCommand = new RelayCommand<object>((p) => {
                return true;
            }, (p) => {
                if (NoAdmin == 3)
                {
                    FilterData();
                    loadTonKhoData();
                }
                else
                {
                    MessageBox.Show("Bạn không có quyền");
                    return;
                }
            }
            );

    }

    private void FilterData()
        {
            using (var db = new QuanLyGiaoDichXeDataContext())
            {
                var totalQuantity = (
                    from input in db.Inputs
                    join inputInfo in db.InputInfos on input.Id equals inputInfo.IdInput
                    where input.DateInput >= StartDate
                    select inputInfo.count).Sum();
                if(totalQuantity==null)
                {
                    MessageBox.Show("Không còn xe");
                    return;
                }    
                // Cập nhật số lượng xe
                NumberOfCars = (int)totalQuantity;
            }
        }
        void loadTonKhoData()
        {
            TonkhoList = new ObservableCollection<TonKho>();
            using (var db = new QuanLyGiaoDichXeDataContext())
            {
                var objectList = db.Objects.ToList();
                int i = 1;
                foreach (var item in objectList)
                {
                    var inputList = db.InputInfos.Where(p => p.IdObject == item.Id).Sum(p => p.count);
                    var outputList = db.OutputInfos.Where(p => p.IdObject == item.Id).Sum(p => p.count);

                    int sumInput = 0;
                    int sumOutput = 0;
                    if (inputList != null)
                    {
                        sumInput = (int)inputList;
                    }
                    if (outputList != null)
                    {
                        sumOutput = (int)outputList;
                    }
                    TonKho tonkho = new TonKho();
                    tonkho.STT = i;
                    tonkho.Count = sumInput - sumOutput;
                    if(tonkho.Count<0)
                    {
                        tonkho.Count = 0;
                    }    
                    tonkho.Object = item;

                    TonkhoList.Add(tonkho);
                    i++;
                }
            }
        }
        private void LoadMainWindow(Window mainWindow)
        {
            Isloaded = true;
            if (mainWindow == null)
                return;
            mainWindow.Hide();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            if (loginWindow.DataContext == null)
                return;
            var loginVM = loginWindow.DataContext as LoginViewModel;
            if (loginVM.IsLogin)
            {
                if (loginVM.IsAdmin == 1)
                {
                    mainWindow.Show();
                    IsAdmin = loginVM.IsAdmin;
                }
                else if (loginVM.NoAdmin == 3) // Nhà cung cấp
                {
                    mainWindow.Show();
                    loadTonKhoData();
                    NoAdmin = loginVM.NoAdmin;
                }
                else if (loginVM.NoAdmin == 2) // Khách hàng
                {
                    mainWindow.Show();
                    NoAdmin = loginVM.NoAdmin;
                }
            }
            else
            {
                mainWindow.Close();
            }
        }
    }
}
