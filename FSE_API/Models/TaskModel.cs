using FSE_API.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSE_API.Models
{
    public class TaskModel
    {

        public int Task_ID { get; set; }
        public Nullable<int> Parent_ID { get; set; }
        public Nullable<int> Project_ID { get; set; }
        public string Task_Name { get; set; }

        public string ParentTask_Name { get; set; }
        public Nullable<System.DateTime> Start_Date { get; set; }
        public Nullable<System.DateTime> End_Date { get; set; }
        public Nullable<int> Priority { get; set; }
        public string Status { get; set; }

        //public virtual ParentTask ParentTask { get; set; }
        //public virtual Project Project { get; set; }

        //public virtual ICollection<User> Users { get; set; }
    }
}