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
        }

        public JObject FindByTrxNo(string trx_no)
        {
            var returnMsg = new JObject();

            var transaction = this._transactionRepository.FindByTrxNo(trx_no);

            returnMsg["status"] = 200;
            returnMsg["message"] = "Data retrieved successfully";
            returnMsg["data"] = transaction;

            return returnMsg;
        }

        public JObject FindByDate(string date1, string date2)
        {
            var returnMsg = new JObject();

            var transactions = this._transactionRepository.FindByDate(date1, date2);

            returnMsg["status"] = 200;
            returnMsg["message"] = "Data retrieved successfully";
            returnMsg["data"] = transactions;

            return returnMsg;
        }

        public JObject Insert(JObject data)
        {
            var returnMsg = new JObject();
            bool insert = this._transactionRepository.Insert(data, out string message);
            
            returnMsg["status"] = 200;
            returnMsg["message"] = message;
            returnMsg["data"] = null;

            return returnMsg;
        }

        public JObject Update(JObject data)
        {
            var returnMsg = new JObject();
            bool update = this._transactionRepository.Update(data, out string message);

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

            return returnMsg;
        }

    }
}
