using QuanLyGiaoDichXe.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Object = QuanLyGiaoDichXe.Model.Object;
using Customer = QuanLyGiaoDichXe.Model.Customer;
using InputInfo = QuanLyGiaoDichXe.Model.InputInfo;
using System.Windows.Controls;

namespace QuanLyGiaoDichXe.ViewModel
{
    public class OutputViewModel:BaseViewModel
    {       
        private ObservableCollection<Model.OutputInfo> _List;
        public ObservableCollection<Model.OutputInfo> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Output> _Output;
        public ObservableCollection<Model.Output> Output { get => _Output; set { _Output = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Object> _Object;
        public ObservableCollection<Model.Object> Object { get => _Object; set { _Object = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Customer> _Customer;
        public ObservableCollection<Model.Customer> Customer { get => _Customer; set { _Customer = value; OnPropertyChanged(); } }
        private ObservableCollection<Model.Suplier> _Suplier;
        public ObservableCollection<Model.Suplier> Suplier { get => _Suplier; set { _Suplier = value; OnPropertyChanged(); } }
        private ObservableCollection<Model.InputInfo> _InputInfo;
        public ObservableCollection<Model.InputInfo> InputInfo { get => _InputInfo; set { _InputInfo = value; OnPropertyChanged(); } }

        private Model.OutputInfo _SelectedItem;
        public Model.OutputInfo SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    OjectDisplayName = SelectedItem.Object.DisplayName;
                    DateOutput = SelectedItem.Output.DateInput;
                    Count = (int)SelectedItem.count;
                    Status = SelectedItem.Status;
                    PriceOutput = SelectedItem.InputInfo.OutputPrice;
                    CustomerDisplayName = SelectedItem.Customer.DisplayName;
                    Phone = SelectedItem.Customer.Phone;
                }
            }
        }

        private string _OjectDisplayName;
        public string OjectDisplayName
        {
            get => _OjectDisplayName; set { _OjectDisplayName = value; OnPropertyChanged(); }
        }
        private double? _PriceOutput;
        public double? PriceOutput
        {
            get => _PriceOutput; set { _PriceOutput = value; OnPropertyChanged(); }
        }
        private DateTime? _DateOutput;
        public DateTime? DateOutput
        {
            get => _DateOutput; set { _DateOutput = value; OnPropertyChanged(); }
        }

        private int _Count;
        public int Count { get => _Count; set { _Count = value; OnPropertyChanged(); } }
        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }

        private string _CustomerDisplayName;
        public string CustomerDisplayName { get => _CustomerDisplayName; set { _CustomerDisplayName = value; OnPropertyChanged(); } }
        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }
        public ICommand AddCommand { get; set; }
        public ICommand ReLoadData { get; set; }
        private QuanLyGiaoDichXeDataContext _dbContext;
        private void ReloadData()
        {
            // Thực hiện lại việc tải dữ liệu từ cơ sở dữ liệu và cập nhật danh sách List
            LoadData();
        }
        public OutputViewModel()
        {
            _dbContext = new QuanLyGiaoDichXeDataContext();
            // Hiển thị danh sách đơn vị tính lên InputViewModel
            LoadData();
            ReLoadData = new RelayCommand<object>((p) =>
            {

                return true;
            }, (p) =>
            {
                ReloadData();
            });        
        }

        private void LoadData()
        {
            string username;
            LoginWindow loginWindow = new LoginWindow();
            var loginVM = loginWindow.DataContext as LoginViewModel;
            username = loginVM.UserName; 
            if (_dbContext.Users.FirstOrDefault(u => u.Username == username && u.IdRole == 2) != null)
            {
                var selectedCustomer = _dbContext.Customers.FirstOrDefault(c => c.Username == username);
                var selectedObject = _dbContext.Objects.FirstOrDefault();
                var idObj = from obj in _dbContext.Objects
                            select obj.Id;

                if (selectedObject != null)
                {
                    int count = _dbContext.OutputInfos.Count(a => a.IdCustomer == selectedCustomer.Id);
                    int count1 = _dbContext.OutputInfos.Count(i => idObj.Contains(i.IdObject));
                    int count2 = _dbContext.Supliers.Count(e => e.Id == selectedObject.IdSuplier);
                    if (count!=0&&count1!=0&&count2!=0)
                    {
                        Object = new ObservableCollection<Object>(_dbContext.Objects.ToList());

                        // Lấy thông tin khách hàng của người dùng
                        Customer = new ObservableCollection<Customer>(_dbContext.Customers.Where(u => u.Username == username));

                        // Lấy danh sách OutputInfo của người dùng dựa trên idCustomer
                        List = new ObservableCollection<OutputInfo>(_dbContext.OutputInfos.Where(o => o.IdCustomer == Customer.FirstOrDefault().Id));
                    }
                }
            }
            else if (_dbContext.Users.FirstOrDefault(u => u.Username == username && u.IdRole == 3) != null)
            {
                // Lấy thông tin khách hàng của người dùng
                // Lấy thông tin khách hàng của người dùng
                // Lấy thông tin khách hàng của người dùng
                var supplier = _dbContext.Supliers.FirstOrDefault(c => c.Username == username);
                if (supplier != null)
                {
                    // Lấy danh sách Object
                    var objectIds = _dbContext.Objects.Where(e => e.IdSuplier == supplier.Id).Select(o => o.Id).ToList();
                    Object = new ObservableCollection<Object>(_dbContext.Objects.Where(e => objectIds.Contains(e.Id)));

                    // Lấy danh sách OutputInfo của người dùng
                    List = new ObservableCollection<OutputInfo>(_dbContext.OutputInfos.Where(o => objectIds.Contains(o.IdObject)));
                }
            }
        }
    }
}

