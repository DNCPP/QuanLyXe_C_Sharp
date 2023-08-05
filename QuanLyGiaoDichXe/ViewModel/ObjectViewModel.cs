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
    public class ObjectViewModel:BaseViewModel
    {
        private ObservableCollection<Model.Object> _List;

        public ObservableCollection<Model.Object> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.InputInfo> _InputInfo;
        public ObservableCollection<Model.InputInfo> InputInfo { get => _InputInfo; set { _InputInfo = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Unit> _Unit;
        public ObservableCollection<Model.Unit> Unit { get => _Unit; set { _Unit = value; OnPropertyChanged(); } }
        private ObservableCollection<Model.Suplier> _Suplier;
        public ObservableCollection<Model.Suplier> Suplier { get => _Suplier; set { _Suplier = value; OnPropertyChanged(); } }

        private Model.Object _SelectedItem;
        public Model.Object SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    SelectedSuplier = SelectedItem.Suplier;
                    SelectedUnit = SelectedItem.Unit;
                    Id = SelectedItem.Id;
                }
            }
        }

        private int? _Count;
        public int? Count
        {
            get => _Count; set { _Count = value; OnPropertyChanged(); }
        }
        private Model.Unit _SelectedUnit;
        public Model.Unit SelectedUnit
        {
            get => _SelectedUnit; set { _SelectedUnit = value; OnPropertyChanged(); }
        }

        private Model.Suplier _SelectedSuplier;
        public Model.Suplier SelectedSuplier
        {
            get => _SelectedSuplier; set { _SelectedSuplier = value; OnPropertyChanged(); }
        }
        private string _Id;
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }
        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private int _IdUnit;
        public int IdUnit { get => _IdUnit; set { _IdUnit = value; OnPropertyChanged(); } }

        private string _IdSuplier;
        public string IdSuplier { get => _IdSuplier; set { _IdSuplier = value; OnPropertyChanged(); } }

        //Ẩn order
        private bool isButtonVisible;
        public bool IsButtonVisible
        {
            get { return isButtonVisible; }
            set
            {
                isButtonVisible = value;
                OnPropertyChanged(nameof(IsButtonVisible));
            }
        }

        private bool isButtonOrder;
        public bool IsButtonOrder
        {
            get { return isButtonOrder; }
            set
            {
                isButtonOrder = value;
                OnPropertyChanged(nameof(IsButtonOrder)); // Kích hoạt sự kiện OnPropertyChanged
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand OrderCommand { get; set; }
        private QuanLyGiaoDichXeDataContext _dbContext;

        public ObjectViewModel()
        {
            _dbContext = new QuanLyGiaoDichXeDataContext();
            string username;
            int idCustomer;
            LoginWindow loginWindow = new LoginWindow();
            var loginVM = loginWindow.DataContext as LoginViewModel;
            username = loginVM.UserName;
            //Hiển thị danh sách đơn vị tính lên SuplierViewModel
            List = new ObservableCollection<Model.Object>(_dbContext.Objects);
            Unit = new ObservableCollection<Model.Unit>(_dbContext.Units);
            Suplier = new ObservableCollection<Model.Suplier>(_dbContext.Supliers);
            InputInfo = new ObservableCollection<InputInfo>(_dbContext.InputInfos);
            if (_dbContext.Users.FirstOrDefault(u => u.Username == username && u.IdRole == 3) != null)
            {
                IsButtonVisible = true;
                IsButtonOrder = false;
                AddCommand = new RelayCommand<object>((p) =>
               {

                   return true;
               }, (p) =>
               {
                   var obj = new Model.Object() { DisplayName = DisplayName, IdSuplier = SelectedSuplier.Id, IdUnit = SelectedUnit.Id, Id = Guid.NewGuid().ToString() };
                   _dbContext.Objects.InsertOnSubmit(obj);
                   _dbContext.SubmitChanges();
                   List.Add(obj); // Cập nhật lại danh sách
                });

                EditCommand = new RelayCommand<object>((p) =>
                {
                    return true;
                }, (p) =>
                {
                    var obj = _dbContext.Objects.FirstOrDefault(x => x.Id == SelectedItem.Id);
                    if (obj != null)
                    {
                        obj.DisplayName = DisplayName;
                        obj.Unit = SelectedUnit;
                        obj.Suplier = SelectedSuplier;
                        obj.Id = Id;
                        _dbContext.SubmitChanges();
                    }
                    else
                    {
                        MessageBox.Show("Chưa tồn tại");
                    }
                });
                DeleteCommand = new RelayCommand<object>((p) =>
                {
                    return true;
                }, (p) =>
                {
                    var obj = _dbContext.Objects.FirstOrDefault(x => x.Id == SelectedItem.Id);
                    if (obj != null)
                    {
                        _dbContext.Objects.DeleteOnSubmit(obj);
                        _dbContext.SubmitChanges();
                        List.Remove(SelectedItem);
                    }
                    else
                    {
                        MessageBox.Show("Chưa tồn tại");
                    }
                });
            }
            
            else if (_dbContext.Users.FirstOrDefault(u => u.Username == username && u.IdRole == 2) != null)
            {
                IsButtonOrder = true;
                IsButtonVisible = false;
                idCustomer = _dbContext.Customers.FirstOrDefault(u => u.Username == username).Id;
                OrderCommand = new RelayCommand<object>((p) =>
                {
                    return true;
                }, (p) =>
                {
                    if(SelectedItem!=null)
                    {
                        Model.Object obj = null;
                        foreach (var item in _dbContext.Objects)
                        {
                            if (item.Id == SelectedItem.Id)
                            {
                                obj = item;
                                break; // Sau khi tìm thấy, thoát khỏi vòng lặp
                            }
                        }
                        if (obj != null)
                        {
                            obj.DisplayName = DisplayName;
                            obj.Unit = SelectedUnit;
                            obj.Suplier = SelectedSuplier;
                            string displayName = obj.DisplayName;
                            int idUnit = obj.Unit.Id;
                            int idSuplier = obj.Suplier.Id;
                            var query = from inputinfo in _dbContext.InputInfos
                                        join obj1 in _dbContext.Objects on inputinfo.IdObject equals obj1.Id
                                        join suplier in _dbContext.Supliers on obj1.IdSuplier equals suplier.Id
                                        select new
                                        {
                                            IdObject = obj1.Id,
                                            IdInputInfo = inputinfo.Id,
                                            Count = inputinfo.count,
                                        };
                            using (var dbContext = new QuanLyGiaoDichXeDataContext())
                            {
                                var output = new Output()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    DateInput = DateTime.Now
                                };
                                dbContext.Outputs.InsertOnSubmit(output);
                                dbContext.SubmitChanges();

                                // ID của đối tượng được tự động sinh và được cập nhật trong output.Id
                                string idOutput = output.Id.ToString();
                                string idObject = obj.Id;

                                // Lọc kết quả từ các bảng
                                var filteredQuery = query.Where(x => x.IdObject == SelectedItem.Id);

                                // Lấy giá trị ID của InputInfo
                                string idInputInfo = filteredQuery.Select(x => x.IdInputInfo).FirstOrDefault();

                                int? count = Count;
                                if (count > 0)
                                {
                                    var outputif = new OutputInfo()
                                    {
                                        Id = idOutput,
                                        IdObject = idObject,
                                        IdInputInfor = idInputInfo,
                                        IdCustomer = idCustomer,
                                        count = count,
                                        Status = "Đã đặt hàng",
                                    };

                                    string query_idInputInfo = idInputInfo;
                                    var inputInfo = _dbContext.InputInfos.FirstOrDefault(x => x.Id == idInputInfo && x.IdObject == obj.Id);
                                    if (inputInfo != null)
                                    {
                                        if (inputInfo.count > 0)
                                        {
                                            int? tam = inputInfo.count;
                                            int? temp = tam - count;
                                            if (temp < 0)
                                            {
                                                MessageBox.Show($"Số xe hiện tại không còn đủ. Chỉ còn lại :{tam} xe");
                                            }
                                            else
                                            {
                                                inputInfo.count = inputInfo.count - count;
                                                _dbContext.OutputInfos.InsertOnSubmit(outputif);
                                                _dbContext.SubmitChanges();
                                                MessageBox.Show("Đặt mua xe thành công");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Không còn xe");
                                        }
                                    }
                                }
                            }
                        }
                    }                        
                });
            }    
            else
            {
                IsButtonOrder = false;
                IsButtonVisible = false;
            }
        }
    }
}