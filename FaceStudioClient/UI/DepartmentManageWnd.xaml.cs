using Face.Contract;
using FaceStudioClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FaceStudioClient.UI
{
    /// <summary>
    /// Interaction logic for DepartmentManageWnd.xaml
    /// </summary>
    public partial class DepartmentManageWnd : UserControl
    {
        public event Action OnClose;

        public DepartmentManageWnd()
        {
            InitializeComponent();
            InitUI();
            BindEvents();
        }

        #region 事件处理
        private void MenuItem_AddNode_Click(object sender, RoutedEventArgs e)
        {
            //
            var sel = treeDepart.SelectedItem as DepartmentUI;
            //
            var depart = new Department() { ParentDepartment = sel.Department };
            var wnd = new DepartmentEditWnd(depart);
            wnd.OnClose += () => {
                Query();
                gridSub.Children.Clear();
                gridSub.Visibility = Visibility.Collapsed;
                gridMain.Visibility = Visibility.Visible;
            };

            gridSub.Children.Clear();
            gridSub.Children.Add(wnd);
            gridMain.Visibility = Visibility.Collapsed;
            gridSub.Visibility = Visibility.Visible;
        }

        private async void MenuItem_DeleteNode_Click(object sender, RoutedEventArgs e)
        {
            var ret = await MetroUIExtender.Confirm("确定要删除这个节点？");
            if(ret == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative)
            {
                //删除
            }
        }

        private void OnButtonAddClick(object sender, RoutedEventArgs e)
        {
            var depart = new Department() { ParentDepartment = null };
            var wnd = new DepartmentEditWnd(depart);
            wnd.OnClose += () => {
                //
                Query();
                //
                gridSub.Children.Clear();
                gridSub.Visibility = Visibility.Collapsed;
                gridMain.Visibility = Visibility.Visible;
            };

            gridSub.Children.Clear();
            gridSub.Children.Add(wnd);
            gridMain.Visibility = Visibility.Collapsed;
            gridSub.Visibility = Visibility.Visible;
        }

        #endregion

        #region 辅助函数
        ObservableCollection<DepartmentUI> departments = new ObservableCollection<DepartmentUI>();
        void InitUI()
        {
            treeDepart.ItemsSource = departments;
        }

        void BindEvents()
        {
            this.Loaded += OnWindowLoaded;
            btnClose.Click += (sender, e) => {
                if (OnClose != null)
                    OnClose();
            };

            btnAdd.Click += OnButtonAddClick;
        }

        void EnableUI(bool bEnable)
        {

        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Query();
        }

        void Query()
        {
            EnableUI(false);
            var service = new Service.DepartmentService();
            service.OnQueryCompleted += (departs) => {
                if(departs != null)
                {
                    this.Dispatcher.BeginInvoke(new Action<Department[]>((list)=> {
                        departments.Clear();
                        List<DepartmentUI> todo = new List<DepartmentUI>();
                        foreach (var v in list)
                        {
                            var node = new DepartmentUI() { Department = v };
                            if (v.ParentDepartment == null)
                            {
                                //
                                departments.Add(node);
                            }
                            else
                            {
                                bool bFind = false;
                                foreach (var item in departments)
                                {
                                    var ret = item.Enumerate(v.ParentDepartment.ID);
                                    if (ret != null)
                                    {
                                        ret.Add(node);
                                        bFind = true;
                                        break;
                                    }
                                }

                                if (!bFind)
                                {
                                    todo.Add(node);
                                }
                            }
                        }
                    }), new object[] { departs });
                }
            };
            service.Query((exp) => {
                MetroUIExtender.Alert(exp.Message);
            });
        }
        #endregion
    }
}
