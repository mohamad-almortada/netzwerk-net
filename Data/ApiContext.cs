using Microsoft.EntityFrameworkCore;
using Netzwerk.Model;

namespace Netzwerk.Data;

public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Marker> Markers { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<Map> Maps { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        
        // builder.Entity<User>()
        //     .HasMany(u => u.Markers)
        //     .WithOne(m => m.User)
        //     .HasForeignKey(m => m.UserId)
        //     .OnDelete(DeleteBehavior.SetNull);
        //
        // builder.Entity<User>()
        //     .HasIndex(u => u.Email)
        //     .IsUnique();
        
        builder.Entity<Marker>()
            .HasOne(m => m.User)
            .WithMany(u => u.Markers)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Marker>()
            .HasOne(m => m.Verifier)
            .WithMany()
            .HasForeignKey(m => m.VerifiedBy)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Vote>()
            .HasOne(v => v.User)
            .WithMany(u => u.Votes)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Vote>()
            .HasOne(v => v.Marker)
            .WithMany(m => m.Votes)
            .HasForeignKey(v => v.MarkerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    
    
    
}