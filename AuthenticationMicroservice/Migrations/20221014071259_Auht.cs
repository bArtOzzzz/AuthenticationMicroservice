using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationMicroservice.Migrations
{
    public partial class Auht : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                values: new object[] { new Guid("8f7e348b-82d2-4081-9bcc-bb3ef8e0fff3"), new DateTime(2022, 10, 14, 7, 12, 59, 215, DateTimeKind.Utc).AddTicks(5536), "Administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "Role" },
                values: new object[] { new Guid("fd010e4d-cd37-4ca9-a98a-222c32e90e4a"), new DateTime(2022, 10, 14, 7, 12, 59, 215, DateTimeKind.Utc).AddTicks(5540), "User" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "EmailAddress", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "Username" },
                values: new object[] { new Guid("1c2b3665-df04-4938-8a78-5ad0db67182c"), new DateTime(2022, 10, 14, 7, 12, 58, 971, DateTimeKind.Utc).AddTicks(8028), "SonicHedgehog@gmail.com", "$2a$11$OGrzACnKXpVtzi4BPtGG/.frca7BZzcI1XPdESf3rVPYtvmqtxv5m", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8f7e348b-82d2-4081-9bcc-bb3ef8e0fff3"), "Sonic" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "EmailAddress", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "Username" },
                values: new object[] { new Guid("ecb68204-0d99-4b2d-9d55-c4b7351af908"), new DateTime(2022, 10, 14, 7, 12, 59, 96, DateTimeKind.Utc).AddTicks(2826), "User@gmail.com", "$2a$11$N8XwZ6bno9hJyFp/PkPmv.Kb6GKoW9jOhc1.0LdANi..661PaY.xG", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fd010e4d-cd37-4ca9-a98a-222c32e90e4a"), "User" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
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
