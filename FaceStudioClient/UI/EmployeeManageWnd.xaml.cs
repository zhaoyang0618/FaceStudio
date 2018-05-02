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
    /// Interaction logic for EmployeeManageWnd.xaml
    /// </summary>
    public partial class EmployeeManageWnd : UserControl
    {
        public event Action OnClose;

        public EmployeeManageWnd()
        {
            InitializeComponent();
            InitUI();
            BindEvents();
        }

        #region 事件处理
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Query();
            //MetroUIExtender.Progress("请稍等......", "正在获取人员数据....", true,
            //    (controller) => { },
            //    (controller) => {
            //        Query();
            //        controller.CloseAsync();
            //    });
        }

        private void OnItemButtonEditClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var em = btn.DataContext as EmployeeUI;
            if (null == em)
                return;

            var wnd = new EmployeeEditWnd(em.Employee);
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

        private async void OnItemButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var camera = btn.DataContext as EmployeeUI;
            if (null == camera)
                return;

            var ret = await MetroUIExtender.Confirm("您确定要删除该人员吗？");
            if (ret == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative)
            {
                //删除
                employeeList.Remove(camera);
            }
        }

        private void OnButtonAddEmployeeClick(object sender, RoutedEventArgs e)
        {
            var em = new Employee();
            var wnd = new EmployeeEditWnd(em);
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

        private void OnButtonDepartmentManageClick(object sender, RoutedEventArgs e)
        {
            var wnd = new DepartmentManageWnd();
            wnd.OnClose += () => {
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
        ObservableCollection<EmployeeUI> employeeList = new ObservableCollection<EmployeeUI>();
        void InitUI()
        {
            listEmployee.ItemsSource = employeeList;
        }

        void BindEvents()
        {
            this.Loaded += OnWindowLoaded;
            btnClose.Click += (sender, e) => {
                if (null != OnClose)
                    OnClose();
            };

            btnAddEmployee.Click += OnButtonAddEmployeeClick;
            btnDepartmentManage.Click += OnButtonDepartmentManageClick;
        }

        void EnableUI(bool bEnable)
        {
            this.IsEnabled = bEnable;
        }

        void Query()
        {
            EnableUI(false);
            var service = new Service.EmployeeService();
            service.OnQueryCompleted += (departs) => {
                if (departs != null)
                {
                    this.Dispatcher.BeginInvoke(new Action<Employee[]>((list) => {
                        employeeList.Clear();
                        foreach (var v in list)
                        {
                            employeeList.Add(new EmployeeUI() { Employee = v });
                            //employeeList.Add(new EmployeeUI() { Employee = new Employee() { Name = "宫兆新", Position = "主任" } });
                        }
                        EnableUI(true);
                    }), new object[] { departs });
                }
            };
            service.Query((exp) => {
                this.Dispatcher.BeginInvoke(new Action(()=> {
                    EnableUI(true);
                    MetroUIExtender.Alert(exp.Message);
                }), null);
            });
        }

        void q()
        {
            //UFaceService service = new UFaceService();
            //service.OnSetPasswordCompleted += (result) =>
            //{
            //    if (result != null && result.result == 1)
            //    {
            //        //
            //        //service.personCreate("192.168.1.123", "12345", new Person() { id = "123456", name = "朝阳", idcardnum = "" }, (exp) => {
            //        //    System.Diagnostics.Debug.WriteLine(exp.Message);
            //        //});
            //        //在设置密码完成的时候获取人员列表
            //        service.personFind("192.168.1.123", "12345", "-1", (exp) =>
            //        {
            //            System.Diagnostics.Debug.WriteLine(exp.Message);
            //        });
            //    }
            //    else
            //    {
            //        System.Diagnostics.Debug.WriteLine("Failed to SetPassword!");
            //    }
            //};
            //service.OnPersonFindCompleted += (result) =>
            //{
            //    if (result != null && result.result == 1)
            //    {
            //        if (null != result.data)
            //        {
            //            /*
            //            System.Diagnostics.Debug.WriteLine(result.data.id);
            //            System.Diagnostics.Debug.WriteLine(result.data.name);
            //            System.Diagnostics.Debug.WriteLine(result.data.idcardnum);
            //            */

            //            this.Dispatcher.BeginInvoke(new Action<Person[]>((list) =>
            //            {
            //                employeeList.Clear();
            //                foreach(var p in list)
            //                {
            //                    employeeList.Add(new EmployeeUI() { Employee = new Employee() { Name = "宫兆新", Position = "主任" } });
            //                }
            //            }), new object[] { result.data });
            //            //employeeList.Add(new EmployeeUI() { Employee = new Employee { Name = result.data[0].name, Position = result.data[0].id } });
            //        }
            //    }
            //    else
            //    {
            //        System.Diagnostics.Debug.WriteLine("Failed to CreatePerson!");
            //    }
            //};

            //service.SetPassword("192.168.1.123", new Service.PasswordSetPara() { oldPass = "12345", newPass = "12345" }, (exp) =>
            //{
            //    System.Diagnostics.Debug.WriteLine(exp.Message);
            //});

        }
        #endregion
    }
}
