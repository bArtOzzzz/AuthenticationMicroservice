using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationMicroservice.Migrations
{
    public partial class FridgeDb : Migration
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
                values: new object[] { new Guid("23b8fdd2-5f83-47d9-96ee-19cefbc356cf"), new DateTime(2022, 10, 2, 10, 18, 28, 288, DateTimeKind.Utc).AddTicks(6977), "Administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "Role" },
                values: new object[] { new Guid("55b14b79-f497-4b37-aaaa-3cf57ba31cd8"), new DateTime(2022, 10, 2, 10, 18, 28, 288, DateTimeKind.Utc).AddTicks(6982), "User" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "EmailAddress", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "Username" },
                values: new object[] { new Guid("66316569-18ae-4096-8158-670804d563a0"), new DateTime(2022, 10, 2, 10, 18, 28, 56, DateTimeKind.Utc).AddTicks(4184), "SonicHedgehog@gmail.com", "$2a$11$9bJrCsPmbvMHkpA9/mH6pOJJh2Jj65JqkWd5oGBzydPcFPxeDtJIS", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("23b8fdd2-5f83-47d9-96ee-19cefbc356cf"), "Sonic" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "EmailAddress", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "Username" },
                values: new object[] { new Guid("7463359d-a42e-43b0-9e4a-05d3d28b030a"), new DateTime(2022, 10, 2, 10, 18, 28, 172, DateTimeKind.Utc).AddTicks(7474), "User@gmail.com", "$2a$11$5vScpRVLsFqdrDjcChKO0ex474weGUpvOotWqtia2YbRdFh6mUtNu", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55b14b79-f497-4b37-aaaa-3cf57ba31cd8"), "User" });

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
