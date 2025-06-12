using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Starters", "Appetizers" },
                    { 2, "Entrees", "Main Courses" },
                    { 3, "Sweet Treats", "Desserts" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerId", "DeliveryAddress", "OrderDate", "OrderType", "Status", "Total" },
                values: new object[] { 1, 1, null, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, 0m });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "CategoryId", "DailyOrderCount", "IsAvailable", "Name", "PreparationTime", "Price" },
                values: new object[,]
                {
                    { 1, 1, 0, true, "Spring Rolls", 10, 5.99m },
                    { 2, 2, 0, true, "Grilled Chicken", 20, 12.99m },
                    { 3, 3, 0, true, "Chocolate Cake", 5, 6.49m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
