using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalEstudoWebApi.Migrations
{
    /// <inheritdoc />
    public partial class questaodb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Alternativas",
                table: "questoes",
                newName: "AlternativaE");

            migrationBuilder.AddColumn<string>(
                name: "AlternativaA",
                table: "questoes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AlternativaB",
                table: "questoes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AlternativaC",
                table: "questoes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AlternativaD",
                table: "questoes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "questoes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "questoes",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlternativaA",
                table: "questoes");

            migrationBuilder.DropColumn(
                name: "AlternativaB",
                table: "questoes");

            migrationBuilder.DropColumn(
                name: "AlternativaC",
                table: "questoes");

            migrationBuilder.DropColumn(
                name: "AlternativaD",
                table: "questoes");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "questoes");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "questoes");

            migrationBuilder.RenameColumn(
                name: "AlternativaE",
                table: "questoes",
                newName: "Alternativas");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "usuario",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
