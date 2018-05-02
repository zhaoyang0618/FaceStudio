using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Web.Models
{
    public class faceFindInfo
    {
        public string faceId { get; set; }
        public string feature { get; set; }
        public string featureKey { get; set; }
        public string path { get; set; }
        public string personId { get; set; }
    }

    public class faceinfo
    {
        public string personid { get; set; }
        public string faceid { get; set; }
        public string imagebase64 { get; set; }
    }

    public class faceDetailInfoForLocal
    {
        public string deviceKey { get; set; }
        public string personid { get; set; }
        public string time { get; set; }
        public string imagePath { get; set; }
        public string faceid { get; set; }
        public string ip { get; set; }
        public string feature { get; set; }
        public string featureKey { get; set; }
    }

    public class passTime
    {
        public string personId { get; set; }
        public string passtime { get; set; }
    }

    public class ufaces
    {
        public int    version { get; set; }
        public string deviceBrand { get; set; }
    }

    public class PasswordSetPara
    {
        public string oldPass { get; set; }
        public string newPass { get; set; }

    }

    public class Person
    {
        public string id { get; set; }
        public string name { get; set; }
        public string idcardnum { get; set; }
    }

    public class PersonLocal
    {
        // 序号,规则定义：待补充
        public string id
        {
            get; set;
        }
        // 名字
        public string name
        {
            get; set;
        }
        // 欢迎信息：如"签到成功"；"欢迎光临"；"打卡成功"
        public string welcomeInfo
        {
            get; set;
        }
        // 设备绑定:数据类型应该是个列表
        public string defaultDevice
        {
            get; set;
        }
        // 考勤规则?数据类型待修改或者用string关联其它表
        public string attendanceRule
        {
            get; set;
        }
        // 照片信息: 应该是三张照片
        public string pictures
        {
            get; set;
        }
        // 以上为必选
        // 工号
        public string idcardNum
        {
            get; set;
        }
        // 部门
        public string department
        {
            get; set;
        }
        // 联系方式
        public string mobilePhone
        {
            get; set;
        }
        // 职位
        public string position
        {
            get; set;
        }
        // 性别
        public string sextype { get; set; }
        // 备注
        public string remarks { get; set; }
    }

    // 设备参数 
    public class DevicePara
    {
        // 数据类型是否需要修改？？？
        /*
         *  create table  deviceparams(
         *  char[24] snid, // 设备序列号
         *  char[20] ipaddress,
         *  char[32] company,
         *  int identifydistance,
         *  ........
         *  int multiFaceDetect,
         *  )
         */

        // 公司名称 
        public string company { get; set; }
        // 识别距离：0~6. 0无限制
        public int identifydistance { get; set; }
        // 识别阈值 60~100, 80 default
        public int identifyscore { get; set; }
        // set to 0
        public int saveIdentifyTime { get; set; }
        // 语音播报模式：1:不播报  2:播报名字   100:自定义
        public int ttsMode { get; set; }
        public string ttsContent { get; set; }
        // 1 or 100
        public int displayMode { get; set; }
        // 自定义显示内容
        public string displayContent { get; set; }
        // 串口模式：1：开门；2：不输出；3：输出人员id；4：输出ID好，100：自定义。
        public int serialportmode { get; set; }
        public string serialportContent { get; set; }
        // bool or enum
        public string screendirection { get; set; }
        // 公司标语 
        public string slogan { get; set; }
        // 改成图片数据类型
        public string logo { get; set; }
        // 公司简介
        public string memo { get; set; }
        // 陌生人开关 1:不识别；2：识别
        public int strangerMode { get; set; }
        // default : 3
        public string strangerTimeThreshold { get; set; }
        //不播报 1, 播报2, 自定义100
        public string ttsStrangerMode { get; set; }
        public string ttsStrangerContent { get; set; }

        // 多人脸检测：默认1， 2：只识别最大人脸
        public int multiFaceDetect { get; set; }
    };

    public class Device
    {
        // 数据类型是否需要修改？？？
        /*
         *  create table  devicelist(
         *  char[24] snid, // 设备序列号
         *  char[20] ipaddress,
         *  char[32] devicename,
         *  char[256] remarks,
         *  )
         */
        public string snid { get; set; }
        public string ipAddress { get; set; }
        public string DeviceName { get; set; }
        public string staticipaddr { get; set; }
        public string remarks { get; set; }
        public DevicePara parameters { get; set; }

    }

    public class IdentifyRecords
    {
        public string ID { get; set; }
        public string personID { get; set; }
        public string path { get; set; }    // 16G 内置存储空间
        public int state { get; set; }
        public long time { get; set; }
        public int type { get; set; }

    }

    public class PageInfo
    {
        public int index { get; set; }
        public int length { get; set; }
        public int size { get; set; }
        public int total { get; set; }
    }

    public class RecordsResult
    {
        public PageInfo pages { get; set; }
        public IdentifyRecords[] records { get; set; }
    }

    public class NetInfo
    {
        public int dHCPMode { get; set; }
        public string dNS1 { get; set; }
        public string dNS2 { get; set; }
        public string gateWay { get; set; }
        public string ipAddress { get; set; }
        public string subNet { get; set; }
        public string msg { get; set; }
    }

    public class ImageBase64Converter
    {
        /// <summary>
        /// 根据base64字符串返回一个封装好的GDI+位图。
        /// </summary>
        /// <param name="base64string">可转换成位图的base64字符串。</param>
        /// <returns>Bitmap对象。</returns>
        public Bitmap GetImageFromBase64(string base64string)
        {
            byte[] b = Convert.FromBase64String(base64string);
            MemoryStream ms = new MemoryStream(b);
            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }

        /// <summary>
        /// 将图片转换成base64字符串。
        /// </summary>
        /// <param name="imagefile">需要转换的图片文件。</param>
        /// <returns>base64字符串。</returns>
        public string GetBase64FromImage(string imagefile)
        {
            string strbaser64 = "";

            try
            {
                Bitmap bmp = new Bitmap(imagefile);
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    ms.Close();

                    strbaser64 = Convert.ToBase64String(arr);
                }
            }
            catch (Exception)
            {
                throw new Exception("Something wrong during convert!");
            }

            return strbaser64;
        }
    }
}
