using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                        // Drop foreign keys only if they exist (some DBs may not have them)
                        migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `appointment` DROP FOREIGN KEY `', CONSTRAINT_NAME, '`') INTO @s
                                FROM information_schema.KEY_COLUMN_USAGE
                                WHERE CONSTRAINT_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'appointment'
                                    AND REFERENCED_TABLE_NAME = 'campaign'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

                        migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `ev_check_detail` DROP FOREIGN KEY `', CONSTRAINT_NAME, '`') INTO @s
                                FROM information_schema.KEY_COLUMN_USAGE
                                WHERE CONSTRAINT_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'ev_check_detail'
                                    AND REFERENCED_TABLE_NAME = 'campaign_detail'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

                        migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `rma_detail` DROP FOREIGN KEY `', CONSTRAINT_NAME, '`') INTO @s
                                FROM information_schema.KEY_COLUMN_USAGE
                                WHERE CONSTRAINT_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'rma_detail'
                                    AND REFERENCED_TABLE_NAME = 'part_item'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            // Drop tables only if they exist to avoid errors when DB state differs
            migrationBuilder.Sql("DROP TABLE IF EXISTS `campaign_detail`;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS `campaign`;");

                        migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `vehicle_part_item` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'vehicle_part_item'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `vehicle_part_item` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'vehicle_part_item'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            // Convert remaining renames into conditional SQL to avoid failures
            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `vehicle` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'vehicle'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `vehicle` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'vehicle'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `staff` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'staff'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `staff` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'staff'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `service_center` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'service_center'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `service_center` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'service_center'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `rma_detail` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'rma_detail'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `rma_detail` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'rma_detail'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `rma` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'rma'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `rma` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'rma'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `price_service` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'price_service'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `price_service` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'price_service'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `part_type` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'part_type'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `part_type` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'part_type'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `part` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'part'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `part` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'part'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `maintenance_stage_detail` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'maintenance_stage_detail'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `maintenance_stage_detail` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'maintenance_stage_detail'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `maintenance_stage` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'maintenance_stage'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `maintenance_stage` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'maintenance_stage'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `maintenance_plan` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'maintenance_plan'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `maintenance_plan` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'maintenance_plan'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `import_note` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'import_note'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `import_note` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'import_note'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `export_note` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'export_note'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `export_note` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'export_note'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `ev_check` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'ev_check'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `ev_check` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'ev_check'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `customer` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'customer'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `customer` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'customer'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `battery_check` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'battery_check'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `battery_check` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'battery_check'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `appointment` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'appointment'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `appointment` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'appointment'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `account` RENAME COLUMN `', COLUMN_NAME, '` TO `updated_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'account'
                                    AND COLUMN_NAME = 'UpdatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.Sql(@"
                                SELECT CONCAT('ALTER TABLE `account` RENAME COLUMN `', COLUMN_NAME, '` TO `created_at`') INTO @s
                                FROM information_schema.COLUMNS
                                WHERE TABLE_SCHEMA = DATABASE()
                                    AND TABLE_NAME = 'account'
                                    AND COLUMN_NAME = 'CreatedAt'
                                LIMIT 1;
                                SET @sql = IFNULL(@s, 'SELECT 0');
                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                        ");

            migrationBuilder.AlterColumn<string>(
                name: "rma_number",
                table: "rma_detail",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "replace_part_id",
                table: "rma_detail",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

                        migrationBuilder.Sql(@"
                                                                SELECT COUNT(*) INTO @colExists
                                                                FROM information_schema.COLUMNS
                                                                WHERE TABLE_SCHEMA = DATABASE()
                                                                    AND TABLE_NAME = 'part_item'
                                                                    AND COLUMN_NAME = 'is_manufacturer_warranty';
                                                                SET @sql = IF(@colExists = 0,
                                                                        'ALTER TABLE `part_item` ADD COLUMN `is_manufacturer_warranty` TINYINT(1) NOT NULL DEFAULT 0',
                                                                        'SELECT 0');
                                                                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
                                                ");

            migrationBuilder.CreateTable(
                name: "program",
                columns: table => new
                {
                    program_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    attachment_url = table.Column<string>(type: "varchar(500)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_by = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    updated_by = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program", x => x.program_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "program_detail",
                columns: table => new
                {
                    program_detail_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    program_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    service_type = table.Column<string>(type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    discount_percent = table.Column<int>(type: "int", nullable: true),
                    bonus_amount = table.Column<int>(type: "int", nullable: true),
                    recall_action = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program_detail", x => x.program_detail_id);
                    table.ForeignKey(
                        name: "FK_program_detail_part_part_id",
                        column: x => x.part_id,
                        principalTable: "part",
                        principalColumn: "part_id");
                    table.ForeignKey(
                        name: "FK_program_detail_program_program_id",
                        column: x => x.program_id,
                        principalTable: "program",
                        principalColumn: "program_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "program_model",
                columns: table => new
                {
                    program_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    vehicle_model_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program_model", x => new { x.program_id, x.vehicle_model_id });
                    table.ForeignKey(
                        name: "FK_program_model_model_vehicle_model_id",
                        column: x => x.vehicle_model_id,
                        principalTable: "model",
                        principalColumn: "model_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_program_model_program_program_id",
                        column: x => x.program_id,
                        principalTable: "program",
                        principalColumn: "program_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_program_detail_part_id",
                table: "program_detail",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_program_detail_program_id",
                table: "program_detail",
                column: "program_id");

            migrationBuilder.CreateIndex(
                name: "IX_program_model_vehicle_model_id",
                table: "program_model",
                column: "vehicle_model_id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointment_program_campaign_id",
                table: "appointment",
                column: "campaign_id",
                principalTable: "program",
                principalColumn: "program_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ev_check_detail_program_detail_campaign_detail_id",
                table: "ev_check_detail",
                column: "campaign_detail_id",
                principalTable: "program_detail",
                principalColumn: "program_detail_id");

            migrationBuilder.AddForeignKey(
                name: "FK_rma_detail_part_item_replace_part_id",
                table: "rma_detail",
                column: "replace_part_id",
                principalTable: "part_item",
                principalColumn: "part_item_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointment_program_campaign_id",
                table: "appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_ev_check_detail_program_detail_campaign_detail_id",
                table: "ev_check_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_rma_detail_part_item_replace_part_id",
                table: "rma_detail");

            migrationBuilder.DropTable(
                name: "program_detail");

            migrationBuilder.DropTable(
                name: "program_model");

            migrationBuilder.DropTable(
                name: "program");

            migrationBuilder.DropColumn(
                name: "is_manufacturer_warranty",
                table: "part_item");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "vehicle_part_item",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "vehicle_part_item",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "vehicle",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "vehicle",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "staff",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "staff",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "service_center",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "service_center",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "rma_detail",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "rma_detail",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "rma",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "rma",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "price_service",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "price_service",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "part_type",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "part_type",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "part",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "part",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "maintenance_stage_detail",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "maintenance_stage_detail",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "maintenance_stage",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "maintenance_stage",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "maintenance_plan",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "maintenance_plan",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "import_note",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "import_note",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "export_note",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "export_note",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ev_check",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ev_check",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "customer",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "customer",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "battery_check",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "battery_check",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "appointment",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "appointment",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "account",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "account",
                newName: "CreatedAt");

            migrationBuilder.UpdateData(
                table: "rma_detail",
                keyColumn: "rma_number",
                keyValue: null,
                column: "rma_number",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "rma_number",
                table: "rma_detail",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "replace_part_id",
                table: "rma_detail",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "campaign",
                columns: table => new
                {
                    campaign_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    end_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    model_name = table.Column<string>(type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campaign", x => x.campaign_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "campaign_detail",
                columns: table => new
                {
                    campaign_detail_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    campaign_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    part_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    action_type = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estimated_time = table.Column<int>(type: "int", nullable: true),
                    is_mandatory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    note = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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

            migrationBuilder.CreateIndex(
                name: "IX_campaign_detail_campaign_id",
                table: "campaign_detail",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_campaign_detail_part_id",
                table: "campaign_detail",
                column: "part_id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointment_campaign_campaign_id",
                table: "appointment",
                column: "campaign_id",
                principalTable: "campaign",
                principalColumn: "campaign_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ev_check_detail_campaign_detail_campaign_detail_id",
                table: "ev_check_detail",
                column: "campaign_detail_id",
                principalTable: "campaign_detail",
                principalColumn: "campaign_detail_id");

            migrationBuilder.AddForeignKey(
                name: "FK_rma_detail_part_item_replace_part_id",
                table: "rma_detail",
                column: "replace_part_id",
                principalTable: "part_item",
                principalColumn: "part_item_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
