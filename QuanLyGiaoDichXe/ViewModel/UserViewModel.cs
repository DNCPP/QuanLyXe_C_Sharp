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
    public class UserViewModel:BaseViewModel
    {
        private ObservableCollection<User> _List;
        public ObservableCollection<User> List { get => _List; set { _List = value; OnPropertyChanged(); } }
        private ObservableCollection<UserRole> _UserRole;
        public ObservableCollection<UserRole> UserRole { get => _UserRole; set { _UserRole = value; OnPropertyChanged(); } }

        
        private Model.User _SelectedItem;
        public Model.User SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Username = SelectedItem.Username;
                    SelectedUserRole = SelectedItem.UserRole;
                    DisplayName = SelectedItem.DisplayName;
                    Password = SelectedItem.Password;
                }
            }
        }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        private string _RePassword;
        public string RePassword { get => _RePassword; set { _RePassword = value; OnPropertyChanged(); } }

        private string _Username;
        public string Username { get => _Username; set { _Username = value;OnPropertyChanged(); } }
        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }
        private Model.UserRole _SelectedUserRole;
        public Model.UserRole SelectedUserRole
        {
            get => _SelectedUserRole; set { _SelectedUserRole = value; OnPropertyChanged(); }
        }

        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }

        private QuanLyGiaoDichXeDataContext _dbContext;
        public UserViewModel()
        {
            string username;
            LoginWindow loginWindow = new LoginWindow();
            var loginVM = loginWindow.DataContext as LoginViewModel;
            username = loginVM.UserName;
            _dbContext = new QuanLyGiaoDichXeDataContext();
            // Hiển thị danh sách đơn vị tính lên CustomerViewModel
            if(username=="admin")
            {
                List = new ObservableCollection<Model.User>(_dbContext.Users);
            }
            else
            {
                List = new ObservableCollection<Model.User>(_dbContext.Users.Where(i => i.Username == username));
            }
            UserRole = new ObservableCollection<Model.UserRole>(_dbContext.UserRoles);
            EditCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                using (var _dbContext = new QuanLyGiaoDichXeDataContext())
                {
                    var user = _dbContext.Users.FirstOrDefault(o => o.Username == username);
                    if (user!=null)
                    {
                        user.DisplayName = DisplayName;
                        user.Username = Username;
                        _dbContext.SubmitChanges();
                        SelectedItem.DisplayName = user.DisplayName;
                        SelectedItem.Username = user.Username;
                    }         
                }
            });
            DeleteCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                // Tìm và xóa các bản ghi trong bảng OutputInfo liên quan đến khách hàng
                var user = _dbContext.Users.FirstOrDefault(o => o.Id == SelectedItem.Id);
                _dbContext.Users.DeleteOnSubmit(user);
                // Xóa thành công
                _dbContext.SubmitChanges();
                // Xóa khỏi danh sách
                List.Remove(SelectedItem);
                SelectedItem = null;
            });
            ChangePasswordCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                // Tìm và xóa các bản ghi trong bảng OutputInfo liên quan đến khách hàng
                var user = _dbContext.Users.FirstOrDefault(i => i.Username == username);
                if(user != null)
                {
                    if(Password!=RePassword)
                    {
                        MessageBox.Show("Mật khẩu không trùng");
                        return;
                    }    
                    else if(Password==RePassword)
                    {
                        user.Password = Password;
                        _dbContext.SubmitChanges();
                        MessageBox.Show("Cập nhật thành công");
                    }    
                }
            });
        }

    }
}
