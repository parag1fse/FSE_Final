using FSE_API.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FSE_API.Repository
{
    public class UsersRepository : IUsersRepository
    {
        protected readonly FSEDBEntities FseDB = new FSEDBEntities();
        public UsersRepository()
        {
            FseDB.Configuration.ProxyCreationEnabled = false;
        }
        public List<User> Get()
        {
            return FseDB.Users.AsEnumerable().ToList();
        }

        public User GetUser(int i)
        {
            return FseDB.Users.Find(i);
        }

        public int Post(User value)
        {
            FseDB.Users.Add(value);
            return FseDB.SaveChanges();
        }

        public int Put(int id, User value)
        {
            FseDB.Entry(value).State = EntityState.Modified;
            return FseDB.SaveChanges();
        }
        public int Delete(int id)
        {
            FseDB.Users.Remove(FseDB.Users.FirstOrDefault(x => x.User_ID == id));
            return FseDB.SaveChanges();
        }
    }
}