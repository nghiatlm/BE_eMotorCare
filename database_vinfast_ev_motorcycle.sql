-- =====================================================
-- VINFAST ELECTRIC MOTORCYCLE MAINTENANCE DATABASE
-- Database Import Script for eMotoCare System
-- =====================================================
use eMotoCare;
-- Set character encoding
SET NAMES utf8mb4;
SET CHARACTER SET utf8mb4;

-- =====================================================
-- 1. INSERT PART TYPES (Loại linh kiện)
-- =====================================================
INSERT INTO part_type (part_type_id, name, description, type, status, created_at, updated_at) VALUES
(UUID(), N'Phanh', N'Các linh kiện liên quan đến hệ thống phanh', 'BRK', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Điện - Điều khiển', N'Các linh kiện điều khiển và hệ thống điện', 'ELEC', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Chế độ lái', N'Các thiết bị điều khiển lái xe', 'CTRL', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Chân chống', N'Chân chống và các bộ phận hỗ trợ', 'STAND', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Khóa', N'Hệ thống khóa và khoá cốp', 'LOCK', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Dầu phanh', N'Dầu phanh và hệ thống thủy lực', 'BRK_OIL', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Má phanh', N'Má phanh và các bộ phận phanh', 'BRK_PAD', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Ống dầu phanh', N'Ống dầu phanh và phụ kiện', 'BRK_PIPE', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Vành xe', N'Vành xe và các bộ phận liên quan', 'WHL_RIM', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Lốp xe', N'Lốp xe và công cụ thay lốp', 'WHL_TIRE', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Ổ trục bánh', N'Ổ trục và vòng bi bánh xe', 'STRG_BEAR', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Hệ thống treo', N'Hệ thống treo và giảm xóc', 'SUS', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Piston phanh', N'Piston phanh và caliper', 'BRK_CALI', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Động cơ', N'Động cơ điện và các bộ phận', 'ENG', 'ACTIVE', NOW(), NOW()),
(UUID(), N'Pin', N'Pin điện và quản lý năng lượng', 'BAT', 'ACTIVE', NOW(), NOW());

-- =====================================================
-- 2. INSERT PARTS (Linh kiện chi tiết)
-- =====================================================
INSERT INTO part (part_id, part_type_id, code, name, quantity, image, description, status, created_at, updated_at) VALUES
-- Brake System Parts
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Phanh' LIMIT 1), 'BR-001', N'Tay phanh trước', 50, NULL, N'Tay phanh trước hệ thống điện', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Phanh' LIMIT 1), 'BR-002', N'Tay phanh sau', 45, NULL, N'Tay phanh sau hệ thống điều khiển', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Dầu phanh' LIMIT 1), 'BR-OIL-001', N'Dầu phanh chất lượng cao', 100, NULL, N'Dầu phanh DOT 4 chuyên dụng', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Má phanh' LIMIT 1), 'BR-PAD-001', N'Má phanh trước', 60, NULL, N'Má phanh trước cao cấp', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Má phanh' LIMIT 1), 'BR-PAD-002', N'Má phanh sau', 60, NULL, N'Má phanh sau cao cấp', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Ống dầu phanh' LIMIT 1), 'BR-HOSE-001', N'Ống dầu phanh trước', 40, NULL, N'Ống dầu phanh trước chịu áp lực cao', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Ống dầu phanh' LIMIT 1), 'BR-HOSE-002', N'Ống dầu phanh sau', 40, NULL, N'Ống dầu phanh sau chịu áp lực cao', 'ACTIVE', NOW(), NOW()),

-- Wheel and Tire Parts
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Vành xe' LIMIT 1), 'WHL-RIM-001', N'Vành xe trước', 30, NULL, N'Vành xe trước hợp kim nhôm', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Vành xe' LIMIT 1), 'WHL-RIM-002', N'Vành xe sau', 30, NULL, N'Vành xe sau hợp kim nhôm', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Lốp xe' LIMIT 1), 'WHL-TIRE-001', N'Lốp trước 80/90-17', 50, NULL, N'Lốp xe máy trước 80/90-17 chính hãng', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Lốp xe' LIMIT 1), 'WHL-TIRE-002', N'Lốp sau 100/90-17', 50, NULL, N'Lốp xe máy sau 100/90-17 chính hãng', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Ổ trục bánh' LIMIT 1), 'BEAR-001', N'Vòng bi bánh trước', 35, NULL, N'Vòng bi bánh trước chất lượng cao', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Ổ trục bánh' LIMIT 1), 'BEAR-002', N'Vòng bi bánh sau', 35, NULL, N'Vòng bi bánh sau chất lượng cao', 'ACTIVE', NOW(), NOW()),

-- Control System Parts
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Chế độ lái' LIMIT 1), 'CTRL-001', N'Vô lăng điều khiển', 25, NULL, N'Vô lăng điều khiển hệ thống', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Chế độ lái' LIMIT 1), 'CTRL-002', N'Thanh ngang điều khiển', 25, NULL, N'Thanh ngang điều khiển hướng xe', 'ACTIVE', NOW(), NOW()),

-- Support System Parts
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Chân chống' LIMIT 1), 'STAND-001', N'Chân chống chính', 30, NULL, N'Chân chống chính kích hoạt tay', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Chân chống' LIMIT 1), 'STAND-002', N'Chân chống bổ trợ', 30, NULL, N'Chân chống bổ trợ an toàn', 'ACTIVE', NOW(), NOW()),

-- Lock System Parts
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Khóa' LIMIT 1), 'LOCK-001', N'Khóa mở cốp', 40, NULL, N'Khóa mở cốp tự động', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Khóa' LIMIT 1), 'LOCK-002', N'Khóa chính điện', 40, NULL, N'Khóa chính điều khiển bằng từ xa', 'ACTIVE', NOW(), NOW()),

-- Suspension Parts
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Hệ thống treo' LIMIT 1), 'SUS-001', N'Giảm xóc trước', 35, NULL, N'Giảm xóc trước chuyên dụng xe máy', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Hệ thống treo' LIMIT 1), 'SUS-002', N'Giảm xóc sau', 35, NULL, N'Giảm xóc sau chuyên dụng xe máy', 'ACTIVE', NOW(), NOW()),

-- Electrical System Parts
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Điện - Điều khiển' LIMIT 1), 'ELEC-001', N'Cảm biến phanh', 50, NULL, N'Cảm biến phanh hệ thống điều khiển', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Điện - Điều khiển' LIMIT 1), 'ELEC-002', N'Mô đun điều khiển chính', 20, NULL, N'Mô đun điều khiển chính xe máy', 'ACTIVE', NOW(), NOW()),

-- Battery System
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Pin' LIMIT 1), 'BAT-001', N'Pin nước chính', 15, NULL, N'Pin nước chính dung lượng cao', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Pin' LIMIT 1), 'BAT-002', N'Bộ quản lý pin', 15, NULL, N'Bộ quản lý pin BMS điện tử', 'ACTIVE', NOW(), NOW()),

-- Motor System
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Động cơ' LIMIT 1), 'MOT-001', N'Động cơ điện trực tiếp', 10, NULL, N'Động cơ điện trực tiếp công suất cao', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Động cơ' LIMIT 1), 'MOT-002', N'Bộ chuyển đổi điện', 10, NULL, N'Bộ chuyển đổi điện công suất cao', 'ACTIVE', NOW(), NOW());

-- =====================================================
-- 3. INSERT MAINTENANCE PLANS (Kế hoạch bảo dưỡng)
-- =====================================================
INSERT INTO maintenance_plan (maintenance_plan_id, code, name, description, unit, total_stages, effective_date, status, created_at, updated_at) VALUES
(UUID(), 'MP-001', N'Kế hoạch bảo dưỡng định kỳ - Cơ bản', N'Kế hoạch bảo dưỡng định kỳ cơ bản theo tiêu chuẩn VinFast', 'KILOMETER', 6, '2024-01-01', 'ACTIVE', NOW(), NOW()),
(UUID(), 'MP-002', N'Kế hoạch bảo dưỡng định kỳ - Nâng cao', N'Kế hoạch bảo dưỡng nâng cao với kiểm tra chi tiết', 'KILOMETER', 8, '2024-01-01', 'ACTIVE', NOW(), NOW()),
(UUID(), 'MP-003', N'Kế hoạch bảo dưỡng hàng năm', N'Kiểm tra toàn bộ hệ thống hàng năm', 'MONTH', 5, '2024-01-01', 'ACTIVE', NOW(), NOW()),
(UUID(), 'MP-004', N'Kế hoạch bảo dưỡng hệ thống pin', N'Bảo dưỡng định kỳ hệ thống pin điện', 'MONTH', 7, '2024-01-01', 'ACTIVE', NOW(), NOW()),
(UUID(), 'MP-005', N'Kế hoạch bảo dưỡng hệ thống phanh', N'Kiểm tra và bảo dưỡng hệ thống phanh', 'KILOMETER', 5, '2024-01-01', 'ACTIVE', NOW(), NOW());

-- =====================================================
-- 4. INSERT MAINTENANCE STAGES (Giai đoạn bảo dưỡng)
-- =====================================================
-- Maintenance Plan 1 Stages (MP-001)
INSERT INTO maintenance_stage (maintenance_stage_id, maintenance_plan_id, name, description, mileage, duration_month, estimated_time, status, created_at, updated_at) VALUES
('00000000-0000-0000-0000-000000000101', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1), N'Kiểm tra ban đầu - 1000 km', N'Kiểm tra toàn bộ hệ thống sau lần mua hàng', 'KM1000', 'MONTH_1', 30, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000102', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1), N'Kiểm tra định kỳ - 5000 km', N'Kiểm tra các bộ phận chính sau 5000 km', 'KM5000', 'MONTH_3', 45, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000103', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1), N'Bảo dưỡng - 10000 km', N'Bảo dưỡng toàn bộ sau 10000 km', 'KM10000', 'MONTH_6', 60, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000104', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1), N'Kiểm tra chi tiết - 15000 km', N'Kiểm tra chi tiết các hệ thống', 'KM15000', 'MONTH_9', 90, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000105', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1), N'Bảo dưỡng toàn diện - 20000 km', N'Bảo dưỡng toàn diện trước bảo hành', 'KM20000', 'MONTH_12', 120, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000106', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1), N'Kiểm tra sau bảo hành - 24000 km', N'Kiểm tra toàn bộ sau kết thúc bảo hành', 'KM24000', 'MONTH_18', 120, 'ACTIVE', NOW(), NOW());

-- Maintenance Plan 2 Stages (MP-002)
INSERT INTO maintenance_stage (maintenance_stage_id, maintenance_plan_id, name, description, mileage, duration_month, estimated_time, status, created_at, updated_at) VALUES
('00000000-0000-0000-0000-000000000201', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1), N'Kiểm tra ban đầu - 1000 km', N'Kiểm tra toàn bộ hệ thống sau lần mua hàng', 'KM1000', 'MONTH_1', 30, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000202', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1), N'Kiểm tra định kỳ - 3000 km', N'Kiểm tra nhanh các bộ phận quan trọng', 'KM3000', 'MONTH_2', 30, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000203', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1), N'Kiểm tra định kỳ - 7000 km', N'Kiểm tra chi tiết hệ thống phanh', 'KM7000', 'MONTH_4', 45, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000204', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1), N'Bảo dưỡng - 12000 km', N'Bảo dưỡng toàn bộ sau 12000 km', 'KM12000', 'MONTH_6', 60, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000205', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1), N'Kiểm tra chi tiết - 18000 km', N'Kiểm tra chi tiết các hệ thống điện', 'KM18000', 'MONTH_9', 90, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000206', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1), N'Kiểm tra chi tiết - 24000 km', N'Kiểm tra chi tiết toàn bộ', 'KM24000', 'MONTH_12', 120, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000207', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1), N'Bảo dưỡng toàn diện - 30000 km', N'Bảo dưỡng toàn diện sau 30000 km', 'KM30000', 'MONTH_18', 150, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000208', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1), N'Kiểm tra sau bảo hành - 36000 km', N'Kiểm tra toàn bộ sau kết thúc bảo hành', 'KM36000', 'MONTH_24', 150, 'ACTIVE', NOW(), NOW());

-- Maintenance Plan 3 Stages (MP-003)
INSERT INTO maintenance_stage (maintenance_stage_id, maintenance_plan_id, name, description, mileage, duration_month, estimated_time, status, created_at, updated_at) VALUES
('00000000-0000-0000-0000-000000000301', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-003' LIMIT 1), N'Kiểm tra hàng năm - Tháng 3', N'Kiểm tra bảo dưỡng hàng năm - lần 1', 'KM1000', 'MONTH_3', 90, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000302', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-003' LIMIT 1), N'Kiểm tra hàng năm - Tháng 6', N'Kiểm tra bảo dưỡng hàng năm - lần 2', 'KM5000', 'MONTH_6', 90, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000303', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-003' LIMIT 1), N'Kiểm tra hàng năm - Tháng 9', N'Kiểm tra bảo dưỡng hàng năm - lần 3', 'KM10000', 'MONTH_9', 90, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000304', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-003' LIMIT 1), N'Kiểm tra hàng năm - Tháng 12', N'Kiểm tra bảo dưỡng hàng năm - lần 4', 'KM15000', 'MONTH_12', 120, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000305', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-003' LIMIT 1), N'Kiểm tra ngoài - Tháng 3', N'Kiểm tra ngoài bảo hành', 'KM20000', 'MONTH_15', 120, 'ACTIVE', NOW(), NOW());

-- Maintenance Plan 4 Stages (MP-004)
INSERT INTO maintenance_stage (maintenance_stage_id, maintenance_plan_id, name, description, mileage, duration_month, estimated_time, status, created_at, updated_at) VALUES
('00000000-0000-0000-0000-000000000401', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-004' LIMIT 1), N'Kiểm tra pin - 1000 km', N'Kiểm tra hệ thống pin sau mua', 'KM1000', 'MONTH_1', 30, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000402', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-004' LIMIT 1), N'Kiểm tra pin - 5000 km', N'Kiểm tra sức khỏe pin định kỳ', 'KM5000', 'MONTH_3', 30, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000403', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-004' LIMIT 1), N'Kiểm tra pin - 10000 km', N'Kiểm tra chi tiết pin và BMS', 'KM10000', 'MONTH_6', 45, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000404', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-004' LIMIT 1), N'Bảo dưỡng pin - 15000 km', N'Bảo dưỡng pin và hệ thống quản lý', 'KM15000', 'MONTH_9', 60, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000405', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-004' LIMIT 1), N'Kiểm tra chi tiết pin - 20000 km', N'Kiểm tra chi tiết dung lượng pin', 'KM20000', 'MONTH_12', 90, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000406', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-004' LIMIT 1), N'Kiểm tra bảo hành pin - 24000 km', N'Kiểm tra bảo hành pin trước hết hạn', 'KM24000', 'MONTH_18', 90, 'ACTIVE', NOW(), NOW()),
('00000000-0000-0000-0000-000000000407', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-004' LIMIT 1), N'Kiểm tra nâng cao pin - 30000 km', N'Kiểm tra nâng cao pin sau bảo hành', 'KM30000', 'MONTH_24', 120, 'ACTIVE', NOW(), NOW());

INSERT INTO maintenance_stage (maintenance_stage_id, maintenance_plan_id, name, description, mileage, duration_month, estimated_time, status, created_at, updated_at) VALUES
	('00000000-0000-0000-0000-000000000501', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-005' LIMIT 1), N'Kiểm tra phanh - 1000 km', N'Kiểm tra hệ thống phanh ban đầu', 'KM1000', 'MONTH_1', 30, 'ACTIVE', NOW(), NOW()),
	('00000000-0000-0000-0000-000000000502', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-005' LIMIT 1), N'Kiểm tra phanh - 5000 km', N'Kiểm tra hiệu suất phanh', 'KM5000', 'MONTH_3', 30, 'ACTIVE', NOW(), NOW()),
	('00000000-0000-0000-0000-000000000503', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-005' LIMIT 1), N'Kiểm tra phanh - 10000 km', N'Kiểm tra chi tiết và mài phanh nếu cần', 'KM10000', 'MONTH_6', 45, 'ACTIVE', NOW(), NOW()),
	('00000000-0000-0000-0000-000000000504', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-005' LIMIT 1), N'Thay thế phanh - 15000 km', N'Thay thế ma phanh nếu lấm lỉ', 'KM15000', 'MONTH_9', 60, 'ACTIVE', NOW(), NOW()),
	('00000000-0000-0000-0000-000000000505', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-005' LIMIT 1), N'Kiểm tra chi tiết phanh - 20000 km', N'Kiểm tra chi tiết cả hệ thống phanh', 'KM20000', 'MONTH_12', 90, 'ACTIVE', NOW(), NOW());

-- =====================================================
-- 5. INSERT MAINTENANCE STAGE DETAILS
-- =====================================================
-- Insert some sample stage details linking parts
INSERT INTO maintenance_stage_detail (maintenance_stage_detail_id, maintenance_stage_id, part_id, action_type, description, status, created_at, updated_at) VALUES
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE name = N'Kiểm tra ban đầu - 1000 km' LIMIT 1), (SELECT part_id FROM part WHERE code = 'BR-001' LIMIT 1), '["INSPECTION"]', N'Kiểm tra tay phanh trước', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE name = N'Kiểm tra ban đầu - 1000 km' LIMIT 1), (SELECT part_id FROM part WHERE code = 'BR-002' LIMIT 1), '["INSPECTION"]', N'Kiểm tra tay phanh sau', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE name = N'Kiểm tra ban đầu - 1000 km' LIMIT 1), (SELECT part_id FROM part WHERE code = 'WHL-TIRE-001' LIMIT 1), '["INSPECTION"]', N'Kiểm tra lốp trước', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE name = N'Kiểm tra ban đầu - 1000 km' LIMIT 1), (SELECT part_id FROM part WHERE code = 'WHL-TIRE-002' LIMIT 1), '["INSPECTION"]', N'Kiểm tra lốp sau', 'ACTIVE', NOW(), NOW());

-- =====================================================
-- 6. INSERT MODELS (Mẫu xe)
-- =====================================================
INSERT INTO model (model_id, code, name, manufacturer, maintenance_plan_id, status) VALUES
(UUID(), 'VF-ES-2024', N'VinFast Evo', N'VinFast', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1), 'ACTIVE'),
(UUID(), 'VF-EM-2024', N'VinFast Evo Max', N'VinFast', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1), 'ACTIVE'),
(UUID(), 'VF-EL-2024', N'VinFast Evo Lite', N'VinFast', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1), 'ACTIVE'),
(UUID(), 'VF-EP-2024', N'VinFast Evo Plus', N'VinFast', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1), 'ACTIVE'),
(UUID(), 'VF-ER-2024', N'VinFast Evo Rider', N'VinFast', (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-003' LIMIT 1), 'ACTIVE');

-- =====================================================
-- 7. INSERT MODEL PARTS (Linh kiện của mẫu xe)
-- =====================================================
INSERT INTO model_part (model_part_id, model_id, part_id) VALUES
(UUID(), (SELECT model_id FROM model WHERE code = 'VF-ES-2024' LIMIT 1), (SELECT part_id FROM part WHERE code = 'BR-001' LIMIT 1)),
(UUID(), (SELECT model_id FROM model WHERE code = 'VF-ES-2024' LIMIT 1), (SELECT part_id FROM part WHERE code = 'WHL-TIRE-001' LIMIT 1)),
(UUID(), (SELECT model_id FROM model WHERE code = 'VF-ES-2024' LIMIT 1), (SELECT part_id FROM part WHERE code = 'BAT-001' LIMIT 1)),
(UUID(), (SELECT model_id FROM model WHERE code = 'VF-EM-2024' LIMIT 1), (SELECT part_id FROM part WHERE code = 'BR-001' LIMIT 1)),
(UUID(), (SELECT model_id FROM model WHERE code = 'VF-EM-2024' LIMIT 1), (SELECT part_id FROM part WHERE code = 'BAT-001' LIMIT 1)),
(UUID(), (SELECT model_id FROM model WHERE code = 'VF-EM-2024' LIMIT 1), (SELECT part_id FROM part WHERE code = 'MOT-001' LIMIT 1)),
(UUID(), (SELECT model_id FROM model WHERE code = 'VF-EL-2024' LIMIT 1), (SELECT part_id FROM part WHERE code = 'WHL-TIRE-002' LIMIT 1)),
(UUID(), (SELECT model_id FROM model WHERE code = 'VF-EL-2024' LIMIT 1), (SELECT part_id FROM part WHERE code = 'LOCK-001' LIMIT 1));

-- =====================================================
-- 8. INSERT PART ITEMS (Số lượng linh kiện trong kho)
-- =====================================================
-- This section will be populated later after service centers are created

-- =====================================================
-- 9. INSERT PRICE SERVICES (Dịch vụ và giá tiền)
-- =====================================================
INSERT INTO price_service (price_service_id, part_type_id, code, remedies, name, labor_cost, effective_date, price, description, status, created_at, updated_at) VALUES
-- Brake System Services
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Phanh' LIMIT 1), 'PS-BR-001', 'INSPECTION', N'Kiểm tra hệ thống phanh', 150000, '2024-01-01', 0, N'Kiểm tra toàn bộ hệ thống phanh', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Phanh' LIMIT 1), 'PS-BR-002', 'CLEAN', N'Làm sạch hệ thống phanh', 200000, '2024-01-01', 0, N'Vệ sinh và làm sạch các bộ phận phanh', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Phanh' LIMIT 1), 'PS-BR-003', 'TUNE', N'Căn chỉnh phanh', 250000, '2024-01-01', 0, N'Căn chỉnh độ nhạy phanh', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Phanh' LIMIT 1), 'PS-BR-004', 'REPLACE', N'Thay thế linh kiện phanh', 300000, '2024-01-01', 150000, N'Thay thế ma phanh hoặc ống dầu phanh', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Phanh' LIMIT 1), 'PS-BR-005', 'WARRANTY', N'Bảo hành hệ thống phanh', 0, '2024-01-01', 0, N'Bảo hành 24 tháng hệ thống phanh', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Phanh' LIMIT 1), 'PS-BR-006', 'REPAIR', N'Sửa chữa hệ thống phanh', 200000, '2024-01-01', 50000, N'Sửa chữa các bộ phận phanh bị hỏng', 'ACTIVE', NOW(), NOW()),

-- Wheel and Tire Services
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Lốp xe' LIMIT 1), 'PS-WHL-001', 'INSPECTION', N'Kiểm tra vành và lốp xe', 150000, '2024-01-01', 0, N'Kiểm tra tình trạng vành và lốp', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Lốp xe' LIMIT 1), 'PS-WHL-002', 'CLEAN', N'Vệ sinh vành xe', 100000, '2024-01-01', 0, N'Làm sạch vành xe', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Lốp xe' LIMIT 1), 'PS-WHL-003', 'TUNE', N'Cân bằng bánh xe', 200000, '2024-01-01', 0, N'Cân bằng và canh chỉnh bánh xe', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Lốp xe' LIMIT 1), 'PS-WHL-004', 'REPLACE', N'Thay lốp xe', 250000, '2024-01-01', 200000, N'Thay lốp xe mới chính hãng', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Lốp xe' LIMIT 1), 'PS-WHL-005', 'REPAIR', N'Sửa chữa vành xe', 200000, '2024-01-01', 50000, N'Sửa chữa vành xe bị hỏng', 'ACTIVE', NOW(), NOW()),

-- Battery System Services
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Pin' LIMIT 1), 'PS-BAT-001', 'INSPECTION', N'Kiểm tra pin điện', 200000, '2024-01-01', 0, N'Kiểm tra sức khỏe pin và BMS', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Pin' LIMIT 1), 'PS-BAT-002', 'CLEAN', N'Vệ sinh hệ thống pin', 150000, '2024-01-01', 0, N'Vệ sinh và kiểm tra các kết nối pin', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Pin' LIMIT 1), 'PS-BAT-003', 'TUNE', N'Cân bằng pin', 250000, '2024-01-01', 0, N'Cân bằng các cell pin', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Pin' LIMIT 1), 'PS-BAT-004', 'REPLACE', N'Thay thế pin', 1000000, '2024-01-01', 5000000, N'Thay thế pin nước mới', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Pin' LIMIT 1), 'PS-BAT-005', 'WARRANTY', N'Bảo hành pin', 0, '2024-01-01', 0, N'Bảo hành pin 36 tháng', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Pin' LIMIT 1), 'PS-BAT-006', 'REPAIR', N'Sửa chữa hệ thống pin', 300000, '2024-01-01', 100000, N'Sửa chữa các bộ phận pin bị hỏng', 'ACTIVE', NOW(), NOW()),

-- Suspension System Services
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Hệ thống treo' LIMIT 1), 'PS-SUS-001', 'INSPECTION', N'Kiểm tra hệ thống treo', 150000, '2024-01-01', 0, N'Kiểm tra giảm xóc và bộ treo', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Hệ thống treo' LIMIT 1), 'PS-SUS-002', 'CLEAN', N'Vệ sinh hệ thống treo', 100000, '2024-01-01', 0, N'Làm sạch bộ treo và giảm xóc', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Hệ thống treo' LIMIT 1), 'PS-SUS-003', 'TUNE', N'Căn chỉnh độ cứng treo', 200000, '2024-01-01', 0, N'Điều chỉnh độ cứng hệ thống treo', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Hệ thống treo' LIMIT 1), 'PS-SUS-004', 'REPLACE', N'Thay thế giảm xóc', 300000, '2024-01-01', 200000, N'Thay thế giảm xóc mới', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Hệ thống treo' LIMIT 1), 'PS-SUS-005', 'REPAIR', N'Sửa chữa hệ thống treo', 200000, '2024-01-01', 50000, N'Sửa chữa các bộ phận treo', 'ACTIVE', NOW(), NOW()),

-- Control System Services
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Chế độ lái' LIMIT 1), 'PS-CTRL-001', 'INSPECTION', N'Kiểm tra hệ thống điều khiển', 150000, '2024-01-01', 0, N'Kiểm tra hệ thống điều khiển và xử lý', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Chế độ lái' LIMIT 1), 'PS-CTRL-002', 'CLEAN', N'Vệ sinh hệ thống điều khiển', 100000, '2024-01-01', 0, N'Làm sạch các cảm biến điều khiển', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Chế độ lái' LIMIT 1), 'PS-CTRL-003', 'TUNE', N'Cập nhật firmware điều khiển', 200000, '2024-01-01', 0, N'Cập nhật phần mềm điều khiển', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Chế độ lái' LIMIT 1), 'PS-CTRL-004', 'REPLACE', N'Thay thế bộ điều khiển', 500000, '2024-01-01', 300000, N'Thay thế bộ điều khiển chính', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Chế độ lái' LIMIT 1), 'PS-CTRL-005', 'WARRANTY', N'Bảo hành hệ thống điều khiển', 0, '2024-01-01', 0, N'Bảo hành hệ thống điều khiển 24 tháng', 'ACTIVE', NOW(), NOW()),
(UUID(), (SELECT part_type_id FROM part_type WHERE name = N'Chế độ lái' LIMIT 1), 'PS-CTRL-006', 'REPAIR', N'Sửa chữa hệ thống điều khiển', 250000, '2024-01-01', 80000, N'Sửa chữa các lỗi điều khiển', 'ACTIVE', NOW(), NOW());

-- =====================================================
-- 10. INSERT SERVICE CENTERS (Trung tâm dịch vụ - chỉ TP.HCM)
-- =====================================================
INSERT INTO service_center (service_center_id, code, name, description, email, phone, address, latitude, longitude, status, created_at, updated_at) VALUES
(UUID(), 'SC-HCM-001', N'Trung tâm dịch vụ VinFast TP.HCM - Quận 1', N'Trung tâm dịch vụ chính VinFast tại TP.HCM', 'service.hcm01@vinfast.com', '+84-8-2345-6789', N'789 Đường Nguyễn Huệ, Quận 1, TP.HCM', '10.7769', '106.6955', 'ACTIVE', NOW(), NOW()),
(UUID(), 'SC-HCM-002', N'Trung tâm dịch vụ VinFast TP.HCM - Quận 7', N'Trung tâm dịch vụ VinFast tại quận 7', 'service.hcm02@vinfast.com', '+84-8-3456-7890', N'321 Đường Lê Lợi, Quận 7, TP.HCM', '10.7621', '106.6956', 'ACTIVE', NOW(), NOW());

-- =====================================================
-- 11. INSERT SERVICE CENTER INVENTORIES (chỉ TP.HCM)
-- =====================================================
INSERT INTO service_center_inventory (service_center_inventory_id, service_center_id, name, status) VALUES
(UUID(), (SELECT service_center_id FROM service_center WHERE code = 'SC-HCM-001' LIMIT 1), N'Kho chính SC TP.HCM 01', 'ACTIVE'),
(UUID(), (SELECT service_center_id FROM service_center WHERE code = 'SC-HCM-002' LIMIT 1), N'Kho chính SC TP.HCM 02', 'ACTIVE');

-- =====================================================
-- 12. INSERT PART ITEMS INTO SERVICE CENTER INVENTORIES (chỉ TP.HCM)
-- =====================================================
-- Insert part items for SC TP.HCM 01
INSERT INTO part_item (part_item_id, part_id, quantity, serial_number, price, status, warranty_period, waranty_start_date, waranty_end_date, service_center_inventory_id, is_manufacturer_warranty) VALUES
(UUID(), (SELECT part_id FROM part WHERE code = 'BR-001' LIMIT 1), 15, 'SN-BR-001-001', 150000, 'IN_STOCK', 24, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 01' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'BR-002' LIMIT 1), 12, 'SN-BR-002-001', 150000, 'IN_STOCK', 24, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 01' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'BR-PAD-001' LIMIT 1), 20, 'SN-BR-PAD-001-001', 100000, 'IN_STOCK', 12, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 01' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'WHL-TIRE-001' LIMIT 1), 12, 'SN-WHL-001-001', 200000, 'IN_STOCK', 24, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 01' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'WHL-TIRE-002' LIMIT 1), 10, 'SN-WHL-002-001', 250000, 'IN_STOCK', 24, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 01' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'BAT-001' LIMIT 1), 5, 'SN-BAT-001-001', 5000000, 'IN_STOCK', 36, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 01' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'MOT-001' LIMIT 1), 2, 'SN-MOT-001-001', 8000000, 'IN_STOCK', 24, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 01' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'SUS-001' LIMIT 1), 8, 'SN-SUS-001-001', 1200000, 'IN_STOCK', 12, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 01' LIMIT 1), true),

-- Insert part items for SC TP.HCM 02
(UUID(), (SELECT part_id FROM part WHERE code = 'BR-001' LIMIT 1), 10, 'SN-BR-001-002', 150000, 'IN_STOCK', 24, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 02' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'BR-PAD-002' LIMIT 1), 18, 'SN-BR-PAD-002-001', 120000, 'IN_STOCK', 12, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 02' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'WHL-RIM-001' LIMIT 1), 6, 'SN-WHL-RIM-001-001', 800000, 'IN_STOCK', 24, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 02' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'WHL-RIM-002' LIMIT 1), 6, 'SN-WHL-RIM-002-001', 850000, 'IN_STOCK', 24, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 02' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'BAT-002' LIMIT 1), 3, 'SN-BAT-002-001', 2000000, 'IN_STOCK', 24, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 02' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'LOCK-001' LIMIT 1), 8, 'SN-LOCK-001-001', 500000, 'IN_STOCK', 12, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 02' LIMIT 1), true),
(UUID(), (SELECT part_id FROM part WHERE code = 'ELEC-001' LIMIT 1), 10, 'SN-ELEC-001-001', 300000, 'IN_STOCK', 24, NULL, NULL, (SELECT service_center_inventory_id FROM service_center_inventory WHERE name = N'Kho chính SC TP.HCM 02' LIMIT 1), true);

-- =====================================================
-- 12B. INSERT INSTALLED PART ITEMS (Linh kiện được gắn trên xe)
-- =====================================================
-- Vehicle 1 (VF-ESC-001-2024) - 3 parts installed
INSERT INTO part_item (part_item_id, part_id, quantity, serial_number, price, status, warranty_period, waranty_start_date, waranty_end_date, service_center_inventory_id, is_manufacturer_warranty) VALUES
(UUID(), (SELECT part_id FROM part WHERE code = 'BR-001' LIMIT 1), 1, 'SN-BR-001-INST-001', 150000, 'INSTALLED', 24, '2024-02-01', DATE_ADD('2024-02-01', INTERVAL 24 MONTH), NULL, true),
(UUID(), (SELECT part_id FROM part WHERE code = 'WHL-TIRE-001' LIMIT 1), 1, 'SN-WHL-001-INST-001', 200000, 'INSTALLED', 24, '2024-02-01', DATE_ADD('2024-02-01', INTERVAL 24 MONTH), NULL, true),
(UUID(), (SELECT part_id FROM part WHERE code = 'BAT-001' LIMIT 1), 1, 'SN-BAT-001-INST-001', 5000000, 'INSTALLED', 36, '2024-02-01', DATE_ADD('2024-02-01', INTERVAL 36 MONTH), NULL, true),

-- Vehicle 2 (VF-EMC-001-2024) - 3 parts installed
(UUID(), (SELECT part_id FROM part WHERE code = 'BR-001' LIMIT 1), 1, 'SN-BR-001-INST-002', 150000, 'INSTALLED', 24, '2024-02-05', DATE_ADD('2024-02-05', INTERVAL 24 MONTH), NULL, true),
(UUID(), (SELECT part_id FROM part WHERE code = 'BR-002' LIMIT 1), 1, 'SN-BR-002-INST-001', 150000, 'INSTALLED', 24, '2024-02-05', DATE_ADD('2024-02-05', INTERVAL 24 MONTH), NULL, true),
(UUID(), (SELECT part_id FROM part WHERE code = 'SUS-001' LIMIT 1), 1, 'SN-SUS-001-INST-001', 1200000, 'INSTALLED', 12, '2024-02-05', DATE_ADD('2024-02-05', INTERVAL 12 MONTH), NULL, true),

-- Vehicle 3 (VF-ELC-001-2024) - 2 parts installed
(UUID(), (SELECT part_id FROM part WHERE code = 'WHL-TIRE-002' LIMIT 1), 1, 'SN-WHL-002-INST-001', 250000, 'INSTALLED', 24, '2024-01-25', DATE_ADD('2024-01-25', INTERVAL 24 MONTH), NULL, true),
(UUID(), (SELECT part_id FROM part WHERE code = 'LOCK-001' LIMIT 1), 1, 'SN-LOCK-001-INST-001', 500000, 'INSTALLED', 12, '2024-01-25', DATE_ADD('2024-01-25', INTERVAL 12 MONTH), NULL, true),

-- Vehicle 4 (VF-EPC-001-2024) - 1 part installed
(UUID(), (SELECT part_id FROM part WHERE code = 'BAT-001' LIMIT 1), 1, 'SN-BAT-001-INST-002', 5000000, 'INSTALLED', 36, '2024-01-28', DATE_ADD('2024-01-28', INTERVAL 36 MONTH), NULL, true),

-- Vehicle 5 (VF-ESC-002-2024) - 2 parts installed
(UUID(), (SELECT part_id FROM part WHERE code = 'BR-001' LIMIT 1), 1, 'SN-BR-001-INST-003', 150000, 'INSTALLED', 24, '2024-02-10', DATE_ADD('2024-02-10', INTERVAL 24 MONTH), NULL, true),
(UUID(), (SELECT part_id FROM part WHERE code = 'WHL-TIRE-001' LIMIT 1), 1, 'SN-WHL-001-INST-002', 200000, 'INSTALLED', 24, '2024-02-10', DATE_ADD('2024-02-10', INTERVAL 24 MONTH), NULL, true);

-- =====================================================
-- 13. INSERT ACCOUNTS (Tài khoản người dùng)
-- =====================================================
-- Password: 123456789 (hashed using SHA2)
INSERT INTO account (account_id, phone, email, password, status, role_name, login_count, created_at, updated_at) VALUES
-- Admin Account
(UUID(), '0900000001', 'admin@vinfast.com', '$2a$12$Hn2hZQA0e8YNeqTndD8K7u.mwrL1C3c9qU0xgT/lh1LkNT42lcmUu', 'ACTIVE', 'ROLE_ADMIN', 0, NOW(), NOW()),

-- Manager Account
(UUID(), '0900000002', 'manager@vinfast.com', '$2a$12$Hn2hZQA0e8YNeqTndD8K7u.mwrL1C3c9qU0xgT/lh1LkNT42lcmUu', 'ACTIVE', 'ROLE_MANAGER', 0, NOW(), NOW()),

-- Staff Accounts
(UUID(), '0900000003', 'staff.01@vinfast.com', '$2a$12$Hn2hZQA0e8YNeqTndD8K7u.mwrL1C3c9qU0xgT/lh1LkNT42lcmUu', 'ACTIVE', 'ROLE_STAFF', 0, NOW(), NOW()),
(UUID(), '0900000004', 'staff.02@vinfast.com', '$2a$12$Hn2hZQA0e8YNeqTndD8K7u.mwrL1C3c9qU0xgT/lh1LkNT42lcmUu', 'ACTIVE', 'ROLE_STAFF', 0, NOW(), NOW()),

-- Technician Accounts
(UUID(), '0900000005', 'tech.01@vinfast.com', '$2a$12$Hn2hZQA0e8YNeqTndD8K7u.mwrL1C3c9qU0xgT/lh1LkNT42lcmUu', 'ACTIVE', 'ROLE_TECHNICIAN', 0, NOW(), NOW()),
(UUID(), '0900000006', 'tech.02@vinfast.com', '$2a$12$Hn2hZQA0e8YNeqTndD8K7u.mwrL1C3c9qU0xgT/lh1LkNT42lcmUu', 'ACTIVE', 'ROLE_TECHNICIAN', 0, NOW(), NOW()),

-- Storekeeper Account
(UUID(), '0900000007', 'storekeeper@vinfast.com', '$2a$12$Hn2hZQA0e8YNeqTndD8K7u.mwrL1C3c9qU0xgT/lh1LkNT42lcmUu', 'ACTIVE', 'ROLE_STOREKEEPER', 0, NOW(), NOW()),

-- Customer Accounts
(UUID(), '0901234567', 'customer.001@email.com', '$2a$12$Hn2hZQA0e8YNeqTndD8K7u.mwrL1C3c9qU0xgT/lh1LkNT42lcmUu', 'ACTIVE', 'ROLE_CUSTOMER', 0, NOW(), NOW()),
(UUID(), '0912345678', 'customer.002@email.com', '$2a$12$Hn2hZQA0e8YNeqTndD8K7u.mwrL1C3c9qU0xgT/lh1LkNT42lcmUu', 'ACTIVE', 'ROLE_CUSTOMER', 0, NOW(), NOW()),
(UUID(), '0923456789', 'customer.003@email.com', '$2a$12$Hn2hZQA0e8YNeqTndD8K7u.mwrL1C3c9qU0xgT/lh1LkNT42lcmUu', 'ACTIVE', 'ROLE_CUSTOMER', 0, NOW(), NOW()),
(UUID(), '0934567890', 'customer.004@email.com', '$2a$12$Hn2hZQA0e8YNeqTndD8K7u.mwrL1C3c9qU0xgT/lh1LkNT42lcmUu', 'ACTIVE', 'ROLE_CUSTOMER', 0, NOW(), NOW()),
(UUID(), '0945678901', 'customer.005@email.com', '$2a$12$Hn2hZQA0e8YNeqTndD8K7u.mwrL1C3c9qU0xgT/lh1LkNT42lcmUu', 'ACTIVE', 'ROLE_CUSTOMER', 0, NOW(), NOW());

-- =====================================================
-- 13B. INSERT STAFF (Nhân viên)
-- =====================================================
INSERT INTO staff (staff_id, staff_code, first_name, last_name, address, citizen_id, date_of_birth, gender, position, service_center_id, account_id, created_at, updated_at) VALUES
-- Admin linked to Staff
(UUID(), 'STF-ADMIN-001', N'Nguyễn', N'Quản Trị', N'TP.HCM', '111111111', '1990-01-01', 'MALE', 'MANAGER_BRANCH', (SELECT service_center_id FROM service_center WHERE code = 'SC-HCM-001' LIMIT 1), (SELECT account_id FROM account WHERE email = 'admin@vinfast.com' LIMIT 1), NOW(), NOW()),

-- Manager linked to Staff
(UUID(), 'STF-MGR-001', N'Trần', N'Quản Lý', N'TP.HCM', '222222222', '1990-06-01', 'MALE', 'MANAGER_BRANCH', (SELECT service_center_id FROM service_center WHERE code = 'SC-HCM-001' LIMIT 1), (SELECT account_id FROM account WHERE email = 'manager@vinfast.com' LIMIT 1), NOW(), NOW()),

-- Staff 1
(UUID(), 'STF-001', N'Phạm', N'Nhân Viên 1', N'TP.HCM', '333333333', '1995-03-15', 'MALE', 'SERVICE_STAFF', (SELECT service_center_id FROM service_center WHERE code = 'SC-HCM-002' LIMIT 1), (SELECT account_id FROM account WHERE email = 'staff.01@vinfast.com' LIMIT 1), NOW(), NOW()),

-- Staff 2
(UUID(), 'STF-002', N'Vũ', N'Nhân Viên 2', N'TP.HCM', '444444444', '1996-07-20', 'FEMALE', 'SERVICE_STAFF', (SELECT service_center_id FROM service_center WHERE code = 'SC-HCM-002' LIMIT 1), (SELECT account_id FROM account WHERE email = 'staff.02@vinfast.com' LIMIT 1), NOW(), NOW()),

-- Technician 1
(UUID(), 'STF-TECH-001', N'Hoàng', N'Kỹ Thuật 1', N'TP.HCM', '555555555', '1992-05-10', 'MALE', 'TECHNICIAN_STAFF', (SELECT service_center_id FROM service_center WHERE code = 'SC-HCM-001' LIMIT 1), (SELECT account_id FROM account WHERE email = 'tech.01@vinfast.com' LIMIT 1), NOW(), NOW()),

-- Technician 2
(UUID(), 'STF-TECH-002', N'Lê', N'Kỹ Thuật 2', N'TP.HCM', '666666666', '1993-09-25', 'FEMALE', 'TECHNICIAN_STAFF', (SELECT service_center_id FROM service_center WHERE code = 'SC-HCM-001' LIMIT 1), (SELECT account_id FROM account WHERE email = 'tech.02@vinfast.com' LIMIT 1), NOW(), NOW()),

-- Storekeeper
(UUID(), 'STF-STORE-001', N'Bùi', N'Kho', N'TP.HCM', '777777777', '1994-11-05', 'MALE', 'STORE_KEEPER', (SELECT service_center_id FROM service_center WHERE code = 'SC-HCM-002' LIMIT 1), (SELECT account_id FROM account WHERE email = 'storekeeper@vinfast.com' LIMIT 1), NOW(), NOW());

-- =====================================================
-- 13C. LINK ACCOUNTS TO CUSTOMERS
-- =====================================================
UPDATE customer SET account_id = (SELECT account_id FROM account WHERE email = 'customer.001@email.com' LIMIT 1) WHERE customer_code = 'CUST-001';
UPDATE customer SET account_id = (SELECT account_id FROM account WHERE email = 'customer.002@email.com' LIMIT 1) WHERE customer_code = 'CUST-002';
UPDATE customer SET account_id = (SELECT account_id FROM account WHERE email = 'customer.003@email.com' LIMIT 1) WHERE customer_code = 'CUST-003';
UPDATE customer SET account_id = (SELECT account_id FROM account WHERE email = 'customer.004@email.com' LIMIT 1) WHERE customer_code = 'CUST-004';
UPDATE customer SET account_id = (SELECT account_id FROM account WHERE email = 'customer.005@email.com' LIMIT 1) WHERE customer_code = 'CUST-005';

-- =====================================================
-- 13D. INSERT CUSTOMERS (Khách hàng - Chủ xe)
-- =====================================================
INSERT INTO customer (customer_id, customer_code, first_name, last_name, address, citizen_id, date_of_birth, gender, created_at, updated_at) VALUES
(UUID(), 'CUST-001', N'Nguyễn Văn', N'An', N'123 Đường Nguyễn Huệ, Quận 1, TP.HCM', '123456789', '1990-05-15', 'MALE', NOW(), NOW()),
(UUID(), 'CUST-002', N'Trần Thị', N'Bình', N'456 Đường Lê Lợi, Quận 7, TP.HCM', '987654321', '1992-08-20', 'FEMALE', NOW(), NOW()),
(UUID(), 'CUST-003', N'Phạm Minh', N'Chiến', N'789 Đường Pasteur, Quận 1, TP.HCM', '456789123', '1988-03-10', 'MALE', NOW(), NOW()),
(UUID(), 'CUST-004', N'Vũ Hương', N'Giang', N'321 Đường Tôn Đức Thắng, Quận 1, TP.HCM', '789123456', '1995-11-25', 'FEMALE', NOW(), NOW()),
(UUID(), 'CUST-005', N'Hoàng Gia', N'Huy', N'654 Đường Trần Hưng Đạo, Quận 5, TP.HCM', '321654987', '1991-07-08', 'MALE', NOW(), NOW());

-- =====================================================
-- 14. INSERT VEHICLES (Xe máy điện - với chủ xe)
-- =====================================================
INSERT INTO vehicle (vehicle_id, image, color, chassis_number, engine_number, status, manufacture_date, purchase_date, warranty_expiry, model_id, customer_id, is_primary, created_at, updated_at) VALUES
-- VinFast Evo for Customer 1
(UUID(), 'https://example.com/evo1.jpg', N'Trắng', 'VF-ESC-001-2024', 'VF-ENG-001-2024', 'ACTIVE', '2024-01-15', '2024-02-01', DATE_ADD('2024-02-01', INTERVAL 24 MONTH), (SELECT model_id FROM model WHERE code = 'VF-ES-2024' LIMIT 1), (SELECT customer_id FROM customer WHERE customer_code = 'CUST-001' LIMIT 1), true, NOW(), NOW()),

-- VinFast Evo Max for Customer 2
(UUID(), 'https://example.com/evomax1.jpg', N'Đen', 'VF-EMC-001-2024', 'VF-ENG-002-2024', 'ACTIVE', '2024-01-20', '2024-02-05', DATE_ADD('2024-02-05', INTERVAL 24 MONTH), (SELECT model_id FROM model WHERE code = 'VF-EM-2024' LIMIT 1), (SELECT customer_id FROM customer WHERE customer_code = 'CUST-002' LIMIT 1), true, NOW(), NOW()),

-- VinFast Evo Lite for Customer 3
(UUID(), 'https://example.com/evolite1.jpg', N'Xám', 'VF-ELC-001-2024', 'VF-ENG-003-2024', 'ACTIVE', '2024-01-10', '2024-01-25', DATE_ADD('2024-01-25', INTERVAL 24 MONTH), (SELECT model_id FROM model WHERE code = 'VF-EL-2024' LIMIT 1), (SELECT customer_id FROM customer WHERE customer_code = 'CUST-003' LIMIT 1), true, NOW(), NOW()),

-- VinFast Evo Plus for Customer 4
(UUID(), 'https://example.com/evoplus1.jpg', N'Đỏ', 'VF-EPC-001-2024', 'VF-ENG-004-2024', 'ACTIVE', '2024-01-12', '2024-01-28', DATE_ADD('2024-01-28', INTERVAL 24 MONTH), (SELECT model_id FROM model WHERE code = 'VF-EP-2024' LIMIT 1), (SELECT customer_id FROM customer WHERE customer_code = 'CUST-004' LIMIT 1), true, NOW(), NOW()),

-- VinFast Evo for Customer 5 (second vehicle)
(UUID(), 'https://example.com/evo2.jpg', N'Xanh', 'VF-ESC-002-2024', 'VF-ENG-005-2024', 'ACTIVE', '2024-02-01', '2024-02-10', DATE_ADD('2024-02-10', INTERVAL 24 MONTH), (SELECT model_id FROM model WHERE code = 'VF-ES-2024' LIMIT 1), (SELECT customer_id FROM customer WHERE customer_code = 'CUST-005' LIMIT 1), true, NOW(), NOW());

-- =====================================================
-- 15. INSERT VEHICLE PART ITEMS (Linh kiện được lắp trên từng xe)
-- =====================================================
INSERT INTO vehicle_part_item (vehicle_part_item_id, install_date, vehicle_id, part_item_id, replace_for_id, created_at, updated_at) VALUES
-- Vehicle 1 (VF-ESC-001-2024) - Tay phanh trước
(UUID(), '2024-02-01', (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-001-2024' LIMIT 1), (SELECT part_item_id FROM part_item WHERE serial_number = 'SN-BR-001-001' LIMIT 1), NULL, NOW(), NOW()),
-- Vehicle 1 - Lốp trước
(UUID(), '2024-02-01', (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-001-2024' LIMIT 1), (SELECT part_item_id FROM part_item WHERE serial_number = 'SN-WHL-001-001' LIMIT 1), NULL, NOW(), NOW()),
-- Vehicle 1 - Pin
(UUID(), '2024-02-01', (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-001-2024' LIMIT 1), (SELECT part_item_id FROM part_item WHERE serial_number = 'SN-BAT-001-001' LIMIT 1), NULL, NOW(), NOW()),

-- Vehicle 2 (VF-EMC-001-2024) - Tay phanh trước
(UUID(), '2024-02-05', (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EMC-001-2024' LIMIT 1), (SELECT part_item_id FROM part_item WHERE serial_number = 'SN-BR-001-002' LIMIT 1), NULL, NOW(), NOW()),
-- Vehicle 2 - Tay phanh sau
(UUID(), '2024-02-05', (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EMC-001-2024' LIMIT 1), (SELECT part_item_id FROM part_item WHERE serial_number = 'SN-BR-002-001' LIMIT 1), NULL, NOW(), NOW()),
-- Vehicle 2 - Giảm xóc trước
(UUID(), '2024-02-05', (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EMC-001-2024' LIMIT 1), (SELECT part_item_id FROM part_item WHERE serial_number = 'SN-SUS-001-001' LIMIT 1), NULL, NOW(), NOW()),

-- Vehicle 3 (VF-ELC-001-2024) - Lốp sau
(UUID(), '2024-01-25', (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ELC-001-2024' LIMIT 1), (SELECT part_item_id FROM part_item WHERE serial_number = 'SN-WHL-002-001' LIMIT 1), NULL, NOW(), NOW()),
-- Vehicle 3 - Khóa cốp
(UUID(), '2024-01-25', (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ELC-001-2024' LIMIT 1), (SELECT part_item_id FROM part_item WHERE serial_number = 'SN-LOCK-001-001' LIMIT 1), NULL, NOW(), NOW()),

-- Vehicle 4 (VF-EPC-001-2024) - Pin
(UUID(), '2024-01-28', (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EPC-001-2024' LIMIT 1), (SELECT part_item_id FROM part_item WHERE serial_number = 'SN-BAT-001-001' LIMIT 1), NULL, NOW(), NOW()),

-- Vehicle 5 (VF-ESC-002-2024) - Tay phanh trước
(UUID(), '2024-02-10', (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-002-2024' LIMIT 1), (SELECT part_item_id FROM part_item WHERE serial_number = 'SN-BR-001-001' LIMIT 1), NULL, NOW(), NOW()),
-- Vehicle 5 - Lốp trước
(UUID(), '2024-02-10', (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-002-2024' LIMIT 1), (SELECT part_item_id FROM part_item WHERE serial_number = 'SN-WHL-001-001' LIMIT 1), NULL, NOW(), NOW());

-- =====================================================
-- 15. INSERT VEHICLE STAGES (Các giai đoạn bảo dưỡng của từng xe)
-- =====================================================
-- Vehicle 1 (VF-ESC-001-2024) - Evo (2024-02-01) linked to MP-001 (6 stages)
INSERT INTO vehicle_stage (vehicle_stage_id, maintenance_stage_id, vehicle_id, actual_maintenance_mileage, actual_maintenance_unit, expected_implementation_date, expected_start_date, expected_end_date, status, created_at, updated_at) VALUES
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM1000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-01', INTERVAL 1 MONTH), DATE_SUB(DATE_ADD('2024-02-01', INTERVAL 1 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-01', INTERVAL 1 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM5000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-01', INTERVAL 3 MONTH), DATE_SUB(DATE_ADD('2024-02-01', INTERVAL 3 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-01', INTERVAL 3 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM10000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-01', INTERVAL 6 MONTH), DATE_SUB(DATE_ADD('2024-02-01', INTERVAL 6 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-01', INTERVAL 6 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM15000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-01', INTERVAL 9 MONTH), DATE_SUB(DATE_ADD('2024-02-01', INTERVAL 9 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-01', INTERVAL 9 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM20000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-01', INTERVAL 12 MONTH), DATE_SUB(DATE_ADD('2024-02-01', INTERVAL 12 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-01', INTERVAL 12 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM24000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-01', INTERVAL 18 MONTH), DATE_SUB(DATE_ADD('2024-02-01', INTERVAL 18 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-01', INTERVAL 18 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW());

-- Vehicle 2 (VF-EMC-001-2024) - Evo Max (2024-02-05) linked to MP-002 (8 stages)
INSERT INTO vehicle_stage (vehicle_stage_id, maintenance_stage_id, vehicle_id, actual_maintenance_mileage, actual_maintenance_unit, expected_implementation_date, expected_start_date, expected_end_date, status, created_at, updated_at) VALUES
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM1000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EMC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-05', INTERVAL 1 MONTH), DATE_SUB(DATE_ADD('2024-02-05', INTERVAL 1 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-05', INTERVAL 1 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM3000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EMC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-05', INTERVAL 2 MONTH), DATE_SUB(DATE_ADD('2024-02-05', INTERVAL 2 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-05', INTERVAL 2 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM7000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EMC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-05', INTERVAL 4 MONTH), DATE_SUB(DATE_ADD('2024-02-05', INTERVAL 4 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-05', INTERVAL 4 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM12000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EMC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-05', INTERVAL 6 MONTH), DATE_SUB(DATE_ADD('2024-02-05', INTERVAL 6 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-05', INTERVAL 6 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM18000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EMC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-05', INTERVAL 9 MONTH), DATE_SUB(DATE_ADD('2024-02-05', INTERVAL 9 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-05', INTERVAL 9 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM24000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EMC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-05', INTERVAL 12 MONTH), DATE_SUB(DATE_ADD('2024-02-05', INTERVAL 12 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-05', INTERVAL 12 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM30000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EMC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-05', INTERVAL 18 MONTH), DATE_SUB(DATE_ADD('2024-02-05', INTERVAL 18 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-05', INTERVAL 18 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM36000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EMC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-05', INTERVAL 24 MONTH), DATE_SUB(DATE_ADD('2024-02-05', INTERVAL 24 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-05', INTERVAL 24 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW());

-- Vehicle 3 (VF-ELC-001-2024) - Evo Lite (2024-01-25) linked to MP-001 (6 stages)
INSERT INTO vehicle_stage (vehicle_stage_id, maintenance_stage_id, vehicle_id, actual_maintenance_mileage, actual_maintenance_unit, expected_implementation_date, expected_start_date, expected_end_date, status, created_at, updated_at) VALUES
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM1000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ELC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-25', INTERVAL 1 MONTH), DATE_SUB(DATE_ADD('2024-01-25', INTERVAL 1 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-25', INTERVAL 1 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM5000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ELC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-25', INTERVAL 3 MONTH), DATE_SUB(DATE_ADD('2024-01-25', INTERVAL 3 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-25', INTERVAL 3 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM10000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ELC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-25', INTERVAL 6 MONTH), DATE_SUB(DATE_ADD('2024-01-25', INTERVAL 6 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-25', INTERVAL 6 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM15000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ELC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-25', INTERVAL 9 MONTH), DATE_SUB(DATE_ADD('2024-01-25', INTERVAL 9 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-25', INTERVAL 9 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM20000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ELC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-25', INTERVAL 12 MONTH), DATE_SUB(DATE_ADD('2024-01-25', INTERVAL 12 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-25', INTERVAL 12 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM24000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ELC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-25', INTERVAL 18 MONTH), DATE_SUB(DATE_ADD('2024-01-25', INTERVAL 18 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-25', INTERVAL 18 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW());

-- Vehicle 4 (VF-EPC-001-2024) - Evo Plus (2024-01-28) linked to MP-002 (8 stages)
INSERT INTO vehicle_stage (vehicle_stage_id, maintenance_stage_id, vehicle_id, actual_maintenance_mileage, actual_maintenance_unit, expected_implementation_date, expected_start_date, expected_end_date, status, created_at, updated_at) VALUES
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM1000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EPC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-28', INTERVAL 1 MONTH), DATE_SUB(DATE_ADD('2024-01-28', INTERVAL 1 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-28', INTERVAL 1 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM3000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EPC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-28', INTERVAL 2 MONTH), DATE_SUB(DATE_ADD('2024-01-28', INTERVAL 2 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-28', INTERVAL 2 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM7000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EPC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-28', INTERVAL 4 MONTH), DATE_SUB(DATE_ADD('2024-01-28', INTERVAL 4 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-28', INTERVAL 4 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM12000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EPC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-28', INTERVAL 6 MONTH), DATE_SUB(DATE_ADD('2024-01-28', INTERVAL 6 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-28', INTERVAL 6 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM18000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EPC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-28', INTERVAL 9 MONTH), DATE_SUB(DATE_ADD('2024-01-28', INTERVAL 9 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-28', INTERVAL 9 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM24000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EPC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-28', INTERVAL 12 MONTH), DATE_SUB(DATE_ADD('2024-01-28', INTERVAL 12 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-28', INTERVAL 12 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM30000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EPC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-28', INTERVAL 18 MONTH), DATE_SUB(DATE_ADD('2024-01-28', INTERVAL 18 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-28', INTERVAL 18 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-002' LIMIT 1) AND mileage = 'KM36000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-EPC-001-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-01-28', INTERVAL 24 MONTH), DATE_SUB(DATE_ADD('2024-01-28', INTERVAL 24 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-01-28', INTERVAL 24 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW());

-- Vehicle 5 (VF-ESC-002-2024) - Evo (2024-02-10) linked to MP-001 (6 stages)
INSERT INTO vehicle_stage (vehicle_stage_id, maintenance_stage_id, vehicle_id, actual_maintenance_mileage, actual_maintenance_unit, expected_implementation_date, expected_start_date, expected_end_date, status, created_at, updated_at) VALUES
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM1000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-002-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-10', INTERVAL 1 MONTH), DATE_SUB(DATE_ADD('2024-02-10', INTERVAL 1 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-10', INTERVAL 1 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM5000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-002-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-10', INTERVAL 3 MONTH), DATE_SUB(DATE_ADD('2024-02-10', INTERVAL 3 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-10', INTERVAL 3 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM10000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-002-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-10', INTERVAL 6 MONTH), DATE_SUB(DATE_ADD('2024-02-10', INTERVAL 6 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-10', INTERVAL 6 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM15000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-002-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-10', INTERVAL 9 MONTH), DATE_SUB(DATE_ADD('2024-02-10', INTERVAL 9 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-10', INTERVAL 9 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM20000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-002-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-10', INTERVAL 12 MONTH), DATE_SUB(DATE_ADD('2024-02-10', INTERVAL 12 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-10', INTERVAL 12 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW()),
(UUID(), (SELECT maintenance_stage_id FROM maintenance_stage WHERE maintenance_plan_id = (SELECT maintenance_plan_id FROM maintenance_plan WHERE code = 'MP-001' LIMIT 1) AND mileage = 'KM24000' LIMIT 1), (SELECT vehicle_id FROM vehicle WHERE chassis_number = 'VF-ESC-002-2024' LIMIT 1), 0, 'KILOMETER', DATE_ADD('2024-02-10', INTERVAL 18 MONTH), DATE_SUB(DATE_ADD('2024-02-10', INTERVAL 18 MONTH), INTERVAL 10 DAY), DATE_ADD(DATE_ADD('2024-02-10', INTERVAL 18 MONTH), INTERVAL 10 DAY), 'UPCOMING', NOW(), NOW());

-- =====================================================
-- 16. VERIFICATION QUERIES (Optional - for testing)
-- =====================================================
-- Verify data insertion counts
SELECT COUNT(*) AS 'Total Part Types' FROM part_type;
SELECT COUNT(*) AS 'Total Parts' FROM part;
SELECT COUNT(*) AS 'Total Maintenance Plans' FROM maintenance_plan;
SELECT COUNT(*) AS 'Total Maintenance Stages' FROM maintenance_stage;
SELECT COUNT(*) AS 'Total Models' FROM model;
SELECT COUNT(*) AS 'Total Price Services' FROM price_service;
SELECT COUNT(*) AS 'Total Service Centers (TP.HCM only)' FROM service_center;
SELECT COUNT(*) AS 'Total Customers' FROM customer;
SELECT COUNT(*) AS 'Total Vehicles' FROM vehicle;
SELECT COUNT(*) AS 'Total Vehicle Part Items' FROM vehicle_part_item;
SELECT COUNT(*) AS 'Total Part Items in Inventory' FROM part_item;

-- =====================================================
-- 17. VEHICLE STAGE STATUS ADJUSTMENT (based on current date)
-- =====================================================
-- Rules:
-- - UPCOMING: expected_start_date is today
-- - EXPIRED: current date is after expected_start_date but on/before expected_end_date
-- - NO_START: current date is after expected_end_date
-- Note: COMPLETED should be set by actual maintenance workflow, not here

-- Set UPCOMING for stages starting today
UPDATE vehicle_stage
SET status = 'UPCOMING'
WHERE DATE(expected_start_date) = CURDATE();

-- Set NO_START for stages whose window already ended
UPDATE vehicle_stage
SET status = 'NO_START'
WHERE CURDATE() > expected_end_date;

-- Set EXPIRED for stages where start has passed but still within end window
UPDATE vehicle_stage
SET status = 'EXPIRED'
WHERE CURDATE() > expected_start_date AND CURDATE() <= expected_end_date;

-- Verify MaintenancePlan has correct number of stages
SELECT mp.code, mp.name, mp.total_stages, COUNT(ms.maintenance_stage_id) AS stage_count
FROM maintenance_plan mp
LEFT JOIN maintenance_stage ms ON mp.maintenance_plan_id = ms.maintenance_plan_id
GROUP BY mp.maintenance_plan_id, mp.code, mp.name, mp.total_stages;

-- Verify PriceServices cover all Remedies
SELECT DISTINCT remedies FROM price_service ORDER BY remedies;

-- Verify Customer and Vehicle relationships
SELECT c.customer_code, CONCAT(c.first_name, ' ', c.last_name) AS customer_name, v.chassis_number, m.name AS model_name
FROM customer c
LEFT JOIN vehicle v ON c.customer_id = v.customer_id
LEFT JOIN model m ON v.model_id = m.model_id
ORDER BY c.customer_code;
