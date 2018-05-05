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
    /// Interaction logic for RuleManageWnd.xaml
    /// </summary>
    public partial class RuleManageWnd : UserControl
    {
        public event Action OnClose;
        public RuleManageWnd()
        {
            InitializeComponent();
            InitUI();
            BindEvents();
        }

        #region 事件处理
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Query();
        }

        private void OnItemRuleEditButtonClick(object sender, RoutedEventArgs e)
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
            var camera = btn.DataContext as SpecialAttendanceRuleUI;
            if (null == camera)
                return;

            var ret = await MetroUIExtender.Confirm("您确定要删除该规则吗？");
            if (ret == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative)
            {
                //删除
                specialRuleList.Remove(camera);
            }
        }

        private void OnButtonAddClick(object sender, RoutedEventArgs e)
        {
            var rule = new AttendanceRule();
            var wnd = new RuleEditWnd(rule);
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

        private void OnButtonAddSpecialClick(object sender, RoutedEventArgs e)
        {
            var rule = new SpecialAttendanceRule();
            var wnd = new SpecialRuleEditWnd(rule);
            wnd.OnClose += () => {
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
        ObservableCollection<AttendanceRuleUI> ruleList = new ObservableCollection<AttendanceRuleUI>();
        ObservableCollection<SpecialAttendanceRuleUI> specialRuleList = new ObservableCollection<SpecialAttendanceRuleUI>();
        void InitUI()
        {
            gridRule.ItemsSource = ruleList;
            gridSpecialRule.ItemsSource = specialRuleList;
        }

        void BindEvents()
        {
            this.Loaded += OnWindowLoaded;
            btnClose.Click += (sender, e) => {
                if (null != OnClose)
                    OnClose();
            };
            btnAdd.Click += OnButtonAddClick;
            btnAddSpecial.Click += OnButtonAddSpecialClick;
        }

        void EnableUI(bool bEnable)
        {
            this.IsEnabled = bEnable;
        }

        void Query()
        {
            EnableUI(false);
            var service = new Service.AttendanceRuleService();
            service.OnQuerySpecialCompleted += (departs) => {
                if (departs != null)
                {
                    this.Dispatcher.BeginInvoke(new Action<SpecialAttendanceRule[]>((list) => {
                        specialRuleList.Clear();
                        foreach (var v in list)
                        {
                            specialRuleList.Add(new SpecialAttendanceRuleUI() { SpecialAttendanceRule = v });
                        }
                        EnableUI(true);
                    }), new object[] { departs });
                }
            };
            service.OnQueryCompleted += (departs) => {
                if (departs != null)
                {
                    this.Dispatcher.BeginInvoke(new Action<AttendanceRule[]>((list) => {
                        ruleList.Clear();
                        foreach (var v in list)
                        {
                            ruleList.Add(new AttendanceRuleUI() { AttendanceRule = v });
                        }
                        EnableUI(true);
                    }), new object[] { departs });
                }
            };
            service.Query((exp) => {
                this.Dispatcher.BeginInvoke(new Action(() => {
                    EnableUI(true);
                    MetroUIExtender.Alert(exp.Message);
                }), null);
            });
            service.QuerySpecial((exp) => {
                this.Dispatcher.BeginInvoke(new Action(() => {
                    EnableUI(true);
                    MetroUIExtender.Alert(exp.Message);
                }), null);
            });
        }

        #endregion
    }
}
