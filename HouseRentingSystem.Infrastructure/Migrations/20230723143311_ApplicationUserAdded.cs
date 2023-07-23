using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Infrastructure.Migrations
{
    public partial class ApplicationUserAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "FirstName", "IsActive", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6dbb79db-46f7-4a56-9585-a22de957d8f8", "Guest", true, "Guestov", "AQAAAAEAACcQAAAAEGA5I+DPxOJu0icQTVH021yU6DtextqBEMMwOsay/0hcwk4O24M3lq3thuug3l+0cg==", "7b064a51-63d2-46b3-a0a4-603e4d038410" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "FirstName", "IsActive", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cabeeffc-0d40-4bdb-9dcb-692cc17ce902", "Linda", true, "Michaels", "AQAAAAEAACcQAAAAEPJTOnJFox2zjP3xl61kQlE8IbCIj4wdq408ErPSjyZpGhbq9nUNUMpVsl74Xnybqg==", "be26ea74-6623-4720-91b4-34a2b63dfe22" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6d4200ce-d726-4fc8-83d9-d6b3ac1f591e", 0, "0a173025-c99f-40e8-83f2-ba0f7e123675", "mario@mail.com", false, "Mario", true, "Anastasov", false, null, "mario@mail.com", "mario@mail.com", "AQAAAAEAACcQAAAAEHRNvmG1YRgCkDMcb0h7gu5ev+SaZKBuPLKKlZyKBBBQkClP2I0shnjLxzCq/5c4Jw==", null, false, "f2ffb74a-09c6-4044-a40b-cac89a14338d", false, "mario@mail.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d4200ce-d726-4fc8-83d9-d6b3ac1f591e");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "583797cc-4368-4663-ab12-a0d0105d1c7f", "AQAAAAEAACcQAAAAEPUmAsGYuLK5bSWm/Rg4oiITh3JaKGIEESOj0q4H5v0ZTbvPSc84oCAUkDHTTLmmOQ==", "b8b6503e-80cd-4e43-a68a-87683beea73b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4b29327a-9de4-4a54-863b-04e7eefb5ef8", "AQAAAAEAACcQAAAAEKWxDv81B6kZdN9ZN7aF1Av6rzL3LWvVMqEn64/NCFFr5gLioFvfNkdIgUbjpAiRxw==", "7c7b362c-6bf4-4318-9f9b-46826bf86d3d" });
        }
    }
}
