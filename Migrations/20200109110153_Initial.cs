using Microsoft.EntityFrameworkCore.Migrations;

namespace Parser.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    DomainName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.DomainName);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Url = table.Column<string>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    DomainName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Url);
                    table.ForeignKey(
                        name: "FK_Records_Domains_DomainName",
                        column: x => x.DomainName,
                        principalTable: "Domains",
                        principalColumn: "DomainName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_DomainName",
                table: "Records",
                column: "DomainName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Domains");
        }
    }
}
