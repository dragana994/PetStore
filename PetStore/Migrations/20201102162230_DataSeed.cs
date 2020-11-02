using Microsoft.EntityFrameworkCore.Migrations;

namespace PetStore.Migrations
{
	public partial class DataSeed : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder
								.Sql("INSERT INTO Brands (Name) Values ('Trixi')");
			migrationBuilder
								.Sql("INSERT INTO Brands (Name) Values ('Kongo')");
			migrationBuilder
								.Sql("INSERT INTO Brands (Name) Values ('Brit')");

			migrationBuilder
								.Sql("INSERT INTO Categories (Name, Description, SuperCategoryId) VALUES ('Dogs', 'Toys for dogs', NULL)");
			migrationBuilder
			.Sql("INSERT INTO Categories (Name, Description, SuperCategoryId) VALUES ('Cats', 'Toys for cats', NULL)");
			migrationBuilder
								.Sql("INSERT INTO Categories (Name, Description, SuperCategoryId) VALUES ('Parrots', 'Toys for parrots', NULL)");

			migrationBuilder
								.Sql("INSERT INTO AspNetRoles (Name, NormalizedName) VALUES('Admin', 'ADMIN')");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("DELETE FROM Brands");
			migrationBuilder.Sql("DELETE FROM Categories");
		}
	}
}