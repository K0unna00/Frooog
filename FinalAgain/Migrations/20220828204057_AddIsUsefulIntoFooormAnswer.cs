using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalAgain.Migrations
{
    public partial class AddIsUsefulIntoFooormAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUseful",
                table: "FooormAnswers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUseful",
                table: "FooormAnswers");
        }
    }
}
