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
    /// Interaction logic for UserManageWnd.xaml
    /// </summary>
    public partial class UserManageWnd : UserControl
    {
        public event Action OnClose;

        public UserManageWnd()
        {
            InitializeComponent();
            InitUI();
            BindEvents();
        }

        #region 事件处理
        private void OnButtonOKClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var username = textUserName.Text;
                var email = textEmail.Text;
                var password = textPassword.Password;
                var confirm = textConfirm.Password;
                var service = new Service.AccountService();
                service.OnRegisterCompleted += () => {
                    this.Dispatcher.BeginInvoke(new Action(()=> {
                        if (null != OnClose)
                            OnClose();
                    }), null);
                };
                service.Register(username, email, password, confirm, (exp) => {
                    this.Dispatcher.BeginInvoke(new Action<string>((msg)=> {
                        MetroUIExtender.Alert(msg);
                    }), new object[] { exp.Message });
                });
            }
            catch(Exception exp)
            {
                MetroUIExtender.Alert(exp.Message);
            }
        }

        private void OnButtonCloseClick(object sender, RoutedEventArgs e)
        {
            if (null != OnClose)
                OnClose();
        }


        #endregion

        #region 辅助函数

        void InitUI()
        {
        }

        void BindEvents()
        {
            btnClose.Click += OnButtonCloseClick;
            btnOK.Click += OnButtonOKClick;
            btnCancel.Click += OnButtonCloseClick;
        }

        #endregion
    }
}
