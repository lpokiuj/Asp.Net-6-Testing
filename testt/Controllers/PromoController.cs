using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using testt.Managers;
using Microsoft.AspNetCore.Authorization;
using testt.Repository;
using testt.Common;

namespace testt.Controllers
{
    [ApiController]
    [Route("promo")]
    public class PromoController : Controller
    {
        private readonly PromoManager _promoManager;
        private readonly LoggingRepository _loggingRepository;
        private readonly CommonFunction _commonFunction;

        public PromoController()
        {
            this._promoManager = new PromoManager();
            this._loggingRepository = new LoggingRepository();
            this._commonFunction = CommonFunction.GetInstance();
        }

        [Authorize]
        [HttpGet("getAllPromos")]
        public JsonResult Index()
        {
            var returnMsg = this._promoManager.Index();
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            // Logging
            this._loggingRepository.Insert(Request.Path, null, returnMsg, returnMsg["status"].Value<int>(), returnMsg["requestTime"].Value<DateTime>(), returnMsg["responseTime"].Value<DateTime>());

            return jsonResult;
        }

        [Authorize]
        [HttpGet("write_to_csv")]
        public JsonResult WriteToCsvFile()
        {
            var returnMsg = this._promoManager.WriteToCsvFile();
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            // Logging
            this._loggingRepository.Insert(Request.Path, null, returnMsg, returnMsg["status"].Value<int>(), returnMsg["requestTime"].Value<DateTime>(), returnMsg["responseTime"].Value<DateTime>());

            return jsonResult;
        }

        [Authorize]
        [HttpGet("read_from_csv")]
        public JsonResult ReadFromCsvFile([FromForm] string filePath)
        {
            var returnMsg = this._promoManager.ReadFromCsvFile(filePath);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);



            // Logging
            var request = new JObject();
            request["filePath"] = filePath;
            this._loggingRepository.Insert(Request.Path, request, returnMsg, returnMsg["status"].Value<int>(), returnMsg["requestTime"].Value<DateTime>(), returnMsg["responseTime"].Value<DateTime>());

            return jsonResult;
        }

        [Authorize]
        [HttpGet("findById/{id}")]
        public JsonResult FindById(int id)
        {
            var returnMsg = this._promoManager.FindById(id);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            // Logging
            var request = new JObject();
            request["id"] = id;
            this._loggingRepository.Insert(Request.Path, request, returnMsg, returnMsg["status"].Value<int>(), returnMsg["requestTime"].Value<DateTime>(), returnMsg["responseTime"].Value<DateTime>());

            return jsonResult;
        }

        [Authorize]
        [HttpPost("insertPromo")]
        public JsonResult Insert()
        {
            var requestJObject = this._commonFunction.RequestToJObject(Request.Form);
            var returnMsg = this._promoManager.Insert(requestJObject);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            // Logging
            this._loggingRepository.Insert(Request.Path, requestJObject, returnMsg, returnMsg["status"].Value<int>(), returnMsg["requestTime"].Value<DateTime>(), returnMsg["responseTime"].Value<DateTime>());

            return jsonResult;
        }

        [Authorize]
        [HttpPut("updatePromo/{id}")]
        public JsonResult Update(int id)
        {
            var requestJObject = this._commonFunction.RequestToJObject(Request.Form);
            var returnMsg = this._promoManager.Update(requestJObject, id);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            // Logging
            requestJObject["id"] = id;
            this._loggingRepository.Insert(Request.Path, requestJObject, returnMsg, returnMsg["status"].Value<int>(), returnMsg["requestTime"].Value<DateTime>(), returnMsg["responseTime"].Value<DateTime>());

            return jsonResult;
        }

        [Authorize]
        [HttpDelete("deletePromo/{id}")]
        public JsonResult Delete(int id)
        {
            var returnMsg = this._promoManager.Delete(id);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            // Logging
            var request = new JObject();
            request["id"] = id;
            this._loggingRepository.Insert(Request.Path, request, returnMsg, returnMsg["status"].Value<int>(), returnMsg["requestTime"].Value<DateTime>(), returnMsg["responseTime"].Value<DateTime>());

            return jsonResult;
        }


    }

}

/*
 
{
    "status_code": 200,
    "message": "Success",
    "data": {
        ...data
    }
}

newtonsoft json
 
 */