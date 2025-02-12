using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedMessageReceiversTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_ReceiverUserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReceiverUserId",
                table: "Messages");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MESSAGE_RECEIVER",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ReceiverUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Messages");

            migrationBuilder.CreateTable(
                name: "MessageReceivers",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReceivers", x => new { x.MessageId, x.UserId });
                    table.ForeignKey(
                        name: "FK_MessageReceivers_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageReceivers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageReceivers_UserId",
                table: "MessageReceivers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageReceivers");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverUserId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverUserId",
                table: "Messages",
                column: "ReceiverUserId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MESSAGE_RECEIVER",
                table: "Messages",
                sql: "(ReceiverUserId IS NOT NULL AND ReceiverGroupId IS NULL) OR (ReceiverUserId IS NULL AND ReceiverGroupId IS NOT NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_ReceiverUserId",
                table: "Messages",
                column: "ReceiverUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
