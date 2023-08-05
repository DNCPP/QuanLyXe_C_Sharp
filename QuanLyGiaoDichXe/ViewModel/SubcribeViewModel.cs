using QuanLyGiaoDichXe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyGiaoDichXe.ViewModel
{
    public class SubcribeViewModel:BaseViewModel
    {
        private string username;
        public string Username { get=> username; set{ username = value;OnPropertyChanged(); } }
        private string password;
        public string Password { get=> password; set{ password = value;OnPropertyChanged(); } }
        private string _ipassword;
        public string iPassword { get => _ipassword; set { _ipassword = value; OnPropertyChanged(); } }
        public ICommand SubPassword { get; set; }
        public ICommand SubiPassword { get; set; }
        public ICommand AcceptCommand { get; set; }

        public SubcribeViewModel()
        {
            SubPassword = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                Password = p.Password;
            }
            );
            SubiPassword = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                iPassword = p.Password;
            }
            );
            AcceptCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (CheckPassword(Username, Password, iPassword))
                {
                    using (var db = new QuanLyGiaoDichXeDataContext())
                    {
                        var kiemtra = db.Users.Any(i => i.Username == Username);
                        if(!kiemtra)
                        {
                            var newUser = new Model.User
                            {
                                Username = Username,
                                Password = Password,
                                IdRole = 2 // Đặt IdRole theo quy tắc của bạn
                            };

                            db.Users.InsertOnSubmit(newUser);
                            db.SubmitChanges();

                            MessageBox.Show("Đăng ký thành công!");

                            // Đóng cửa sổ sau khi đăng ký thành công
                            p.Close();
                        }  
                        else
                        {
                            MessageBox.Show("Tên đăng nhập đã tồn tại!");

                        }
                    }
                }
            });
        }
        bool CheckPassword(string username, string password, string ipassword)
        {
            if (string.IsNullOrEmpty(Username))
            {
                MessageBox.Show("Bạn chưa nhập Tên đăng nhập");
                return false;
            }
            else if (Password != iPassword)
            {
                if (Password != iPassword)
                {
                    MessageBox.Show("Bạn đã nhập mật khẩu không trùng nhau");
                    return false;
                }
            }
            return true;
        }
    }
}
