using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using testt.DBProcesses;

namespace testt.Managers
{
    public class PromoManager
    {
        private readonly PromoRepository _promoRepository;

        public PromoManager() {
            _promoRepository = new PromoRepository();
        }

        public JObject Index()
        {
            var returnMsg = new JObject();

            var promos = this._promoRepository.Index();

            returnMsg["status"] = 200;
            returnMsg["message"] = "Data retrieved successfully";
            returnMsg["data"] = promos;
            

            return returnMsg;
        }

        public JObject WriteToCsvFile()
        {
            var returnMsg = new JObject();
            bool write = this._promoRepository.WriteToCsvFile(out string message);

            if (write)
            {
                returnMsg["status"] = 200;
                returnMsg["message"] = message;
                returnMsg["data"] = null;
            }
            else
            {
                returnMsg["status"] = 400;
                returnMsg["message"] = message;
                returnMsg["data"] = null;
            }

            return returnMsg;
        }

        public JObject ReadFromCsvFile(string filePath)
        {
            var returnMsg = new JObject();
            bool read = this._promoRepository.ReadFromCsvFile(filePath, out string message);

            if (read)
            {
                returnMsg["status"] = 200;
                returnMsg["message"] = message;
                returnMsg["data"] = null;
            }
            else
            {
                returnMsg["status"] = 400;
                returnMsg["message"] = message;
                returnMsg["data"] = null;
            }

            return returnMsg;
        }

        public JObject FindById(int id)
        {
            var returnMsg = new JObject();

            var promo = this._promoRepository.FindById(id);

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

        public JObject Insert(string p_prm_code, string p_prm_name, string p_prm_description,
            string p_prm_start, string p_prm_end, int p_prm_percentage, string p_added_by, string p_updated_by)
        {
            var returnMsg = new JObject();

            var data = new JObject();
            data["p_prm_code"] = p_prm_code;
            data["p_prm_name"] = p_prm_name;
            data["p_prm_description"] = p_prm_description;
            data["p_prm_start"] = p_prm_start;
            data["p_prm_end"] = p_prm_end;
            data["p_prm_percentage"] = p_prm_percentage;
            data["p_added_by"] = p_added_by;
            data["p_updated_by"] = p_updated_by;
            bool insert = this._promoRepository.Insert(data);

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

        public JObject Update(string p_prm_code, string p_prm_name, string p_prm_description, int id)
        {
            var returnMsg = new JObject();

            var data = new JObject();
            data["p_prm_code"] = p_prm_code;
            data["p_prm_name"] = p_prm_name;
            data["p_prm_description"] = p_prm_description;
            bool insert = this._promoRepository.Update(data, id);

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

            bool insert = this._promoRepository.Delete(id, out string message);

            if (insert)
            {
                returnMsg["status"] = 200;
                returnMsg["message"] = message;
                returnMsg["data"] = null;
            }
            else
            {
                returnMsg["status"] = 404;
                returnMsg["message"] = message;
                returnMsg["data"] = null;
            }

            return returnMsg;
        }
    }
}
