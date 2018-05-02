using Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Model
{
    class AttendanceRuleUI
    {
        public AttendanceRule AttendanceRule
        {
            get;
            set;
        }

        /// <summary>
        /// 时间段
        /// </summary>
        public string TimeInterval
        {
            get
            {
                if (AttendanceRule == null) return null;
                return string.Format("{0} - {1}", AttendanceRule.StartTime.ToString("HH:MM"), AttendanceRule.EndTime.ToString("HH:MM"));
            }
        }

        string _workdate = null;
        /// <summary>
        /// 工作日
        /// </summary>
        public string WorkDate
        {
            get
            {
                if(string.IsNullOrEmpty(_workdate))
                {
                    if(AttendanceRule != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        if(AttendanceRule.Monday)
                        {
                            sb.Append("星期一; ");
                        }
                        if (AttendanceRule.Tuesday)
                        {
                            sb.Append("星期二; ");
                        }
                        if (AttendanceRule.Wednesday)
                        {
                            sb.Append("星期三; ");
                        }
                        if (AttendanceRule.Thursday)
                        {
                            sb.Append("星期四; ");
                        }
                        if (AttendanceRule.Friday)
                        {
                            sb.Append("星期五; ");
                        }
                        if (AttendanceRule.Saturday)
                        {
                            sb.Append("星期六; ");
                        }
                        if (AttendanceRule.Sunday)
                        {
                            sb.Append("星期日; ");
                        }
                    }
                }

                return _workdate;

            }
        }
    }

    class SpecialAttendanceRuleUI
    {
        public SpecialAttendanceRule SpecialAttendanceRule
        {
            get;set;
        }
    }
}
