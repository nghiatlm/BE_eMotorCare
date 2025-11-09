using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    phone = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role_ame = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.account_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "campaign",
                columns: table => new
                {
                    campaign_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campaign", x => x.campaign_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "maintenance_plan",
                columns: table => new
                {
                    maintenance_plan_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    unit = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    total_stages = table.Column<int>(type: "int", nullable: false),
                    effective_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenance_plan", x => x.maintenance_plan_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "part_type",
                columns: table => new
                {
                    part_type_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part_type", x => x.part_type_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "service_center",
                columns: table => new
                {
                    service_center_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    code = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(400)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    latitude = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    longitude = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_center", x => x.service_center_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    customer_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    customer_code = table.Column<string>(type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    first_name = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(400)", nullable: true),
                    citizen_id = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_of_birth = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    gender = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar_url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.customer_id);
                    table.ForeignKey(
                        name: "FK_customer_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "maintenance_stage",
                columns: table => new
                {
                    maintenance_stage_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    maintenance_plan_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mileage = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    duration_month = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estimated_time = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenance_stage", x => x.maintenance_stage_id);
                    table.ForeignKey(
                        name: "FK_maintenance_stage_maintenance_plan_maintenance_plan_id",
                        column: x => x.maintenance_plan_id,
                        principalTable: "maintenance_plan",
                        principalColumn: "maintenance_plan_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "model",
                columns: table => new
                {
                    model_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    code = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    manufacturer = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    maintenance_plan_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_model", x => x.model_id);
                    table.ForeignKey(
                        name: "FK_model_maintenance_plan_maintenance_plan_id",
                        column: x => x.maintenance_plan_id,
                        principalTable: "maintenance_plan",
                        principalColumn: "maintenance_plan_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "part",
                columns: table => new
                {
                    part_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_type_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part", x => x.part_id);
                    table.ForeignKey(
                        name: "FK_part_part_type_part_type_id",
                        column: x => x.part_type_id,
                        principalTable: "part_type",
                        principalColumn: "part_type_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "price_service",
                columns: table => new
                {
                    price_service_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_type_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    remedies = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    labor_cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    effective_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_price_service", x => x.price_service_id);
                    table.ForeignKey(
                        name: "FK_price_service_part_type_part_type_id",
                        column: x => x.part_type_id,
                        principalTable: "part_type",
                        principalColumn: "part_type_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "service_center_inventory",
                columns: table => new
                {
                    service_center_inventory_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    service_center_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_center_inventory", x => x.service_center_inventory_id);
                    table.ForeignKey(
                        name: "FK_service_center_inventory_service_center_service_center_id",
                        column: x => x.service_center_id,
                        principalTable: "service_center",
                        principalColumn: "service_center_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "service_center_slot",
                columns: table => new
                {
                    service_center_slot_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    service_center_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    day_of_week = table.Column<string>(type: "varchar(16)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    slot_time = table.Column<int>(type: "int", nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_center_slot", x => x.service_center_slot_id);
                    table.ForeignKey(
                        name: "FK_service_center_slot_service_center_service_center_id",
                        column: x => x.service_center_id,
                        principalTable: "service_center",
                        principalColumn: "service_center_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    staff_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    staff_code = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(400)", nullable: true),
                    citizen_id = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_of_birth = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    gender = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar_url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    position = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    service_center_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff", x => x.staff_id);
                    table.ForeignKey(
                        name: "FK_staff_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_staff_service_center_service_center_id",
                        column: x => x.service_center_id,
                        principalTable: "service_center",
                        principalColumn: "service_center_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "model_part_type",
                columns: table => new
                {
                    model_part_type_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    model_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_type_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_model_part_type", x => x.model_part_type_id);
                    table.ForeignKey(
                        name: "FK_model_part_type_model_model_id",
                        column: x => x.model_id,
                        principalTable: "model",
                        principalColumn: "model_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_model_part_type_part_type_part_type_id",
                        column: x => x.part_type_id,
                        principalTable: "part_type",
                        principalColumn: "part_type_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicle",
                columns: table => new
                {
                    vehicle_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    vin_number = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    color = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    chassis_number = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    engine_number = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    manufacture_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    purchase_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    warranty_expiry = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    model_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    customer_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle", x => x.vehicle_id);
                    table.ForeignKey(
                        name: "FK_vehicle_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "customer_id");
                    table.ForeignKey(
                        name: "FK_vehicle_model_model_id",
                        column: x => x.model_id,
                        principalTable: "model",
                        principalColumn: "model_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "campaign_detail",
                columns: table => new
                {
                    campaign_detail_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    campaign_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    part_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    action_type = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    note = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_mandatory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    estimated_time = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campaign_detail", x => x.campaign_detail_id);
                    table.ForeignKey(
                        name: "FK_campaign_detail_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "campaign",
                        principalColumn: "campaign_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_campaign_detail_part_part_id",
                        column: x => x.part_id,
                        principalTable: "part",
                        principalColumn: "part_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "maintenance_stage_detail",
                columns: table => new
                {
                    maintenance_stage_detail_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    maintenance_stage_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    action_type = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenance_stage_detail", x => x.maintenance_stage_detail_id);
                    table.ForeignKey(
                        name: "FK_maintenance_stage_detail_maintenance_stage_maintenance_stage~",
                        column: x => x.maintenance_stage_id,
                        principalTable: "maintenance_stage",
                        principalColumn: "maintenance_stage_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_maintenance_stage_detail_part_part_id",
                        column: x => x.part_id,
                        principalTable: "part",
                        principalColumn: "part_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "export_note",
                columns: table => new
                {
                    export_note_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    code = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    export_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    type = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    export_to = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    total_quantity = table.Column<int>(type: "int", nullable: false),
                    total_value = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    note = table.Column<string>(type: "nvarchar(400)", nullable: true),
                    export_by_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    service_center_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    export_note_status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_export_note", x => x.export_note_id);
                    table.ForeignKey(
                        name: "FK_export_note_service_center_service_center_id",
                        column: x => x.service_center_id,
                        principalTable: "service_center",
                        principalColumn: "service_center_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_export_note_staff_export_by_id",
                        column: x => x.export_by_id,
                        principalTable: "staff",
                        principalColumn: "staff_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "import_note",
                columns: table => new
                {
                    import_note_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    code = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    import_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    import_from = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    supplier = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    type = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    total_amout = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    import_by_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    service_center_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    import_note_status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_import_note", x => x.import_note_id);
                    table.ForeignKey(
                        name: "FK_import_note_service_center_service_center_id",
                        column: x => x.service_center_id,
                        principalTable: "service_center",
                        principalColumn: "service_center_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_import_note_staff_import_by_id",
                        column: x => x.import_by_id,
                        principalTable: "staff",
                        principalColumn: "staff_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rma",
                columns: table => new
                {
                    rma_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    code = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    rma_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    return_address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_by_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rma", x => x.rma_id);
                    table.ForeignKey(
                        name: "FK_rma_staff_create_by_id",
                        column: x => x.create_by_id,
                        principalTable: "staff",
                        principalColumn: "staff_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicle_stage",
                columns: table => new
                {
                    vehicle_stage_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    maintenance_stage_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    actual_maintenance_mileage = table.Column<int>(type: "int", nullable: false),
                    actual_maintenance_unit = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vehicle_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    date_of_implementation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_stage", x => x.vehicle_stage_id);
                    table.ForeignKey(
                        name: "FK_vehicle_stage_maintenance_stage_maintenance_stage_id",
                        column: x => x.maintenance_stage_id,
                        principalTable: "maintenance_stage",
                        principalColumn: "maintenance_stage_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vehicle_stage_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicle",
                        principalColumn: "vehicle_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "part_item",
                columns: table => new
                {
                    part_item_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    export_note_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    import_note_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    serial_number = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    warranty_period = table.Column<int>(type: "int", nullable: true),
                    waranty_start_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    waranty_end_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    service_center_inventory_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part_item", x => x.part_item_id);
                    table.ForeignKey(
                        name: "FK_part_item_export_note_export_note_id",
                        column: x => x.export_note_id,
                        principalTable: "export_note",
                        principalColumn: "export_note_id");
                    table.ForeignKey(
                        name: "FK_part_item_import_note_import_note_id",
                        column: x => x.import_note_id,
                        principalTable: "import_note",
                        principalColumn: "import_note_id");
                    table.ForeignKey(
                        name: "FK_part_item_part_part_id",
                        column: x => x.part_id,
                        principalTable: "part",
                        principalColumn: "part_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_part_item_service_center_inventory_service_center_inventory_~",
                        column: x => x.service_center_inventory_id,
                        principalTable: "service_center_inventory",
                        principalColumn: "service_center_inventory_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "appointment",
                columns: table => new
                {
                    appointment_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    approve_by_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    service_center_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    customer_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    vehicle_stage_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    campaign_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    appointment_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    slot_time = table.Column<int>(type: "int", nullable: false),
                    estimated_cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    actual_cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    checkin_qr_code = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointment", x => x.appointment_id);
                    table.ForeignKey(
                        name: "FK_appointment_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "campaign",
                        principalColumn: "campaign_id");
                    table.ForeignKey(
                        name: "FK_appointment_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointment_service_center_service_center_id",
                        column: x => x.service_center_id,
                        principalTable: "service_center",
                        principalColumn: "service_center_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointment_staff_approve_by_id",
                        column: x => x.approve_by_id,
                        principalTable: "staff",
                        principalColumn: "staff_id");
                    table.ForeignKey(
                        name: "FK_appointment_vehicle_stage_vehicle_stage_id",
                        column: x => x.vehicle_stage_id,
                        principalTable: "vehicle_stage",
                        principalColumn: "vehicle_stage_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicle_part_item",
                columns: table => new
                {
                    vehicle_part_item_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    install_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_item_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    replace_for_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_part_item", x => x.vehicle_part_item_id);
                    table.ForeignKey(
                        name: "FK_vehicle_part_item_part_item_part_item_id",
                        column: x => x.part_item_id,
                        principalTable: "part_item",
                        principalColumn: "part_item_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vehicle_part_item_part_item_replace_for_id",
                        column: x => x.replace_for_id,
                        principalTable: "part_item",
                        principalColumn: "part_item_id");
                    table.ForeignKey(
                        name: "FK_vehicle_part_item_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicle",
                        principalColumn: "vehicle_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ev_check",
                columns: table => new
                {
                    ev_check_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    check_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    total_amout = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    odometer = table.Column<int>(type: "int", nullable: true),
                    appointment_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    task_executor_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ev_check", x => x.ev_check_id);
                    table.ForeignKey(
                        name: "FK_ev_check_appointment_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "appointment",
                        principalColumn: "appointment_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ev_check_staff_task_executor_id",
                        column: x => x.task_executor_id,
                        principalTable: "staff",
                        principalColumn: "staff_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payment",
                columns: table => new
                {
                    payment_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    transaction_code = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payment_method = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    appointment_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    amount = table.Column<double>(type: "double", nullable: false),
                    currency = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_payment_appointment_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "appointment",
                        principalColumn: "appointment_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ev_check_detail",
                columns: table => new
                {
                    ev_check_detail_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    maintenance_stage_detail_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    campaign_detail_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    part_item_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ev_check_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    replace_part_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    result = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    remedies = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    unit = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    price_part = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    price_service = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ev_check_detail", x => x.ev_check_detail_id);
                    table.ForeignKey(
                        name: "FK_ev_check_detail_campaign_detail_campaign_detail_id",
                        column: x => x.campaign_detail_id,
                        principalTable: "campaign_detail",
                        principalColumn: "campaign_detail_id");
                    table.ForeignKey(
                        name: "FK_ev_check_detail_ev_check_ev_check_id",
                        column: x => x.ev_check_id,
                        principalTable: "ev_check",
                        principalColumn: "ev_check_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ev_check_detail_maintenance_stage_detail_maintenance_stage_d~",
                        column: x => x.maintenance_stage_detail_id,
                        principalTable: "maintenance_stage_detail",
                        principalColumn: "maintenance_stage_detail_id");
                    table.ForeignKey(
                        name: "FK_ev_check_detail_part_item_part_item_id",
                        column: x => x.part_item_id,
                        principalTable: "part_item",
                        principalColumn: "part_item_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ev_check_detail_part_item_replace_part_id",
                        column: x => x.replace_part_id,
                        principalTable: "part_item",
                        principalColumn: "part_item_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "battery_check",
                columns: table => new
                {
                    battery_check_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    time = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    voltage = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    current = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    power = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    capacity = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    energy = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    temp = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    soc = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    soh = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    solution = table.Column<string>(type: "nvarchar(400)", nullable: true),
                    ev_check_detail_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_battery_check", x => x.battery_check_id);
                    table.ForeignKey(
                        name: "FK_battery_check_ev_check_detail_ev_check_detail_id",
                        column: x => x.ev_check_detail_id,
                        principalTable: "ev_check_detail",
                        principalColumn: "ev_check_detail_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rma_detail",
                columns: table => new
                {
                    rma_detail_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(400)", nullable: true),
                    rma_number = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    release_date_rma = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    expiration_date_rma = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    inspector = table.Column<string>(type: "nvarchar(400)", nullable: true),
                    result = table.Column<string>(type: "nvarchar(400)", nullable: true),
                    solution = table.Column<string>(type: "nvarchar(400)", nullable: true),
                    ev_check_detail_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    rma_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rma_detail", x => x.rma_detail_id);
                    table.ForeignKey(
                        name: "FK_rma_detail_ev_check_detail_ev_check_detail_id",
                        column: x => x.ev_check_detail_id,
                        principalTable: "ev_check_detail",
                        principalColumn: "ev_check_detail_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rma_detail_rma_rma_id",
                        column: x => x.rma_id,
                        principalTable: "rma",
                        principalColumn: "rma_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_approve_by_id",
                table: "appointment",
                column: "approve_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_campaign_id",
                table: "appointment",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_customer_id",
                table: "appointment",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_service_center_id",
                table: "appointment",
                column: "service_center_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_vehicle_stage_id",
                table: "appointment",
                column: "vehicle_stage_id");

            migrationBuilder.CreateIndex(
                name: "IX_battery_check_ev_check_detail_id",
                table: "battery_check",
                column: "ev_check_detail_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_campaign_detail_campaign_id",
                table: "campaign_detail",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_campaign_detail_part_id",
                table: "campaign_detail",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_account_id",
                table: "customer",
                column: "account_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ev_check_appointment_id",
                table: "ev_check",
                column: "appointment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ev_check_task_executor_id",
                table: "ev_check",
                column: "task_executor_id");

            migrationBuilder.CreateIndex(
                name: "IX_ev_check_detail_campaign_detail_id",
                table: "ev_check_detail",
                column: "campaign_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_ev_check_detail_ev_check_id",
                table: "ev_check_detail",
                column: "ev_check_id");

            migrationBuilder.CreateIndex(
                name: "IX_ev_check_detail_maintenance_stage_detail_id",
                table: "ev_check_detail",
                column: "maintenance_stage_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_ev_check_detail_part_item_id",
                table: "ev_check_detail",
                column: "part_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_ev_check_detail_replace_part_id",
                table: "ev_check_detail",
                column: "replace_part_id");

            migrationBuilder.CreateIndex(
                name: "IX_export_note_export_by_id",
                table: "export_note",
                column: "export_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_export_note_service_center_id",
                table: "export_note",
                column: "service_center_id");

            migrationBuilder.CreateIndex(
                name: "IX_import_note_import_by_id",
                table: "import_note",
                column: "import_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_import_note_service_center_id",
                table: "import_note",
                column: "service_center_id");

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_stage_maintenance_plan_id",
                table: "maintenance_stage",
                column: "maintenance_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_stage_detail_maintenance_stage_id",
                table: "maintenance_stage_detail",
                column: "maintenance_stage_id");

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_stage_detail_part_id",
                table: "maintenance_stage_detail",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_model_maintenance_plan_id",
                table: "model",
                column: "maintenance_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_model_part_type_model_id",
                table: "model_part_type",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_model_part_type_part_type_id",
                table: "model_part_type",
                column: "part_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_part_type_id",
                table: "part",
                column: "part_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_item_export_note_id",
                table: "part_item",
                column: "export_note_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_item_import_note_id",
                table: "part_item",
                column: "import_note_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_item_part_id",
                table: "part_item",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_item_service_center_inventory_id",
                table: "part_item",
                column: "service_center_inventory_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_appointment_id",
                table: "payment",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "IX_price_service_part_type_id",
                table: "price_service",
                column: "part_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_rma_create_by_id",
                table: "rma",
                column: "create_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_rma_detail_ev_check_detail_id",
                table: "rma_detail",
                column: "ev_check_detail_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rma_detail_rma_id",
                table: "rma_detail",
                column: "rma_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_center_inventory_service_center_id",
                table: "service_center_inventory",
                column: "service_center_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_center_slot_service_center_id",
                table: "service_center_slot",
                column: "service_center_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_account_id",
                table: "staff",
                column: "account_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staff_service_center_id",
                table: "staff",
                column: "service_center_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_customer_id",
                table: "vehicle",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_model_id",
                table: "vehicle",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_part_item_part_item_id",
                table: "vehicle_part_item",
                column: "part_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_part_item_replace_for_id",
                table: "vehicle_part_item",
                column: "replace_for_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_part_item_vehicle_id",
                table: "vehicle_part_item",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_stage_maintenance_stage_id",
                table: "vehicle_stage",
                column: "maintenance_stage_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_stage_vehicle_id",
                table: "vehicle_stage",
                column: "vehicle_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "battery_check");

            migrationBuilder.DropTable(
                name: "model_part_type");

            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropTable(
                name: "price_service");

            migrationBuilder.DropTable(
                name: "rma_detail");

            migrationBuilder.DropTable(
                name: "service_center_slot");

            migrationBuilder.DropTable(
                name: "vehicle_part_item");

            migrationBuilder.DropTable(
                name: "ev_check_detail");

            migrationBuilder.DropTable(
                name: "rma");

            migrationBuilder.DropTable(
                name: "campaign_detail");

            migrationBuilder.DropTable(
                name: "ev_check");

            migrationBuilder.DropTable(
                name: "maintenance_stage_detail");

            migrationBuilder.DropTable(
                name: "part_item");

            migrationBuilder.DropTable(
                name: "appointment");

            migrationBuilder.DropTable(
                name: "export_note");

            migrationBuilder.DropTable(
                name: "import_note");

            migrationBuilder.DropTable(
                name: "part");

            migrationBuilder.DropTable(
                name: "service_center_inventory");

            migrationBuilder.DropTable(
                name: "campaign");

            migrationBuilder.DropTable(
                name: "vehicle_stage");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "part_type");

            migrationBuilder.DropTable(
                name: "maintenance_stage");

            migrationBuilder.DropTable(
                name: "vehicle");

            migrationBuilder.DropTable(
                name: "service_center");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "model");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "maintenance_plan");
        }
    }
}
