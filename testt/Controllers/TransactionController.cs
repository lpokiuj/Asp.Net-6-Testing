using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Transactions;

namespace testt.Controllers
{
    [ApiController]
    [Route("transaction")]
    public class TransactionController : Controller
    {
        private readonly Managers.TransactionManager _transactionManager;

        public TransactionController()
        {
            this._transactionManager = new Managers.TransactionManager();
        }

        [HttpGet("find_by_trx_no")]
        public JsonResult FindByTrxNo(string trx_no) 
        {
            var returnMsg = this._transactionManager.FindByTrxNo(trx_no);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }

        [HttpGet("find_by_date")]
        public JsonResult FindByDate(string date1, string date2) 
        { 
            var returnMsg = this._transactionManager.FindByDate(date1, date2);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }

        [HttpPost]
        public JsonResult Insert([FromBody] JObject data)
        {
            var returnMsg = this._transactionManager.Insert(data);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }

        [HttpPut]
        public JsonResult Update([FromBody] JObject data)
        {
            var returnMsg = this._transactionManager.Update(data);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }
    }
}
