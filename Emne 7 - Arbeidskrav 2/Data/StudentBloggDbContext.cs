using Microsoft.EntityFrameworkCore;

namespace Emne_7___Arbeidskrav_2.Data;

public class StudentBloggDbContext : DbContext
{
    public StudentBlogDbContext(DbContextOptions<StudentBloggDbContext> options) : base(options)
    {
    }

    // Define DbSet properties for each entity
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships and constraints

        // User-Post relationship: One-to-Many (User has many Posts)
        modelBuilder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete to remove all posts when a user is deleted

        // Post-Comment relationship: One-to-Many (Post has many Comments)
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete to remove comments when a post is deleted

        // User-Comment relationship: One-to-Many (User has many Comments)
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete to remove comments when a user is deleted

        // Additional configurations, such as unique constraints
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}