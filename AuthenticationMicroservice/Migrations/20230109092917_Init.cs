using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationMicroservice.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("85becf9f-7834-4472-a095-79a31c6930d9"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e0bbd88a-6e56-498d-b137-eee4d17ef13b"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("8aa7be75-8787-467c-abc0-2da4fb96cc06"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d75a52f6-813d-4dd5-b759-be87c43137d9"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "Role" },
                values: new object[] { new Guid("75d1350d-85bf-43b6-bc56-31a2812940a2"), new DateTime(2023, 1, 9, 9, 29, 17, 567, DateTimeKind.Utc).AddTicks(2345), "User" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "Role" },
                values: new object[] { new Guid("b2c3f37f-fba5-45ff-a25f-5e95c071f6aa"), new DateTime(2023, 1, 9, 9, 29, 17, 567, DateTimeKind.Utc).AddTicks(2342), "Administrator" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "EmailAddress", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "Username" },
                values: new object[] { new Guid("4de2202a-acb1-456e-8de0-ba8a970a2837"), new DateTime(2023, 1, 9, 9, 29, 17, 449, DateTimeKind.Utc).AddTicks(9063), "User@gmail.com", "$2a$11$ad1EzdllZcDfz36YPCJ40.18fxD9morfQLCl4y2iVT2c4t5exBJhO", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("75d1350d-85bf-43b6-bc56-31a2812940a2"), "User" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "EmailAddress", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "Username" },
                values: new object[] { new Guid("d03d3dc3-6749-47b8-8cac-d6db024209a2"), new DateTime(2023, 1, 9, 9, 29, 17, 332, DateTimeKind.Utc).AddTicks(8133), "SonicHedgehog@gmail.com", "$2a$11$piUZWqUWiDgR0DEGSWRLwOPNfIaJUmAO1l660x1QElEV/Vqm31Ny2", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b2c3f37f-fba5-45ff-a25f-5e95c071f6aa"), "Administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4de2202a-acb1-456e-8de0-ba8a970a2837"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d03d3dc3-6749-47b8-8cac-d6db024209a2"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("75d1350d-85bf-43b6-bc56-31a2812940a2"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3f37f-fba5-45ff-a25f-5e95c071f6aa"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "Role" },
                values: new object[] { new Guid("8aa7be75-8787-467c-abc0-2da4fb96cc06"), new DateTime(2022, 12, 15, 9, 26, 34, 428, DateTimeKind.Utc).AddTicks(1988), "Administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "Role" },
                values: new object[] { new Guid("d75a52f6-813d-4dd5-b759-be87c43137d9"), new DateTime(2022, 12, 15, 9, 26, 34, 428, DateTimeKind.Utc).AddTicks(1991), "User" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "EmailAddress", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "Username" },
                values: new object[] { new Guid("85becf9f-7834-4472-a095-79a31c6930d9"), new DateTime(2022, 12, 15, 9, 26, 34, 190, DateTimeKind.Utc).AddTicks(7223), "SonicHedgehog@gmail.com", "$2a$11$Lr/dzNnJ3YXGpn.V1rFUQelJCaZ/s1YuYeUzlTh9lyQ2f7iXlyrMq", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8aa7be75-8787-467c-abc0-2da4fb96cc06"), "Administrator" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "EmailAddress", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "Username" },
                values: new object[] { new Guid("e0bbd88a-6e56-498d-b137-eee4d17ef13b"), new DateTime(2022, 12, 15, 9, 26, 34, 307, DateTimeKind.Utc).AddTicks(535), "User@gmail.com", "$2a$11$xCwdNGMtpNCChX2bA6.82ex97Uyu.5kpqnJFf.dv4DOFbbVRKZIiK", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("d75a52f6-813d-4dd5-b759-be87c43137d9"), "User" });
        }
    }
}
