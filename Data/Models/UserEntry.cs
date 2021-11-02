namespace Data.Models
{
    public class UserEntry
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int EntryId { get; set; }
        public Entry Entry { get; set; }
    }
}
