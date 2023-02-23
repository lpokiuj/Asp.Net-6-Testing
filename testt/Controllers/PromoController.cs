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
        private readonly IConfiguration _configuration;
        private readonly PromoManager _promoManager;

        public PromoController(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._promoManager = new PromoManager(this._configuration); ;
        }
        [HttpGet]
        public JObject Index()
        {
            return this._promoManager.Index();
        }

        [HttpGet("{id}")]
        public JObject FindById(int id)
        {
            return this._promoManager.FindById(id);
        }

        [HttpPost]
        public JObject Insert([FromBody] JObject data)
        {
            return this._promoManager.Insert(data);
        }

        [HttpPut("{id}")]
        public JObject Update([FromBody] JObject data, int id)
        {
            return this._promoManager.Update(data, id);

        }

        [HttpDelete("{id}")]
        public JObject Delete(int id)
        {
            return this._promoManager.Delete(id);
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