using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackRobotTDM.Migrations
{
    public partial class RFC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SocketId",
                table: "RobotsUsage");

            migrationBuilder.DropColumn(
                name: "UserToken",
                table: "RobotsUsage");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RobotsUsage",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "System",
                table: "RobotsUsage",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RobotsUsage",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "RobotsUsage",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Limit",
                table: "RobotsUsage",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "For",
                table: "RobotsUsage",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Current",
                table: "RobotsUsage",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "RobotsUsage",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "RobotsUsage",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "RobotsUsage",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserIp",
                table: "PeticionesActas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PeticionesActas",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "TransposeId",
                table: "PeticionesActas",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "RobotTaken",
                table: "PeticionesActas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nombres",
                table: "PeticionesActas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                table: "PeticionesActas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FechaNac",
                table: "PeticionesActas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "PeticionesActas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Deadline",
                table: "PeticionesActas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "PeticionesActas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Cadena",
                table: "PeticionesActas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CURP",
                table: "PeticionesActas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Apellidos",
                table: "PeticionesActas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PeticionesActas",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegId",
                table: "PeticionesActas",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "actas_reg",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    document = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    dataset = table.Column<string>(type: "text", nullable: true),
                    nameinside = table.Column<string>(type: "text", nullable: true),
                    namefile = table.Column<string>(type: "text", nullable: true),
                    level0 = table.Column<int>(type: "integer", nullable: true),
                    price0 = table.Column<double>(type: "double precision", nullable: true),
                    level1 = table.Column<int>(type: "integer", nullable: true),
                    price1 = table.Column<double>(type: "double precision", nullable: true),
                    level2 = table.Column<int>(type: "integer", nullable: true),
                    price2 = table.Column<double>(type: "double precision", nullable: true),
                    level3 = table.Column<int>(type: "integer", nullable: true),
                    price3 = table.Column<double>(type: "double precision", nullable: true),
                    level4 = table.Column<int>(type: "integer", nullable: true),
                    price4 = table.Column<double>(type: "double precision", nullable: true),
                    level5 = table.Column<int>(type: "integer", nullable: true),
                    price5 = table.Column<double>(type: "double precision", nullable: true),
                    corte = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    send = table.Column<bool>(type: "boolean", nullable: true),
                    idcreated = table.Column<int>(type: "integer", nullable: true),
                    hidden = table.Column<bool>(type: "boolean", nullable: true),
                    idhidden = table.Column<int>(type: "integer", nullable: true),
                    createdtime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    idtranspose = table.Column<int>(type: "integer", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    id2 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actas_reg", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PeticionesActasLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestStatus = table.Column<string>(type: "text", nullable: false),
                    RobotStatus = table.Column<string>(type: "text", nullable: false),
                    ResultStatus = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeticionesActasLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeticionesRFC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Search = table.Column<string>(type: "text", nullable: false),
                    CURP = table.Column<string>(type: "text", nullable: true),
                    Nombres = table.Column<string>(type: "text", nullable: true),
                    Apellidos = table.Column<string>(type: "text", nullable: true),
                    RFC = table.Column<string>(type: "text", nullable: true),
                    Ciudad = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Comments = table.Column<string>(type: "text", nullable: true),
                    Filename = table.Column<string>(type: "text", nullable: true),
                    Deadline = table.Column<string>(type: "text", nullable: true),
                    UserIp = table.Column<string>(type: "text", nullable: true),
                    TransposeId = table.Column<int>(type: "integer", nullable: true),
                    Downloaded = table.Column<bool>(type: "boolean", nullable: false),
                    RobotTaken = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RegId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeticionesRFC", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "actas_reg");

            migrationBuilder.DropTable(
                name: "PeticionesActasLog");

            migrationBuilder.DropTable(
                name: "PeticionesRFC");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "RobotsUsage");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "RobotsUsage");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "RobotsUsage");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PeticionesActas");

            migrationBuilder.DropColumn(
                name: "RegId",
                table: "PeticionesActas");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RobotsUsage",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "System",
                table: "RobotsUsage",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RobotsUsage",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "RobotsUsage",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Limit",
                table: "RobotsUsage",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "For",
                table: "RobotsUsage",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Current",
                table: "RobotsUsage",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocketId",
                table: "RobotsUsage",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserToken",
                table: "RobotsUsage",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserIp",
                table: "PeticionesActas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PeticionesActas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TransposeId",
                table: "PeticionesActas",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RobotTaken",
                table: "PeticionesActas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombres",
                table: "PeticionesActas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                table: "PeticionesActas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FechaNac",
                table: "PeticionesActas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "PeticionesActas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Deadline",
                table: "PeticionesActas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "PeticionesActas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cadena",
                table: "PeticionesActas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CURP",
                table: "PeticionesActas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apellidos",
                table: "PeticionesActas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
