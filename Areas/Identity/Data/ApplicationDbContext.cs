using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<FinalProjectUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // กำหนดชื่อ Table ใหม่
            builder.Entity<FinalProjectUser>(entity =>
            {
                entity.ToTable("NewUsers");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("NewRoles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("NewUserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("NewUserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("NewUserLogins");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("NewUserTokens");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("NewRoleClaims");
            });
        }
    }
}