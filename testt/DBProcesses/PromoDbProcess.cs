using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;
using testt.Models;

namespace testt.DBProcesses
{
    public class PromoDbProcess
    {

        private readonly IConfiguration _configuration;

        public PromoDbProcess(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public JArray Index()
        {
            string constr = _configuration.GetConnectionString("SampleDbConnection");
            string query = "get_all_promos";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            var sdr = cmd.ExecuteReader();
            var promos = new JArray();
            while (sdr.Read())
            {
                var data = new JObject();
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    data.Add(sdr.GetName(i), sdr.IsDBNull(i) ? null : JToken.FromObject(sdr[i]));
                }
                promos.Add(data);
            }
            con.Close();

            return promos;
        }

        public JObject FindById(int id)
        {
            string constr = _configuration.GetConnectionString("SampleDbConnection");
            string query = "get_promo_by_id";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_prm_id", id);
            con.Open();
            var sdr = cmd.ExecuteReader();
            var promo = new JObject();
            while (sdr.Read())
            {
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    promo.Add(sdr.GetName(i), sdr.IsDBNull(i) ? null : JToken.FromObject(sdr[i]));
                }
            }
            con.Close();

            return promo;
        }

        public bool Insert(JObject data)
        {
            string constr = _configuration.GetConnectionString("SampleDbConnection");
            string query = "insert_promo";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_prm_code", data["promoCode"].ToString());
            cmd.Parameters.AddWithValue("p_prm_name", data["promoName"].ToString());
            cmd.Parameters.AddWithValue("p_prm_description", data["promoDescription"].ToString());
            cmd.Parameters.AddWithValue("p_prm_start", DateOnly.ParseExact(data["promoStart"].ToString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("p_prm_end", DateOnly.ParseExact(data["promoEnd"].ToString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("p_added_by", data["addedBy"].ToString());
            cmd.Parameters.AddWithValue("p_updated_by", data["updatedBy"].ToString());
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        public bool Update(JObject data, int id)
        {
            string constr = _configuration.GetConnectionString("SampleDbConnection");
            string query = "update_promo_by_id";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_prm_id", id);
            cmd.Parameters.AddWithValue("p_prm_code", data["promoCode"].ToString());
            cmd.Parameters.AddWithValue("p_prm_name", data["promoName"].ToString());
            cmd.Parameters.AddWithValue("p_prm_description", data["promoDescription"].ToString());
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }
        
        public bool Delete(int id) 
        {
            string constr = _configuration.GetConnectionString("SampleDbConnection");
            string query = "delete_promo_by_id";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_prm_id", id);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }
    }
}
