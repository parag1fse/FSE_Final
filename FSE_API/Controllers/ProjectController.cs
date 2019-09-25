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
    public class ProjectController : BaseAPIController
    {
        IProjectRepository repository = null;
        public ProjectController()
        {
            repository = new ProjectRepository();
        }

        public ProjectController(IProjectRepository Repository)
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
        [Route("api/project")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                var project = repository.Get().Select(b =>
                    new ProjectModel()
                    {
                        Project_ID = b.Project_ID,
                        Project_Name = b.Project_Name,
                        Start_Date = b.Start_Date,
                        End_Date = b.End_Date,
                        Priority = (int)b.Priority,
                        NoOfTasks =  b.Tasks.Count,
                        CompletedTask = b.Tasks.Count == 0 ? 0 :((double)b.Tasks.Count(i => i.Status == "C")) / b.Tasks.Count * 100
                    });

                return ToJson(project);
            }
            catch (Exception ex)
            {
                LogError.Log(ex);
                return ToJson(null);
            }
        }
        [Route("api/project/{i:int}")]
        [HttpGet]
        public HttpResponseMessage GetProject(int i)
        {
            try
            {
                var project = repository.GetProject(i);

                var projectdto = new ProjectModel()
                {
                    Project_ID = project.Project_ID,
                    Project_Name = project.Project_Name,
                    Start_Date = project.Start_Date,
                    End_Date = project.End_Date,
                    Priority = (int)project.Priority,
                    NoOfTasks = project.Tasks.Count,
                    CompletedTask = project.Tasks.Count == 0 ? 0 : ((double)project.Tasks.Count(c =>c.Status == "C")) / project.Tasks.Count * 100
                };


                return ToJson(projectdto);
               // return ToJson(repository.GetProject(i));
            }
            catch (Exception ex)
            {
                LogError.Log(ex);
                return ToJson(null);
            }
        }

        [Route("api/project")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Project value)
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

        [Route("api/project/{id:int}")]
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]Project value)
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
        [Route("api/project/{id:int}")]
        [HttpDelete]
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
