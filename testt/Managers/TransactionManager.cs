using Newtonsoft.Json.Linq;
using testt.Repository;

namespace testt.Managers
{
    public class TransactionManager
    {
        private readonly TransactionRepository _transactionRepository;
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
            returnMsg["requestTime"] = requestTime;
            returnMsg["responseTime"] = responseTime;

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
            returnMsg["requestTime"] = requestTime;
            returnMsg["responseTime"] = responseTime;

            return returnMsg;
        }

        public JObject Insert(JObject data)
        {
            var returnMsg = new JObject();

            DateTime requestTime = DateTime.Now;
            bool insert = this._transactionRepository.Insert(data, out string message);
            DateTime responseTime = DateTime.Now;

            returnMsg["status"] = 200;
            returnMsg["message"] = message;
            returnMsg["data"] = null;

            // Logging
            returnMsg["requestTime"] = requestTime;
            returnMsg["responseTime"] = responseTime;

            return returnMsg;
        }

        public JObject Update(JObject data)
        {
            var returnMsg = new JObject();
            DateTime requestTime = DateTime.Now;
            DateTime responseTime = DateTime.Now;
            try
            {
                requestTime = DateTime.Now;
                bool update = this._transactionRepository.Update(data, out string message);
                responseTime = DateTime.Now;

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
            } catch(Exception ex)
            {
                returnMsg["status"] = 500;
                returnMsg["message"] = "Transaction Exception";
                returnMsg["data"] = null;
                returnMsg["error"] = ex.Message;
            }

            // Logging
            returnMsg["requestTime"] = requestTime;
            returnMsg["responseTime"] = responseTime;

            return returnMsg;
        }

    }
}
