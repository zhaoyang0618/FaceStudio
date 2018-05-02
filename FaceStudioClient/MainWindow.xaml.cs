using FaceStudioClient.Utils;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace FaceStudioClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            InitUI();
            BindEvents();
        }

        #region 事件处理
        private void OnWndLoaded(object sender, RoutedEventArgs e)
        {
            //var ret = FaceRecognizeWrapper.SNET_Startup(wndVideo.Handle, 0x0400 + 100, 0, false, false, null);
            //if (ret == 0)
            //{
            //    ret = FaceRecognizeWrapper.SNET_MessageCallback(MessageNotifyCallback, IntPtr.Zero);

            //    //播放器
            //    ret = HH5PlayerWrapper.HH5PLAYER_InitSDK(wndVideo.Handle);
            //    ret = HH5PlayerWrapper.HH5PLAYER_SetDecoderQulity(false);  // 高质量图象显示

            //    //初始化播放器
            //    ret = HH5PlayerWrapper.HH5PLAYER_InitPlayer2(0, wndPicture.Handle, true);
            //    ret = HH5PlayerWrapper.HH5PLAYER_OpenStream(0);

            //    //HHCBOnDraw
            //    int err = HH5PlayerWrapper.HH5PLAYER_RegCBOnDraw(0, onDraw, IntPtr.Zero);
            //}
        }

        private void OnWndUnloaded(object sender, RoutedEventArgs e)
        {
            //m_bExit = true;
            //if (m_hOpenVideo != IntPtr.Zero)
            //{
            //    FaceRecognizeWrapper.SNET_CloseChannel(m_hOpenVideo);
            //    m_hOpenVideo = IntPtr.Zero;
            //}
            //HH5PlayerWrapper.HH5PLAYER_CloseStream(0);
            //HH5PlayerWrapper.HH5PLAYER_ReleaseSDK();
            //FaceRecognizeWrapper.SNET_Cleanup();
        }

        //private void OnButtonCloseStreamClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if(m_hOpenVideo != IntPtr.Zero)
        //        {
        //            var ret = FaceRecognizeWrapper.SNET_CloseChannel(m_hOpenVideo);
        //            m_hOpenVideo = IntPtr.Zero;

        //            btnCloseStream.Visibility = Visibility.Collapsed;
        //            btnOpenStream.Visibility = Visibility.Visible;
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        System.Diagnostics.Debug.WriteLine(exp);
        //    }
        //}

        //private void OnButtonOpenStreamClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        openInfo.dwClientID = 0;
        //        openInfo.nOpenChannel = 0;
        //        openInfo.protocolType = NET_PROTOCOL_TYPE.NET_PROTOCOL_TCP;
        //        openInfo.funcStreamCallback = ChannelStreamCallback;
        //        openInfo.pCallbackContext = IntPtr.Zero;

        //        var errCode = FaceRecognizeWrapper.SNET_OpenChannel("172.168.2.100", 5000, "", "admin", "admin", ref openInfo, out m_hOpenVideo, IntPtr.Zero);
        //        if (0 == errCode)
        //        {
        //            btnOpenStream.Visibility = Visibility.Collapsed;
        //            btnCloseStream.Visibility = Visibility.Visible;
        //        }
        //        else
        //        {
        //            throw new Exception(string.Format("发生错误: {0}", errCode));
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        System.Diagnostics.Debug.WriteLine(exp);
        //    }
        //}

        //private void OnButtonClosePictureClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var ret = FaceRecognizeWrapper.SNET_ClosePicture(hPicture);
        //        if (ret == 0)
        //        {
        //            hPicture = IntPtr.Zero;
        //            btnClosePicture.Visibility = Visibility.Collapsed;
        //            btnOpenPicture.Visibility = Visibility.Visible;
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        System.Diagnostics.Debug.WriteLine(exp);
        //    }
        //}

        //private void OnButtonOpenPictureClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        sOpenPicInfo.dwClientID = 0;
        //        sOpenPicInfo.nOpenChannel = 0;
        //        sOpenPicInfo.protocolType = NET_PROTOCOL_TYPE.NET_PROTOCOL_TCP;
        //        sOpenPicInfo.funcPictureCallback = CapturePictureCallback;
        //        sOpenPicInfo.pCallbackContext = IntPtr.Zero;
        //        var ret = FaceRecognizeWrapper.SNET_OpenPicture("172.168.2.100", 5000, "", "admin", "admin", ref sOpenPicInfo, out hPicture, IntPtr.Zero);
        //        if (ret == 0)
        //        {
        //            btnOpenPicture.Visibility = Visibility.Collapsed;
        //            btnClosePicture.Visibility = Visibility.Visible;
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        System.Diagnostics.Debug.WriteLine(exp);
        //    }

        //}

        private void OnButtonRegisterClick(object sender, RoutedEventArgs e)
        {
            btnRegister.Visibility = Visibility.Collapsed;
            var wnd = new UI.UserManageWnd();
            wnd.OnClose += () => {
                btnRegister.Visibility = Visibility.Visible;
                gridSub.Children.Clear();
                gridSub.Visibility = Visibility.Collapsed;
                gridMain.Visibility = Visibility.Visible;
            };

            gridSub.Children.Clear();
            gridSub.Children.Add(wnd);
            gridMain.Visibility = Visibility.Collapsed;
            gridSub.Visibility = Visibility.Visible;
        }

        private void OnButtonLogonClick(object sender, RoutedEventArgs e)
        {
            //登录
            try
            {
                string user = textUser.Text;
                if (string.IsNullOrEmpty(user))
                    throw new Exception("请输入用户名!");
                string pwd = textPwd.Password;
                if (string.IsNullOrEmpty(pwd))
                    throw new Exception("请输入密码!");

                UI.MetroUIExtender.Progress("正在登录...", "请稍等......", true, 
                    (controller) => {
                        controller.Canceled += (ss, ee) => {
                            controller.CloseAsync();
                        };
                        controller.Closed += (ss, ee) => {
                            if(!controller.IsCanceled)
                            {
                                gridLogon.Visibility = Visibility.Collapsed;
                                gridMenu.Visibility = Visibility.Visible;
                                btnLogout.Visibility = Visibility.Visible;
                            }
                        };
                    },
                    (controller)=> {
                        var service = new Service.AccountService();
                        service.OnLogonCompleted += () => {
                            controller.CloseAsync();
                        };
                        service.Logon(user, pwd, (exp) => {
                            controller.SetMessage(exp.Message);
                            controller.SetCancelable(true);
                        });
                    });

            }
            catch (Exception exp)
            {
                UI.MetroUIExtender.Alert(exp.Message);
            }
        }

        private void OnButtonLogoutClick(object sender, RoutedEventArgs e)
        {
            try
            {
                UI.MetroUIExtender.Progress("正在注销...", "请稍等......", true,
                    (controller) => {
                        controller.Canceled += (ss, ee) => {
                            controller.CloseAsync();
                        };
                        controller.Closed += (ss, ee) => {
                            if (!controller.IsCanceled)
                            {
                                btnLogout.Visibility = Visibility.Collapsed;
                                gridMenu.Visibility = Visibility.Collapsed;
                                gridLogon.Visibility = Visibility.Visible;
                            }
                        };
                    },
                    (controller) => {
                        var service = new Service.AccountService();
                        service.OnLogonCompleted += () => {
                            controller.CloseAsync();
                        };
                        service.Logoff((exp) => {
                            controller.SetMessage(exp.Message);
                            controller.SetCancelable(true);
                        });
                    });

            }
            catch (Exception exp)
            {
                UI.MetroUIExtender.Alert(exp.Message);
            }
        }

        private void OnTileEmployeeClick(object sender, RoutedEventArgs e)
        {
            var wnd = new UI.EmployeeManageWnd();
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

        private void OnTileCameraClick(object sender, RoutedEventArgs e)
        {
            //
            var wnd = new UI.CameraManageWnd();
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

        private void OnTileCheclInClick(object sender, RoutedEventArgs e)
        {
            //
            var wnd = new UI.CheckManageWnd();
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
        //IntPtr hPicture = IntPtr.Zero;
        //IntPtr m_hOpenVideo = IntPtr.Zero;
        //HHOPEN_PICTURE_INFO sOpenPicInfo = new HHOPEN_PICTURE_INFO();
        //HHOPEN_CHANNEL_INFO openInfo = new HHOPEN_CHANNEL_INFO();
        //ObservableCollection<byte[]> images = new ObservableCollection<byte[]>();
        //HHCBOnDraw onDraw = null;
        //bool m_bExit = false;

        void InitUI()
        {
            //Read();
            //onDraw = OnCallbackDrawDemand;
            //imageFaceList.ItemsSource = images;
        }

        void BindEvents()
        {
            this.Loaded += OnWndLoaded;
            this.Unloaded += OnWndUnloaded;
            //btnOpenPicture.Click += OnButtonOpenPictureClick;
            //btnClosePicture.Click += OnButtonClosePictureClick;
            //btnOpenStream.Click += OnButtonOpenStreamClick;
            //btnCloseStream.Click += OnButtonCloseStreamClick;
            btnRegister.Click += OnButtonRegisterClick;
            btnLogon.Click += OnButtonLogonClick;
            btnLogout.Click += OnButtonLogoutClick;
            tileEmployee.Click += OnTileEmployeeClick;
            tileCamera.Click += OnTileCameraClick;
            tileCheckIn.Click += OnTileCheclInClick;
        }

        //int MessageNotifyCallback(IntPtr wParam, IntPtr lParam, IntPtr pContext)
        //{
        //    System.Diagnostics.Debug.WriteLine("MessageNotifyCallback");
        //    return 0;
        //}

        //int CapturePictureCallback(IntPtr hPictureChn, IntPtr pPicData, int nPicLen, UInt32 dwClientID, IntPtr pContext)
        //{
        //    try
        //    {
        //        var prev = DateTime.Now;
        //        System.Diagnostics.Debug.WriteLine(string.Format("PREV: {0}:{1}", prev.Second, prev.Millisecond));
        //        FACE_PICS_INFO_S stImageInfo = (FACE_PICS_INFO_S)Marshal.PtrToStructure(pPicData, typeof(FACE_PICS_INFO_S));
        //        if (stImageInfo.dwBgnFlag == 0x12345678 && stImageInfo.dwEndFlag == 0x87654321)
        //        {
        //            int dwDatalen = 0;
        //            for (int i = 0; i < stImageInfo.byPicNum; i++)
        //            {
        //                int nDataLen1 = (int)stImageInfo.stPics[i].dwPicLen;
        //                if (nDataLen1 > 0)
        //                {
        //                    byte[] buf = new byte[nDataLen1];
        //                    IntPtr addr = IntPtr.Add(pPicData, (int)Marshal.SizeOf(stImageInfo) + (int)dwDatalen);
        //                    Marshal.Copy(addr, buf, 0, nDataLen1);
        //                    dwDatalen += nDataLen1;
        //                    this.Dispatcher.BeginInvoke(new Action<byte[]>((img) =>
        //                    {
        //                        images.Add(img);

        //                        //BitmapImage bitmapImage = new BitmapImage();
        //                        //using (var ms = new MemoryStream(img))
        //                        //{
        //                        //    bitmapImage.BeginInit();
        //                        //    bitmapImage.StreamSource = ms;
        //                        //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //                        //    bitmapImage.EndInit();
        //                        //    bitmapImage.Freeze();
        //                        //}

        //                        //.Source = bitmapImage;
        //                    }), new object[] { buf });
        //                }
        //            }
        //        }

        //        //FaceRecognizeWrapper.CopyMemory(Marshal.A, pPicData, Marshal.SizeOf(stImageInfo));

        //        //HH_PICTURE_INFO sPicInfo;
        //        //var ret = FaceRecognizeWrapper.SNET_ReadPictureInfo(hPictureChn, out sPicInfo);
        //        //if(ret  == 0)
        //        //{
        //        //    //
        //        //}
        //        var now = DateTime.Now;
        //        System.Diagnostics.Debug.WriteLine(string.Format("NOW: {0}:{1}", now.Second, now.Millisecond));
        //    }
        //    catch (Exception exp)
        //    {
        //        System.Diagnostics.Debug.WriteLine(exp);
        //    }
        //    return 0;
        //}

        //int OnCallbackDrawDemand(UInt16 nPort, IntPtr hdc, int nWidth, int nHeight, IntPtr pContext)
        //{
        //    System.Diagnostics.Debug.WriteLine("OnCallbackDrawDemand");
        //    return 0;
        //}

        //int ChannelStreamCallback(IntPtr hOpenChannel,
        //                                   IntPtr pStreamData,
        //                                   UInt32 dwClientID,
        //                                   IntPtr pContext,
        //                                   ENCODE_VIDEO_TYPE encodeVideoType,
        //                                   ref HHAV_INFO pAVInfo)
        //{
        //    if(m_bExit)
        //    {
        //        return 0;
        //    }

        //    var wDisplayWnd = (UInt16)pContext;   //播放器编号

        //    UInt32 dwFrameSize = 0;
        //    HV_FRAME_HEAD pFrameHead = (HV_FRAME_HEAD)Marshal.PtrToStructure(pStreamData, typeof(HV_FRAME_HEAD));
        //    int ret = 0;

        //    dwFrameSize = (uint)Marshal.SizeOf(pFrameHead) + pFrameHead.nByteNum;
        //    //byte[] buf = new byte[dwFrameSize];
        //    //Marshal.Copy(pStreamData, buf, 0, (int)dwFrameSize);
        //    ret = HH5PlayerWrapper.HH5PLAYER_PutDecStreamDataEx(wDisplayWnd, pStreamData, dwFrameSize, (uint)encodeVideoType, ref pAVInfo);
        //    System.Diagnostics.Debug.WriteLine("ChannelStreamCallback");
        //    return 0;
        //}

        //void Read()
        //{
        //    using (var file = File.Open("D:\\error_log.txt", FileMode.Open))
        //    {
        //        List<string> txt = new List<string>();
        //        using (var stream = new StreamReader(file))
        //        {
        //            while (!stream.EndOfStream)
        //            {
        //                txt.Add(stream.ReadLine());
        //            }
        //        }

        //        System.Diagnostics.Debug.WriteLine(txt.Count);
        //        for(int i=txt.Count - 100;i<txt.Count;i++)
        //        {
        //            System.Diagnostics.Debug.WriteLine(txt[i]);
        //        }
        //        file.Close();
        //    }
        //}
        #endregion
    }
}
