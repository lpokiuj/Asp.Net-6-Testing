using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;
using testt.Config;

namespace testt.Repository
{
    public class TransactionRepository
    {
        private readonly string constr = DbSingleton.Configuration.GetConnectionString("SampleDbConnection");

        public TransactionRepository() { }

        public JObject FindByTrxNo(string trx_no)
        {
            string query = "get_transaction_by_trx_no";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_trx_no", trx_no);
            con.Open();
            var sdr = cmd.ExecuteReader();
            var transaction = new JObject();
            while (sdr.Read())
            {
                for(int i = 0; i < sdr.FieldCount; i++)
                {
                    transaction.Add(sdr.GetName(i), sdr.IsDBNull(i) ? null : JToken.FromObject(sdr[i]));
                }
            }
            con.Close();

            return transaction;
        }

        public JArray FindByDate(string date1, string date2)
        {
            string query = "get_transaction_by_date";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);


            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_date_1", DateTime.ParseExact(date1, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("p_date_2", DateTime.ParseExact(date2, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
            con.Open();
            var sdr = cmd.ExecuteReader();
            var transactions = new JArray();
            while (sdr.Read())
            {
                var data = new JObject();
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    data.Add(sdr.GetName(i), sdr.IsDBNull(i) ? null : JToken.FromObject(sdr[i]));
                }
                transactions.Add(data);
            }
            con.Close();

            return transactions;
        }

        public bool Insert(JObject data, out string message)
        {
            string query = "insert_transaction";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);

            message = "Data Inserted Successfully";
            bool returnValue = true;
            try
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                /*foreach (JProperty _data in data.Properties())
                {
                    cmd.Parameters.AddWithValue(_data.Name, _data.Value);
                }*/
                cmd.Parameters.AddWithValue("p_trx_no", data["p_trx_no"].ToString());
                cmd.Parameters.AddWithValue("p_trx_product_name", data["p_trx_product_name"].ToString());
                cmd.Parameters.AddWithValue("p_trx_amount", data["p_trx_amount"].Value<int>());
                if (data.ContainsKey("p_trx_promo_code") && !string.IsNullOrWhiteSpace(data["p_trx_promo_code"].ToString()))
                {
                    cmd.Parameters.AddWithValue("p_trx_promo_code", data["p_trx_promo_code"].ToString());
                }
                con.Open();
                returnValue = cmd.ExecuteNonQuery() <= 0 ? false : true;
                con.Close();
            } catch (Exception ex)
            {
                returnValue = false;
                message = ex.Message;
            }

            return returnValue;
        }

        public bool Update(JObject data, out string message)
        {
            string query = "update_transaction_by_trx_no";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);

            message = "Data Updated Successfully";
            bool returnValue = true;
            try
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("p_trx_no", data["p_trx_no"].ToString());
                if (!data["p_trx_promo_code"].IsNullOrEmpty())
                {
                    cmd.Parameters.AddWithValue("p_trx_promo_code", data["p_trx_promo_code"].ToString());
                }
                
                con.Open();
                returnValue = cmd.ExecuteNonQuery() <= 0 ? false : true;
                con.Close();
            } catch(Exception ex)
            {
                returnValue = false;
                message = ex.Message;
            }

            return returnValue;
        }
    }
}
