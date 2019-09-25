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
    public class UsersController : BaseAPIController
    {
        IUsersRepository repository = null;
        public UsersController()
        {
            repository = new UsersRepository();
        }

        public UsersController(IUsersRepository Repository)
        {
            repository = Repository;
        }
        public HttpResponseMessage Get()
        {
            try
            {
                return ToJson(repository.Get());
            }
            catch (Exception ex)
            {
                LogError.Log(ex);
                return ToJson(null);
            }
        }

        public HttpResponseMessage GetUser(int i)
        {
            try
            {
                return ToJson(repository.GetUser(i));
            }
            catch (Exception ex)
            {
                LogError.Log(ex);
                return ToJson(null);
            }
        }

        public HttpResponseMessage Post([FromBody]User value)
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

        public HttpResponseMessage Put(int id, [FromBody]User value)
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
