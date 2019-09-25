using FSE_API.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSE_API.Models
{
    public class ProjectModel
    {

        public int Project_ID { get; set; }
        public string Project_Name { get; set; }

        
        public Nullable<System.DateTime> Start_Date { get; set; }
        public Nullable<System.DateTime> End_Date { get; set; }
        public int Priority { get; set; }

        public int NoOfTasks { get; set; }

        public double CompletedTask { get; set; }

    }
}