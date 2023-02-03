using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;
using System.Runtime.CompilerServices;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext

    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options ) : base( options )
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User_Roles>()
                .HasOne(x => x.user)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(x => x.UserId);


            modelBuilder.Entity<User_Roles>()
                .HasOne(x => x.role)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(x => x.RoleId);
        }

        public DbSet<Region> Regions{ get; set; }
        public DbSet<Walk> Walks { get; set; }

        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }
        public DbSet<User> Users  { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<User_Roles> Users_Roles { get; set; }

    }
}
