using Face.Contract;
using FaceStudioClient.Model;
using FaceStudioClient.Service;
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
    /// Interaction logic for EmployeeEditWnd.xaml
    /// </summary>
    public partial class EmployeeEditWnd : UserControl
    {
        public event Action OnClose;

        public EmployeeEditWnd(Employee em)
        {
            InitializeComponent();
            current = em;
            InitUI();
            BindEvents();
        }

        #region 事件处理
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            QueryDepartment();
            QueryRules();
            QueryCameras();
            if (current != null)
            {
                if(current.FirstPhoto != null)
                {
                    ImageDecoder.SetSource(imageFirst, PhotoImageService.GetImageFileURL(current.FirstPhoto.ID));
                }
                if (current.SecondPhoto != null)
                {
                    ImageDecoder.SetSource(imageSecond, PhotoImageService.GetImageFileURL(current.SecondPhoto.ID));
                }
                if (current.ThirdPhoto != null)
                {
                    ImageDecoder.SetSource(imageThird, PhotoImageService.GetImageFileURL(current.ThirdPhoto.ID));
                }
            }
        }

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
                try
                {
                    var sel = cboDepart.SelectedItem as DepartmentUI;
                    if (null != sel)
                    {
                        current.Deparment = sel.Department;
                    }

                    var selRule = cboRule.SelectedItem as AttendanceRule;
                    if (null != selRule)
                    {
                        current.AttendanceRule = selRule;
                    }

                    current.Cameras.Clear();
                    foreach (var c in cameras)
                    {
                        if (c.IsChecked)
                        {
                            var r = new EmployeeCameraRelation()
                            {
                                CameraID = c.Camera.ID,
                                EmployeeID = current.ID,
                            };
                            current.Cameras.Add(r);
                        }
                    }

                    //
                    var service = new Service.EmployeeService();
                    service.OnSaveCompleted += (depart) => {
                        this.Dispatcher.BeginInvoke(new Action(() => {
                            if (null != OnClose)
                                OnClose();
                        }), null);
                    };
                    EnableUI(false);
                    service.Save(current, (exp) => {
                        this.Dispatcher.BeginInvoke(new Action<string>((msg) => {
                            EnableUI(true);
                            MetroUIExtender.Alert(msg);
                        }), new object[] { exp.Message });
                    });
                }
                catch (Exception exp)
                {
                    MetroUIExtender.Alert(exp.Message);
                }
            }
        }

        private void OnButtonFirstSelectClick(object sender, RoutedEventArgs e)
        {
            //选择图片
            var dlg = new OpenFileDialog();
            if(dlg.ShowDialog() == true)
            {
                //
                string filepath = dlg.FileName;
                UploadFile(filepath, 1);
            }
        }

        private void OnButtonSecondSelectClick(object sender, RoutedEventArgs e)
        {
            //选择图片
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                //
                string filepath = dlg.FileName;
                UploadFile(filepath, 2);
            }
        }

        private void OnButtonThirdSelectClick(object sender, RoutedEventArgs e)
        {
            //选择图片
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                //
                string filepath = dlg.FileName;
                UploadFile(filepath, 3);
            }
        }

        private void UploadFile(string filepath, int flag)
        {
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
                            //显示图片
                            var fs = System.IO.File.OpenRead(filepath);
                            var decoder = BitmapDecoder.Create(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                            switch (flag)
                            {
                                case 1:
                                    imageFirst.Source = decoder.Frames[0];
                                    break;
                                case 2:
                                    imageSecond.Source = decoder.Frames[0];
                                    break;
                                case 3:
                                    imageThird.Source = decoder.Frames[0];
                                    break;
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
                        if (null != current)
                        {
                            switch(flag)
                            {
                                case 1:
                                    current.FirstPhoto = file;
                                    break;
                                case 2:
                                    current.SecondPhoto = file;
                                    break;
                                case 3:
                                    current.ThirdPhoto = file;
                                    break;
                            }
                        }
                        controller.CloseAsync();
                    };
                    service.Upload(data, filename, (exp) =>
                    {
                        controller.SetMessage(exp.Message);
                        controller.SetCancelable(true);
                    });
                });
        }

        #endregion

        #region 辅助函数
        ObservableCollection<DepartmentUI> departments = new ObservableCollection<DepartmentUI>();
        ObservableCollection<AttendanceRule> rules = new ObservableCollection<AttendanceRule>();
        ObservableCollection<CameraUI> cameras = new ObservableCollection<CameraUI>();
        Employee current = null;
        void InitUI()
        {
            cboDepart.ItemsSource = departments;
            cboRule.ItemsSource = rules;
            listCameras.ItemsSource = cameras;
            gridContent.DataContext = current;
        }

        void BindEvents()
        {
            this.Loaded += OnWindowLoaded;
            btnClose.Click += OnButtonCloseClick;
            btnCancel.Click += OnButtonCloseClick;
            btnOK.Click += OnButtonOKClick;
            btnFirstSelect.Click += OnButtonFirstSelectClick;
            btnSecondSelect.Click += OnButtonSecondSelectClick;
            btnThirdSelect.Click += OnButtonThirdSelectClick;
        }

        void EnableUI(bool bEnable)
        {
            this.IsEnabled = bEnable;
        }

        void QueryDepartment()
        {
            EnableUI(false);
            var service = new Service.DepartmentService();
            service.OnQueryCompleted += (departs) => {
                if (departs != null)
                {
                    this.Dispatcher.BeginInvoke(new Action<Department[]>((list) => {
                        departments.Clear();
                        DepartmentUI sel = null;
                        foreach (var v in list)
                        {
                            var node = new DepartmentUI() { Department = v };
                            if(current != null && current.Deparment != null && v.ID == current.Deparment.ID)
                            {
                                sel = node;
                            }
                            departments.Add(node);
                        }

                        if (sel != null)
                            cboDepart.SelectedItem = sel;
                        EnableUI(true);
                    }), new object[] { departs });
                }
            };
            service.Query((exp) => {
                EnableUI(true);
                MetroUIExtender.Alert(exp.Message);
            });
        }

        void QueryRules()
        {
            var service = new Service.AttendanceRuleService();
            service.OnQueryCompleted += (departs) => {
                if (departs != null)
                {
                    this.Dispatcher.BeginInvoke(new Action<AttendanceRule[]>((list) => {
                        rules.Clear();
                        AttendanceRule sel = null;
                        foreach (var v in list)
                        {
                            if (current != null && current.AttendanceRule != null && v.ID == current.AttendanceRule.ID)
                            {
                                sel = v;
                            }
                            rules.Add(v);
                        }

                        if (sel != null)
                            cboRule.SelectedItem = sel;
                        EnableUI(true);
                    }), new object[] { departs });
                }
            };
            service.Query((exp) => {
                EnableUI(true);
                MetroUIExtender.Alert(exp.Message);
            });
        }

        void QueryCameras()
        {
            var service = new Service.CameraService();
            service.OnQueryCompleted += (departs) => {
                if (departs != null)
                {
                    this.Dispatcher.BeginInvoke(new Action<Camera[]>((list) => {
                        cameras.Clear();
                        foreach (var v in list)
                        {
                            var c = new CameraUI() { Camera = v };
                            //如果在列表中,选中
                            if(current != null)
                            {
                                bool bExists = false;

                                foreach (var l in current.Cameras)
                                {
                                    if(l.CameraID == v.ID)
                                    {
                                        bExists = true;
                                        break;
                                    }
                                }

                                if (bExists)
                                    c.IsChecked = true;
                            }
                            cameras.Add(c);
                        }

                        EnableUI(true);
                    }), new object[] { departs });
                }
            };
            service.Query((exp) => {
                EnableUI(true);
                MetroUIExtender.Alert(exp.Message);
            });
        }
        #endregion
    }
}
