using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;
using System.Globalization;
using testt.Config;
using testt.Models;

namespace testt.DBProcesses
{
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
            string filePath = "C:\\Users\\micha\\Documents\\Test\\testt.csv";

            message = "CSV File Updated!";
            bool returnValue = true;
            try
            {
                var records = JsonConvert.DeserializeObject<List<Promo>>(promos.ToString());
                if (!File.Exists(filePath)){
                    FileStream fs = File.Create(filePath);
                    fs.Close();
                }
                using (var writer = new StreamWriter(filePath))
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

        public bool ReadFromCsvFile(string filePath, out string message)
        {
            var promos = new JArray();

            using (var reader = new StreamReader(filePath))
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

            string path = "C:\\Users\\micha\\Documents\\" + DateTime.Now.ToString("yyyy") + "\\" + DateTime.Now.ToString("yyyyMM");

            try
            {
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
            } catch(Exception ex)
            {
                message = ex.Message;
                return false;
            }

            path += "\\log.txt";
            FileStream fs = File.Create(path);
            fs.Close();
            using (StreamWriter writer = File.CreateText(path))
            {
                foreach(var promo in promos)
                {
                    writer.WriteLine(promo.ToString());
                }
            }

            message = "txt file successfully created/updated";

            return true;
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
            /*foreach (JProperty _data in data.Properties())
            {
                cmd.Parameters.AddWithValue(_data.Name, _data.Value);
            }*/
            cmd.Parameters.AddWithValue("p_prm_code", data["p_prm_code"].ToString());
            cmd.Parameters.AddWithValue("p_prm_name", data["p_prm_name"].ToString());
            cmd.Parameters.AddWithValue("p_prm_description", data["p_prm_description"].ToString());
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
