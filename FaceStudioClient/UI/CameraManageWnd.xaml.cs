using Face.Contract;
using FaceStudioClient.Model;
using Microsoft.Win32;
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

        private void OnCameraEdit(Camera obj)
        {
            var wnd = new CameraEditWnd(obj);
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

        private async void OnCameraDelete(Camera obj)
        {
            var ret = await MetroUIExtender.Confirm("您确定要删除该设备吗？");
            if (ret == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative)
            {
                //删除
                CameraUI camera = null;
                foreach(var v in cameraList)
                {
                    if(v.Camera.ID == obj.ID)
                    {
                        camera = v;
                        break;
                    }
                }
                if(null != camera)
                {
                    cameraList.Remove(camera);
                }
            }
        }

        private void OnCameraChangeLogo(Camera obj)
        {
            //首先上传照片,然后更改Logo
            var dlg = new OpenFileDialog();
            dlg.Filter = "图像文件(*.jpg;*.png;*.tif)|*.jpg;*.png;*.tif";
            if (dlg.ShowDialog() == true)
            {
                //
                string filepath = dlg.FileName;
                ChangeLogo(filepath, obj);
            }
        }

        private void OnCameraRestartDevice(Camera obj)
        {
            if (null == obj) return;
            RestartDevice(obj);
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

        private void ChangeLogo(string filepath, Camera camera)
        {
            if (null == camera)
                return;

            var fi = new System.IO.FileInfo(filepath);
            string filename = fi.Name;
            MetroUIExtender.Progress("正在上传文件......", "请稍等......", true,
                (controller) =>
                {
                    controller.Canceled += (ss, ee) =>
                    {
                        controller.CloseAsync();
                    };
                    controller.Closed += (ss, ee) =>
                    {
                        if (!controller.IsCanceled)
                        {
                            CameraUI ui = null;
                            foreach(var v in cameraList)
                            {
                                if(v.Camera.ID == camera.ID)
                                {
                                    ui = v;
                                    break;
                                }
                            }
                            if(ui != null)
                            {
                                ui.ChangeLogo(camera.Logo);
                            }
                        }
                    };
                },
                (controller) =>
                {
                    var data = System.IO.File.ReadAllBytes(filepath);
                    var service = new Service.PhotoImageService();
                    service.OnUploadCompleted += (file) =>
                    {
                        //在设备中更改数据
                        if(file != null)
                        {
                            camera.Logo = file;
                            camera.PhotoImageID = file.ID;
                            var serviceC = new Service.CameraService();
                            serviceC.OnChangeLogoCompleted += () => {
                                controller.CloseAsync();
                            };
                            serviceC.ChangeLogo(camera, (exp) => {
                                controller.SetMessage(exp.Message);
                                controller.SetCancelable(true);
                            });
                        }
                        else
                        {
                            controller.CloseAsync();
                        }
                    };
                    service.Upload(data, filename, (exp) =>
                    {
                        controller.SetMessage(exp.Message);
                        controller.SetCancelable(true);
                    });
                });
        }

        private void RestartDevice(Camera camera)
        {
            if (null == camera)
                return;

            MetroUIExtender.Progress("正在重启设备......", "请稍等......", true,
                (controller) =>
                {
                    controller.Canceled += (ss, ee) =>
                    {
                        controller.CloseAsync();
                    };
                    controller.Closed += (ss, ee) =>
                    {
                        if (!controller.IsCanceled)
                        {
                        }
                    };
                },
                (controller) =>
                {
                    var serviceC = new Service.CameraService();
                    serviceC.OnRestartDeviceCompleted += () => {
                        controller.CloseAsync();
                    };
                    serviceC.RestartDevice(camera, (exp) => {
                        controller.SetMessage(exp.Message);
                        controller.SetCancelable(true);
                    });
                });
        }
        #endregion
    }
}
