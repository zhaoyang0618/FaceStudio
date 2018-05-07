using Face.Contract;
using FaceStudioClient.Model;
using MaterialDesignThemes.Wpf.Transitions;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FaceStudioClient.UI
{
    /// <summary>
    /// Interaction logic for CameraViewWnd.xaml
    /// </summary>
    public partial class CameraViewWnd : UserControl
    {
        public event Action<Camera> OnEdit;
        public event Action<Camera> OnDelete;
        public event Action<Camera> OnChangeLogo;
        public event Action<Camera> OnRestartDevice;

        public CameraViewWnd()
        {
            InitializeComponent();
            InitUI();
            BindEvents();
        }

        #region 事件处理
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnItemButtonDeviceClick(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.To = 180d;
            this.axr.BeginAnimation(System.Windows.Media.Media3D.AxisAngleRotation3D.AngleProperty, da);
        }

        private void OnItemButtonBackClick(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.To = 0d;
            this.axr.BeginAnimation(System.Windows.Media.Media3D.AxisAngleRotation3D.AngleProperty, da);
        }

        private void OnItemButtonEditClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var camera = btn.DataContext as CameraUI;
            if (null == camera)
                return;
            if (null != OnEdit)
                OnEdit(camera.Camera);
        }

        private void OnItemButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var camera = btn.DataContext as CameraUI;
            if (null == camera)
                return;
            if (null != OnDelete)
                OnDelete(camera.Camera);
        }

        private void OnItemButtonChangeLogoClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var camera = btn.DataContext as CameraUI;
            if (null == camera)
                return;
            if (null != OnChangeLogo)
                OnChangeLogo(camera.Camera);
        }

        private void OnItemButtonRestartDeviceClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var camera = btn.DataContext as CameraUI;
            if (null == camera)
                return;
            if (null != OnRestartDevice)
                OnRestartDevice(camera.Camera);
        }
        #endregion

        #region 辅助函数
        void InitUI()
        {
            //Command="{x:Static materialDesign:Transitioner.MoveNextCommand}"
            //Command="{x:Static materialDesign:Transitioner.MovePreviousCommand}"
        }

        void BindEvents()
        {

        }
        #endregion
    }
}
