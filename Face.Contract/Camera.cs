using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Contract
{
    public class Camera
    {
        public Guid ID
        {
            get;set;
        }

        [MaxLength(100)]
        public string Name
        {
            get;set;
        }

        [MaxLength(100)]
        public string IP
        {
            get;set;
        }

        public int Port
        {
            get;set;
        }

        [MaxLength(64)]
        public string OldPwd
        {
            get; set;
        }

        [MaxLength(64)]
        public string Pwd
        {
            get;set;
        }

        /// <summary>
        /// 摄像头位置
        /// </summary>
        [MaxLength(200)]
        public string Pos
        {
            get;set;
        }

        /// <summary>
        /// 地理位置：经度
        /// </summary>
        public double Longitude
        {
            get;set;
        }

        /// <summary>
        /// 地理位置：纬度
        /// </summary>
        public double Latitude
        {
            get; set;
        }

        public string Remark
        {
            get;set;
        }

        // 公司名称 
        public string Company
        {
            get; set;
        }

        // 识别距离：0~6. 0无限制
        public int Identifydistance
        {
            get; set;
        }

        // 识别阈值 60~100, 80 default
        public int Identifyscore
        {
            get; set;
        }

        // set to 0
        public int SaveIdentifyTime
        {
            get; set;
        }

        // 语音播报模式：1:不播报  2:播报名字   100:自定义
        public int TTSMode
        {
            get; set;
        }

        public string TTSContent
        {
            get; set;
        }

        // 1 or 100
        public int DisplayMode
        {
            get; set;
        }

        // 自定义显示内容
        public string DisplayContent
        {
            get; set;
        }

        // 串口模式：1：开门；2：不输出；3：输出人员id；4：输出ID好，100：自定义。
        public int SerialPortMode
        {
            get; set;
        }

        public string SerialPortContent
        {
            get; set;
        }

        // bool or enum
        public string ScreenDirection
        {
            get; set;
        }

        // 公司标语 
        public string Slogan
        {
            get; set;
        }

        // 公司Logo图片
        public Guid? PhotoImageID
        {
            get;set;
        }

        [ForeignKey("PhotoImageID")]
        public virtual PhotoImage Logo
        {
            get; set;
        }

        // 公司简介
        public string Memo
        {
            get; set;
        }

        // 陌生人开关 1:不识别；2：识别
        public int StrangerMode
        {
            get; set;
        }

        // default : 3
        public string StrangerTimeThreshold
        {
            get; set;
        }

        //不播报 1, 播报2, 自定义100
        public string TTSStrangerMode
        {
            get; set;
        }

        public string TTSStrangerContent
        {
            get; set;
        }

        // 多人脸检测：默认1， 2：只识别最大人脸
        public int MultiFaceDetect
        {
            get; set;
        }

        public DateTime CreateTime
        {
            get; set;
        }

        [MaxLength(100)]
        public string CreateUser
        {
            get; set;
        }

        public DateTime UpdateTime
        {
            get; set;
        }

        [MaxLength(100)]
        public string UpdateUser
        {
            get; set;
        }

        public virtual List<EmployeeCameraRelation> EmployeeCameraRelations
        {
            get;set;
        }

        public Camera()
        {
            this.EmployeeCameraRelations = new List<EmployeeCameraRelation>();
            this.Port = 8090;
            this.OldPwd = "12345";
            this.Pwd = "12345";
            this.Identifyscore = 80;
            this.TTSMode = 100;
            this.TTSContent = "{name}欢迎光临";
            this.DisplayMode = 100;
            this.DisplayContent = "{name}签到成功";
            this.SerialPortMode = 1;
            this.StrangerMode = 1;
            this.StrangerTimeThreshold = "3";
            this.TTSStrangerMode = "1";
            this.MultiFaceDetect = 1;
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
        }
    }
}
