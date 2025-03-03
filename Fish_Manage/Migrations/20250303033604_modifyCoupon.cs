using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fish_Manage.Migrations
{
    /// <inheritdoc />
    public partial class modifyCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponPrice",
                table: "CouponModels");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CouponModels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CouponPrice",
                table: "CouponModels",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CouponModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
