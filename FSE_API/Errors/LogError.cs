using FSE_API.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSE_API.Errors
{
    public static class LogError
    {

        private static FSEDBEntities FseDB = new FSEDBEntities();

        public static void Log(Exception ex)
        {
            var error = new Error();

            error.Error_Date = DateTime.Now;
            error.Error_Description = ex.StackTrace;
            error.Error_Message = ex.Message;

            FseDB.Errors.Add(error);
        }

    }
}