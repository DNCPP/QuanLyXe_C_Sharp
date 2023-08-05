using QuanLyGiaoDichXe.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyGiaoDichXe.ViewModel
{
    public class UnitViewModel : BaseViewModel 
    {
        private ObservableCollection<Unit> _List;

        public ObservableCollection<Unit> List { get=>_List; set { _List = value; OnPropertyChanged(); } }


        private Unit _SelectedItem;
        public Unit SelectedItem { get => _SelectedItem; set { 
                _SelectedItem = value;OnPropertyChanged();
                if(SelectedItem!=null)
                {
                    DisplayName = SelectedItem.DisplayName;
                }    
            } }
        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }


        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public UnitViewModel()
        {
            //Hiển thị danh sách đơn vị tính lên UnitViewModel
            using (var db = new QuanLyGiaoDichXeDataContext())
            {
                List = new ObservableCollection<Unit>(db.Units);
            }
            AddCommand = new RelayCommand<object>((p) =>
            {
                var db = new QuanLyGiaoDichXeDataContext();
                var displayList = db.Units.Where(x => x.DisplayName == DisplayName);
                if (displayList==null|| displayList.Count() != 0)
                {
                    MessageBox.Show("Trùng đơn vị");
                    return false;
                }                      
                return true;
            }, (p) =>
            {
                var db = new QuanLyGiaoDichXeDataContext();
                var unit = new Unit() { DisplayName = DisplayName };
                db.Units.InsertOnSubmit(unit);
                db.SubmitChanges();
                List.Add(unit); // Cập nhật lại danh sách
            });
            EditCommand = new RelayCommand<object>((p) =>
            {
                var db = new QuanLyGiaoDichXeDataContext();
                var displayList = db.Units.Where(x => x.DisplayName == DisplayName);
                if (displayList == null || displayList.Count() != 0)
                {
                    MessageBox.Show("Trùng đơn vị");
                    return false;
                }   
                return true;
            }, (p) =>
            {
                var db = new QuanLyGiaoDichXeDataContext();
                var unit = db.Units.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                unit.DisplayName = DisplayName;
                db.SubmitChanges();
                SelectedItem.DisplayName = DisplayName;
            });
        }
    }
}
