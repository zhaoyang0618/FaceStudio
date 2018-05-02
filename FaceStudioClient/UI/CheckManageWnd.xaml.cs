using Face.Contract;
using System;
using System.Collections.Generic;
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

        #endregion

        #region 辅助函数
        void InitUI()
        {

        }

        void BindEvents()
        {
            btnClose.Click += (sender, e) => {
                if (null != OnClose)
                    OnClose();
            };
            btnRule.Click += OnButtonRuleClick;
        }

        #endregion
    }
}
