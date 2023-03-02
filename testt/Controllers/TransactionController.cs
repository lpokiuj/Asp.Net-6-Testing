using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using testt.Common;
using testt.Repository;

namespace testt.Controllers
{
    [ApiController]
    [Route("transaction")]
    public class TransactionController : Controller
    {
        private readonly Managers.TransactionManager _transactionManager;
        private readonly LoggingRepository _loggingRepository;
        private readonly CommonFunction _commonFunction;

        public TransactionController()
        {
            this._transactionManager = new Managers.TransactionManager();
            this._loggingRepository = new LoggingRepository();
            this._commonFunction = CommonFunction.GetInstance();
        }

        [Authorize]
        [HttpGet("find_by_trx_no")]
        public JsonResult FindByTrxNo(string trx_no) 
        {
            var returnMsg = this._transactionManager.FindByTrxNo(trx_no);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            // Logging
            var request = new JObject();
            request["trxNo"] = trx_no;
            this._loggingRepository.Insert(Request.Path, request, returnMsg, returnMsg["status"].Value<int>(), returnMsg["requestTime"].Value<DateTime>(), returnMsg["responseTime"].Value<DateTime>());

            return jsonResult;
        }

        [Authorize]
        [HttpGet("find_by_date")]
        public JsonResult FindByDate(string date1, string date2) 
        { 
            var returnMsg = this._transactionManager.FindByDate(date1, date2);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            // Logging
            var request = new JObject();
            request["date1"] = date1;
            request["date2"] = date2;
            this._loggingRepository.Insert(Request.Path, request, returnMsg, returnMsg["status"].Value<int>(), returnMsg["requestTime"].Value<DateTime>(), returnMsg["responseTime"].Value<DateTime>());

            return jsonResult;
        }

        [Authorize]
        [HttpPost("insertTransaction")]
        public JsonResult Insert()
        {
            var requestJObject = this._commonFunction.RequestToJObject(Request.Form);
            var returnMsg = this._transactionManager.Insert(requestJObject);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            // Logging
            this._loggingRepository.Insert(Request.Path, requestJObject, returnMsg, returnMsg["status"].Value<int>(), returnMsg["requestTime"].Value<DateTime>(), returnMsg["responseTime"].Value<DateTime>());

            return jsonResult;
        }

        [Authorize]
        [HttpPut("updateTransaction")]
        public JsonResult Update()
        {
            var requestJObject = this._commonFunction.RequestToJObject(Request.Form);
            var returnMsg = this._transactionManager.Update(requestJObject);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            // Logging
            this._loggingRepository.Insert(Request.Path, requestJObject, returnMsg, returnMsg["status"].Value<int>(), returnMsg["requestTime"].Value<DateTime>(), returnMsg["responseTime"].Value<DateTime>());

            return jsonResult;
        }
    }
}
