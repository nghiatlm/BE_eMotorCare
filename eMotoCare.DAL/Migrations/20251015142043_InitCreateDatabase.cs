using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitCreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MaintenancePlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenancePlans", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PartTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServiceCenters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCenters", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MaintenanceStages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    maintenance_plan_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceStages_MaintenancePlans_maintenance_plan_id",
                        column: x => x.maintenance_plan_id,
                        principalTable: "MaintenancePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    maintenance_plan_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_MaintenancePlans_maintenance_plan_id",
                        column: x => x.maintenance_plan_id,
                        principalTable: "MaintenancePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_type_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parts_PartTypes_part_type_id",
                        column: x => x.part_type_id,
                        principalTable: "PartTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PriceServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_type_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceServices_PartTypes_part_type_id",
                        column: x => x.part_type_id,
                        principalTable: "PartTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ServiceCenterId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staffs_Accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Staffs_ServiceCenters_ServiceCenterId",
                        column: x => x.ServiceCenterId,
                        principalTable: "ServiceCenters",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ModelPartTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    model_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_type_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelPartTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelPartTypes_Models_model_id",
                        column: x => x.model_id,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelPartTypes_PartTypes_part_type_id",
                        column: x => x.part_type_id,
                        principalTable: "PartTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    model_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    customer_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vehicles_Models_model_id",
                        column: x => x.model_id,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CampaignDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    campaign_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignDetails_Campaigns_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignDetails_Parts_part_id",
                        column: x => x.part_id,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MaintenanceStageDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    maintenance_stage_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceStageDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceStageDetails_MaintenanceStages_maintenance_stage_~",
                        column: x => x.maintenance_stage_id,
                        principalTable: "MaintenanceStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceStageDetails_Parts_part_id",
                        column: x => x.part_id,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExportNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    export_by_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    service_center_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExportNotes_ServiceCenters_service_center_id",
                        column: x => x.service_center_id,
                        principalTable: "ServiceCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExportNotes_Staffs_export_by_id",
                        column: x => x.export_by_id,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ImportNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    import_by_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    service_center_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportNotes_ServiceCenters_service_center_id",
                        column: x => x.service_center_id,
                        principalTable: "ServiceCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportNotes_Staffs_import_by_id",
                        column: x => x.import_by_id,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RMAs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    create_by_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    customer_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RMAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RMAs_Customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RMAs_Staffs_create_by_id",
                        column: x => x.create_by_id,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VehicleStages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    maintenance_stage_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    vehicle_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleStages_MaintenanceStages_maintenance_stage_id",
                        column: x => x.maintenance_stage_id,
                        principalTable: "MaintenanceStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleStages_Vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    export_note_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    import_note_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartItems_ExportNotes_export_note_id",
                        column: x => x.export_note_id,
                        principalTable: "ExportNotes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PartItems_ImportNotes_import_note_id",
                        column: x => x.import_note_id,
                        principalTable: "ImportNotes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PartItems_Parts_part_id",
                        column: x => x.part_id,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    approve_by_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    customer_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    vehicle_stage_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    campaign_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Campaigns_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_Customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_Staffs_approve_by_id",
                        column: x => x.approve_by_id,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_VehicleStages_vehicle_stage_id",
                        column: x => x.vehicle_stage_id,
                        principalTable: "VehicleStages",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServiceCenterInventorys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    service_center_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_item_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCenterInventorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCenterInventorys_PartItems_part_item_id",
                        column: x => x.part_item_id,
                        principalTable: "PartItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceCenterInventorys_ServiceCenters_service_center_id",
                        column: x => x.service_center_id,
                        principalTable: "ServiceCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VehiclePartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    vehicle_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_item_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    replace_for_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclePartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehiclePartItems_PartItems_part_item_id",
                        column: x => x.part_item_id,
                        principalTable: "PartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehiclePartItems_PartItems_replace_for_id",
                        column: x => x.replace_for_id,
                        principalTable: "PartItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VehiclePartItems_Vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EVChecks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    appointment_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    task_executor_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EVChecks_Appointments_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EVChecks_Staffs_task_executor_id",
                        column: x => x.task_executor_id,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    appointment_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    customer_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Appointments_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EVCheckDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    maintenance_stage_detail_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    campaign_detail_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    part_item_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ev_check_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    replace_part_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVCheckDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EVCheckDetails_CampaignDetails_campaign_detail_id",
                        column: x => x.campaign_detail_id,
                        principalTable: "CampaignDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EVCheckDetails_EVChecks_ev_check_id",
                        column: x => x.ev_check_id,
                        principalTable: "EVChecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EVCheckDetails_MaintenanceStageDetails_maintenance_stage_det~",
                        column: x => x.maintenance_stage_detail_id,
                        principalTable: "MaintenanceStageDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EVCheckDetails_PartItems_part_item_id",
                        column: x => x.part_item_id,
                        principalTable: "PartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EVCheckDetails_PartItems_replace_part_id",
                        column: x => x.replace_part_id,
                        principalTable: "PartItems",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BatteryChecks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ev_check_detail_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_item_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatteryChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatteryChecks_EVCheckDetails_ev_check_detail_id",
                        column: x => x.ev_check_detail_id,
                        principalTable: "EVCheckDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BatteryChecks_PartItems_part_item_id",
                        column: x => x.part_item_id,
                        principalTable: "PartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RMADetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_item_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ev_check_detail_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    rma_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RMADetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RMADetails_EVCheckDetails_ev_check_detail_id",
                        column: x => x.ev_check_detail_id,
                        principalTable: "EVCheckDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RMADetails_PartItems_part_item_id",
                        column: x => x.part_item_id,
                        principalTable: "PartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RMADetails_RMAs_rma_id",
                        column: x => x.rma_id,
                        principalTable: "RMAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_approve_by_id",
                table: "Appointments",
                column: "approve_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_campaign_id",
                table: "Appointments",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_customer_id",
                table: "Appointments",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_vehicle_stage_id",
                table: "Appointments",
                column: "vehicle_stage_id");

            migrationBuilder.CreateIndex(
                name: "IX_BatteryChecks_ev_check_detail_id",
                table: "BatteryChecks",
                column: "ev_check_detail_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BatteryChecks_part_item_id",
                table: "BatteryChecks",
                column: "part_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDetails_campaign_id",
                table: "CampaignDetails",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDetails_part_id",
                table: "CampaignDetails",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_account_id",
                table: "Customers",
                column: "account_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EVCheckDetails_campaign_detail_id",
                table: "EVCheckDetails",
                column: "campaign_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_EVCheckDetails_ev_check_id",
                table: "EVCheckDetails",
                column: "ev_check_id");

            migrationBuilder.CreateIndex(
                name: "IX_EVCheckDetails_maintenance_stage_detail_id",
                table: "EVCheckDetails",
                column: "maintenance_stage_detail_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EVCheckDetails_part_item_id",
                table: "EVCheckDetails",
                column: "part_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_EVCheckDetails_replace_part_id",
                table: "EVCheckDetails",
                column: "replace_part_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EVChecks_appointment_id",
                table: "EVChecks",
                column: "appointment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EVChecks_task_executor_id",
                table: "EVChecks",
                column: "task_executor_id");

            migrationBuilder.CreateIndex(
                name: "IX_ExportNotes_export_by_id",
                table: "ExportNotes",
                column: "export_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_ExportNotes_service_center_id",
                table: "ExportNotes",
                column: "service_center_id");

            migrationBuilder.CreateIndex(
                name: "IX_ImportNotes_import_by_id",
                table: "ImportNotes",
                column: "import_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_ImportNotes_service_center_id",
                table: "ImportNotes",
                column: "service_center_id");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceStageDetails_maintenance_stage_id",
                table: "MaintenanceStageDetails",
                column: "maintenance_stage_id");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceStageDetails_part_id",
                table: "MaintenanceStageDetails",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceStages_maintenance_plan_id",
                table: "MaintenanceStages",
                column: "maintenance_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_ModelPartTypes_model_id",
                table: "ModelPartTypes",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_ModelPartTypes_part_type_id",
                table: "ModelPartTypes",
                column: "part_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Models_maintenance_plan_id",
                table: "Models",
                column: "maintenance_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_PartItems_export_note_id",
                table: "PartItems",
                column: "export_note_id");

            migrationBuilder.CreateIndex(
                name: "IX_PartItems_import_note_id",
                table: "PartItems",
                column: "import_note_id");

            migrationBuilder.CreateIndex(
                name: "IX_PartItems_part_id",
                table: "PartItems",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_part_type_id",
                table: "Parts",
                column: "part_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_appointment_id",
                table: "Payments",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_customer_id",
                table: "Payments",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_PriceServices_part_type_id",
                table: "PriceServices",
                column: "part_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_RMADetails_ev_check_detail_id",
                table: "RMADetails",
                column: "ev_check_detail_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RMADetails_part_item_id",
                table: "RMADetails",
                column: "part_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_RMADetails_rma_id",
                table: "RMADetails",
                column: "rma_id");

            migrationBuilder.CreateIndex(
                name: "IX_RMAs_create_by_id",
                table: "RMAs",
                column: "create_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_RMAs_customer_id",
                table: "RMAs",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterInventorys_part_item_id",
                table: "ServiceCenterInventorys",
                column: "part_item_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterInventorys_service_center_id",
                table: "ServiceCenterInventorys",
                column: "service_center_id");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_account_id",
                table: "Staffs",
                column: "account_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_ServiceCenterId",
                table: "Staffs",
                column: "ServiceCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePartItems_part_item_id",
                table: "VehiclePartItems",
                column: "part_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePartItems_replace_for_id",
                table: "VehiclePartItems",
                column: "replace_for_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePartItems_vehicle_id",
                table: "VehiclePartItems",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_customer_id",
                table: "Vehicles",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_model_id",
                table: "Vehicles",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleStages_maintenance_stage_id",
                table: "VehicleStages",
                column: "maintenance_stage_id");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleStages_vehicle_id",
                table: "VehicleStages",
                column: "vehicle_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatteryChecks");

            migrationBuilder.DropTable(
                name: "ModelPartTypes");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PriceServices");

            migrationBuilder.DropTable(
                name: "RMADetails");

            migrationBuilder.DropTable(
                name: "ServiceCenterInventorys");

            migrationBuilder.DropTable(
                name: "VehiclePartItems");

            migrationBuilder.DropTable(
                name: "EVCheckDetails");

            migrationBuilder.DropTable(
                name: "RMAs");

            migrationBuilder.DropTable(
                name: "CampaignDetails");

            migrationBuilder.DropTable(
                name: "EVChecks");

            migrationBuilder.DropTable(
                name: "MaintenanceStageDetails");

            migrationBuilder.DropTable(
                name: "PartItems");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "ExportNotes");

            migrationBuilder.DropTable(
                name: "ImportNotes");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "VehicleStages");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "PartTypes");

            migrationBuilder.DropTable(
                name: "MaintenanceStages");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "ServiceCenters");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "MaintenancePlans");
        }
    }
}
