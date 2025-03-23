﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreekRecruit.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentsTableAddAuthorColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "comment_author",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "comment_author",
                table: "Comments");
        }
    }
}
