using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMessage_Channel_ChannelId",
                table: "ChannelMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMessage_Message_MessageId",
                table: "ChannelMessage");

            migrationBuilder.DropTable(
                name: "UserMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelMessage",
                table: "ChannelMessage");

            migrationBuilder.RenameTable(
                name: "ChannelMessage",
                newName: "ChannelMessages");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelMessage_MessageId",
                table: "ChannelMessages",
                newName: "IX_ChannelMessages_MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelMessage_ChannelId",
                table: "ChannelMessages",
                newName: "IX_ChannelMessages_ChannelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelMessages",
                table: "ChannelMessages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserMessages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReceiverId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMessages_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMessages_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_MessageId",
                table: "UserMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_ReceiverId",
                table: "UserMessages",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMessages_Channel_ChannelId",
                table: "ChannelMessages",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMessages_Message_MessageId",
                table: "ChannelMessages",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMessages_Channel_ChannelId",
                table: "ChannelMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMessages_Message_MessageId",
                table: "ChannelMessages");

            migrationBuilder.DropTable(
                name: "UserMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelMessages",
                table: "ChannelMessages");

            migrationBuilder.RenameTable(
                name: "ChannelMessages",
                newName: "ChannelMessage");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelMessages_MessageId",
                table: "ChannelMessage",
                newName: "IX_ChannelMessage_MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelMessages_ChannelId",
                table: "ChannelMessage",
                newName: "IX_ChannelMessage_ChannelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelMessage",
                table: "ChannelMessage",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserMessage",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReceiverId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChannelId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMessage_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserMessage_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMessage_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessage_ChannelId",
                table: "UserMessage",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessage_MessageId",
                table: "UserMessage",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessage_ReceiverId",
                table: "UserMessage",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMessage_Channel_ChannelId",
                table: "ChannelMessage",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMessage_Message_MessageId",
                table: "ChannelMessage",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
