using Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Model
{
    class CheckinRecordUI
    {
        public CheckinRecord Record
        {
            get;set;
        }

        public string Name
        {
            get
            {
                if (null == this.Record) return null;
                if (null == this.Record.Employee) return null;
                return this.Record.Employee.Name;
            }
        }

        public string Date
        {
            get
            {
                if (null == this.Record) return null;
                return this.Record.CheckinTime.ToString("yyyy-MM-dd");
            }
        }

        public string Time
        {
            get
            {
                if (null == this.Record) return null;
                return this.Record.CheckinTime.ToString(@"hh\:mm\:ss");
            }
        }

        public string Result
        {
            get
            {
                if (null == this.Record) return null;
                if (null == this.Record.Employee) return null;
                if (null == this.Record.Employee.AttendanceRule) return null;
                var week = this.Record.CheckinTime.DayOfWeek;
                //工作日
                bool bWork = (week == DayOfWeek.Monday && this.Record.Employee.AttendanceRule.Monday) ||
                    (week == DayOfWeek.Tuesday && this.Record.Employee.AttendanceRule.Tuesday) ||
                    (week == DayOfWeek.Wednesday && this.Record.Employee.AttendanceRule.Wednesday) ||
                    (week == DayOfWeek.Thursday && this.Record.Employee.AttendanceRule.Thursday) ||
                    (week == DayOfWeek.Friday && this.Record.Employee.AttendanceRule.Friday) ||
                    (week == DayOfWeek.Saturday && this.Record.Employee.AttendanceRule.Saturday) ||
                    (week == DayOfWeek.Sunday && this.Record.Employee.AttendanceRule.Sunday);

                if(bWork)
                {
                    var time = new TimeSpan(this.Record.CheckinTime.Hour, this.Record.CheckinTime.Minute, this.Record.CheckinTime.Second);
                    var mid = new TimeSpan(12, 0, 0);
                    if (time <= this.Record.Employee.AttendanceRule.StartTime ||
                        time >= this.Record.Employee.AttendanceRule.EndTime)
                    {
                        return "正常";
                    }
                    else if (time > this.Record.Employee.AttendanceRule.StartTime && time < mid)
                    {
                        return "迟到";
                    }
                    else
                    {
                        return "早退";
                    }
                }
                else
                {
                    return "休息日加班";
                }
            }
        }
    }
}
