using Microsoft.AspNetCore.Mvc;
using testt.Models;
using Npgsql;
using System.Text.Json.Nodes;
using System.Data;
using Newtonsoft.Json.Linq;
using testt.Managers;

namespace testt.Controllers
{
    [ApiController]
    [Route("promo")]
    public class PromoController : Controller
    {
        private readonly PromoManager _promoManager;

        public PromoController()
        {
            this._promoManager = new PromoManager();
        }
        [HttpGet]
        public JsonResult Index()
        {
            var returnMsg = this._promoManager.Index();
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }


        [HttpGet("write_to_csv")]
        public JsonResult WriteToCsvFile()
        {
            var returnMsg = this._promoManager.WriteToCsvFile();
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }

        [HttpGet("read_from_csv")]
        public JsonResult ReadFromCsvFile()
        {
            var returnMsg = this._promoManager.ReadFromCsvFile();
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }

        [HttpGet("{id}")]
        public JsonResult FindById(int id)
        {
            var returnMsg = this._promoManager.FindById(id);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }

        [HttpPost]
        public JsonResult Insert([FromBody] JObject data)
        {
            var returnMsg = this._promoManager.Insert(data);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }

        [HttpPut("{id}")]
        public JsonResult Update([FromBody] JObject data, int id)
        {
            var returnMsg = this._promoManager.Update(data, id);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;

        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var returnMsg = this._promoManager.Delete(id);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

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