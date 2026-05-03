using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGLogin.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "candidates",
                columns: table => new
                {
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    My_Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_candidates", x => x.CandidateId);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    City_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    City_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AreaName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.City_Id);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Login_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Login_Id);
                });

            migrationBuilder.CreateTable(
                name: "page_Masters",
                columns: table => new
                {
                    Page_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Page_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_page_Masters", x => x.Page_Id);
                });

            migrationBuilder.CreateTable(
                name: "registers",
                columns: table => new
                {
                    RegisterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Confirm_Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastPassword1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastPassword2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registers", x => x.RegisterID);
                });

            migrationBuilder.CreateTable(
                name: "role_Masters",
                columns: table => new
                {
                    Role_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Admin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_Masters", x => x.Role_Id);
                });

            migrationBuilder.CreateTable(
                name: "areas",
                columns: table => new
                {
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    City_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_areas", x => x.AreaId);
                    table.ForeignKey(
                        name: "FK_areas_cities_City_Id",
                        column: x => x.City_Id,
                        principalTable: "cities",
                        principalColumn: "City_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_Page_Mappers",
                columns: table => new
                {
                    Role_Page_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Page_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_Page_Mappers", x => x.Role_Page_ID);
                    table.ForeignKey(
                        name: "FK_role_Page_Mappers_page_Masters_Page_Id",
                        column: x => x.Page_Id,
                        principalTable: "page_Masters",
                        principalColumn: "Page_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_Page_Mappers_role_Masters_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "role_Masters",
                        principalColumn: "Role_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    User_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    User_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.User_Id);
                    table.ForeignKey(
                        name: "FK_users_role_Masters_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "role_Masters",
                        principalColumn: "Role_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pGs",
                columns: table => new
                {
                    PG_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PG_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SharingType = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pGs", x => x.PG_Id);
                    table.ForeignKey(
                        name: "FK_pGs_areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomNumber = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    RentPerMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PG_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_rooms_pGs_PG_Id",
                        column: x => x.PG_Id,
                        principalTable: "pGs",
                        principalColumn: "PG_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    SecurityDeposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Seats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_bookings_candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookings_rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_areas_City_Id",
                table: "areas",
                column: "City_Id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_CandidateId",
                table: "bookings",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_RoomId",
                table: "bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_pGs_AreaId",
                table: "pGs",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_role_Page_Mappers_Page_Id",
                table: "role_Page_Mappers",
                column: "Page_Id");

            migrationBuilder.CreateIndex(
                name: "IX_role_Page_Mappers_Role_Id",
                table: "role_Page_Mappers",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_PG_Id",
                table: "rooms",
                column: "PG_Id");

            migrationBuilder.CreateIndex(
                name: "IX_users_Role_Id",
                table: "users",
                column: "Role_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "registers");

            migrationBuilder.DropTable(
                name: "role_Page_Mappers");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "candidates");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "page_Masters");

            migrationBuilder.DropTable(
                name: "role_Masters");

            migrationBuilder.DropTable(
                name: "pGs");

            migrationBuilder.DropTable(
                name: "areas");

            migrationBuilder.DropTable(
                name: "cities");
        }
    }
}
