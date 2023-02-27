using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;
using System.Globalization;
using testt.Config;
using testt.Models;

namespace testt.DBProcesses
{
    public class Foo
    {
        public List<Promo> data { get; set; }
    }

    public class PromoRepository
    {
        private readonly string constr = DbSingleton.Configuration.GetConnectionString("SampleDbConnection");

        public PromoRepository(){ }

        public JArray Index()
        {
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

        public bool WriteToCsvFile(out string message)
        {
            var promos = this.Index();

            message = "CSV File Updated!";
            bool returnValue = true;
            try
            {
                var records = JsonConvert.DeserializeObject<List<Promo>>(promos.ToString());
                using (var writer = new StreamWriter("C:\\Users\\micha\\Documents\\test.csv"))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteHeader<Promo>();
                        csv.NextRecord();
                        foreach (var promo in records)
                        {
                            csv.WriteRecord(promo);
                            csv.NextRecord();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
                message = ex.Message;
            }

            return returnValue;
        }

        public JArray ReadFromCsvFile()
        {
            var promos = new JArray();

            using (var reader = new StreamReader("C:\\Users\\micha\\Documents\\test.csv"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Promo>();
                    foreach(var record in records)
                    {
                        var jsonObject = JObject.FromObject(record);
                        promos.Add(jsonObject);
                    }
                }
            }

            return promos;
        }

        public JObject FindById(int id)
        {
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
            string query = "insert_promo";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            /*foreach (JProperty _data in data.Properties())
            {
                if(_data.Type == JTokenType.Date)
                {

                }
                var value = _data.Value.ToString();
                cmd.Parameters.AddWithValue(_data.Name, _data.Value.ToString());
            }*/
            cmd.Parameters.AddWithValue("p_prm_code", data["p_prm_code"].ToString());
            cmd.Parameters.AddWithValue("p_prm_name", data["p_prm_name"].ToString());
            cmd.Parameters.AddWithValue("p_prm_description", data["p_prm_description"].ToString());
            cmd.Parameters.AddWithValue("p_prm_start", DateOnly.ParseExact(data["p_prm_start"].ToString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("p_prm_end", DateOnly.ParseExact(data["p_prm_end"].ToString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("p_prm_percentage", data["p_prm_percentage"].Value<int>());
            cmd.Parameters.AddWithValue("p_added_by", data["p_added_by"].ToString());
            cmd.Parameters.AddWithValue("p_updated_by", data["p_updated_by"].ToString());
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        public bool Update(JObject data, int id)
        {
            string query = "update_promo_by_id";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_prm_id", id);
            foreach (JProperty _data in data.Properties())
            {
                cmd.Parameters.AddWithValue(_data.Name, _data.Value);
            }
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }
        
        public bool Delete(int id, out string message) 
        {
            string query = "delete_promo_by_id";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);

            message = "Data Deleted Successfully";
            bool returnValue = true;
            try
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_prm_id", id);

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
