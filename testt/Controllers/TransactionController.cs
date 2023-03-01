using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("find_by_trx_no")]
        public JsonResult FindByTrxNo(string trx_no) 
        {
            var returnMsg = this._transactionManager.FindByTrxNo(trx_no);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }

        [Authorize]
        [HttpGet("find_by_date")]
        public JsonResult FindByDate(string date1, string date2) 
        { 
            var returnMsg = this._transactionManager.FindByDate(date1, date2);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }

        [Authorize]
        [HttpPost]
        public JsonResult Insert([FromForm] string p_trx_no, [FromForm] string p_trx_product_name,
            [FromForm] string p_trx_promo_code, [FromForm] int p_trx_amount)
        {
            var returnMsg = this._transactionManager.Insert(p_trx_no, p_trx_product_name, p_trx_promo_code, p_trx_amount);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }

        [Authorize]
        [HttpPut]
        public JsonResult Update([FromForm] string p_trx_no, [FromForm] string p_trx_promo_code)
        {
            var returnMsg = this._transactionManager.Update(p_trx_no, p_trx_promo_code);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }
    }
}
