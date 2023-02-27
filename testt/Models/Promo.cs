using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;

namespace testt.Models
{

    public class Promo
    {
        [JsonProperty("prm_id")]
        public string PromoId { get; set; } = "";

        [JsonProperty("prm_code")]
        public string PromoCode { get; set; } = "";

        [JsonProperty("prm_name")]
        public string PromoName { get; set;} = "";

        [JsonProperty("prm_description")]
        public string PromoDescription { get; set;} = "";

        [JsonProperty("prm_start")]
        public DateTime PromoStart { get; set;}

        [JsonProperty("prm_end")]
        public DateTime PromoEnd { get; set;}

        [JsonProperty("prm_percentage")]
        public string PromoPercentage { get; set;}
    }
}
