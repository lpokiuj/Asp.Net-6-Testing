using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using testt.Repository;

namespace testt.Managers
{
    public class TransactionManager
    {
        private readonly TransactionRepository _transactionRepository;
        private readonly LoggingRepository _loggingRepository;
        public TransactionManager()
        {
            _transactionRepository = new TransactionRepository();
            _loggingRepository = new LoggingRepository();
        }

        public JObject FindByTrxNo(string trx_no)
        {
            var returnMsg = new JObject();

            DateTime requestTime = DateTime.Now;
            var transaction = this._transactionRepository.FindByTrxNo(trx_no);
            DateTime responseTime = DateTime.Now;

            returnMsg["status"] = 200;
            returnMsg["message"] = "Data retrieved successfully";
            returnMsg["data"] = transaction;

            // Logging
            this._loggingRepository.Insert("get_transaction_by_trx_no", new JObject { "trx_no", trx_no }, returnMsg, returnMsg["status"].Value<int>(), requestTime, responseTime);

            return returnMsg;
        }

        public JObject FindByDate(string date1, string date2)
        {
            var returnMsg = new JObject();

            DateTime requestTime = DateTime.Now;
            var transactions = this._transactionRepository.FindByDate(date1, date2);
            DateTime responseTime = DateTime.Now;

            returnMsg["status"] = 200;
            returnMsg["message"] = "Data retrieved successfully";
            returnMsg["data"] = transactions;

            // Logging
            this._loggingRepository.Insert("get_transaction_by_date", new JObject { { "date1", date1 }, { "date2", date2 } }, returnMsg, returnMsg["status"].Value<int>(), requestTime, responseTime);

            return returnMsg;
        }

        public JObject Insert(string p_trx_no, string p_trx_product_name,
            string p_trx_promo_code, int p_trx_amount)
        {
            var returnMsg = new JObject();

            var data = new JObject();
            data["p_trx_no"] = p_trx_no;
            data["p_trx_product_name"] = p_trx_product_name;
            data["p_trx_promo_code"] = p_trx_promo_code;
            data["p_trx_amount"] = p_trx_amount;

            DateTime requestTime = DateTime.Now;
            bool insert = this._transactionRepository.Insert(data, out string message);
            DateTime responseTime = DateTime.Now;

            returnMsg["status"] = 200;
            returnMsg["message"] = message;
            returnMsg["data"] = null;

            // Logging
            this._loggingRepository.Insert("insert_transaction", data, returnMsg, returnMsg["status"].Value<int>(), requestTime, responseTime);

            return returnMsg;
        }

        public JObject Update(string p_trx_no, string p_trx_promo_code)
        {
            var returnMsg = new JObject();

            var data = new JObject();
            data["p_trx_no"] = p_trx_no;
            data["p_trx_promo_code"] = p_trx_promo_code;

            DateTime requestTime = DateTime.Now;
            bool update = this._transactionRepository.Update(data, out string message);
            DateTime responseTime = DateTime.Now;

            if (update)
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
            this._loggingRepository.Insert("update_transaction", data, returnMsg, returnMsg["status"].Value<int>(), requestTime, responseTime);

            return returnMsg;
        }

    }
}
