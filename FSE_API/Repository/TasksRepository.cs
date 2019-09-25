using FSE_API.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FSE_API.Repository
{
    public class TasksRepository : ITasksRepository
    {
        protected readonly FSEDBEntities FseDB = new FSEDBEntities();
        public TasksRepository()
        {
            FseDB.Configuration.ProxyCreationEnabled = true;
        }
        public List<Task> Get()
        {
            return FseDB.Tasks.AsEnumerable().ToList();
        }

        public Task GetTask(int i)
        {
            return FseDB.Tasks.Find(i);
        }

        public int Post(Task value)
        {
            FseDB.Tasks.Add(value);
            return FseDB.SaveChanges();
        }

        public int Put(int id, Task value)
        {
            FseDB.Entry(value).State = EntityState.Modified;
            return FseDB.SaveChanges();
        }
        public int Delete(int id)
        {
            FseDB.Tasks.Remove(FseDB.Tasks.FirstOrDefault(x => x.Task_ID == id));
            return FseDB.SaveChanges();
        }
    }
}