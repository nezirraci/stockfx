using ElcomManage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElcomManage.Data
{
    public class ElcomDb : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
      
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<StockLocation> StockLocation { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }

        public ElcomDb(DbContextOptions<ElcomDb> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = BuildConnectionString(); // Your connection string logic here

            optionsBuilder.UseNpgsql(connString);
        }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().Property(p => p.Email).IsRequired(false);
            modelBuilder.Entity<ApplicationUser>().HasIndex(p => p.UserName).IsUnique();

            modelBuilder.Entity<ApplicationUser>().Property(p => p.Id).HasIdentityOptions(startValue: 50);

            modelBuilder.Entity<Node>().Property(p => p.Id).HasIdentityOptions(startValue: 50);

            modelBuilder.Entity<ProductCategory>().Property(p => p.Id).HasIdentityOptions(startValue: 50);

            modelBuilder.Entity<Product>().Property(p => p.Id).HasIdentityOptions(startValue: 50);

            modelBuilder.Entity<StockLocation>().Property(p => p.Id).HasIdentityOptions(startValue: 50);

            modelBuilder.Entity<Activity>().Property(p => p.Id).HasIdentityOptions(startValue: 50);


            var user = new ApplicationUser
            {
                Id = 1,
                UserName = "Admin",
                Email = "Admin",
                FirstName = "Admin",
                LastName = "Admin",
                SecurityStamp=Guid.NewGuid().ToString()
            };

            var PasswordHasher = new PasswordHasher<ApplicationUser>();
            var PasswordHashed = PasswordHasher.HashPassword(user, "3lc0m");
            user.PasswordHash = PasswordHashed;
            //Add Initial User
            modelBuilder.Entity<ApplicationUser>().HasData(user);

            //Add Initial Roles
            modelBuilder.Entity<IdentityRole<int>>().HasData(new IdentityRole<int>
            {

                Id = 1,
                Name = "ADMIN",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<int>
            {
                Id = 2,
                Name = "PUNETOR BAZE",
                NormalizedName = "PUNETOR BAZE"
            },
            new IdentityRole<int>
            {
                Id = 3,
                Name = "PUNETOR TERENI",
                NormalizedName = "PUNETOR TERENI"
            },
            new IdentityRole<int>
            {
                Id = 4,
                Name = "KONTABILIST",
                NormalizedName = "KONTABILIST"
            },
            new IdentityRole<int>
            {
                Id = 5,
                Name = "IT",
                NormalizedName = "IT"
            });


            //Add UserToRole
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>
            {
                RoleId = 1,
                UserId = 1
            });

            modelBuilder.Entity<StockLocation>().HasData(
           new StockLocation {
                Id=1,
                Name="BAZE 1",
                InBase=true,
                InHouse=true
            },
           new StockLocation
           {
               Id = 2,
               Name = "BAZE 2",
               InBase = true,
               InHouse = true
           },
           new StockLocation
           {
               Id = 3,
               Name = "PIKAP 1",
               InBase = false,
               InHouse = true
           },
           new StockLocation
           {
               Id = 4,
               Name = "PIKAP 2",
               InBase = false,
               InHouse = true
           },
           new StockLocation
           {
               Id = 5,
               Name = "VALI@NET",
               InBase = false,
               InHouse = false
           },
           new StockLocation
           {
               Id = 6,
               Name = "KLIENT",
               InBase = false,
               InHouse = true
           });
      
            modelBuilder.Entity<ProductCategory>().HasData(
           new ProductCategory
           {
               Id = 1,
               Name="MODEM"
           },
           new ProductCategory
           {
               Id = 2,
               Name = "WIRELESS ROUTER"
           },
           new ProductCategory
           {
               Id = 3,
               Name = "RECEIVER"
           },
           new ProductCategory
           {
               Id = 4,
               Name = "ONU"
           },
           new ProductCategory
           {
               Id = 5,
               Name = "CARD"
           },
           new ProductCategory
           {
               Id = 6,
               Name = "CABLE"
           },   
             new ProductCategory
             {
                 Id = 7,
                 Name = "ANTENA"
             },
              new ProductCategory
              {
                  Id = 8,
                  Name = "TAP COAXIAL"
              },
              new ProductCategory
              {
                  Id = 9,
                  Name = "SPLITTER"
              },
              new ProductCategory
              {
                  Id = 10,
                  Name = "HALLKE"
              },
               new ProductCategory
               {
                   Id = 11,
                   Name = "KASETE"
               },
               new ProductCategory
               {
                   Id = 12,
                   Name = "POE"
               },
               new ProductCategory
               {
                   Id = 13,
                   Name = "SWITCH"
               });


        }

        private string BuildConnectionString()
        {
            var DATABASE_SERVICE = Environment.GetEnvironmentVariable("DATABASE_SERVICE");
            var DATABASE_PORT = Environment.GetEnvironmentVariable("DATABASE_PORT");
            var DATABASE_NAME = Environment.GetEnvironmentVariable("DATABASE_NAME");
            var DATABASE_USER = Environment.GetEnvironmentVariable("DATABASE_USER");
            var DATABASE_PASSWORD = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

            string connectionString = $"Server={DATABASE_SERVICE};Port={DATABASE_PORT};Database={DATABASE_NAME};User Id={DATABASE_USER};Password={DATABASE_PASSWORD};";

            return connectionString;
        }

    }
}
