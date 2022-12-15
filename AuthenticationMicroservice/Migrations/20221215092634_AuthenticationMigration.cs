using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationMicroservice.Migrations
{
    public partial class AuthenticationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
