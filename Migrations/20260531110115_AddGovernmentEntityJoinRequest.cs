using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gov_API.Migrations
{
    /// <inheritdoc />
    public partial class AddGovernmentEntityJoinRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentAxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssessmentType = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentAxes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GovernmentEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CompliancePercentage = table.Column<double>(type: "double", nullable: false),
                    ReadinessScore = table.Column<double>(type: "double", nullable: false),
                    MaturityScore = table.Column<double>(type: "double", nullable: false),
                    SecurityStatus = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovernmentEntities", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AssessmentQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssessmentAxisId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssessmentType = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentQuestions_AssessmentAxes_AssessmentAxisId",
                        column: x => x.AssessmentAxisId,
                        principalTable: "AssessmentAxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AssessmentSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GovernmentEntityId = table.Column<int>(type: "int", nullable: false),
                    AssessmentType = table.Column<int>(type: "int", nullable: false),
                    FinalScore = table.Column<double>(type: "double", nullable: false),
                    ResultLevel = table.Column<int>(type: "int", nullable: false),
                    SubmittedByUserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubmittedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentSubmissions_AspNetUsers_SubmittedByUserId",
                        column: x => x.SubmittedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentSubmissions_GovernmentEntities_GovernmentEntityId",
                        column: x => x.GovernmentEntityId,
                        principalTable: "GovernmentEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AssessmentAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssessmentSubmissionId = table.Column<int>(type: "int", nullable: false),
                    AssessmentQuestionId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentAnswers_AssessmentQuestions_AssessmentQuestionId",
                        column: x => x.AssessmentQuestionId,
                        principalTable: "AssessmentQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentAnswers_AssessmentSubmissions_AssessmentSubmission~",
                        column: x => x.AssessmentSubmissionId,
                        principalTable: "AssessmentSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GovernmentEntityId",
                table: "AspNetUsers",
                column: "GovernmentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentAnswers_AssessmentQuestionId",
                table: "AssessmentAnswers",
                column: "AssessmentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentAnswers_AssessmentSubmissionId",
                table: "AssessmentAnswers",
                column: "AssessmentSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQuestions_AssessmentAxisId",
                table: "AssessmentQuestions",
                column: "AssessmentAxisId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentSubmissions_GovernmentEntityId",
                table: "AssessmentSubmissions",
                column: "GovernmentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentSubmissions_SubmittedByUserId",
                table: "AssessmentSubmissions",
                column: "SubmittedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_GovernmentEntities_GovernmentEntityId",
                table: "AspNetUsers",
                column: "GovernmentEntityId",
                principalTable: "GovernmentEntities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_GovernmentEntities_GovernmentEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AssessmentAnswers");

            migrationBuilder.DropTable(
                name: "AssessmentQuestions");

            migrationBuilder.DropTable(
                name: "AssessmentSubmissions");

            migrationBuilder.DropTable(
                name: "AssessmentAxes");

            migrationBuilder.DropTable(
                name: "GovernmentEntities");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GovernmentEntityId",
                table: "AspNetUsers");
        }
    }
}
