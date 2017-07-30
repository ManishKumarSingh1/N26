using N26Restful.Helper;
using N26Restful.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using N26Restful.BusinessLogic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace N26Restful.Controllers
{
    public class N26Controller : ApiController
    {        
        [System.Web.Http.HttpPost]
        public HttpResponseMessage transactions([FromBody] TransactionDetails TransactionData)
        {
            if(DateTime.UtcNow.Subtract(UtcHelper.FromUnixTime(TransactionData.TimeStamp)).TotalSeconds > 60)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent); 
            }
            var status = Api.addToTransactionalData(TransactionData.TimeStamp, TransactionData.Amount);
            return status ? new HttpResponseMessage(HttpStatusCode.Created) : new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [System.Web.Http.HttpGet]
        public Statistics statistics()
        {
            return Api.getStats();
            //System.Web.Mvc.JsonResult res = new JsonResult();
            //return Json(JsonConvert.SerializeObject(stats), JsonRequestBehavior.AllowGet);
        }
    }
}
