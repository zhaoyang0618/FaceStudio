﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Contract
{
    public class CheckinRecordQueryInfo
    {
        public string EmployeeName
        {
            get;set;
        }

        public DateTime? StartDate
        {
            get;set;
        }

        public DateTime? EndDate
        {
            get;set;
        }
    }
}

