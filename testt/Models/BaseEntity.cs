namespace testt.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public string AddedBy { get; set; } = "";
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = "";
    }
}
