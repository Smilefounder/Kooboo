using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kooboo.Mail.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "__DbHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT COLLATE NOCASE", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___DbHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true),
                    AddressType = table.Column<int>(type: "INTEGER", nullable: false),
                    ForwardAddress = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Folder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true),
                    Subscribed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SmtpMessageId = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AddressId = table.Column<int>(type: "INTEGER", nullable: false),
                    OutGoing = table.Column<bool>(type: "INTEGER", nullable: false),
                    FolderId = table.Column<int>(type: "INTEGER", nullable: false),
                    MailFrom = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true),
                    RcptTo = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true),
                    From = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true),
                    To = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true),
                    Cc = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true),
                    Bcc = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true),
                    Subject = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true),
                    BodyPosition = table.Column<long>(type: "INTEGER", nullable: false),
                    Summary = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Read = table.Column<bool>(type: "INTEGER", nullable: false),
                    Answered = table.Column<bool>(type: "INTEGER", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Flagged = table.Column<bool>(type: "INTEGER", nullable: false),
                    Recent = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreationTimeTick = table.Column<long>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TargetAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true),
                    Address = table.Column<string>(type: "TEXT COLLATE NOCASE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetAddress", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__DbHistory");

            migrationBuilder.DropTable(
                name: "EmailAddress");

            migrationBuilder.DropTable(
                name: "Folder");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "TargetAddress");
        }
    }
}
