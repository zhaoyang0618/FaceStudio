using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Utils
{
    public delegate int HHCBOnDraw(UInt16 nPort, IntPtr hdc, int nWidth, int nHeight, IntPtr pContext);   //支持外部调用HDC

    [StructLayout(LayoutKind.Sequential)]
    public struct HH5KAV_INFO
    {
        //视频参数
        public UInt32 nVideoEncodeType;      //视频编码格式
        public UInt32 nVideoHeight;          //视频图像高
        public UInt32 nVideoWidth;           //视频图像宽
        //音频参数
        public UInt32 nAudioEncodeType;      //音频编码格式
        public UInt32 nAudioChannels;            //通道数
        public UInt32 nAudioBits;                //位数
        public UInt32 nAudioSamples;         //采样率
    }

    public class HH5PlayerWrapper
    {
        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_InitSDK", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_InitSDK(IntPtr hHwnd);                  //初始化SDK资源

        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_ReleaseSDK", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_ReleaseSDK();                         //释放  SDK资源

        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_InitPlayer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_InitPlayer(UInt16 nPort, UInt32 hWnd);  //初始化播放器(参见HH5PLAYER_InitPlayer2)

        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_ReleasePlayer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_ReleasePlayer(UInt16 nPort);          //释放播放器

        //================================================================
        //流播放
        //================================================================
        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_OpenStream", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_OpenStream(UInt16 nPort);

        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_PutDecStreamData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_PutDecStreamData(UInt16 nPort, IntPtr pBuf, UInt32 nSize, Int32 nDataType);

        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_PutDecStreamDataEx", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_PutDecStreamDataEx(UInt16 nPort, IntPtr pBuf, UInt32 nSize, UInt32 nDataType, ref HHAV_INFO pAVInfo);

        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_StopStream", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_StopStream(UInt16 nPort);
        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_CloseStream", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_CloseStream(UInt16 nPort);
        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_GetStreamFrameSize", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_GetStreamFrameSize(UInt16 nPort);
        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_GetAudioFrameSize", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_GetAudioFrameSize(UInt16 nPort);
        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_SetStreamPlaySpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_SetStreamPlaySpeed(UInt16 nPort, UInt16 nSpeed);
        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_GetStreamPlaySpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_GetStreamPlaySpeed(UInt16 nPort);

        ////fValue:每帧播放时间, 如: 40ms, 33.3ms, bDelayAdjust:是否在有丢帧时, 校准时间(注:若是对关键帧播放,一定要置为false)
        //[DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_ReleasePlayer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        //public static extern int HH5PLAYER_SetStreamPlaySpeed2(USHORT nPort, float fValue, bool bDelayAdjust = true);
        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_SetDecoderQulity", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_SetDecoderQulity(Boolean bQulity);               //0: 高质量  1: 低质量

        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_InitPlayer2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_InitPlayer2(UInt16 nPort, IntPtr hWnd, bool bSupportDraw = false);

        //[DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_SetDecoderQulity", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        //public static extern int HH5PLAYER_DrawImage(UInt16 nPort, int x, int y, unsigned char* pBmpImage, int nSize);
        //[DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_SetDecoderQulity", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        //public static extern int HH5PLAYER_DrawText(UInt16 nPort, int x, int y, char* pszText, LOGFONT ft, COLORREF crText, COLORREF crBack = -1/*crBack -1: 透明*/);
        //[DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_SetDecoderQulity", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        //public static extern int HH5PLAYER_SetDrawPen(UInt16 nPort, int nPenStyle, int nWidth, COLORREF crColor);
        //[DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_SetDecoderQulity", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        //public static extern int HH5PLAYER_DrawLine(UInt16 nPort, int x1, int y1, int x2, int y2);
        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_DrawRect", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_DrawRect(UInt16 nPort, int x1, int y1, int x2, int y2);
        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_ClearDraw", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_ClearDraw(UInt16 nPort, int nType);                   //nType: 0 all, 1 image, 2 text, 3 line, rect

        [DllImport("HH5PlayerSDK.dll", EntryPoint = "HH5PLAYER_RegCBOnDraw", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int HH5PLAYER_RegCBOnDraw(UInt16 nPort, HHCBOnDraw pCBOnDraw, IntPtr pContext);

    }
}
