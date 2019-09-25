using FSE_API.DBContext;
using FSE_API.Errors;
using FSE_API.Models;
using FSE_API.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FSE_API.Controllers
{
    public class TasksController : BaseAPIController
    {
        
        ITasksRepository repository = null;

        public TasksController()
        {
            repository = new TasksRepository();
        }
        public TasksController(ITasksRepository Repository)
        {
            repository = Repository;
        }
        //public HttpResponseMessage Get()
        //{
        //    try
        //    {
        //        return ToJson(repository.Get());
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError.Log(ex);
        //        return ToJson(null);
        //    }
        //}
        [Route("api/tasks")]
        public HttpResponseMessage Get()
        {
            try
            {
                var project = repository.Get().Select(b =>
                    new TaskModel()
                    {
                        Project_ID = b.Project_ID,
                        Task_Name = b.Task_Name,
                        Start_Date = b.Start_Date,
                        End_Date = b.End_Date,
                        Priority = (int)b.Priority,
                        ParentTask_Name = b.ParentTask == null ? "" : b.ParentTask.Parent_Task,
                        Parent_ID = b.Parent_ID,
                        Task_ID = b.Task_ID,
                        Status = b.Status
                    });

                return ToJson(project);
            }
            catch (Exception ex)
            {
                LogError.Log(ex);
                return ToJson(null);
            }
        }

        //public HttpResponseMessage GetTask(int i)
        //{
        //    try
        //    {
        //        return ToJson(repository.GetTask(i));
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError.Log(ex);
        //        return ToJson(null);
        //    }
        //}
        [Route("api/tasks/{i:int}")]
        [HttpGet]
        public HttpResponseMessage GetTask(int i)
        {
            try
            {
                var task = repository.GetTask(i);

                var taskdto = new TaskModel()
                {
                    Project_ID = task.Project_ID,
                    Task_Name = task.Task_Name,
                    Start_Date = task.Start_Date,
                    End_Date = task.End_Date,
                    Priority = (int)task.Priority,
                    ParentTask_Name = task.ParentTask == null ? "" : task.ParentTask.Parent_Task,
                    Parent_ID = task.Parent_ID,
                    Task_ID = task.Task_ID,
                    Status = task.Status
                };


                return ToJson(taskdto);
            }
            catch (Exception ex)
            {
                LogError.Log(ex);
                return ToJson(null);
            }
        }

        [Route("api/projectTasks/{i:int}")]
        [HttpGet]
        public HttpResponseMessage GetProjectTask(int i)
        {
            try
            {
                var task = repository.Get().FindAll(b => (int)b.Project_ID == i).Select(b =>
                   new TaskModel()
                   {
                       Project_ID = b.Project_ID,
                       Task_Name = b.Task_Name,
                       Start_Date = b.Start_Date,
                       End_Date = b.End_Date,
                       Priority = (int)b.Priority,
                       ParentTask_Name = b.ParentTask == null ? "" : b.ParentTask.Parent_Task,
                       Parent_ID = b.Parent_ID,
                       Task_ID = b.Task_ID,
                       Status = b.Status
                   });

                return ToJson(task);
            }
            catch (Exception ex)
            {
                LogError.Log(ex);
                return ToJson(null);
            }
        }

        [Route("api/tasks")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Task value)
        {
            try
            {
                return ToJson(repository.Post(value));
            }
            catch (Exception ex)
            {
                LogError.Log(ex);
                return ToJson(null);
            }

        }

        [Route("api/endtask/{id:int}")]
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]Task value)
        {
            try
            {
                return ToJson(repository.Put(id, value));
            }
            catch (Exception ex)
            {
                LogError.Log(ex);
                return ToJson(null);
            }

        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                return ToJson(repository.Delete(id));
            }
            catch (Exception ex)
            {
                LogError.Log(ex);
                return ToJson(null);
            }

        }
    }
}
