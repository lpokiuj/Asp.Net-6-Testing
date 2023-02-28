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
        public JsonResult ReadFromCsvFile([FromForm] string filePath)
        {
            var returnMsg = this._promoManager.ReadFromCsvFile(filePath);
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
        public JsonResult Insert([FromForm] string p_prm_code, [FromForm] string p_prm_name, [FromForm] string p_prm_description,
            [FromForm] string p_prm_start, [FromForm] string p_prm_end, [FromForm] int p_prm_percentage, [FromForm] string p_added_by, [FromForm] string p_updated_by)
        {
            var returnMsg = this._promoManager.Insert(p_prm_code, p_prm_name, p_prm_description, p_prm_start, p_prm_end, p_prm_percentage, p_added_by, p_updated_by);
            var jsonResult = new JsonResult(returnMsg);
            jsonResult.StatusCode = Convert.ToInt32(returnMsg["status"]);

            return jsonResult;
        }

        [HttpPut("{id}")]
        public JsonResult Update([FromForm] string p_prm_code, [FromForm] string p_prm_name, [FromForm] string p_prm_description, int id)
        {
            var returnMsg = this._promoManager.Update(p_prm_code, p_prm_name, p_prm_description, id);
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