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
    class SuplierViewModel : BaseViewModel
    {
        private ObservableCollection<Suplier> _List;

        public ObservableCollection<Suplier> List { get => _List; set { _List = value; OnPropertyChanged(); } }


        private Suplier _SelectedItem;
        public Suplier SelectedItem
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
        public SuplierViewModel()
        {
            string username;
            LoginWindow loginWindow = new LoginWindow();
            var loginVM = loginWindow.DataContext as LoginViewModel;
            username = loginVM.UserName;
            _dbContext = new QuanLyGiaoDichXeDataContext();
            List = new ObservableCollection<Suplier>(_dbContext.Supliers.Where(o=>o.Username==username));
            EditCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                var Suplier = _dbContext.Supliers.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                Suplier.DisplayName = DisplayName;
                Suplier.Address = Address;
                Suplier.Phone = Phone;
                Suplier.Email = Email;
                Suplier.MoreInfo = MoreInfo;
                Suplier.ContractDate = ContractDate;
                _dbContext.SubmitChanges();
                SelectedItem.DisplayName = DisplayName;
                SelectedItem.Address = Address;
                SelectedItem.Phone = Phone;
                SelectedItem.Email = Email;
                SelectedItem.MoreInfo = MoreInfo;
                SelectedItem.ContractDate = ContractDate;                   
            });
        } 
    }
}
