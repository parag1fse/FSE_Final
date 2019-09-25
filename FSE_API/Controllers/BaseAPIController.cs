using FSE_API.DBContext;
using FSE_API.Errors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace FSE_API.Controllers
{
    public class BaseAPIController : ApiController
    {

        protected readonly FSEDBEntities FseDB = new FSEDBEntities();
        protected HttpResponseMessage ToJson(dynamic obj)
        {
            try {
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                LogError.Log(ex);
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }
    }
}
