using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.Server.Data.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7675506d-9cd0-454a-a83a-5fbedeec43ae", 0, "61503f8f-bb44-4de9-bc44-2a40c9373ea7", "user2@email.com", true, true, null, "USER2@EMAIL.COM", "USER2@EMAIL.COM", "AQAAAAEAACcQAAAAEHs+NffhB0wdGrOvFE7TPseS/YOtjMcG9BnBNlSiaIit5IOpdY2VOC6UIZ1/qYl6Hg==", null, false, "XL6DCPUZVN77JPCIE47V6CLOUVCUWTJB", false, "user2@email.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "cf231359-76d2-489c-836b-85357ded6650", 0, "bed371fa-b5da-4a9c-8c70-c9598f917d77", "user1@email.com", true, true, null, "USER1@EMAIL.COM", "USER1@EMAIL.COM", "AQAAAAEAACcQAAAAEO2Lq4DiSQDypXXSDLTZdGQ0Nl3cwOUvoAGLIuX/LRm2gklpbyKMZOsqO3dTW6z+Cw==", null, false, "D2AQODZTXHHT56TQYVLLALAUWLPEQUZD", false, "user1@email.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7675506d-9cd0-454a-a83a-5fbedeec43ae");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cf231359-76d2-489c-836b-85357ded6650");
        }
    }
}
