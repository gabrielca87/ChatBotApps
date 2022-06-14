using Chat.Server.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Chat.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Department
            modelBuilder.Entity<ApplicationUser>()
                        .HasData(
                            new ApplicationUser
                            {
                                Id = "7675506d-9cd0-454a-a83a-5fbedeec43ae",
                                UserName = "user2@email.com",
                                NormalizedUserName = "USER2@EMAIL.COM",
                                Email = "user2@email.com",
                                NormalizedEmail = "USER2@EMAIL.COM",
                                EmailConfirmed = true,
                                PasswordHash = "AQAAAAEAACcQAAAAEHs+NffhB0wdGrOvFE7TPseS/YOtjMcG9BnBNlSiaIit5IOpdY2VOC6UIZ1/qYl6Hg==",
                                SecurityStamp = "XL6DCPUZVN77JPCIE47V6CLOUVCUWTJB",
                                ConcurrencyStamp = "61503f8f-bb44-4de9-bc44-2a40c9373ea7",
                                PhoneNumber = null,
                                PhoneNumberConfirmed = false,
                                TwoFactorEnabled = false,
                                LockoutEnd = null,
                                LockoutEnabled = true,
                                AccessFailedCount = 0
                            },
                            new ApplicationUser
                            {
                                Id = "cf231359-76d2-489c-836b-85357ded6650",
                                UserName = "user1@email.com",
                                NormalizedUserName = "USER1@EMAIL.COM",
                                Email = "user1@email.com",
                                NormalizedEmail = "USER1@EMAIL.COM",
                                EmailConfirmed = true,
                                PasswordHash = "AQAAAAEAACcQAAAAEO2Lq4DiSQDypXXSDLTZdGQ0Nl3cwOUvoAGLIuX/LRm2gklpbyKMZOsqO3dTW6z+Cw==",
                                SecurityStamp = "D2AQODZTXHHT56TQYVLLALAUWLPEQUZD",
                                ConcurrencyStamp = "bed371fa-b5da-4a9c-8c70-c9598f917d77",
                                PhoneNumber = null,
                                PhoneNumberConfirmed = false,
                                TwoFactorEnabled = false,
                                LockoutEnd = null,
                                LockoutEnabled = true,
                                AccessFailedCount = 0
                            });

        }
    }
}
