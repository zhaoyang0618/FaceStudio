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
    /// Interaction logic for CheckManageWnd.xaml
    /// </summary>
    public partial class CheckManageWnd : UserControl
    {
        public event Action OnClose;

        public CheckManageWnd()
        {
            InitializeComponent();
            InitUI();
            BindEvents();
        }

        #region 事件处理
        private void OnButtonRuleClick(object sender, RoutedEventArgs e)
        {
            var wnd = new RuleManageWnd();
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

        private void OnButtonQueryClick(object sender, RoutedEventArgs e)
        {
            Query();
        }

        #endregion

        #region 辅助函数
        ObservableCollection<CheckinRecordUI> ruleList = new ObservableCollection<CheckinRecordUI>();
        CheckinRecordQueryInfo currentQuery = new CheckinRecordQueryInfo();

        void InitUI()
        {
            gridCheckedIn.ItemsSource = ruleList;
            paneQuery.DataContext = currentQuery;
        }

        void BindEvents()
        {
            btnClose.Click += (sender, e) => {
                if (null != OnClose)
                    OnClose();
            };
            btnRule.Click += OnButtonRuleClick;
            btnQuery.Click += OnButtonQueryClick;
        }

        void EnableUI(bool bEnable)
        {
            this.IsEnabled = bEnable;
        }

        void Query()
        {
            EnableUI(false);
            var service = new Service.CheckinService();
            service.OnQueryCompleted += (departs) => {
                if (departs != null)
                {
                    this.Dispatcher.BeginInvoke(new Action<CheckinRecord[]>((list) => {
                        ruleList.Clear();
                        foreach (var v in list)
                        {
                            ruleList.Add(new CheckinRecordUI() { Record = v });
                        }
                        EnableUI(true);
                    }), new object[] { departs });
                }
            };
            service.Query(currentQuery, (exp) => {
                this.Dispatcher.BeginInvoke(new Action(() => {
                    EnableUI(true);
                    MetroUIExtender.Alert(exp.Message);
                }), null);
            });
        }
        #endregion
    }
}
