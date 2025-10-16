using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDatebase_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Campaigns_campaign_id",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Customers_customer_id",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Staffs_approve_by_id",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_VehicleStages_vehicle_stage_id",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_BatteryChecks_EVCheckDetails_ev_check_detail_id",
                table: "BatteryChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_BatteryChecks_PartItems_part_item_id",
                table: "BatteryChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignDetails_Campaigns_campaign_id",
                table: "CampaignDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignDetails_Parts_part_id",
                table: "CampaignDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Accounts_account_id",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_EVCheckDetails_CampaignDetails_campaign_detail_id",
                table: "EVCheckDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EVCheckDetails_EVChecks_ev_check_id",
                table: "EVCheckDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EVCheckDetails_MaintenanceStageDetails_maintenance_stage_det~",
                table: "EVCheckDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EVCheckDetails_PartItems_part_item_id",
                table: "EVCheckDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EVCheckDetails_PartItems_replace_part_id",
                table: "EVCheckDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EVChecks_Appointments_appointment_id",
                table: "EVChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_EVChecks_Staffs_task_executor_id",
                table: "EVChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_ExportNotes_ServiceCenters_service_center_id",
                table: "ExportNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ExportNotes_Staffs_export_by_id",
                table: "ExportNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportNotes_ServiceCenters_service_center_id",
                table: "ImportNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportNotes_Staffs_import_by_id",
                table: "ImportNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceStageDetails_MaintenanceStages_maintenance_stage_~",
                table: "MaintenanceStageDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceStageDetails_Parts_part_id",
                table: "MaintenanceStageDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceStages_MaintenancePlans_maintenance_plan_id",
                table: "MaintenanceStages");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelPartTypes_Models_model_id",
                table: "ModelPartTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelPartTypes_PartTypes_part_type_id",
                table: "ModelPartTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Models_MaintenancePlans_maintenance_plan_id",
                table: "Models");

            migrationBuilder.DropForeignKey(
                name: "FK_PartItems_ExportNotes_export_note_id",
                table: "PartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PartItems_ImportNotes_import_note_id",
                table: "PartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PartItems_Parts_part_id",
                table: "PartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Parts_PartTypes_part_type_id",
                table: "Parts");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Appointments_appointment_id",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Customers_customer_id",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceServices_PartTypes_part_type_id",
                table: "PriceServices");

            migrationBuilder.DropForeignKey(
                name: "FK_RMADetails_EVCheckDetails_ev_check_detail_id",
                table: "RMADetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RMADetails_PartItems_part_item_id",
                table: "RMADetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RMADetails_RMAs_rma_id",
                table: "RMADetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RMAs_Customers_customer_id",
                table: "RMAs");

            migrationBuilder.DropForeignKey(
                name: "FK_RMAs_Staffs_create_by_id",
                table: "RMAs");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCenterInventorys_PartItems_part_item_id",
                table: "ServiceCenterInventorys");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCenterInventorys_ServiceCenters_service_center_id",
                table: "ServiceCenterInventorys");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Accounts_account_id",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_ServiceCenters_ServiceCenterId",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_VehiclePartItems_PartItems_part_item_id",
                table: "VehiclePartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_VehiclePartItems_PartItems_replace_for_id",
                table: "VehiclePartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_VehiclePartItems_Vehicles_vehicle_id",
                table: "VehiclePartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Customers_customer_id",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Models_model_id",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleStages_MaintenanceStages_maintenance_stage_id",
                table: "VehicleStages");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleStages_Vehicles_vehicle_id",
                table: "VehicleStages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleStages",
                table: "VehicleStages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehiclePartItems",
                table: "VehiclePartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Staffs",
                table: "Staffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceCenters",
                table: "ServiceCenters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceCenterInventorys",
                table: "ServiceCenterInventorys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RMAs",
                table: "RMAs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RMADetails",
                table: "RMADetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceServices",
                table: "PriceServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartTypes",
                table: "PartTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parts",
                table: "Parts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartItems",
                table: "PartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Models",
                table: "Models");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelPartTypes",
                table: "ModelPartTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaintenanceStages",
                table: "MaintenanceStages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaintenanceStageDetails",
                table: "MaintenanceStageDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaintenancePlans",
                table: "MaintenancePlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImportNotes",
                table: "ImportNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExportNotes",
                table: "ExportNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EVChecks",
                table: "EVChecks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EVCheckDetails",
                table: "EVCheckDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaigns",
                table: "Campaigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignDetails",
                table: "CampaignDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BatteryChecks",
                table: "BatteryChecks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "VehicleStages",
                newName: "vehicle_stage");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "vehicle");

            migrationBuilder.RenameTable(
                name: "VehiclePartItems",
                newName: "vehicle_part_item");

            migrationBuilder.RenameTable(
                name: "Staffs",
                newName: "staff");

            migrationBuilder.RenameTable(
                name: "ServiceCenters",
                newName: "service_center");

            migrationBuilder.RenameTable(
                name: "ServiceCenterInventorys",
                newName: "service_center_inventory");

            migrationBuilder.RenameTable(
                name: "RMAs",
                newName: "rma");

            migrationBuilder.RenameTable(
                name: "RMADetails",
                newName: "rma_detail");

            migrationBuilder.RenameTable(
                name: "PriceServices",
                newName: "price_service");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "payment");

            migrationBuilder.RenameTable(
                name: "PartTypes",
                newName: "part_type");

            migrationBuilder.RenameTable(
                name: "Parts",
                newName: "part");

            migrationBuilder.RenameTable(
                name: "PartItems",
                newName: "part_item");

            migrationBuilder.RenameTable(
                name: "Models",
                newName: "model");

            migrationBuilder.RenameTable(
                name: "ModelPartTypes",
                newName: "model_part_type");

            migrationBuilder.RenameTable(
                name: "MaintenanceStages",
                newName: "maintenance_stage");

            migrationBuilder.RenameTable(
                name: "MaintenanceStageDetails",
                newName: "maintenance_stage_detail");

            migrationBuilder.RenameTable(
                name: "MaintenancePlans",
                newName: "maintenance_plan");

            migrationBuilder.RenameTable(
                name: "ImportNotes",
                newName: "import_note");

            migrationBuilder.RenameTable(
                name: "ExportNotes",
                newName: "export_not");

            migrationBuilder.RenameTable(
                name: "EVChecks",
                newName: "ev_check");

            migrationBuilder.RenameTable(
                name: "EVCheckDetails",
                newName: "ev_check_detail");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "customer");

            migrationBuilder.RenameTable(
                name: "Campaigns",
                newName: "campaign");

            migrationBuilder.RenameTable(
                name: "CampaignDetails",
                newName: "campaign_detail");

            migrationBuilder.RenameTable(
                name: "BatteryChecks",
                newName: "battery_check");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "appointment");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "account");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "vehicle_stage",
                newName: "vehicle_stage_id");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleStages_vehicle_id",
                table: "vehicle_stage",
                newName: "IX_vehicle_stage_vehicle_id");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleStages_maintenance_stage_id",
                table: "vehicle_stage",
                newName: "IX_vehicle_stage_maintenance_stage_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "vehicle",
                newName: "vehicle_id");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_model_id",
                table: "vehicle",
                newName: "IX_vehicle_model_id");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_customer_id",
                table: "vehicle",
                newName: "IX_vehicle_customer_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "vehicle_part_item",
                newName: "vehicle_part_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_VehiclePartItems_vehicle_id",
                table: "vehicle_part_item",
                newName: "IX_vehicle_part_item_vehicle_id");

            migrationBuilder.RenameIndex(
                name: "IX_VehiclePartItems_replace_for_id",
                table: "vehicle_part_item",
                newName: "IX_vehicle_part_item_replace_for_id");

            migrationBuilder.RenameIndex(
                name: "IX_VehiclePartItems_part_item_id",
                table: "vehicle_part_item",
                newName: "IX_vehicle_part_item_part_item_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "staff",
                newName: "staff_id");

            migrationBuilder.RenameIndex(
                name: "IX_Staffs_ServiceCenterId",
                table: "staff",
                newName: "IX_staff_ServiceCenterId");

            migrationBuilder.RenameIndex(
                name: "IX_Staffs_account_id",
                table: "staff",
                newName: "IX_staff_account_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "service_center",
                newName: "service_center_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "service_center_inventory",
                newName: "service_center_inventory_id");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceCenterInventorys_service_center_id",
                table: "service_center_inventory",
                newName: "IX_service_center_inventory_service_center_id");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceCenterInventorys_part_item_id",
                table: "service_center_inventory",
                newName: "IX_service_center_inventory_part_item_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "rma",
                newName: "rma_id");

            migrationBuilder.RenameIndex(
                name: "IX_RMAs_customer_id",
                table: "rma",
                newName: "IX_rma_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_RMAs_create_by_id",
                table: "rma",
                newName: "IX_rma_create_by_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "rma_detail",
                newName: "rma_detail_id");

            migrationBuilder.RenameIndex(
                name: "IX_RMADetails_rma_id",
                table: "rma_detail",
                newName: "IX_rma_detail_rma_id");

            migrationBuilder.RenameIndex(
                name: "IX_RMADetails_part_item_id",
                table: "rma_detail",
                newName: "IX_rma_detail_part_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_RMADetails_ev_check_detail_id",
                table: "rma_detail",
                newName: "IX_rma_detail_ev_check_detail_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "price_service",
                newName: "price_service_id");

            migrationBuilder.RenameIndex(
                name: "IX_PriceServices_part_type_id",
                table: "price_service",
                newName: "IX_price_service_part_type_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "payment",
                newName: "payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_customer_id",
                table: "payment",
                newName: "IX_payment_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_appointment_id",
                table: "payment",
                newName: "IX_payment_appointment_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "part_type",
                newName: "part_type_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "part",
                newName: "part_id");

            migrationBuilder.RenameIndex(
                name: "IX_Parts_part_type_id",
                table: "part",
                newName: "IX_part_part_type_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "part_item",
                newName: "part_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_PartItems_part_id",
                table: "part_item",
                newName: "IX_part_item_part_id");

            migrationBuilder.RenameIndex(
                name: "IX_PartItems_import_note_id",
                table: "part_item",
                newName: "IX_part_item_import_note_id");

            migrationBuilder.RenameIndex(
                name: "IX_PartItems_export_note_id",
                table: "part_item",
                newName: "IX_part_item_export_note_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "model",
                newName: "model_id");

            migrationBuilder.RenameIndex(
                name: "IX_Models_maintenance_plan_id",
                table: "model",
                newName: "IX_model_maintenance_plan_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "model_part_type",
                newName: "model_part_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_ModelPartTypes_part_type_id",
                table: "model_part_type",
                newName: "IX_model_part_type_part_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_ModelPartTypes_model_id",
                table: "model_part_type",
                newName: "IX_model_part_type_model_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "maintenance_stage",
                newName: "maintenance_stage_id");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceStages_maintenance_plan_id",
                table: "maintenance_stage",
                newName: "IX_maintenance_stage_maintenance_plan_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "maintenance_stage_detail",
                newName: "maintenance_stage_detail_id");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceStageDetails_part_id",
                table: "maintenance_stage_detail",
                newName: "IX_maintenance_stage_detail_part_id");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceStageDetails_maintenance_stage_id",
                table: "maintenance_stage_detail",
                newName: "IX_maintenance_stage_detail_maintenance_stage_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "maintenance_plan",
                newName: "maintenance_plan_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "import_note",
                newName: "import_note_id");

            migrationBuilder.RenameIndex(
                name: "IX_ImportNotes_service_center_id",
                table: "import_note",
                newName: "IX_import_note_service_center_id");

            migrationBuilder.RenameIndex(
                name: "IX_ImportNotes_import_by_id",
                table: "import_note",
                newName: "IX_import_note_import_by_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "export_not",
                newName: "export_note_id");

            migrationBuilder.RenameIndex(
                name: "IX_ExportNotes_service_center_id",
                table: "export_not",
                newName: "IX_export_not_service_center_id");

            migrationBuilder.RenameIndex(
                name: "IX_ExportNotes_export_by_id",
                table: "export_not",
                newName: "IX_export_not_export_by_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ev_check",
                newName: "ev_check_id");

            migrationBuilder.RenameIndex(
                name: "IX_EVChecks_task_executor_id",
                table: "ev_check",
                newName: "IX_ev_check_task_executor_id");

            migrationBuilder.RenameIndex(
                name: "IX_EVChecks_appointment_id",
                table: "ev_check",
                newName: "IX_ev_check_appointment_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ev_check_detail",
                newName: "ev_check_detail_id");

            migrationBuilder.RenameIndex(
                name: "IX_EVCheckDetails_replace_part_id",
                table: "ev_check_detail",
                newName: "IX_ev_check_detail_replace_part_id");

            migrationBuilder.RenameIndex(
                name: "IX_EVCheckDetails_part_item_id",
                table: "ev_check_detail",
                newName: "IX_ev_check_detail_part_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_EVCheckDetails_maintenance_stage_detail_id",
                table: "ev_check_detail",
                newName: "IX_ev_check_detail_maintenance_stage_detail_id");

            migrationBuilder.RenameIndex(
                name: "IX_EVCheckDetails_ev_check_id",
                table: "ev_check_detail",
                newName: "IX_ev_check_detail_ev_check_id");

            migrationBuilder.RenameIndex(
                name: "IX_EVCheckDetails_campaign_detail_id",
                table: "ev_check_detail",
                newName: "IX_ev_check_detail_campaign_detail_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "customer",
                newName: "customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_account_id",
                table: "customer",
                newName: "IX_customer_account_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "campaign",
                newName: "campaign_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "campaign_detail",
                newName: "campaign_detail_id");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignDetails_part_id",
                table: "campaign_detail",
                newName: "IX_campaign_detail_part_id");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignDetails_campaign_id",
                table: "campaign_detail",
                newName: "IX_campaign_detail_campaign_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "battery_check",
                newName: "battery_check_id");

            migrationBuilder.RenameIndex(
                name: "IX_BatteryChecks_part_item_id",
                table: "battery_check",
                newName: "IX_battery_check_part_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_BatteryChecks_ev_check_detail_id",
                table: "battery_check",
                newName: "IX_battery_check_ev_check_detail_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "appointment",
                newName: "appointment_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_vehicle_stage_id",
                table: "appointment",
                newName: "IX_appointment_vehicle_stage_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_customer_id",
                table: "appointment",
                newName: "IX_appointment_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_campaign_id",
                table: "appointment",
                newName: "IX_appointment_campaign_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_approve_by_id",
                table: "appointment",
                newName: "IX_appointment_approve_by_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "account",
                newName: "account_id");

            migrationBuilder.AddColumn<DateTime>(
                name: "actual_maintenance_mileage",
                table: "vehicle_stage",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_of_implementation",
                table: "vehicle_stage",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "vehicle_stage",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "vehicle",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "vehicle",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "chassis_number",
                table: "vehicle",
                type: "nvarchar(300)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "vehicle",
                type: "nvarchar(300)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "engine_number",
                table: "vehicle",
                type: "nvarchar(300)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "vehicle",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "manufacture_date",
                table: "vehicle",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "purchase_date",
                table: "vehicle",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "vehicle",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "vin_number",
                table: "vehicle",
                type: "nvarchar(300)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "warranty_expiry",
                table: "vehicle",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "vehicle_part_item",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "vehicle_part_item",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "install_date",
                table: "vehicle_part_item",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "staff",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "staff",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "staff",
                type: "nvarchar(400)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                table: "staff",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "citizen_id",
                table: "staff",
                type: "varchar(15)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "date_of_birth",
                table: "staff",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "staff",
                type: "nvarchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "staff",
                type: "varchar(200)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "staff",
                type: "nvarchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "position",
                table: "staff",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "staff_code",
                table: "staff",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "service_center",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "service_center",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "service_center",
                type: "nvarchar(500)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "service_center",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "service_center",
                type: "nvarchar(400)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "service_center",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "latitude",
                table: "service_center",
                type: "nvarchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "longitude",
                table: "service_center",
                type: "nvarchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "service_center",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "service_center",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "service_center",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "rma",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "rma",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "rma",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "rma",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "return_address",
                table: "rma",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "rma_date",
                table: "rma",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "rma",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "rma_detail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "rma_detail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "expiration_date_rma",
                table: "rma_detail",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "inspector",
                table: "rma_detail",
                type: "nvarchar(400)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "rma_detail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "reason",
                table: "rma_detail",
                type: "nvarchar(400)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "release_date_rma",
                table: "rma_detail",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "result",
                table: "rma_detail",
                type: "nvarchar(400)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rma_number",
                table: "rma_detail",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "solution",
                table: "rma_detail",
                type: "nvarchar(400)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "price_service",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "price_service",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "effective_date",
                table: "price_service",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "labor_cost",
                table: "price_service",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "price_service",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "price_service",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "remedies",
                table: "price_service",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "amount",
                table: "payment",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "payment",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "payment_method",
                table: "payment",
                type: "varchar(150)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "payment",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "transaction_code",
                table: "payment",
                type: "varchar(150)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "part_type",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "part_type",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "part",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "part",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "part",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "part",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "part",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "export_id",
                table: "part_item",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "part_item",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "part_item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "serial_number",
                table: "part_item",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "part_item",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "warranty_period",
                table: "part_item",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "model",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "manufacturer",
                table: "model",
                type: "nvarchar(300)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "model",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "maintenance_stage",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "duration_month",
                table: "maintenance_stage",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "estimated_time",
                table: "maintenance_stage",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mileage",
                table: "maintenance_stage",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "maintenance_stage",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "maintenance_stage",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "action_type",
                table: "maintenance_stage_detail",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "maintenance_stage_detail",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "maintenance_plan",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "maintenance_plan",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "effective_date",
                table: "maintenance_plan",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "maintenance_plan",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "maintenance_plan",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "total_stages",
                table: "maintenance_plan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "unit",
                table: "maintenance_plan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "import_note",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "import_note",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "import_note",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "import_date",
                table: "import_note",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "import_from",
                table: "import_note",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "supplier",
                table: "import_note",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_amout",
                table: "import_note",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "import_note",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "export_not",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "export_not",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "export_not",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "export_date",
                table: "export_not",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "export_to",
                table: "export_not",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "export_not",
                type: "nvarchar(400)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_quantity",
                table: "export_not",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "total_value",
                table: "export_not",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "export_not",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ev_check",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ev_check",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "check_date",
                table: "ev_check",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "odometer",
                table: "ev_check",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "ev_check",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "total_amout",
                table: "ev_check",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "price_part",
                table: "ev_check_detail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "price_service",
                table: "ev_check_detail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "quantity",
                table: "ev_check_detail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "remedies",
                table: "ev_check_detail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "result",
                table: "ev_check_detail",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "ev_check_detail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount",
                table: "ev_check_detail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "ev_check_detail",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "customer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "customer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "customer",
                type: "nvarchar(400)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                table: "customer",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "citizen_id",
                table: "customer",
                type: "varchar(15)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "date_of_birth",
                table: "customer",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "customer",
                type: "nvarchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "customer",
                type: "varchar(200)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "customer",
                type: "nvarchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "campaign",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "campaign",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "end_date",
                table: "campaign",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "campaign",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "start_date",
                table: "campaign",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "campaign",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "campaign",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "campaign_detail",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "EVCheckDetailId",
                table: "campaign_detail",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "action_type",
                table: "campaign_detail",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "estimated_time",
                table: "campaign_detail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_mandatory",
                table: "campaign_detail",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "campaign_detail",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "battery_check",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "battery_check",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "capacity",
                table: "battery_check",
                type: "json",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "current",
                table: "battery_check",
                type: "json",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "energy",
                table: "battery_check",
                type: "json",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "power",
                table: "battery_check",
                type: "json",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "soc",
                table: "battery_check",
                type: "json",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "soh",
                table: "battery_check",
                type: "json",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "solution",
                table: "battery_check",
                type: "nvarchar(400)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "temp",
                table: "battery_check",
                type: "json",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "time",
                table: "battery_check",
                type: "json",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "voltage",
                table: "battery_check",
                type: "json",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "appointment",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<decimal>(
                name: "actual_cost",
                table: "appointment",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "appointment_date",
                table: "appointment",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "appointment",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "estimated_cost",
                table: "appointment",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "service_center_id",
                table: "appointment",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "appointment",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "time_slot",
                table: "appointment",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "appointment",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "account",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "account",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "account",
                type: "varchar(15)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "role_ame",
                table: "account",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "account",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vehicle_stage",
                table: "vehicle_stage",
                column: "vehicle_stage_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vehicle",
                table: "vehicle",
                column: "vehicle_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vehicle_part_item",
                table: "vehicle_part_item",
                column: "vehicle_part_item_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_staff",
                table: "staff",
                column: "staff_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_service_center",
                table: "service_center",
                column: "service_center_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_service_center_inventory",
                table: "service_center_inventory",
                column: "service_center_inventory_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rma",
                table: "rma",
                column: "rma_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rma_detail",
                table: "rma_detail",
                column: "rma_detail_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_price_service",
                table: "price_service",
                column: "price_service_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_payment",
                table: "payment",
                column: "payment_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_part_type",
                table: "part_type",
                column: "part_type_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_part",
                table: "part",
                column: "part_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_part_item",
                table: "part_item",
                column: "part_item_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_model",
                table: "model",
                column: "model_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_model_part_type",
                table: "model_part_type",
                column: "model_part_type_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_maintenance_stage",
                table: "maintenance_stage",
                column: "maintenance_stage_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_maintenance_stage_detail",
                table: "maintenance_stage_detail",
                column: "maintenance_stage_detail_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_maintenance_plan",
                table: "maintenance_plan",
                column: "maintenance_plan_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_import_note",
                table: "import_note",
                column: "import_note_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_export_not",
                table: "export_not",
                column: "export_note_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ev_check",
                table: "ev_check",
                column: "ev_check_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ev_check_detail",
                table: "ev_check_detail",
                column: "ev_check_detail_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customer",
                table: "customer",
                column: "customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_campaign",
                table: "campaign",
                column: "campaign_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_campaign_detail",
                table: "campaign_detail",
                column: "campaign_detail_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_battery_check",
                table: "battery_check",
                column: "battery_check_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_appointment",
                table: "appointment",
                column: "appointment_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_account",
                table: "account",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_service_center_id",
                table: "appointment",
                column: "service_center_id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointment_campaign_campaign_id",
                table: "appointment",
                column: "campaign_id",
                principalTable: "campaign",
                principalColumn: "campaign_id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointment_customer_customer_id",
                table: "appointment",
                column: "customer_id",
                principalTable: "customer",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_appointment_service_center_service_center_id",
                table: "appointment",
                column: "service_center_id",
                principalTable: "service_center",
                principalColumn: "service_center_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_appointment_staff_approve_by_id",
                table: "appointment",
                column: "approve_by_id",
                principalTable: "staff",
                principalColumn: "staff_id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointment_vehicle_stage_vehicle_stage_id",
                table: "appointment",
                column: "vehicle_stage_id",
                principalTable: "vehicle_stage",
                principalColumn: "vehicle_stage_id");

            migrationBuilder.AddForeignKey(
                name: "FK_battery_check_ev_check_detail_ev_check_detail_id",
                table: "battery_check",
                column: "ev_check_detail_id",
                principalTable: "ev_check_detail",
                principalColumn: "ev_check_detail_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_battery_check_part_item_part_item_id",
                table: "battery_check",
                column: "part_item_id",
                principalTable: "part_item",
                principalColumn: "part_item_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_campaign_detail_campaign_campaign_id",
                table: "campaign_detail",
                column: "campaign_id",
                principalTable: "campaign",
                principalColumn: "campaign_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_campaign_detail_part_part_id",
                table: "campaign_detail",
                column: "part_id",
                principalTable: "part",
                principalColumn: "part_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_customer_account_account_id",
                table: "customer",
                column: "account_id",
                principalTable: "account",
                principalColumn: "account_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ev_check_appointment_appointment_id",
                table: "ev_check",
                column: "appointment_id",
                principalTable: "appointment",
                principalColumn: "appointment_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ev_check_staff_task_executor_id",
                table: "ev_check",
                column: "task_executor_id",
                principalTable: "staff",
                principalColumn: "staff_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ev_check_detail_campaign_detail_campaign_detail_id",
                table: "ev_check_detail",
                column: "campaign_detail_id",
                principalTable: "campaign_detail",
                principalColumn: "campaign_detail_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ev_check_detail_ev_check_ev_check_id",
                table: "ev_check_detail",
                column: "ev_check_id",
                principalTable: "ev_check",
                principalColumn: "ev_check_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ev_check_detail_maintenance_stage_detail_maintenance_stage_d~",
                table: "ev_check_detail",
                column: "maintenance_stage_detail_id",
                principalTable: "maintenance_stage_detail",
                principalColumn: "maintenance_stage_detail_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ev_check_detail_part_item_part_item_id",
                table: "ev_check_detail",
                column: "part_item_id",
                principalTable: "part_item",
                principalColumn: "part_item_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ev_check_detail_part_item_replace_part_id",
                table: "ev_check_detail",
                column: "replace_part_id",
                principalTable: "part_item",
                principalColumn: "part_item_id");

            migrationBuilder.AddForeignKey(
                name: "FK_export_not_service_center_service_center_id",
                table: "export_not",
                column: "service_center_id",
                principalTable: "service_center",
                principalColumn: "service_center_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_export_not_staff_export_by_id",
                table: "export_not",
                column: "export_by_id",
                principalTable: "staff",
                principalColumn: "staff_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_import_note_service_center_service_center_id",
                table: "import_note",
                column: "service_center_id",
                principalTable: "service_center",
                principalColumn: "service_center_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_import_note_staff_import_by_id",
                table: "import_note",
                column: "import_by_id",
                principalTable: "staff",
                principalColumn: "staff_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_maintenance_stage_maintenance_plan_maintenance_plan_id",
                table: "maintenance_stage",
                column: "maintenance_plan_id",
                principalTable: "maintenance_plan",
                principalColumn: "maintenance_plan_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_maintenance_stage_detail_maintenance_stage_maintenance_stage~",
                table: "maintenance_stage_detail",
                column: "maintenance_stage_id",
                principalTable: "maintenance_stage",
                principalColumn: "maintenance_stage_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_maintenance_stage_detail_part_part_id",
                table: "maintenance_stage_detail",
                column: "part_id",
                principalTable: "part",
                principalColumn: "part_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_model_maintenance_plan_maintenance_plan_id",
                table: "model",
                column: "maintenance_plan_id",
                principalTable: "maintenance_plan",
                principalColumn: "maintenance_plan_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_model_part_type_model_model_id",
                table: "model_part_type",
                column: "model_id",
                principalTable: "model",
                principalColumn: "model_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_model_part_type_part_type_part_type_id",
                table: "model_part_type",
                column: "part_type_id",
                principalTable: "part_type",
                principalColumn: "part_type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_part_part_type_part_type_id",
                table: "part",
                column: "part_type_id",
                principalTable: "part_type",
                principalColumn: "part_type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_part_item_export_not_export_note_id",
                table: "part_item",
                column: "export_note_id",
                principalTable: "export_not",
                principalColumn: "export_note_id");

            migrationBuilder.AddForeignKey(
                name: "FK_part_item_import_note_import_note_id",
                table: "part_item",
                column: "import_note_id",
                principalTable: "import_note",
                principalColumn: "import_note_id");

            migrationBuilder.AddForeignKey(
                name: "FK_part_item_part_part_id",
                table: "part_item",
                column: "part_id",
                principalTable: "part",
                principalColumn: "part_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_payment_appointment_appointment_id",
                table: "payment",
                column: "appointment_id",
                principalTable: "appointment",
                principalColumn: "appointment_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_payment_customer_customer_id",
                table: "payment",
                column: "customer_id",
                principalTable: "customer",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_price_service_part_type_part_type_id",
                table: "price_service",
                column: "part_type_id",
                principalTable: "part_type",
                principalColumn: "part_type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rma_customer_customer_id",
                table: "rma",
                column: "customer_id",
                principalTable: "customer",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rma_staff_create_by_id",
                table: "rma",
                column: "create_by_id",
                principalTable: "staff",
                principalColumn: "staff_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rma_detail_ev_check_detail_ev_check_detail_id",
                table: "rma_detail",
                column: "ev_check_detail_id",
                principalTable: "ev_check_detail",
                principalColumn: "ev_check_detail_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rma_detail_part_item_part_item_id",
                table: "rma_detail",
                column: "part_item_id",
                principalTable: "part_item",
                principalColumn: "part_item_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rma_detail_rma_rma_id",
                table: "rma_detail",
                column: "rma_id",
                principalTable: "rma",
                principalColumn: "rma_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_service_center_inventory_part_item_part_item_id",
                table: "service_center_inventory",
                column: "part_item_id",
                principalTable: "part_item",
                principalColumn: "part_item_id");

            migrationBuilder.AddForeignKey(
                name: "FK_service_center_inventory_service_center_service_center_id",
                table: "service_center_inventory",
                column: "service_center_id",
                principalTable: "service_center",
                principalColumn: "service_center_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_staff_account_account_id",
                table: "staff",
                column: "account_id",
                principalTable: "account",
                principalColumn: "account_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_staff_service_center_ServiceCenterId",
                table: "staff",
                column: "ServiceCenterId",
                principalTable: "service_center",
                principalColumn: "service_center_id");

            migrationBuilder.AddForeignKey(
                name: "FK_vehicle_customer_customer_id",
                table: "vehicle",
                column: "customer_id",
                principalTable: "customer",
                principalColumn: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_vehicle_model_model_id",
                table: "vehicle",
                column: "model_id",
                principalTable: "model",
                principalColumn: "model_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicle_part_item_part_item_part_item_id",
                table: "vehicle_part_item",
                column: "part_item_id",
                principalTable: "part_item",
                principalColumn: "part_item_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicle_part_item_part_item_replace_for_id",
                table: "vehicle_part_item",
                column: "replace_for_id",
                principalTable: "part_item",
                principalColumn: "part_item_id");

            migrationBuilder.AddForeignKey(
                name: "FK_vehicle_part_item_vehicle_vehicle_id",
                table: "vehicle_part_item",
                column: "vehicle_id",
                principalTable: "vehicle",
                principalColumn: "vehicle_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicle_stage_maintenance_stage_maintenance_stage_id",
                table: "vehicle_stage",
                column: "maintenance_stage_id",
                principalTable: "maintenance_stage",
                principalColumn: "maintenance_stage_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicle_stage_vehicle_vehicle_id",
                table: "vehicle_stage",
                column: "vehicle_id",
                principalTable: "vehicle",
                principalColumn: "vehicle_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointment_campaign_campaign_id",
                table: "appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_appointment_customer_customer_id",
                table: "appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_appointment_service_center_service_center_id",
                table: "appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_appointment_staff_approve_by_id",
                table: "appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_appointment_vehicle_stage_vehicle_stage_id",
                table: "appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_battery_check_ev_check_detail_ev_check_detail_id",
                table: "battery_check");

            migrationBuilder.DropForeignKey(
                name: "FK_battery_check_part_item_part_item_id",
                table: "battery_check");

            migrationBuilder.DropForeignKey(
                name: "FK_campaign_detail_campaign_campaign_id",
                table: "campaign_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_campaign_detail_part_part_id",
                table: "campaign_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_customer_account_account_id",
                table: "customer");

            migrationBuilder.DropForeignKey(
                name: "FK_ev_check_appointment_appointment_id",
                table: "ev_check");

            migrationBuilder.DropForeignKey(
                name: "FK_ev_check_staff_task_executor_id",
                table: "ev_check");

            migrationBuilder.DropForeignKey(
                name: "FK_ev_check_detail_campaign_detail_campaign_detail_id",
                table: "ev_check_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_ev_check_detail_ev_check_ev_check_id",
                table: "ev_check_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_ev_check_detail_maintenance_stage_detail_maintenance_stage_d~",
                table: "ev_check_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_ev_check_detail_part_item_part_item_id",
                table: "ev_check_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_ev_check_detail_part_item_replace_part_id",
                table: "ev_check_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_export_not_service_center_service_center_id",
                table: "export_not");

            migrationBuilder.DropForeignKey(
                name: "FK_export_not_staff_export_by_id",
                table: "export_not");

            migrationBuilder.DropForeignKey(
                name: "FK_import_note_service_center_service_center_id",
                table: "import_note");

            migrationBuilder.DropForeignKey(
                name: "FK_import_note_staff_import_by_id",
                table: "import_note");

            migrationBuilder.DropForeignKey(
                name: "FK_maintenance_stage_maintenance_plan_maintenance_plan_id",
                table: "maintenance_stage");

            migrationBuilder.DropForeignKey(
                name: "FK_maintenance_stage_detail_maintenance_stage_maintenance_stage~",
                table: "maintenance_stage_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_maintenance_stage_detail_part_part_id",
                table: "maintenance_stage_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_model_maintenance_plan_maintenance_plan_id",
                table: "model");

            migrationBuilder.DropForeignKey(
                name: "FK_model_part_type_model_model_id",
                table: "model_part_type");

            migrationBuilder.DropForeignKey(
                name: "FK_model_part_type_part_type_part_type_id",
                table: "model_part_type");

            migrationBuilder.DropForeignKey(
                name: "FK_part_part_type_part_type_id",
                table: "part");

            migrationBuilder.DropForeignKey(
                name: "FK_part_item_export_not_export_note_id",
                table: "part_item");

            migrationBuilder.DropForeignKey(
                name: "FK_part_item_import_note_import_note_id",
                table: "part_item");

            migrationBuilder.DropForeignKey(
                name: "FK_part_item_part_part_id",
                table: "part_item");

            migrationBuilder.DropForeignKey(
                name: "FK_payment_appointment_appointment_id",
                table: "payment");

            migrationBuilder.DropForeignKey(
                name: "FK_payment_customer_customer_id",
                table: "payment");

            migrationBuilder.DropForeignKey(
                name: "FK_price_service_part_type_part_type_id",
                table: "price_service");

            migrationBuilder.DropForeignKey(
                name: "FK_rma_customer_customer_id",
                table: "rma");

            migrationBuilder.DropForeignKey(
                name: "FK_rma_staff_create_by_id",
                table: "rma");

            migrationBuilder.DropForeignKey(
                name: "FK_rma_detail_ev_check_detail_ev_check_detail_id",
                table: "rma_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_rma_detail_part_item_part_item_id",
                table: "rma_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_rma_detail_rma_rma_id",
                table: "rma_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_service_center_inventory_part_item_part_item_id",
                table: "service_center_inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_service_center_inventory_service_center_service_center_id",
                table: "service_center_inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_staff_account_account_id",
                table: "staff");

            migrationBuilder.DropForeignKey(
                name: "FK_staff_service_center_ServiceCenterId",
                table: "staff");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicle_customer_customer_id",
                table: "vehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicle_model_model_id",
                table: "vehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicle_part_item_part_item_part_item_id",
                table: "vehicle_part_item");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicle_part_item_part_item_replace_for_id",
                table: "vehicle_part_item");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicle_part_item_vehicle_vehicle_id",
                table: "vehicle_part_item");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicle_stage_maintenance_stage_maintenance_stage_id",
                table: "vehicle_stage");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicle_stage_vehicle_vehicle_id",
                table: "vehicle_stage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vehicle_stage",
                table: "vehicle_stage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vehicle_part_item",
                table: "vehicle_part_item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vehicle",
                table: "vehicle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_staff",
                table: "staff");

            migrationBuilder.DropPrimaryKey(
                name: "PK_service_center_inventory",
                table: "service_center_inventory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_service_center",
                table: "service_center");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rma_detail",
                table: "rma_detail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rma",
                table: "rma");

            migrationBuilder.DropPrimaryKey(
                name: "PK_price_service",
                table: "price_service");

            migrationBuilder.DropPrimaryKey(
                name: "PK_payment",
                table: "payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_part_type",
                table: "part_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_part_item",
                table: "part_item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_part",
                table: "part");

            migrationBuilder.DropPrimaryKey(
                name: "PK_model_part_type",
                table: "model_part_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_model",
                table: "model");

            migrationBuilder.DropPrimaryKey(
                name: "PK_maintenance_stage_detail",
                table: "maintenance_stage_detail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_maintenance_stage",
                table: "maintenance_stage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_maintenance_plan",
                table: "maintenance_plan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_import_note",
                table: "import_note");

            migrationBuilder.DropPrimaryKey(
                name: "PK_export_not",
                table: "export_not");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ev_check_detail",
                table: "ev_check_detail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ev_check",
                table: "ev_check");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customer",
                table: "customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_campaign_detail",
                table: "campaign_detail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_campaign",
                table: "campaign");

            migrationBuilder.DropPrimaryKey(
                name: "PK_battery_check",
                table: "battery_check");

            migrationBuilder.DropPrimaryKey(
                name: "PK_appointment",
                table: "appointment");

            migrationBuilder.DropIndex(
                name: "IX_appointment_service_center_id",
                table: "appointment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_account",
                table: "account");

            migrationBuilder.DropColumn(
                name: "actual_maintenance_mileage",
                table: "vehicle_stage");

            migrationBuilder.DropColumn(
                name: "date_of_implementation",
                table: "vehicle_stage");

            migrationBuilder.DropColumn(
                name: "status",
                table: "vehicle_stage");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "vehicle_part_item");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "vehicle_part_item");

            migrationBuilder.DropColumn(
                name: "install_date",
                table: "vehicle_part_item");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "chassis_number",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "color",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "engine_number",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "image",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "manufacture_date",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "purchase_date",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "status",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "vin_number",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "warranty_expiry",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "address",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "avatar_url",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "citizen_id",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "position",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "staff_code",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "service_center");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "service_center");

            migrationBuilder.DropColumn(
                name: "address",
                table: "service_center");

            migrationBuilder.DropColumn(
                name: "code",
                table: "service_center");

            migrationBuilder.DropColumn(
                name: "description",
                table: "service_center");

            migrationBuilder.DropColumn(
                name: "email",
                table: "service_center");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "service_center");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "service_center");

            migrationBuilder.DropColumn(
                name: "name",
                table: "service_center");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "service_center");

            migrationBuilder.DropColumn(
                name: "status",
                table: "service_center");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "rma_detail");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "rma_detail");

            migrationBuilder.DropColumn(
                name: "expiration_date_rma",
                table: "rma_detail");

            migrationBuilder.DropColumn(
                name: "inspector",
                table: "rma_detail");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "rma_detail");

            migrationBuilder.DropColumn(
                name: "reason",
                table: "rma_detail");

            migrationBuilder.DropColumn(
                name: "release_date_rma",
                table: "rma_detail");

            migrationBuilder.DropColumn(
                name: "result",
                table: "rma_detail");

            migrationBuilder.DropColumn(
                name: "rma_number",
                table: "rma_detail");

            migrationBuilder.DropColumn(
                name: "solution",
                table: "rma_detail");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "rma");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "rma");

            migrationBuilder.DropColumn(
                name: "code",
                table: "rma");

            migrationBuilder.DropColumn(
                name: "note",
                table: "rma");

            migrationBuilder.DropColumn(
                name: "return_address",
                table: "rma");

            migrationBuilder.DropColumn(
                name: "rma_date",
                table: "rma");

            migrationBuilder.DropColumn(
                name: "status",
                table: "rma");

            migrationBuilder.DropColumn(
                name: "code",
                table: "price_service");

            migrationBuilder.DropColumn(
                name: "description",
                table: "price_service");

            migrationBuilder.DropColumn(
                name: "effective_date",
                table: "price_service");

            migrationBuilder.DropColumn(
                name: "labor_cost",
                table: "price_service");

            migrationBuilder.DropColumn(
                name: "name",
                table: "price_service");

            migrationBuilder.DropColumn(
                name: "price",
                table: "price_service");

            migrationBuilder.DropColumn(
                name: "remedies",
                table: "price_service");

            migrationBuilder.DropColumn(
                name: "amount",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "currency",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "payment_method",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "status",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "transaction_code",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "description",
                table: "part_type");

            migrationBuilder.DropColumn(
                name: "name",
                table: "part_type");

            migrationBuilder.DropColumn(
                name: "export_id",
                table: "part_item");

            migrationBuilder.DropColumn(
                name: "price",
                table: "part_item");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "part_item");

            migrationBuilder.DropColumn(
                name: "serial_number",
                table: "part_item");

            migrationBuilder.DropColumn(
                name: "status",
                table: "part_item");

            migrationBuilder.DropColumn(
                name: "warranty_period",
                table: "part_item");

            migrationBuilder.DropColumn(
                name: "code",
                table: "part");

            migrationBuilder.DropColumn(
                name: "image",
                table: "part");

            migrationBuilder.DropColumn(
                name: "name",
                table: "part");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "part");

            migrationBuilder.DropColumn(
                name: "status",
                table: "part");

            migrationBuilder.DropColumn(
                name: "code",
                table: "model");

            migrationBuilder.DropColumn(
                name: "manufacturer",
                table: "model");

            migrationBuilder.DropColumn(
                name: "name",
                table: "model");

            migrationBuilder.DropColumn(
                name: "action_type",
                table: "maintenance_stage_detail");

            migrationBuilder.DropColumn(
                name: "description",
                table: "maintenance_stage_detail");

            migrationBuilder.DropColumn(
                name: "description",
                table: "maintenance_stage");

            migrationBuilder.DropColumn(
                name: "duration_month",
                table: "maintenance_stage");

            migrationBuilder.DropColumn(
                name: "estimated_time",
                table: "maintenance_stage");

            migrationBuilder.DropColumn(
                name: "mileage",
                table: "maintenance_stage");

            migrationBuilder.DropColumn(
                name: "name",
                table: "maintenance_stage");

            migrationBuilder.DropColumn(
                name: "status",
                table: "maintenance_stage");

            migrationBuilder.DropColumn(
                name: "code",
                table: "maintenance_plan");

            migrationBuilder.DropColumn(
                name: "description",
                table: "maintenance_plan");

            migrationBuilder.DropColumn(
                name: "effective_date",
                table: "maintenance_plan");

            migrationBuilder.DropColumn(
                name: "name",
                table: "maintenance_plan");

            migrationBuilder.DropColumn(
                name: "status",
                table: "maintenance_plan");

            migrationBuilder.DropColumn(
                name: "total_stages",
                table: "maintenance_plan");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "maintenance_plan");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "import_note");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "import_note");

            migrationBuilder.DropColumn(
                name: "code",
                table: "import_note");

            migrationBuilder.DropColumn(
                name: "import_date",
                table: "import_note");

            migrationBuilder.DropColumn(
                name: "import_from",
                table: "import_note");

            migrationBuilder.DropColumn(
                name: "supplier",
                table: "import_note");

            migrationBuilder.DropColumn(
                name: "total_amout",
                table: "import_note");

            migrationBuilder.DropColumn(
                name: "type",
                table: "import_note");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "export_not");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "export_not");

            migrationBuilder.DropColumn(
                name: "code",
                table: "export_not");

            migrationBuilder.DropColumn(
                name: "export_date",
                table: "export_not");

            migrationBuilder.DropColumn(
                name: "export_to",
                table: "export_not");

            migrationBuilder.DropColumn(
                name: "note",
                table: "export_not");

            migrationBuilder.DropColumn(
                name: "total_quantity",
                table: "export_not");

            migrationBuilder.DropColumn(
                name: "total_value",
                table: "export_not");

            migrationBuilder.DropColumn(
                name: "type",
                table: "export_not");

            migrationBuilder.DropColumn(
                name: "price_part",
                table: "ev_check_detail");

            migrationBuilder.DropColumn(
                name: "price_service",
                table: "ev_check_detail");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "ev_check_detail");

            migrationBuilder.DropColumn(
                name: "remedies",
                table: "ev_check_detail");

            migrationBuilder.DropColumn(
                name: "result",
                table: "ev_check_detail");

            migrationBuilder.DropColumn(
                name: "status",
                table: "ev_check_detail");

            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "ev_check_detail");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "ev_check_detail");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ev_check");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ev_check");

            migrationBuilder.DropColumn(
                name: "check_date",
                table: "ev_check");

            migrationBuilder.DropColumn(
                name: "odometer",
                table: "ev_check");

            migrationBuilder.DropColumn(
                name: "status",
                table: "ev_check");

            migrationBuilder.DropColumn(
                name: "total_amout",
                table: "ev_check");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "address",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "avatar_url",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "citizen_id",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "campaign_detail");

            migrationBuilder.DropColumn(
                name: "EVCheckDetailId",
                table: "campaign_detail");

            migrationBuilder.DropColumn(
                name: "action_type",
                table: "campaign_detail");

            migrationBuilder.DropColumn(
                name: "estimated_time",
                table: "campaign_detail");

            migrationBuilder.DropColumn(
                name: "is_mandatory",
                table: "campaign_detail");

            migrationBuilder.DropColumn(
                name: "note",
                table: "campaign_detail");

            migrationBuilder.DropColumn(
                name: "code",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "description",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "end_date",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "name",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "start_date",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "status",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "type",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "capacity",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "current",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "energy",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "power",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "soc",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "soh",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "solution",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "temp",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "time",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "voltage",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "actual_cost",
                table: "appointment");

            migrationBuilder.DropColumn(
                name: "appointment_date",
                table: "appointment");

            migrationBuilder.DropColumn(
                name: "code",
                table: "appointment");

            migrationBuilder.DropColumn(
                name: "estimated_cost",
                table: "appointment");

            migrationBuilder.DropColumn(
                name: "service_center_id",
                table: "appointment");

            migrationBuilder.DropColumn(
                name: "status",
                table: "appointment");

            migrationBuilder.DropColumn(
                name: "time_slot",
                table: "appointment");

            migrationBuilder.DropColumn(
                name: "type",
                table: "appointment");

            migrationBuilder.DropColumn(
                name: "email",
                table: "account");

            migrationBuilder.DropColumn(
                name: "password",
                table: "account");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "account");

            migrationBuilder.DropColumn(
                name: "role_ame",
                table: "account");

            migrationBuilder.DropColumn(
                name: "status",
                table: "account");

            migrationBuilder.RenameTable(
                name: "vehicle_stage",
                newName: "VehicleStages");

            migrationBuilder.RenameTable(
                name: "vehicle_part_item",
                newName: "VehiclePartItems");

            migrationBuilder.RenameTable(
                name: "vehicle",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "staff",
                newName: "Staffs");

            migrationBuilder.RenameTable(
                name: "service_center_inventory",
                newName: "ServiceCenterInventorys");

            migrationBuilder.RenameTable(
                name: "service_center",
                newName: "ServiceCenters");

            migrationBuilder.RenameTable(
                name: "rma_detail",
                newName: "RMADetails");

            migrationBuilder.RenameTable(
                name: "rma",
                newName: "RMAs");

            migrationBuilder.RenameTable(
                name: "price_service",
                newName: "PriceServices");

            migrationBuilder.RenameTable(
                name: "payment",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "part_type",
                newName: "PartTypes");

            migrationBuilder.RenameTable(
                name: "part_item",
                newName: "PartItems");

            migrationBuilder.RenameTable(
                name: "part",
                newName: "Parts");

            migrationBuilder.RenameTable(
                name: "model_part_type",
                newName: "ModelPartTypes");

            migrationBuilder.RenameTable(
                name: "model",
                newName: "Models");

            migrationBuilder.RenameTable(
                name: "maintenance_stage_detail",
                newName: "MaintenanceStageDetails");

            migrationBuilder.RenameTable(
                name: "maintenance_stage",
                newName: "MaintenanceStages");

            migrationBuilder.RenameTable(
                name: "maintenance_plan",
                newName: "MaintenancePlans");

            migrationBuilder.RenameTable(
                name: "import_note",
                newName: "ImportNotes");

            migrationBuilder.RenameTable(
                name: "export_not",
                newName: "ExportNotes");

            migrationBuilder.RenameTable(
                name: "ev_check_detail",
                newName: "EVCheckDetails");

            migrationBuilder.RenameTable(
                name: "ev_check",
                newName: "EVChecks");

            migrationBuilder.RenameTable(
                name: "customer",
                newName: "Customers");

            migrationBuilder.RenameTable(
                name: "campaign_detail",
                newName: "CampaignDetails");

            migrationBuilder.RenameTable(
                name: "campaign",
                newName: "Campaigns");

            migrationBuilder.RenameTable(
                name: "battery_check",
                newName: "BatteryChecks");

            migrationBuilder.RenameTable(
                name: "appointment",
                newName: "Appointments");

            migrationBuilder.RenameTable(
                name: "account",
                newName: "Accounts");

            migrationBuilder.RenameColumn(
                name: "vehicle_stage_id",
                table: "VehicleStages",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_vehicle_stage_vehicle_id",
                table: "VehicleStages",
                newName: "IX_VehicleStages_vehicle_id");

            migrationBuilder.RenameIndex(
                name: "IX_vehicle_stage_maintenance_stage_id",
                table: "VehicleStages",
                newName: "IX_VehicleStages_maintenance_stage_id");

            migrationBuilder.RenameColumn(
                name: "vehicle_part_item_id",
                table: "VehiclePartItems",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_vehicle_part_item_vehicle_id",
                table: "VehiclePartItems",
                newName: "IX_VehiclePartItems_vehicle_id");

            migrationBuilder.RenameIndex(
                name: "IX_vehicle_part_item_replace_for_id",
                table: "VehiclePartItems",
                newName: "IX_VehiclePartItems_replace_for_id");

            migrationBuilder.RenameIndex(
                name: "IX_vehicle_part_item_part_item_id",
                table: "VehiclePartItems",
                newName: "IX_VehiclePartItems_part_item_id");

            migrationBuilder.RenameColumn(
                name: "vehicle_id",
                table: "Vehicles",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_vehicle_model_id",
                table: "Vehicles",
                newName: "IX_Vehicles_model_id");

            migrationBuilder.RenameIndex(
                name: "IX_vehicle_customer_id",
                table: "Vehicles",
                newName: "IX_Vehicles_customer_id");

            migrationBuilder.RenameColumn(
                name: "staff_id",
                table: "Staffs",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_staff_ServiceCenterId",
                table: "Staffs",
                newName: "IX_Staffs_ServiceCenterId");

            migrationBuilder.RenameIndex(
                name: "IX_staff_account_id",
                table: "Staffs",
                newName: "IX_Staffs_account_id");

            migrationBuilder.RenameColumn(
                name: "service_center_inventory_id",
                table: "ServiceCenterInventorys",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_service_center_inventory_service_center_id",
                table: "ServiceCenterInventorys",
                newName: "IX_ServiceCenterInventorys_service_center_id");

            migrationBuilder.RenameIndex(
                name: "IX_service_center_inventory_part_item_id",
                table: "ServiceCenterInventorys",
                newName: "IX_ServiceCenterInventorys_part_item_id");

            migrationBuilder.RenameColumn(
                name: "service_center_id",
                table: "ServiceCenters",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "rma_detail_id",
                table: "RMADetails",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_rma_detail_rma_id",
                table: "RMADetails",
                newName: "IX_RMADetails_rma_id");

            migrationBuilder.RenameIndex(
                name: "IX_rma_detail_part_item_id",
                table: "RMADetails",
                newName: "IX_RMADetails_part_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_rma_detail_ev_check_detail_id",
                table: "RMADetails",
                newName: "IX_RMADetails_ev_check_detail_id");

            migrationBuilder.RenameColumn(
                name: "rma_id",
                table: "RMAs",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_rma_customer_id",
                table: "RMAs",
                newName: "IX_RMAs_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_rma_create_by_id",
                table: "RMAs",
                newName: "IX_RMAs_create_by_id");

            migrationBuilder.RenameColumn(
                name: "price_service_id",
                table: "PriceServices",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_price_service_part_type_id",
                table: "PriceServices",
                newName: "IX_PriceServices_part_type_id");

            migrationBuilder.RenameColumn(
                name: "payment_id",
                table: "Payments",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_payment_customer_id",
                table: "Payments",
                newName: "IX_Payments_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_payment_appointment_id",
                table: "Payments",
                newName: "IX_Payments_appointment_id");

            migrationBuilder.RenameColumn(
                name: "part_type_id",
                table: "PartTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "part_item_id",
                table: "PartItems",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_part_item_part_id",
                table: "PartItems",
                newName: "IX_PartItems_part_id");

            migrationBuilder.RenameIndex(
                name: "IX_part_item_import_note_id",
                table: "PartItems",
                newName: "IX_PartItems_import_note_id");

            migrationBuilder.RenameIndex(
                name: "IX_part_item_export_note_id",
                table: "PartItems",
                newName: "IX_PartItems_export_note_id");

            migrationBuilder.RenameColumn(
                name: "part_id",
                table: "Parts",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_part_part_type_id",
                table: "Parts",
                newName: "IX_Parts_part_type_id");

            migrationBuilder.RenameColumn(
                name: "model_part_type_id",
                table: "ModelPartTypes",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_model_part_type_part_type_id",
                table: "ModelPartTypes",
                newName: "IX_ModelPartTypes_part_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_model_part_type_model_id",
                table: "ModelPartTypes",
                newName: "IX_ModelPartTypes_model_id");

            migrationBuilder.RenameColumn(
                name: "model_id",
                table: "Models",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_model_maintenance_plan_id",
                table: "Models",
                newName: "IX_Models_maintenance_plan_id");

            migrationBuilder.RenameColumn(
                name: "maintenance_stage_detail_id",
                table: "MaintenanceStageDetails",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_maintenance_stage_detail_part_id",
                table: "MaintenanceStageDetails",
                newName: "IX_MaintenanceStageDetails_part_id");

            migrationBuilder.RenameIndex(
                name: "IX_maintenance_stage_detail_maintenance_stage_id",
                table: "MaintenanceStageDetails",
                newName: "IX_MaintenanceStageDetails_maintenance_stage_id");

            migrationBuilder.RenameColumn(
                name: "maintenance_stage_id",
                table: "MaintenanceStages",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_maintenance_stage_maintenance_plan_id",
                table: "MaintenanceStages",
                newName: "IX_MaintenanceStages_maintenance_plan_id");

            migrationBuilder.RenameColumn(
                name: "maintenance_plan_id",
                table: "MaintenancePlans",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "import_note_id",
                table: "ImportNotes",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_import_note_service_center_id",
                table: "ImportNotes",
                newName: "IX_ImportNotes_service_center_id");

            migrationBuilder.RenameIndex(
                name: "IX_import_note_import_by_id",
                table: "ImportNotes",
                newName: "IX_ImportNotes_import_by_id");

            migrationBuilder.RenameColumn(
                name: "export_note_id",
                table: "ExportNotes",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_export_not_service_center_id",
                table: "ExportNotes",
                newName: "IX_ExportNotes_service_center_id");

            migrationBuilder.RenameIndex(
                name: "IX_export_not_export_by_id",
                table: "ExportNotes",
                newName: "IX_ExportNotes_export_by_id");

            migrationBuilder.RenameColumn(
                name: "ev_check_detail_id",
                table: "EVCheckDetails",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_ev_check_detail_replace_part_id",
                table: "EVCheckDetails",
                newName: "IX_EVCheckDetails_replace_part_id");

            migrationBuilder.RenameIndex(
                name: "IX_ev_check_detail_part_item_id",
                table: "EVCheckDetails",
                newName: "IX_EVCheckDetails_part_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_ev_check_detail_maintenance_stage_detail_id",
                table: "EVCheckDetails",
                newName: "IX_EVCheckDetails_maintenance_stage_detail_id");

            migrationBuilder.RenameIndex(
                name: "IX_ev_check_detail_ev_check_id",
                table: "EVCheckDetails",
                newName: "IX_EVCheckDetails_ev_check_id");

            migrationBuilder.RenameIndex(
                name: "IX_ev_check_detail_campaign_detail_id",
                table: "EVCheckDetails",
                newName: "IX_EVCheckDetails_campaign_detail_id");

            migrationBuilder.RenameColumn(
                name: "ev_check_id",
                table: "EVChecks",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_ev_check_task_executor_id",
                table: "EVChecks",
                newName: "IX_EVChecks_task_executor_id");

            migrationBuilder.RenameIndex(
                name: "IX_ev_check_appointment_id",
                table: "EVChecks",
                newName: "IX_EVChecks_appointment_id");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "Customers",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_customer_account_id",
                table: "Customers",
                newName: "IX_Customers_account_id");

            migrationBuilder.RenameColumn(
                name: "campaign_detail_id",
                table: "CampaignDetails",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_campaign_detail_part_id",
                table: "CampaignDetails",
                newName: "IX_CampaignDetails_part_id");

            migrationBuilder.RenameIndex(
                name: "IX_campaign_detail_campaign_id",
                table: "CampaignDetails",
                newName: "IX_CampaignDetails_campaign_id");

            migrationBuilder.RenameColumn(
                name: "campaign_id",
                table: "Campaigns",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "battery_check_id",
                table: "BatteryChecks",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_battery_check_part_item_id",
                table: "BatteryChecks",
                newName: "IX_BatteryChecks_part_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_battery_check_ev_check_detail_id",
                table: "BatteryChecks",
                newName: "IX_BatteryChecks_ev_check_detail_id");

            migrationBuilder.RenameColumn(
                name: "appointment_id",
                table: "Appointments",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_appointment_vehicle_stage_id",
                table: "Appointments",
                newName: "IX_Appointments_vehicle_stage_id");

            migrationBuilder.RenameIndex(
                name: "IX_appointment_customer_id",
                table: "Appointments",
                newName: "IX_Appointments_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_appointment_campaign_id",
                table: "Appointments",
                newName: "IX_Appointments_campaign_id");

            migrationBuilder.RenameIndex(
                name: "IX_appointment_approve_by_id",
                table: "Appointments",
                newName: "IX_Appointments_approve_by_id");

            migrationBuilder.RenameColumn(
                name: "account_id",
                table: "Accounts",
                newName: "Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "Appointments",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleStages",
                table: "VehicleStages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehiclePartItems",
                table: "VehiclePartItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Staffs",
                table: "Staffs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceCenterInventorys",
                table: "ServiceCenterInventorys",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceCenters",
                table: "ServiceCenters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RMADetails",
                table: "RMADetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RMAs",
                table: "RMAs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceServices",
                table: "PriceServices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartTypes",
                table: "PartTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartItems",
                table: "PartItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parts",
                table: "Parts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelPartTypes",
                table: "ModelPartTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Models",
                table: "Models",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaintenanceStageDetails",
                table: "MaintenanceStageDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaintenanceStages",
                table: "MaintenanceStages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaintenancePlans",
                table: "MaintenancePlans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImportNotes",
                table: "ImportNotes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExportNotes",
                table: "ExportNotes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EVCheckDetails",
                table: "EVCheckDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EVChecks",
                table: "EVChecks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignDetails",
                table: "CampaignDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaigns",
                table: "Campaigns",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BatteryChecks",
                table: "BatteryChecks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Campaigns_campaign_id",
                table: "Appointments",
                column: "campaign_id",
                principalTable: "Campaigns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Customers_customer_id",
                table: "Appointments",
                column: "customer_id",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Staffs_approve_by_id",
                table: "Appointments",
                column: "approve_by_id",
                principalTable: "Staffs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_VehicleStages_vehicle_stage_id",
                table: "Appointments",
                column: "vehicle_stage_id",
                principalTable: "VehicleStages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BatteryChecks_EVCheckDetails_ev_check_detail_id",
                table: "BatteryChecks",
                column: "ev_check_detail_id",
                principalTable: "EVCheckDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BatteryChecks_PartItems_part_item_id",
                table: "BatteryChecks",
                column: "part_item_id",
                principalTable: "PartItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignDetails_Campaigns_campaign_id",
                table: "CampaignDetails",
                column: "campaign_id",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignDetails_Parts_part_id",
                table: "CampaignDetails",
                column: "part_id",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Accounts_account_id",
                table: "Customers",
                column: "account_id",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EVCheckDetails_CampaignDetails_campaign_detail_id",
                table: "EVCheckDetails",
                column: "campaign_detail_id",
                principalTable: "CampaignDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EVCheckDetails_EVChecks_ev_check_id",
                table: "EVCheckDetails",
                column: "ev_check_id",
                principalTable: "EVChecks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EVCheckDetails_MaintenanceStageDetails_maintenance_stage_det~",
                table: "EVCheckDetails",
                column: "maintenance_stage_detail_id",
                principalTable: "MaintenanceStageDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EVCheckDetails_PartItems_part_item_id",
                table: "EVCheckDetails",
                column: "part_item_id",
                principalTable: "PartItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EVCheckDetails_PartItems_replace_part_id",
                table: "EVCheckDetails",
                column: "replace_part_id",
                principalTable: "PartItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EVChecks_Appointments_appointment_id",
                table: "EVChecks",
                column: "appointment_id",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EVChecks_Staffs_task_executor_id",
                table: "EVChecks",
                column: "task_executor_id",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExportNotes_ServiceCenters_service_center_id",
                table: "ExportNotes",
                column: "service_center_id",
                principalTable: "ServiceCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExportNotes_Staffs_export_by_id",
                table: "ExportNotes",
                column: "export_by_id",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportNotes_ServiceCenters_service_center_id",
                table: "ImportNotes",
                column: "service_center_id",
                principalTable: "ServiceCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportNotes_Staffs_import_by_id",
                table: "ImportNotes",
                column: "import_by_id",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceStageDetails_MaintenanceStages_maintenance_stage_~",
                table: "MaintenanceStageDetails",
                column: "maintenance_stage_id",
                principalTable: "MaintenanceStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceStageDetails_Parts_part_id",
                table: "MaintenanceStageDetails",
                column: "part_id",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceStages_MaintenancePlans_maintenance_plan_id",
                table: "MaintenanceStages",
                column: "maintenance_plan_id",
                principalTable: "MaintenancePlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelPartTypes_Models_model_id",
                table: "ModelPartTypes",
                column: "model_id",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelPartTypes_PartTypes_part_type_id",
                table: "ModelPartTypes",
                column: "part_type_id",
                principalTable: "PartTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Models_MaintenancePlans_maintenance_plan_id",
                table: "Models",
                column: "maintenance_plan_id",
                principalTable: "MaintenancePlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartItems_ExportNotes_export_note_id",
                table: "PartItems",
                column: "export_note_id",
                principalTable: "ExportNotes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartItems_ImportNotes_import_note_id",
                table: "PartItems",
                column: "import_note_id",
                principalTable: "ImportNotes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartItems_Parts_part_id",
                table: "PartItems",
                column: "part_id",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_PartTypes_part_type_id",
                table: "Parts",
                column: "part_type_id",
                principalTable: "PartTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Appointments_appointment_id",
                table: "Payments",
                column: "appointment_id",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Customers_customer_id",
                table: "Payments",
                column: "customer_id",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceServices_PartTypes_part_type_id",
                table: "PriceServices",
                column: "part_type_id",
                principalTable: "PartTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RMADetails_EVCheckDetails_ev_check_detail_id",
                table: "RMADetails",
                column: "ev_check_detail_id",
                principalTable: "EVCheckDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RMADetails_PartItems_part_item_id",
                table: "RMADetails",
                column: "part_item_id",
                principalTable: "PartItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RMADetails_RMAs_rma_id",
                table: "RMADetails",
                column: "rma_id",
                principalTable: "RMAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RMAs_Customers_customer_id",
                table: "RMAs",
                column: "customer_id",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RMAs_Staffs_create_by_id",
                table: "RMAs",
                column: "create_by_id",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCenterInventorys_PartItems_part_item_id",
                table: "ServiceCenterInventorys",
                column: "part_item_id",
                principalTable: "PartItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCenterInventorys_ServiceCenters_service_center_id",
                table: "ServiceCenterInventorys",
                column: "service_center_id",
                principalTable: "ServiceCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Accounts_account_id",
                table: "Staffs",
                column: "account_id",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_ServiceCenters_ServiceCenterId",
                table: "Staffs",
                column: "ServiceCenterId",
                principalTable: "ServiceCenters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehiclePartItems_PartItems_part_item_id",
                table: "VehiclePartItems",
                column: "part_item_id",
                principalTable: "PartItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehiclePartItems_PartItems_replace_for_id",
                table: "VehiclePartItems",
                column: "replace_for_id",
                principalTable: "PartItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehiclePartItems_Vehicles_vehicle_id",
                table: "VehiclePartItems",
                column: "vehicle_id",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Customers_customer_id",
                table: "Vehicles",
                column: "customer_id",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Models_model_id",
                table: "Vehicles",
                column: "model_id",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleStages_MaintenanceStages_maintenance_stage_id",
                table: "VehicleStages",
                column: "maintenance_stage_id",
                principalTable: "MaintenanceStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleStages_Vehicles_vehicle_id",
                table: "VehicleStages",
                column: "vehicle_id",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
