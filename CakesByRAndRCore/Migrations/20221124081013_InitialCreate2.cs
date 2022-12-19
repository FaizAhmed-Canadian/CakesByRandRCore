using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CakesByRAndRCore.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "User",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4e9df228-fd7c-4b74-95a1-38f333c3493b", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAECz6nWRmcooFSN9G8Z0zEUrTlRf5Ivm6j9v3cjB9quttAXhrNgqG4LpcEI/0qxQywQ==", "6b4b9591-ee81-4de7-a9ee-4a4bee8a6946" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "User",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b9091bc2-eb7f-4df7-9e58-dc2545689810", null, null, "AQAAAAEAACcQAAAAEM+oOUgDRPRoQP0/93TPwdB13jTsV6ywvAsv/KPJJihosQtwEVlDNuErKXPhSf6irg==", "c96dc8c2-5c0c-4793-a6fb-b5bb1dff9d1b" });
        }
    }
}
