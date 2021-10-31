using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class PhoneBookContext : DbContext
    {
        public PhoneBookContext(DbContextOptions<PhoneBookContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired();

            modelBuilder.Entity<Entry>()
                .Property(e => e.Name)
                .IsRequired();

            modelBuilder.Entity<Entry>()
                .Property(e => e.Phone)
                .IsRequired();

            modelBuilder.Entity<UserEntry>()
                .HasKey(ue => new { ue.UserId, ue.EntryId });

            modelBuilder.Entity<UserEntry>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.UserEntries)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<UserEntry> UserEntries { get; set; }
    }
}
