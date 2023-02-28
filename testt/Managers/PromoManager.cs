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

        public JObject ReadFromCsvFile()
        {
            var returnMsg = new JObject();
            bool read = this._promoRepository.ReadFromCsvFile(out string message);

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

        public JObject Insert(JObject data)
        {
            var returnMsg = new JObject();

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

        public JObject Update(JObject data, int id)
        {
            var returnMsg = new JObject();

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
