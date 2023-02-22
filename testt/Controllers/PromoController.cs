using Microsoft.AspNetCore.Mvc;
using testt.Models;
using Npgsql;
using System.Text.Json.Nodes;
using System.Data;

namespace testt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromoController : Controller
    {
        private readonly IConfiguration _configuration;

        public PromoController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        [HttpGet]
        public List<Promo> Index()
        {
            List<Promo> promos = new List<Promo>();
            string constr = _configuration.GetConnectionString("SampleDbConnection");
            string query = "SELECT * FROM promos";
            using (NpgsqlConnection con = new NpgsqlConnection(constr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while(sdr.Read())
                        {
                            promos.Add(new Promo
                            {
                                Id = sdr.GetInt32(sdr.GetOrdinal("id")),
                                PromoCode = sdr.GetString(sdr.GetOrdinal("promo_code")),
                                PromoName = sdr.GetString(sdr.GetOrdinal("promo_name")),
                                PromoDescription = sdr.GetString(sdr.GetOrdinal("promo_description")),
                                PromoStart = sdr.GetDateTime(sdr.GetOrdinal("promo_start")),
                                PromoEnd = sdr.GetDateTime(sdr.GetOrdinal("promo_end")),
                            });
                        }
                    }
                    con.Close();
                }
            }
            if(promos.Count == 0 )
            {
                promos.Add(new Promo());
            }

            return promos;
        }

        [HttpGet("{id}")]
        public Promo findById(int id)
        {
            Promo promo = new Promo();
            string query = "SELECT * FROM promos WHERE id = @PromoId";
            string constr = _configuration.GetConnectionString("SampleDbConnection");
            using (NpgsqlConnection con = new NpgsqlConnection(constr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("PromoId", id);
                    con.Open();
                    using (NpgsqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            promo.Id = sdr.GetInt32(sdr.GetOrdinal("id"));
                            promo.PromoCode = sdr.GetString(sdr.GetOrdinal("promo_code"));
                            promo.PromoName = sdr.GetString(sdr.GetOrdinal("promo_name"));
                            promo.PromoDescription = sdr.GetString(sdr.GetOrdinal("promo_description"));
                            promo.PromoStart = sdr.GetDateTime(sdr.GetOrdinal("promo_start"));
                            promo.PromoEnd = sdr.GetDateTime(sdr.GetOrdinal("promo_end"));
                        }
                    }
                    con.Close();
                }
            }

            return promo;
        }

        [HttpPost("insert_promo")]
        public string insert([FromBody] JsonObject data)
        {
            string query = "INSERT INTO promos (promo_code, promo_name, promo_description, promo_start, promo_end, added_by, updated_by) " +
                "VALUES (@PromoCode, @PromoName, @PromoDescription, @PromoStart, @PromoEnd, @AddedBy, @UpdatedBy)";
            string constr = _configuration.GetConnectionString("SampleDbConnection");
            using (NpgsqlConnection con = new NpgsqlConnection(constr)) 
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("PromoCode", data["promoCode"].ToString());
                    cmd.Parameters.AddWithValue("PromoName", data["promoName"].ToString());
                    cmd.Parameters.AddWithValue("PromoDescription", data["promoDescription"].ToString());
                    cmd.Parameters.AddWithValue("PromoStart", DateTime.ParseExact(data["promoStart"].ToString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("PromoEnd", DateTime.ParseExact(data["promoEnd"].ToString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("AddedBy", data["addedBy"].ToString());
                    cmd.Parameters.AddWithValue("UpdatedBy", data["updatedBy"].ToString());
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return "Success";

        }

        [HttpPut("{id}")]
        public string update([FromBody] JsonObject data, int id)
        {
            string query = "UPDATE promos SET promo_code = @promoCode, promo_name = @PromoName, promo_description = @PromoDescription, promo_start = @PromoStart, promo_end = @PromoEnd, updated_by = @UpdatedBy " +
                "WHERE id = @PromoId";
            string constr = _configuration.GetConnectionString("SampleDbConnection");
            using (NpgsqlConnection con = new NpgsqlConnection(constr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("PromoId", id);
                    cmd.Parameters.AddWithValue("promoCode", data["promoCode"].ToString());
                    cmd.Parameters.AddWithValue("PromoName", data["promoName"].ToString());
                    cmd.Parameters.AddWithValue("PromoDescription", data["promoDescription"].ToString());
                    cmd.Parameters.AddWithValue("PromoStart", DateTime.ParseExact(data["promoStart"].ToString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("PromoEnd", DateTime.ParseExact(data["promoEnd"].ToString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("UpdatedBy", data["updatedBy"].ToString());
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return "Success";

        }

        [HttpDelete("{id}")]
        public string delete(int id)
        {
            string query = "DELETE FROM promos WHERE id = @PromoId";
            string constr = _configuration.GetConnectionString("SampleDbConnection");
            using (NpgsqlConnection con = new NpgsqlConnection(constr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("PromoId", id);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return "Success";

        }


    }

}
