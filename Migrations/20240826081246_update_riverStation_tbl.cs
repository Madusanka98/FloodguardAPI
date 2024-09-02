using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodguardAPI.Migrations
{
    /// <inheritdoc />
    public partial class update_riverStation_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "alertLevel",
                table: "tbl_riverStationUsers");

            migrationBuilder.DropColumn(
                name: "majorLevel",
                table: "tbl_riverStationUsers");

            migrationBuilder.DropColumn(
                name: "minorLevel",
                table: "tbl_riverStationUsers");

            migrationBuilder.AddColumn<double>(
                name: "alertLevel",
                table: "tbl_riverStation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "majorLevel",
                table: "tbl_riverStation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "minorLevel",
                table: "tbl_riverStation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "alertLevel",
                table: "tbl_riverStation");

            migrationBuilder.DropColumn(
                name: "majorLevel",
                table: "tbl_riverStation");

            migrationBuilder.DropColumn(
                name: "minorLevel",
                table: "tbl_riverStation");

            migrationBuilder.AddColumn<double>(
                name: "alertLevel",
                table: "tbl_riverStationUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "majorLevel",
                table: "tbl_riverStationUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "minorLevel",
                table: "tbl_riverStationUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
