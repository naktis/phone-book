using System.Collections.Generic;
namespace Data.Models
{
    public class Entry
    {
        public int EntryId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<UserEntry> UserEntries { get; set; }
    }
}
