using FSE_API.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FSE_API.Repository
{
    public interface IProjectRepository
    {
        int Delete(int id);
        List<Project> Get();
        int Post(Project value);
        int Put(int id, Project value);
        Project GetProject(int i);
    }

    public interface ITasksRepository
    {
        int Delete(int id);
        List<Task> Get();
        int Post(Task value);
        int Put(int id, Task value);
        Task GetTask(int i);
    }

    public interface IUsersRepository
    {
        int Delete(int id);
        List<User> Get();
        int Post(User value);
        int Put(int id, User value);
        User GetUser(int i);
    }
}
