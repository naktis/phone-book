using System.Collections.Generic;

namespace Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Entry> Entries { get; set; }
        public ICollection<UserEntry> UserEntries { get; set; }
    }
}
