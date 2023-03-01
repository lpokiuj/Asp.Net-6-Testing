using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;
using testt.Config;

namespace testt.Repository
{
    public class LoggingRepository
    {
        private readonly string constr = DbSingleton.Configuration.GetConnectionString("SampleDbConnection");
        
        public LoggingRepository() { }

        public void Insert(string endpoint, JObject request, JObject response, int status_code, DateTime request_time, DateTime response_time)
        {
            string query = "insert_log_apis";
            var con = new NpgsqlConnection(constr);
            var cmd = new NpgsqlCommand(query, con);

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_log_endpoint_name", endpoint);
            cmd.Parameters.AddWithValue("p_log_response_api", response.ToString());
            cmd.Parameters.AddWithValue("p_log_status_code", status_code);
            cmd.Parameters.AddWithValue("p_log_request_time", request_time);
            cmd.Parameters.AddWithValue("p_log_response_time", response_time);
            if(request != null)
            {
                cmd.Parameters.AddWithValue("p_log_request_api", request.ToString());
            }

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

    }
}
