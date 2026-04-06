using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VgcCollege.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExamAndAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Assignments");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Exams",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Exams",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                table: "Exams",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Assignments",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "MaxScore",
                table: "Assignments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "Exams");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Exams",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Assignments",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxScore",
                table: "Assignments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Grade",
                table: "Assignments",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
