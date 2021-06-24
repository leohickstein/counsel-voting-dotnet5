using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CounselVoting.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Measures",
                columns: table => new
                {
                    MeasureId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Subject = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    IsComplete = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measures", x => x.MeasureId);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    RuleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Identifier = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.RuleId);
                });

            migrationBuilder.CreateTable(
                name: "MeasureVotes",
                columns: table => new
                {
                    VoteId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    VoteChoice = table.Column<int>(type: "INTEGER", nullable: false),
                    VoteDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    MeasureId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasureVotes", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_MeasureVotes_Measures_MeasureId",
                        column: x => x.MeasureId,
                        principalTable: "Measures",
                        principalColumn: "MeasureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeasureRules",
                columns: table => new
                {
                    MeasureId = table.Column<int>(type: "INTEGER", nullable: false),
                    RuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasureRules", x => new { x.MeasureId, x.RuleId });
                    table.ForeignKey(
                        name: "FK_MeasureRules_Measures_MeasureId",
                        column: x => x.MeasureId,
                        principalTable: "Measures",
                        principalColumn: "MeasureId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeasureRules_Rules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rules",
                        principalColumn: "RuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Measures",
                columns: new[] { "MeasureId", "CompletedDate", "Description", "IsComplete", "Status", "Subject" },
                values: new object[] { 1, null, "Description for installing a front gate", false, 0, "Install a front gate" });

            migrationBuilder.InsertData(
                table: "Measures",
                columns: new[] { "MeasureId", "CompletedDate", "Description", "IsComplete", "Status", "Subject" },
                values: new object[] { 2, null, "Description for fixing the current generator", false, 0, "Fix the current generator" });

            migrationBuilder.InsertData(
                table: "Measures",
                columns: new[] { "MeasureId", "CompletedDate", "Description", "IsComplete", "Status", "Subject" },
                values: new object[] { 3, null, "Description for incresing the number of strata counsels", false, 0, "Increase the number of strata counsels" });

            migrationBuilder.InsertData(
                table: "Rules",
                columns: new[] { "RuleId", "Description", "Discriminator", "Identifier", "Name" },
                values: new object[] { 1, "Description for Minimum Votes Required Completion Rule", "CompletionRule", "MinimumVotesRequiredCompletionRule", "Minimum Votes Required Completion Rule" });

            migrationBuilder.InsertData(
                table: "Rules",
                columns: new[] { "RuleId", "Description", "Discriminator", "Identifier", "Name" },
                values: new object[] { 2, "Description for Minimum Percentage Yes Votes Required Completion Rule", "CompletionRule", "MinimumPercentageYesVotesRequiredCompletionRule", "Minimum Percentage Yes Votes Required Completion Rule" });

            migrationBuilder.InsertData(
                table: "MeasureRules",
                columns: new[] { "MeasureId", "RuleId", "Value" },
                values: new object[] { 1, 1, "5" });

            migrationBuilder.InsertData(
                table: "MeasureRules",
                columns: new[] { "MeasureId", "RuleId", "Value" },
                values: new object[] { 2, 2, "80" });

            migrationBuilder.InsertData(
                table: "MeasureRules",
                columns: new[] { "MeasureId", "RuleId", "Value" },
                values: new object[] { 3, 1, "10" });

            migrationBuilder.InsertData(
                table: "MeasureRules",
                columns: new[] { "MeasureId", "RuleId", "Value" },
                values: new object[] { 3, 2, "50" });

            migrationBuilder.CreateIndex(
                name: "IX_MeasureRules_RuleId",
                table: "MeasureRules",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasureVotes_MeasureId",
                table: "MeasureVotes",
                column: "MeasureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeasureRules");

            migrationBuilder.DropTable(
                name: "MeasureVotes");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "Measures");
        }
    }
}
