using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackRobotTDM.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PeticionesActas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Search = table.Column<string>(type: "text", nullable: false),
                    CURP = table.Column<string>(type: "text", nullable: false),
                    Nombres = table.Column<string>(type: "text", nullable: false),
                    Apellidos = table.Column<string>(type: "text", nullable: false),
                    FechaNac = table.Column<string>(type: "text", nullable: false),
                    Cadena = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    Preferences = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: false),
                    Filename = table.Column<string>(type: "text", nullable: false),
                    Deadline = table.Column<string>(type: "text", nullable: false),
                    UserIp = table.Column<string>(type: "text", nullable: false),
                    TransposeId = table.Column<int>(type: "integer", nullable: false),
                    Downloaded = table.Column<bool>(type: "boolean", nullable: false),
                    RobotTaken = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeticionesActas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RobotsUsage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserToken = table.Column<string>(type: "text", nullable: false),
                    SocketId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    System = table.Column<string>(type: "text", nullable: false),
                    For = table.Column<string>(type: "text", nullable: false),
                    Limit = table.Column<string>(type: "text", nullable: false),
                    Current = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobotsUsage", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeticionesActas");

            migrationBuilder.DropTable(
                name: "RobotsUsage");
        }
    }
}
