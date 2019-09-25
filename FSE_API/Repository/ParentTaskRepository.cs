using FSE_API.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FSE_API.Repository
{
    public class ParentTaskRepository : IParentTaskRepository
    {
        protected readonly FSEDBEntities FseDB = new FSEDBEntities();
        public ParentTaskRepository()
        {
            FseDB.Configuration.ProxyCreationEnabled = false;
        }
        public List<ParentTask> Get()
        {
            return FseDB.ParentTasks.AsEnumerable().ToList();
        }

        public ParentTask GetParentTask(int i)
        {
            return FseDB.ParentTasks.Find(i);
        }

        public int Post(ParentTask value)
        {
            FseDB.ParentTasks.Add(value);
            return FseDB.SaveChanges();
        }

        public int Put(int id, ParentTask value)
        {
            FseDB.Entry(value).State = EntityState.Modified;
            return FseDB.SaveChanges();
        }
        public int Delete(int id)
        {
            FseDB.ParentTasks.Remove(FseDB.ParentTasks.FirstOrDefault(x => x.Parent_ID == id));
            return FseDB.SaveChanges();
        }
    }
}