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
    /// Interaction logic for CameraManageWnd.xaml
    /// </summary>
    public partial class CameraManageWnd : UserControl
    {
        public event Action OnClose;

        public CameraManageWnd()
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

        private void OnItemButtonEditClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var camera = btn.DataContext as CameraUI;
            if (null == camera)
                return;

            var wnd = new CameraEditWnd(camera.Camera);
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
            var camera = btn.DataContext as CameraUI;
            if (null == camera)
                return;

            var ret = await MetroUIExtender.Confirm("您确定要删除该设备吗？");
            if(ret == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative)
            {
                //删除
                cameraList.Remove(camera);
            }
        }

        private void OnButtonAddCameraClick(object sender, RoutedEventArgs e)
        {
            var camera = new Camera();
            var wnd = new CameraEditWnd(camera);
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

        #endregion

        #region 辅助函数
        ObservableCollection<CameraUI> cameraList = new ObservableCollection<CameraUI>();
        void InitUI()
        {
            listCamera.ItemsSource = cameraList;
            //cameraList.Add(new CameraUI() {  Camera = new Camera() { Name="测试机器", IP="192.168.1.123" } });
        }

        void BindEvents()
        {
            this.Loaded += OnWindowLoaded;
            btnClose.Click += (sender, e) => {
                if (null != OnClose)
                    OnClose();
            };
            btnAddCamera.Click += OnButtonAddCameraClick;
        }

        void EnableUI(bool bEnable)
        {
            this.IsEnabled = bEnable;
        }

        void Query()
        {
            EnableUI(false);
            var service = new Service.CameraService();
            service.OnQueryCompleted += (departs) => {
                if (departs != null)
                {
                    this.Dispatcher.BeginInvoke(new Action<Camera[]>((list) => {
                        cameraList.Clear();
                        foreach (var v in list)
                        {
                            cameraList.Add(new CameraUI() { Camera = v });
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
        }

        #endregion
    }
}
