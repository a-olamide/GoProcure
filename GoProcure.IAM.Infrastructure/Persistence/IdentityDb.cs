using GoProcure.IAM.Infrastructure.Auth;
using GoProcure.IAM.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.IAM.Infrastructure.Persistence
{
    public class IdentityDb : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public IdentityDb(DbContextOptions<IdentityDb> options) : base(options) { }
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Department> Departments => Set<Department>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>().HasData(
                new Department { Id = new Guid("bf3da4a6-314c-4b73-a0c8-7e8bb758b570"), Name = "Finance" },
                new Department { Id = new Guid("13829f1c-043b-4143-912c-b94e4741e9b8"), Name = "Procurement" },
                new Department { Id = new Guid("ad21d88e-66f0-4e10-b752-68a96f554497"), Name = "IT" },
                new Department { Id = new Guid("63b03900-0481-4196-b57e-fe01daea2b56"), Name = "HR" },
                new Department { Id = new Guid("9e7de21f-26ad-43ae-8c36-4c6b9b7c1d59"), Name = "Admin" }
            );
            modelBuilder.Entity<AppUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<AppRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AspNetUserRoles");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AspNetUserClaims");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AspNetUserLogins"); // keys come from base
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AspNetRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AspNetUserTokens");
        }
    }
}
