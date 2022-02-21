using CrystalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CrystalApi.Data;

public class DbDataContext : DbContext
{
    public DbSet<Auth> Auths { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Invite> Invites { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql("Server=localhost;User Id=root;Password=;Database=crystal_api",
                ServerVersion.AutoDetect("Server=localhost;User Id=root;Password=;Database=crystal_api"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auth>().ToTable("auths")
            .HasOne(p => p.User)
            .WithMany(b => b.Auths)
            .HasForeignKey(p => p.UserId);
        modelBuilder.Entity<Comment>().ToTable("comments")
            .HasOne(x => x.Post)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.PostId);
        modelBuilder.Entity<Invite>().ToTable("invites")
            .HasOne(x => x.User)
            .WithMany(x => x.Invites)
            .HasForeignKey(x => x.ToId);
        modelBuilder.Entity<Like>().ToTable("likes")
            .HasOne(x => x.Post)
            .WithMany(x => x.Likes)
            .HasForeignKey(x => x.PostId);
        modelBuilder.Entity<Post>().ToTable("posts")
            .HasOne(x => x.User)
            .WithMany(x => x.Posts)
            .HasForeignKey(x => x.UserId);
        modelBuilder.Entity<User>().ToTable("users");
    }
}