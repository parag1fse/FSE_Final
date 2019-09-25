using FSE_API.DBContext;
using FSE_API.Errors;
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
    public class ParentTaskController : BaseAPIController
    {
        IParentTaskRepository repository = null;
        public ParentTaskController()
        {
            repository = new ParentTaskRepository();
        }

        public ParentTaskController(IParentTaskRepository Repository)
        {
            repository = Repository;
        }

        public HttpResponseMessage Get()
        {
            try
            {
                return ToJson(repository.Get());
            }
            catch(Exception ex)
            {
                LogError.Log(ex);
                return ToJson(null);
            }
        }

        public HttpResponseMessage GetParentTask(int i)
        {
            try
            {
                return ToJson(repository.GetParentTask(i));
            }
            catch (Exception ex)
            {
                LogError.Log(ex);
                return ToJson(null);
            }
        }

        public HttpResponseMessage Post([FromBody]ParentTask value)
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

        public HttpResponseMessage Put(int id, [FromBody]ParentTask value)
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
