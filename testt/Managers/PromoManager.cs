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
            returnMsg["requestTime"] = requestTime;
            returnMsg["responseTime"] = responseTime;

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
            returnMsg["requestTime"] = requestTime;
            returnMsg["responseTime"] = responseTime;

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
            returnMsg["requestTime"] = requestTime;
            returnMsg["responseTime"] = responseTime;

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
            returnMsg["requestTime"] = requestTime;
            returnMsg["responseTime"] = responseTime;

            return returnMsg;
        }

        public JObject Insert(JObject data)
        {
            var returnMsg = new JObject();

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
            returnMsg["requestTime"] = requestTime;
            returnMsg["responseTime"] = responseTime;
            
            return returnMsg;
        }

        public JObject Update(JObject data, int id)
        {
            var returnMsg = new JObject();

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
            returnMsg["requestTime"] = requestTime;
            returnMsg["responseTime"] = responseTime;

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
            returnMsg["requestTime"] = requestTime;
            returnMsg["responseTime"] = responseTime;

            return returnMsg;
        }
    }
}
