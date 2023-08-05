using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyGiaoDichXe.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        //Mọi thứ xử lý sẽ nămg trong này
        private string username;
        public string UserName { get => username; set { username = value; OnPropertyChanged(); } }
        private string password;
        public string Password { get => password; set { password = value; OnPropertyChanged(); } }
        public bool IsLogin { get; set; }//Được tạo ra để đăng nhập hay chưa
        public int IsAdmin { get; set; }
        public int NoAdmin { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand SubcribeCommand { get; set; }

        public LoginViewModel()
        {
            IsLogin = false;
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                bool isValidPassword = CheckPasswordValidity(UserName, Password);

                if (isValidPassword)
                {
                    if (IsAdmin == 1)
                    {
                        IsLogin = true;
                        Login(p);
                        p.Close();
                    }
                    else if(NoAdmin == 2)//Khách hàng
                    {
                        IsLogin = true;
                        Login(p);
                        p.Close();
                    }
                    else if (NoAdmin == 3)//Nhà cung cấp
                    {
                        IsLogin = true;
                        Login(p);
                        p.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu");
                }
            });

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                Password = p.Password;
            }
            );
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            }
            );
            SubcribeCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Hide(); // Ẩn cửa sổ đăng nhập

                SubcribeWindow sb = new SubcribeWindow();
                sb.Closed += (sender, args) =>
                {
                    p.Show(); // Hiển thị lại cửa sổ đăng nhập khi cửa sổ đăng ký được đóng
                };
                sb.Show();
            });

        }
        void Login(Window p)
        {
            if (p == null)
                return;

            // Kiểm tra xem mật khẩu có hợp lệ không
            bool isValidPassword = CheckPasswordValidity(UserName, Password);

            if (isValidPassword)
            {
                if (IsAdmin!=0)
                {
                    IsLogin = true;
                    p.Close();    
                }
            }
            else
            {
                // Xử lý khi mật khẩu không hợp lệ
                MessageBox.Show("Sai tài khoản hoặc mật khẩu");
            }
        }

        bool CheckPasswordValidity(string username, string password)
        {
            using (var dbContext = new Model.QuanLyGiaoDichXeDataContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
                if (user != null)
                {
                    var userRole = dbContext.UserRoles.FirstOrDefault(ur => ur.Id == user.IdRole);
                    if (userRole != null)
                    {
                        if (userRole.Id == 1)
                        {
                            // Gán quyền admin cho người dùng\
                            IsAdmin = 1;
                            // Code xử lý cấp quyền admin ở đây
                        }
                        else if (userRole.Id == 2)
                        {
                            // Đăng nhập với quyền Khách hàng
                            NoAdmin = 2;
                        }
                        else if (userRole.Id == 3)
                        {
                            // Đăng nhập với quyền nhà cung cấp
                            NoAdmin = 3;
                        }
                    }
                    return true;
                }
                return false;
            }
        }

    }
}
