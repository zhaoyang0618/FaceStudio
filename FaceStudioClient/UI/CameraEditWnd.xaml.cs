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
    /// Interaction logic for CameraEditWnd.xaml
    /// </summary>
    public partial class CameraEditWnd : UserControl
    {
        public event Action OnClose;

        public CameraEditWnd(Camera c)
        {
            InitializeComponent();
            current = c;
            InitUI();
            BindEvents();
        }

        #region 事件处理

        private void OnButtonCloseClick(object sender, RoutedEventArgs e)
        {
            if (null != OnClose)
                OnClose();
        }

        private void OnButtonOKClick(object sender, RoutedEventArgs e)
        {
            if (current != null)
            {
                //
                var service = new Service.CameraService();
                service.OnSaveCompleted += (depart) => {
                    this.Dispatcher.BeginInvoke(new Action(() => {
                        if (null != OnClose)
                            OnClose();
                    }), null);
                };
                service.Save(current, (exp) => {
                    this.Dispatcher.BeginInvoke(new Action<string>((msg) => {
                        MetroUIExtender.Alert(msg);
                    }), new object[] { exp.Message });
                });
            }
        }

        #endregion

        #region 辅助函数
        Camera current = null;

        void InitUI()
        {
            gridContent.DataContext = current;
        }

        void BindEvents()
        {
            btnClose.Click += OnButtonCloseClick;
            btnCancel.Click += OnButtonCloseClick;
            btnOK.Click += OnButtonOKClick;
        }

        void EnableUI(bool bEnable)
        {
            this.IsEnabled = bEnable;
        }

        #endregion
    }
}
