using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Utils
{ 
    public delegate int ChannelStreamCallback(IntPtr hOpenChannel, IntPtr pStreamData, UInt32 dwClientID, IntPtr pContext, ENCODE_VIDEO_TYPE encodeVideoType, ref HHAV_INFO pAVInfo);
    public delegate int PictureCallback(IntPtr hPictureChn, IntPtr pPicData, int nPicLen, UInt32 dwClientID, IntPtr pContext);
    public delegate int MessageNotifyCallback(IntPtr wParam, IntPtr lParam, IntPtr pContext);

    public enum NET_PROTOCOL_TYPE
    {
        NET_PROTOCOL_TCP = 0,           //TCP协议
        NET_PROTOCOL_UDP = 1,           //UDP协议
        NET_PROTOCOL_MULTI = 2			//多播协议
    }

    public enum CONNECT_STATUS
    {
        CONNECT_STATUS_NONE,            //未连接
        CONNECT_STATUS_OK,              //已经连接
        CONNECT_STATUS_DATA,
        CONNECT_STATUS_EXIST,
    }

    //========================================================
    //视频编码格式									
    //========================================================
    public enum ENCODE_VIDEO_TYPE
    {
        EV_TYPE_NONE = 0xFFFF,
        EV_TYPE_PAL_D1 = 0x00,      //PAL制D1		704 * 576
        EV_TYPE_PAL_HD1 = 0x01,     //PAL制HalfD1	704 * 288
        EV_TYPE_PAL_CIF = 0x02,     //PAL制CIF		352 * 288
        EV_TYPE_VGA = 0x03,     //VGA			640 * 480
        EV_TYPE_HVGA = 0x04,        //HVGA			640 * 240
        EV_TYPE_CVGA = 0x05,        //CVGA			320 * 240
        EV_TYPE_PAL_QCIF = 0x06,        //PAL制QCIF		176 * 144
        EV_TYPE_QVGA = 0x07,        //QVGA			160 * 120
        EV_TYPE_NTSC_D1 = 0x08,     //N制D1			704 * 480
        EV_TYPE_NTSC_HD1 = 0x09,        //N制HalfD1		704 * 240
        EV_TYPE_NTSC_CIF = 0x0A,        //N制CIF		352 * 240
        EV_TYPE_NTSC_QCIF = 0x0E,       //N制QCIF		176 * 120

        //H.264
        EV_H264_PAL_D1 = 0x10,      //H264_2,PAL制D1		704 * 576
        EV_H264_PAL_HD1 = 0x11,     //H264_2,PAL制HalfD1	704 * 288
        EV_H264_PAL_CIF = 0x12,     //H264_2,PAL制CIF		352 * 288
        EV_H264_VGA = 0x13,     //H264_2,VGA			640 * 480
        EV_H264_HVGA = 0x14,        //H264_2,HVGA			640 * 240
        EV_H264_CVGA = 0x15,        //H264_2,CVGA			320 * 240
        EV_H264_PAL_QCIF = 0x16,        //H264_2,PAL制QCIF		176 * 144
        EV_H264_QVGA = 0x17,        //H264_2,QVGA			160 * 120
        EV_H264_NTSC_D1 = 0x18,     //H264_2,N制D1			704 * 480
        EV_H264_NTSC_HD1 = 0x19,        //H264_2,N制HalfD1		704 * 240
        EV_H264_NTSC_CIF = 0x1A,        //H264_2,N制CIF			352 * 240
        EV_H264_NTSC_QCIF = 0x1E,       //H264_2,N制QCIF		176 * 120

        //标准MPEG4
        EV_MPEG4_PAL_D1 = 0x20,     //PAL制D1				704 * 576
        EV_MPEG4_PAL_HD1 = 0x21,        //PAL制HalfD1			704 * 288
        EV_MPEG4_PAL_CIF = 0x22,        //PAL制CIF				352 * 288
        EV_MPEG4_VGA = 0x23,        //VGA					640 * 480
        EV_MPEG4_HVGA = 0x24,       //HVGA					640 * 240
        EV_MPEG4_CVGA = 0x25,       //CVGA					320 * 240
        EV_MPEG4_PAL_QCIF = 0x26,       //PAL制QCIF				176 * 144
        EV_MPEG4_QVGA = 0x27,       //QVGA					160 * 120
        EV_MPEG4_NTSC_D1 = 0x28,        //N制D1					704 * 480
        EV_MPEG4_NTSC_HD1 = 0x29,       //N制HalfD1				704 * 240
        EV_MPEG4_NTSC_CIF = 0x2A,       //N制CIF				352 * 240
        EV_MPEG4_NTSC_QCIF = 0x2E,      //N制QCIF				176 * 120

        //MJPEG
        EV_MJPEG_PAL_D1 = 0x30,     //MJPEG,PAL制D1        704 * 576
        EV_MJPEG_PAL_HD1 = 0x31,     //MJPEG,PAL制HalfD1    704 * 288
        EV_MJPEG_PAL_CIF = 0x32,     //MJPEG,PAL制CIF       352 * 288
        EV_MJPEG_VGA = 0x33,     //MJPEG,VGA            640 * 480
        EV_MJPEG_HVGA = 0x34,     //MJPEG,HVGA           640 * 240
        EV_MJPEG_CVGA = 0x35,     //MJPEG,CVGA           320 * 240
        EV_MJPEG_PAL_QCIF = 0x36,     //MJPEG,PAL制QCIF      176 * 144
        EV_MJPEG_QVGA = 0x37,     //MJPEG,QVGA           160 * 120
        EV_MJPEG_NTSC_D1 = 0x38,     //MJPEG,N制D1          704 * 480
        EV_MJPEG_NTSC_HD1 = 0x39,     //MJPEG,N制HalfD1      704 * 240
        EV_MJPEG_NTSC_CIF = 0x3A,     //MJPEG,N制CIF         352 * 240
        EV_MJPEG_NTSC_QCIF = 0x3E,     //MJPEG,N制QCIF        176 * 120

        //JPEG
        EV_JPEG_PAL_D1 = 0x40,     //JPEG,PAL制D1        704 * 576
        EV_JPEG_PAL_HD1 = 0x41,     //JPEG,PAL制HalfD1    704 * 288
        EV_JPEG_PAL_CIF = 0x42,     //JPEG,PAL制CIF       352 * 288
        EV_JPEG_VGA = 0x43,     //JPEG,VGA            640 * 480
        EV_JPEG_HVGA = 0x44,     //JPEG,HVGA           640 * 240
        EV_JPEG_CVGA = 0x45,     //JPEG,CVGA           320 * 240
        EV_JPEG_PAL_QCIF = 0x46,     //JPEG,PAL制QCIF      176 * 144
        EV_JPEG_QVGA = 0x47,     //JPEG,QVGA           160 * 120
        EV_JPEG_NTSC_D1 = 0x48,     //JPEG,N制D1          704 * 480
        EV_JPEG_NTSC_HD1 = 0x49,     //JPEG,N制HalfD1      704 * 240
        EV_JPEG_NTSC_CIF = 0x4A,     //JPEG,N制CIF         352 * 240
        EV_JPEG_NTSC_QCIF = 0x4E,     //JPEG,N制QCIF        176 * 120

        //
        EA_G722_S16B16C1 = 0x0100,  //音频，G722
        EA_G711A_S16B16C1 = 0x0200, //音频，G711A
        EA_G711MU_S16B16C1 = 0x0300,    //音频，G711MU
        EA_ADPCM_S16B16C1 = 0x0400, //音频，ADPCM
        EA_G726_S16B16C1 = 0x0500,  //音频，G726
        EA_BUTT_S16B16C1 = 0x0600,  //音频，BUTT
        EA_MPT_S16B16C1 = 0x0700,   //音频，MPT
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HHOPEN_PICTURE_INFO
    {
        public UInt32 dwClientID;
        public UInt32 nOpenChannel;
        public NET_PROTOCOL_TYPE protocolType;
        public PictureCallback funcPictureCallback;
        public IntPtr pCallbackContext;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HH_PICTURE_INFO
    {
        public IntPtr hOpenChannel;

        [MarshalAs(UnmanagedType.ByValTStr,SizeConst=129)]        
        public byte[] szServerIP;
        public UInt32 nServerPort;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public byte[] szDeviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public byte[] szUserName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public byte[] szUserPassword;

        public UInt32 dwClientID;

        public CONNECT_STATUS openStatus;
        public UInt32 nVersion;
        public UInt32 nOpenID;
        public UInt32 nPriority;
        public UInt32 nOpenChannelNo;
        public UInt32 nMulticastAddr;
        public UInt32 nMulticastPort;
        public UInt32 nPicWidth;
        public UInt32 nPicHeight;
        public UInt32 nPicBits;
        public Int32 picFormatType;//0:JPEG,1:BMP

        public NET_PROTOCOL_TYPE protocolType;
        public PictureCallback funcStreamCallback;
        public IntPtr pCallbackContext;

        public UInt32 dwDeviceID;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FACE_AREA_S
    {
        public UInt16 wRoiLeft;
        public UInt16 wRoiTop;
        public UInt16 wRoiRight;
        public UInt16 wRoiBottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FACE_SNAP_PIC_INFO
    {
        public UInt32 dwPicLen;           //图片长度
        public Byte byCapType;          //抓拍类型	0x1 人离开后抓拍 0x2 实时抓拍 0x4 间隔抓拍
        public Byte byImageType;        //图片类型	0x1 场景图 0x2 人脸图 0x4 人体图
        public Byte byQulity;           //0~100
        public Byte byEncoderType;      //目前，值为0代表jpeg图片
        public Int64 u64CapTime;         //抓图的时间    
        public UInt16 wImageWidth;        //图片宽度
        public UInt16 wImageHeight;       //图片高度  
        public FACE_AREA_S stTgtFaceArea;      //人脸坐标
        public UInt16 wCapMsec;           //抓图的时间，毫秒
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
        public Byte[] byRes;
    }    /*图片信息*/

    [StructLayout(LayoutKind.Sequential)]
    public struct FACE_FEATURE_INFO
    {
        public UInt32 dwDataLen;
    } /*人脸特征信息*/

    [StructLayout(LayoutKind.Sequential)]
    public struct IP6ADDR
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] u6_addr8;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FACE_PICS_INFO_S
    {
        public UInt32 dwBgnFlag;          //0x12345678
        public UInt32 dwSize;             //消息长度 = wHeadSize + dwTotDataLen;为一次图片传输事务发起的消息通讯包的总长度
        public UInt32 dwIPAddr;           //设备IP,当byIpAddrType为0时有效
        public UInt32 dwDeviceID;         //设备ID
        public UInt32 dwTrackID;          //人脸跟踪号
        public UInt16 wVersion;           //协议版本号，目前为1
        public Byte byIpAddrType;       //摄像机IP地址类型: 0: ipv4 1:ipv6
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public Byte[] byRes1;
        public UInt16 wHeadSize;          //结构大小 sizeof(FACE_PICS_INFO_S)
        public Byte byFeatureNum;       //人脸特征数据个数，目前不支持，值为0
        public Byte byPicNum;           //图片数
        public UInt32 dwTotDataLen;       //图片裸数据+人脸特征数据总长度，目前暂不支持人脸特征数据，为图片裸数据总长度
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public FACE_SNAP_PIC_INFO[] stPics;          //图片信息
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public FACE_FEATURE_INFO[] stFeature;       //人脸特征数据信息
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
        public Byte[] byRes2;            //备用2	
        public UInt32 dwDevSendPicTimes;  //摄像机上传图片计数，设备启动后，第一次上传图片为0，每次上传图片计数加1
        public IP6ADDR stIpAddr6;          //摄像机ipv6地址，当byIpAddrType为1时有效，按struct in6_addr结构解析
        Int64 u64TgtCreateTime;   //人脸目标生成时间
        public UInt32 dwTgtSendPicTimes;  //当前抓拍目标上传图片计数，目标第一次上传图片为0，该目标每次上传图片，计数加1
        public UInt32 dwEndFlag;          //0x87654321
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HHOPEN_CHANNEL_INFO
    {
        public UInt32 dwClientID;
        public UInt32 nOpenChannel;
        public NET_PROTOCOL_TYPE protocolType;
        public ChannelStreamCallback funcStreamCallback;
        public IntPtr pCallbackContext;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HH_CHANNEL_INFO
    {
        public IntPtr hOpenChannel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
        public Byte[] szServerIP;
        public UInt32 nServerPort;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
        public Byte[] szDeviceName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public Byte[] szUserName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public Byte[] szUserPassword;

        public UInt32 dwClientID;

        public CONNECT_STATUS openStatus;
        public UInt32 nVersion;
        public UInt32 nOpenID;
        public UInt32 nPriority;
        public UInt32 nOpenChannelNo;
        public UInt32 nMulticastAddr;
        public UInt32 nMulticastPort;
        public HHAV_INFO avInformation;
        public ENCODE_VIDEO_TYPE encodeVideoType;

        public NET_PROTOCOL_TYPE protocolType;
        public ChannelStreamCallback funcStreamCallback;
        public IntPtr pCallbackContext;

        public UInt32 dwDeviceID;   //V4.0 add
    }

    public class FaceRecognizeWrapper
    {
        [DllImport("SNetapi.dll", EntryPoint = "SNET_Startup", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int SNET_Startup(IntPtr hNotifyWnd, UInt32 nCommandID, UInt32 dwFrameBufNum, Boolean bReadyRelay, Boolean bReadyCenter, string pLocalAddr);

        //关闭服务
        [DllImport("SNetapi.dll", EntryPoint = "SNET_Cleanup", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int SNET_Cleanup();

        //改变库通知应用的方式为回调函数
        [DllImport("SNetapi.dll", EntryPoint = "SNET_MessageCallback", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int SNET_MessageCallback(MessageNotifyCallback pCallback, IntPtr pContext);

        [DllImport("SNetapi.dll", EntryPoint = "SNET_OpenPicture", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int SNET_OpenPicture(string pServerIP, Int16 wServerPort, string pDeviceName, string pUserName, string pUserPassword, ref HHOPEN_PICTURE_INFO pOpenInfo, out IntPtr hOpenChannel, IntPtr hNotifyWindow);

        [DllImport("SNetapi.dll", EntryPoint = "SNET_ClosePicture", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int SNET_ClosePicture(IntPtr hOpenPicture);

        [DllImport("SNetapi.dll", EntryPoint = "SNET_ReadPictureInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int SNET_ReadPictureInfo(IntPtr hOpenPicture, out HH_PICTURE_INFO channelInfo);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", CharSet = CharSet.Ansi)]
        public extern static long CopyMemory(IntPtr dest, IntPtr source, int size);

        #region 视频预览通道
        //打开视频通道
        [DllImport("SNetapi.dll", EntryPoint = "SNET_OpenChannel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int SNET_OpenChannel(string pServerIP, UInt16 wServerPort, string pDeviceName, string pUserName, string pUserPassword, ref HHOPEN_CHANNEL_INFO pOpenInfo, out IntPtr hOpenChannel, IntPtr hNotifyWindow);

        //关闭视频通道
        [DllImport("SNetapi.dll", EntryPoint = "SNET_CloseChannel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int SNET_CloseChannel(IntPtr hOpenChannel);

        //读取打开视频通道信息
        [DllImport("SNetapi.dll", EntryPoint = "SNET_ReadChannelInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int SNET_ReadChannelInfo(UIntPtr hOpenChannel, out HH_CHANNEL_INFO channelInfo);

        #endregion

        //public unsafe static long CopyMemoryEx(byte[] dest, int DestStart, ref FACE_PICS_INFO_S source)
        //{
        //    IntPtr dp;
        //    IntPtr sp;
        //    fixed (byte* ds = &dest[DestStart])
        //    {
        //        fixed (FACE_PICS_INFO_S* sr = &source)
        //        {
        //            dp = (IntPtr)ds;
        //            sp = (IntPtr)sr;
        //            return CopyMemory(dp, sp, sizeof(FACE_PICS_INFO_S));
        //        }
        //    }
        //}
    }
}
