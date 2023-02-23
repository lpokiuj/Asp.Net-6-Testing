using Microsoft.AspNetCore.Mvc;
using testt.Models;
using Npgsql;
using System.Text.Json.Nodes;
using System.Data;
using Newtonsoft.Json.Linq;

namespace testt.Controllers
{
    [ApiController]
    [Route("promo")]
    public class PromoController : Controller
    {
        private readonly IConfiguration _configuration;

        public PromoController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        [HttpGet]
        public JObject Index()
        {
            var returnMsg = new JObject();

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

            returnMsg["statusCode"] = 200;
            returnMsg["message"] = "Data retrieved successfully";
            returnMsg["data"] = promos;
            return returnMsg;
        }

        [HttpGet("{id}")]
        public JObject FindById(int id)
        {
            var returnMsg = new JObject();

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

            returnMsg["statusCode"] = 200;
            returnMsg["message"] = "Data retrieved successfully";
            returnMsg["data"] = promo;
            return returnMsg;
        }

        [HttpPost]
        public JObject Insert([FromBody] JObject data)
        {
            var returnMsg = new JObject();

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

            returnMsg["statusCode"] = 201;
            returnMsg["message"] = "Data updated successfully";
            return returnMsg;

        }

        [HttpPut("{id}")]
        public JObject Update([FromBody] JObject data, int id)
        {
            var returnMsg = new JObject();

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

            returnMsg["statusCode"] = 200;
            returnMsg["message"] = "Data updated successfully";
            return returnMsg;

        }

        [HttpDelete("{id}")]
        public JObject Delete(int id)
        {
            var returnMsg = new JObject();

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

            returnMsg["statusCode"] = 200;
            returnMsg["message"] = "Data deleted successfully";
            return returnMsg;

        }


    }

}

/*
 
{
    "status_code": 200,
    "message": "Success",
    "data": {
        ...data
    }
}

newtonsoft json
 
 */