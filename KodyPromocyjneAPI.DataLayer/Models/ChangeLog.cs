namespace KodyPromocyjneAPI.DataLayer.Models
{
    public class ChangeLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public int CodeId { get; set; }
        public string NewValue { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
