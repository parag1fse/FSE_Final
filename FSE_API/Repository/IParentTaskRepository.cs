using System.Collections.Generic;
using FSE_API.DBContext;

namespace FSE_API.Repository
{
    public interface IParentTaskRepository
    {
        int Delete(int id);
        List<ParentTask> Get();
        int Post(ParentTask value);
        int Put(int id, ParentTask value);
        ParentTask GetParentTask(int i);

    }
}