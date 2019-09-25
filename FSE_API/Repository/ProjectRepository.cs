using FSE_API.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FSE_API.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        protected readonly FSEDBEntities FseDB = new FSEDBEntities();
        public ProjectRepository()
        {
            FseDB.Configuration.ProxyCreationEnabled = true;
        }
        public List<Project> Get()
        {
            return FseDB.Projects.AsEnumerable().ToList();
        }

        public Project GetProject(int i)
        {
            return FseDB.Projects.Find(i);
        }

        public int Post(Project value)
        {
            FseDB.Projects.Add(value);
            return FseDB.SaveChanges();
        }

        public int Put(int id, Project value)
        {
            FseDB.Entry(value).State = EntityState.Modified;
            return FseDB.SaveChanges();
        }
        public int Delete(int id)
        {
            FseDB.Projects.Remove(FseDB.Projects.FirstOrDefault(x => x.Project_ID == id));
            return FseDB.SaveChanges();
        }
    }
}