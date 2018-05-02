using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Utils
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HHAV_INFO
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

    [StructLayout(LayoutKind.Sequential)]
    public struct HV_FRAME_HEAD
    {
        public Int16 zeroFlag;                // 0
        public Byte oneFlag;              // 1
        public Byte streamFlag;               // 数据帧标志 FRAME_FLAG_VP，FRAME_FLAG_VI，FRAME_FLAG_A
        public UInt32 nByteNum;             //数据帧大小
        public UInt32 nTimestamp;               //时间戳
    }
}
