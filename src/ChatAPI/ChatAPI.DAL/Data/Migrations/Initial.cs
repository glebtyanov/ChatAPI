#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ChatAPI.DAL.Data
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>("character varying(100)", maxLength: 100, nullable: false),
                    ConnectionId = table.Column<string>("text", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

            migrationBuilder.CreateTable(
                "Chats",
                table => new
                {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>("character varying(100)", maxLength: 100, nullable: false),
                    AdminId = table.Column<int>("integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        "FK_Chats_Users_AdminId",
                        x => x.AdminId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Messages",
                table => new
                {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>("character varying(500)", maxLength: 500, nullable: false),
                    Timestamp = table.Column<DateTime>("timestamp with time zone", nullable: false),
                    AuthorId = table.Column<int>("integer", nullable: false),
                    ChatId = table.Column<int>("integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        "FK_Messages_Chats_ChatId",
                        x => x.ChatId,
                        "Chats",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Messages_Users_AuthorId",
                        x => x.AuthorId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Chats_AdminId",
                "Chats",
                "AdminId");

            migrationBuilder.CreateIndex(
                "IX_Messages_AuthorId",
                "Messages",
                "AuthorId");

            migrationBuilder.CreateIndex(
                "IX_Messages_ChatId",
                "Messages",
                "ChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Messages");

            migrationBuilder.DropTable(
                "Chats");

            migrationBuilder.DropTable(
                "Users");
        }
    }
}