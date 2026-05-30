using Microsoft.EntityFrameworkCore;
using RestReads.Core.Entities;
using RestReads.Core.Enums;

namespace RestReads.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<ReadingList> ReadingLists => Set<ReadingList>();
    public DbSet<ReadingEntry> ReadingEntries => Set<ReadingEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.HasIndex(u => u.Username).IsUnique();
        });

        modelBuilder.Entity<ReadingList>(e =>
        {
            e.HasOne(l => l.User)
                .WithMany(u => u.ReadingLists)
                .HasForeignKey(l => l.UserId);
        });

        modelBuilder.Entity<ReadingEntry>(e =>
        {
            e.HasOne(re => re.ReadingList)
                .WithMany(l => l.Entries)
                .HasForeignKey(re => re.ReadingListId);

            e.HasOne(re => re.Book)
                .WithMany()
                .HasForeignKey(re => re.BookId);

            // A book can only appear once per list
            e.HasIndex(re => new { re.ReadingListId, re.BookId }).IsUnique();
        });
    }
}
