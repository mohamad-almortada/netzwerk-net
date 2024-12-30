using Microsoft.EntityFrameworkCore;
using Netzwerk.Model;

namespace Netzwerk.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<UserKeyword> UserKeyword { get; set; }
        public DbSet<GeoLocation> GeoLocations { get; set; }


         protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

             builder.Entity<UserKeyword>()
                .HasKey(uk => new { uk.UserId, uk.KeywordId });

            builder.Entity<UserKeyword>()
                .HasOne(ba => ba.User)
                .WithMany(b => b.UserKeywords)
                .HasForeignKey(ba => ba.UserId);
            
            builder.Entity<UserKeyword>()
                .HasOne(ba => ba.Keyword)
                .WithMany(a => a.UserKeywords)
                .HasForeignKey(ba => ba.KeywordId);
            builder.Entity<GeoLocation>()
                .HasKey(cs => new { cs.Latitude, cs.Longitude });
            builder.Entity<GeoLocation>()
                .HasMany(cs => cs.Users)
                .WithOne(u => u.GeoLocation)
                .HasForeignKey(u => new { u.GeoLocationLat, u.GeoLocationLon });
        }
    }
}