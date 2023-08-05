using QuanLyGiaoDichXe.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Object = QuanLyGiaoDichXe.Model.Object;
using Suplier = QuanLyGiaoDichXe.Model.Suplier;

namespace QuanLyGiaoDichXe.ViewModel
{
    public class InputViewModel : BaseViewModel
    {
        private ObservableCollection<Model.InputInfo> _List;
        public ObservableCollection<Model.InputInfo> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Input> _Input;
        public ObservableCollection<Model.Input> Input { get => _Input; set { _Input = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Object> _Object;
        public ObservableCollection<Model.Object> Object { get => _Object; set { _Object = value; OnPropertyChanged(); } }


        private Model.InputInfo _SelectedItem;
        public Model.InputInfo SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    SelectObject = SelectedItem.Object.DisplayName;
                    SelectedDate = SelectedItem.Input.DateInput;
                    count = (int)SelectedItem.count;
                    InputPrice = SelectedItem.InputPrice;
                    OutputPrice = SelectedItem.OutputPrice;
                    Status = SelectedItem.Status;
                }
            }
        }

        private string _Id;
        public string Id
        {
            get => _Id; set { _Id = value; OnPropertyChanged(); }
        }
        private string _SelectObject;
        public string SelectObject
        {
            get => _SelectObject; set { _SelectObject = value; OnPropertyChanged(); }
        }

        private DateTime? _SelectedDate;
        public DateTime? SelectedDate
        {
            get => _SelectedDate; set { _SelectedDate = value; OnPropertyChanged(); }
        }

        private int _Count;
        public int count { get => _Count; set { _Count = value; OnPropertyChanged(); } }

        private double? _InputPrice;
        public double? InputPrice { get => _InputPrice; set { _InputPrice = value; OnPropertyChanged(); } }

        private double? _OutputPrice;
        public double? OutputPrice { get => _OutputPrice; set { _OutputPrice = value; OnPropertyChanged(); } }

        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        private QuanLyGiaoDichXeDataContext _dbContext;
        public InputViewModel()
        {
            string username;
            LoginWindow loginWindow = new LoginWindow();
            var loginVM = loginWindow.DataContext as LoginViewModel;
            username = loginVM.UserName;
            //Hiển thị danh sách đơn vị tính lên InputViewModel
            _dbContext = new QuanLyGiaoDichXeDataContext();
            LoadData();

            AddCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                // Kiểm tra các giá trị cần thiết trước khi thêm vào bảng InputInfo
                if (SelectedDate != null && count > 0 && InputPrice != null && OutputPrice != null && !string.IsNullOrEmpty(Status))
                {
                    string displayName = SelectObject;
                    string idObject = _dbContext.Objects.FirstOrDefault(obj => obj.DisplayName == displayName).Id;
                    //Kiểm tra Id mới nhập có tồn tại trong bảng Object không?
                    bool isExisting = _dbContext.Objects.Any(obj => obj.DisplayName == displayName&&obj.Id== idObject);
                    if (isExisting)
                    {
                        // Tạo một đối tượng Input và InputInfo mới
                        var input = new Input()
                        {
                            Id = Guid.NewGuid().ToString(), // Tạo một ID mới
                            DateInput = SelectedDate.Value // Lấy giá trị ngày nhập
                        };
                        var inputInfo = new InputInfo
                        {
                            Id = Guid.NewGuid().ToString().Substring(0, 7), // Tạo một ID mới
                            IdObject = idObject, // Lấy ID của đối tượng được chọn
                            IdInput = input.Id, // Sử dụng ID của Input vừa tạo
                            count = count, // Lấy giá trị số lượng
                            InputPrice = InputPrice.Value, // Lấy giá trị giá nhập
                            OutputPrice = OutputPrice.Value, // Lấy giá trị giá xuất
                            Status = Status // Lấy giá trị trạng thái
                        };
                        // Thêm Input và InputInfo vào cơ sở dữ liệu
                        _dbContext.Inputs.InsertOnSubmit(input);
                        _dbContext.InputInfos.InsertOnSubmit(inputInfo);
                        _dbContext.SubmitChanges();
                        LoadData();
                        //// Cập nhật lại danh sách List
                        //List.Add(inputInfo);
                    }
                    else
                    {
                        MessageBox.Show("Xe không tồn tại trong Quản lý xe");
                        return;
                    }
                }
            });
            DeleteCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                // Xóa dòng được chọn từ danh sách List
                if (SelectedItem != null)
                {
                    var deleteInputInfo = _dbContext.InputInfos.FirstOrDefault(o => o.Id == SelectedItem.Id);
                    string idInputInfo = SelectedItem.Id;
                    var deleteInput = _dbContext.Inputs.FirstOrDefault(i => i.Id == SelectedItem.IdInput);
                    var deleteOutputInfo = _dbContext.OutputInfos.FirstOrDefault(i => i.IdInputInfor == idInputInfo);

                    if(deleteOutputInfo!=null)
                    {
                        _dbContext.OutputInfos.DeleteOnSubmit(deleteOutputInfo);
                        _dbContext.SubmitChanges();
                        List.Remove(SelectedItem);
                    }    
                    if(deleteInputInfo!=null)
                    {
                        _dbContext.InputInfos.DeleteOnSubmit(deleteInputInfo);
                        _dbContext.SubmitChanges();
                        List.Remove(SelectedItem);
                    }
                    if (deleteInput != null)
                    {
                        _dbContext.Inputs.DeleteOnSubmit(deleteInput);
                        _dbContext.SubmitChanges();
                        List.Remove(SelectedItem);
                    }
                }
            });
        }
        private void LoadData()
        {
            List = new ObservableCollection<InputInfo>(_dbContext.InputInfos);
            Object = new ObservableCollection<Object>(_dbContext.Objects);
            Input = new ObservableCollection<Input>(_dbContext.Inputs);       
        }
    }
}
