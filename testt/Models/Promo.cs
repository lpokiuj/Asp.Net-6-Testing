namespace testt.Models
{
    public class Promo : BaseEntity
    {
        public string PromoCode { get; set; } = "";
        public string PromoName { get; set;} = "";
        public string PromoDescription { get; set;} = "";
        public DateTime PromoStart { get; set;}
        public DateTime PromoEnd { get; set;}
    }
}
