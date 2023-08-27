using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokedexApp.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PokemonSprites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FrontDefault = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrontShiny = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrontFemale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrontShinyFemale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackDefault = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackShiny = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackFemale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackShinyFemale = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonSprites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseExperience = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    PokemonSpriteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pokemons_PokemonSprites_PokemonSpriteId",
                        column: x => x.PokemonSpriteId,
                        principalTable: "PokemonSprites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PokemonType",
                columns: table => new
                {
                    PokemonsId = table.Column<int>(type: "int", nullable: false),
                    TypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonType", x => new { x.PokemonsId, x.TypesId });
                    table.ForeignKey(
                        name: "FK_PokemonType_Pokemons_PokemonsId",
                        column: x => x.PokemonsId,
                        principalTable: "Pokemons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonType_Types_TypesId",
                        column: x => x.TypesId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_PokemonSpriteId",
                table: "Pokemons",
                column: "PokemonSpriteId");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonType_TypesId",
                table: "PokemonType",
                column: "TypesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PokemonType");

            migrationBuilder.DropTable(
                name: "Pokemons");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropTable(
                name: "PokemonSprites");
        }
    }
}
