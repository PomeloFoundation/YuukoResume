using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YuukoResume.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Bytes = table.Column<byte[]>(nullable: true),
                    ContentLength = table.Column<long>(nullable: false),
                    ContentType = table.Column<string>(maxLength: 128, nullable: true),
                    FileName = table.Column<string>(maxLength: 128, nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    From = table.Column<DateTime>(nullable: false),
                    Profession = table.Column<string>(nullable: true),
                    School = table.Column<string>(maxLength: 64, nullable: true),
                    To = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Company = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    From = table.Column<DateTime>(nullable: false),
                    Position = table.Column<string>(maxLength: 128, nullable: true),
                    To = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Level = table.Column<float>(nullable: false),
                    Performance = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BlobId = table.Column<Guid>(nullable: false),
                    PRI = table.Column<long>(nullable: false),
                    Title = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_Blobs_BlobId",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BlobId = table.Column<Guid>(nullable: false),
                    Catalog = table.Column<string>(maxLength: 128, nullable: true),
                    DemoUrl = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    From = table.Column<DateTime>(nullable: false),
                    GitHub = table.Column<string>(maxLength: 256, nullable: true),
                    Tags = table.Column<string>(maxLength: 256, nullable: true),
                    Title = table.Column<string>(maxLength: 128, nullable: true),
                    To = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Blobs_BlobId",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blobs_FileName",
                table: "Blobs",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_Blobs_Time",
                table: "Blobs",
                column: "Time");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_BlobId",
                table: "Certificates",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_PRI",
                table: "Certificates",
                column: "PRI");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_From",
                table: "Educations",
                column: "From");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_From",
                table: "Experiences",
                column: "From");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_IsRead",
                table: "Logs",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Time",
                table: "Logs",
                column: "Time");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_BlobId",
                table: "Projects",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_From",
                table: "Projects",
                column: "From");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Level",
                table: "Skills",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Performance",
                table: "Skills",
                column: "Performance");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "Experiences");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Blobs");
        }
    }
}
