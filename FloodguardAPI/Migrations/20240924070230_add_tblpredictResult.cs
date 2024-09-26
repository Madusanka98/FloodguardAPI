using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodguardAPI.Migrations
{
    /// <inheritdoc />
    public partial class add_tblpredictResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_predictResult",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RangeDate = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ConfigTime = table.Column<int>(type: "int", nullable: false),
                    RiverId = table.Column<int>(type: "int", nullable: false),
                    StationId = table.Column<int>(type: "int", nullable: false),
                    Rainfall = table.Column<double>(type: "float", nullable: false),
                    RiverHeight = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_predictResult", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_predictResult");
        }
    }
}
