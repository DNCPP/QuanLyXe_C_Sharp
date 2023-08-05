using QuanLyGiaoDichXe.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyGiaoDichXe.ViewModel
{
    public class CustomerViewModel : BaseViewModel
    {
        private ObservableCollection<Customer> _List;

        public ObservableCollection<Customer> List { get => _List; set { _List = value; OnPropertyChanged(); } }


        private Customer _SelectedItem;
        public Customer SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Address = SelectedItem.Address;
                    Phone = SelectedItem.Phone;
                    Email = SelectedItem.Email;
                    MoreInfo = SelectedItem.MoreInfo;
                    DisplayName = SelectedItem.DisplayName;
                    ContractDate = SelectedItem.ContractDate;
                }
            }
        }
        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _Address;
        public string Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _MoreInfo;
        public string MoreInfo { get => _MoreInfo; set { _MoreInfo = value; OnPropertyChanged(); } }

        private DateTime? _ContractDate;
        public DateTime? ContractDate { get => _ContractDate; set { _ContractDate = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        private QuanLyGiaoDichXeDataContext _dbContext;
        public CustomerViewModel()
        {
            string username;
            LoginWindow loginWindow = new LoginWindow();
            var loginVM = loginWindow.DataContext as LoginViewModel;
            username = loginVM.UserName;
            _dbContext = new QuanLyGiaoDichXeDataContext();
            // Hiển thị danh sách đơn vị tính lên CustomerViewModel
            List = new ObservableCollection<Customer>(_dbContext.Customers.Where(o=>o.Username==username));

            AddCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                var isCustomerExist = _dbContext.Customers.Any(x => x.Username == username);
                if (!isCustomerExist)
                {
                    var customer = new Customer()
                    {
                        DisplayName = DisplayName,
                        Address = Address,
                        Phone = Phone,
                        Email = Email,
                        MoreInfo = MoreInfo,
                        ContractDate = ContractDate,
                        Username = username // Gán username cho khách hàng mới
                    };

                    _dbContext.Customers.InsertOnSubmit(customer);
                    _dbContext.SubmitChanges();

                    List.Add(customer); // Cập nhật lại danh sách
                }
            });


            EditCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                var customer = _dbContext.Customers.FirstOrDefault(x => x.Id == SelectedItem.Id);
                if (customer != null)
                {
                    customer.DisplayName = DisplayName;
                    customer.Address = Address;
                    customer.Phone = Phone;
                    customer.Email = Email;
                    customer.MoreInfo = MoreInfo;
                    customer.ContractDate = ContractDate;
                    SelectedItem.DisplayName = DisplayName;
                    SelectedItem.Address = Address;
                    SelectedItem.Phone = Phone;
                    SelectedItem.Email = Email;
                    SelectedItem.MoreInfo = MoreInfo;
                    SelectedItem.ContractDate = ContractDate;
                }
            });

        }
    }
}
