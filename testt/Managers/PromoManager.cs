using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using testt.DBProcesses;
using testt.Repository;

namespace testt.Managers
{
    public class PromoManager
    {
        private readonly PromoRepository _promoRepository;
        private readonly LoggingRepository _loggingRepository;

        public PromoManager() {
            _promoRepository = new PromoRepository();
            _loggingRepository = new LoggingRepository();
        }

        public JObject Index()
        {
            var returnMsg = new JObject();

            DateTime requestTime = DateTime.Now;
            var promos = this._promoRepository.Index();
            DateTime responseTime = DateTime.Now;

            returnMsg["status"] = 200;
            returnMsg["message"] = "Data retrieved successfully";
            returnMsg["data"] = promos;

            // Logging
            this._loggingRepository.Insert("get_all_promo", null, returnMsg, returnMsg["status"].Value<int>(), requestTime, responseTime);

            return returnMsg;
        }

        public JObject WriteToCsvFile()
        {
            var returnMsg = new JObject();

            DateTime requestTime = DateTime.Now;
            bool write = this._promoRepository.WriteToCsvFile(out string message);
            DateTime responseTime = DateTime.Now;

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

            // Logging
            this._loggingRepository.Insert("write_promo_to_csv_file", null, returnMsg, returnMsg["status"].Value<int>(), requestTime, responseTime);

            return returnMsg;
        }

        public JObject ReadFromCsvFile(string filePath)
        {
            var returnMsg = new JObject();

            DateTime requestTime = DateTime.Now;
            bool read = this._promoRepository.ReadFromCsvFile(filePath, out string message);
            DateTime responseTime = DateTime.Now;

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

            // Logging
            this._loggingRepository.Insert("read_promo_from_csv_file", new JObject{ "filepath", filePath }, returnMsg, returnMsg["status"].Value<int>(), requestTime, responseTime);

            return returnMsg;
        }

        public JObject FindById(int id)
        {
            var returnMsg = new JObject();

            DateTime requestTime = DateTime.Now;
            var promo = this._promoRepository.FindById(id);
            DateTime responseTime = DateTime.Now;

            if (promo == null)
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

            // Logging
            this._loggingRepository.Insert("find_promo_by_id", null, returnMsg, returnMsg["status"].Value<int>(), requestTime, responseTime);

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

            DateTime requestTime = DateTime.Now;
            bool insert = this._promoRepository.Insert(data);
            DateTime responseTime = DateTime.Now;

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

            // Logging
            this._loggingRepository.Insert("insert_promo", data, returnMsg, returnMsg["status"].Value<int>(), requestTime, responseTime);

            return returnMsg;
        }

        public JObject Update(string p_prm_code, string p_prm_name, string p_prm_description, int id)
        {
            var returnMsg = new JObject();

            var data = new JObject();
            data["p_prm_code"] = p_prm_code;
            data["p_prm_name"] = p_prm_name;
            data["p_prm_description"] = p_prm_description;

            DateTime requestTime = DateTime.Now;
            bool insert = this._promoRepository.Update(data, id);
            DateTime responseTime = DateTime.Now;

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

            // Logging
            data["id"] = id;
            this._loggingRepository.Insert("update_promo", data, returnMsg, returnMsg["status"].Value<int>(), requestTime, responseTime);

            return returnMsg;
        }

        public JObject Delete(int id)
        {
            var returnMsg = new JObject();

            DateTime requestTime = DateTime.Now;
            bool insert = this._promoRepository.Delete(id, out string message);
            DateTime responseTime = DateTime.Now;

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

            // Logging
            this._loggingRepository.Insert("delete_promo", new JObject { "id", id }, returnMsg, returnMsg["status"].Value<int>(), requestTime, responseTime);

            return returnMsg;
        }
    }
}
