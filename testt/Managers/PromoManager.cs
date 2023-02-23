using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using testt.DBProcesses;
using testt.Models;

namespace testt.Managers
{
    public class PromoManager
    {
        private readonly PromoDbProcess _promoDbProcess;
        public PromoManager(IConfiguration configuration) { 
            _promoDbProcess = new PromoDbProcess(configuration);
        }
        public JObject Index()
        {
            var returnMsg = new JObject();

            var promos = new JArray();
            promos = this._promoDbProcess.Index();

            if (promos == null)
            {
                returnMsg["status"] = 200;
                returnMsg["message"] = "No Data Available";
                returnMsg["data"] = null;
            }
            else
            {
                returnMsg["status"] = 200;
                returnMsg["message"] = "Data retrieved successfully";
                returnMsg["data"] = promos;
            }

            return returnMsg;
        }

        public JObject FindById(int id)
        {
            var returnMsg = new JObject();

            var promo = this._promoDbProcess.FindById(id);

            if(promo == null)
            {
                returnMsg["status"] = 200;
                returnMsg["message"] = "Data not found";
                returnMsg["data"] = promo;
            }
            else
            {
                returnMsg["status"] = 200;
                returnMsg["message"] = "Data retrieved successfully";
                returnMsg["data"] = promo;
            }

            return returnMsg;
        }

        public JObject Insert(JObject data)
        {
            var returnMsg = new JObject();

            bool insert = this._promoDbProcess.Insert(data);

            if (insert)
            {
                returnMsg["status"] = 201;
                returnMsg["message"] = "Data Inserted";
                returnMsg["data"] = null;
            }
            else
            {
                returnMsg["status"] = 400;
                returnMsg["message"] = "Bad Request";
                returnMsg["data"] = null;
            }

            return returnMsg;
        }

        public JObject Update(JObject data, int id)
        {
            var returnMsg = new JObject();

            bool insert = this._promoDbProcess.Update(data, id);

            if (insert)
            {
                returnMsg["status"] = 200;
                returnMsg["message"] = "Data Updated";
                returnMsg["data"] = null;
            }
            else
            {
                returnMsg["status"] = 400;
                returnMsg["message"] = "Bad Request";
                returnMsg["data"] = null;
            }

            return returnMsg;
        }

        public JObject Delete(int id)
        {
            var returnMsg = new JObject();

            bool insert = this._promoDbProcess.Delete(id);

            if (insert)
            {
                returnMsg["status"] = 200;
                returnMsg["message"] = "Data Deleted";
                returnMsg["data"] = null;
            }
            else
            {
                returnMsg["status"] = 404;
                returnMsg["message"] = "Data not found";
                returnMsg["data"] = null;
            }

            return returnMsg;
        }
    }
}
