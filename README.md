# BE_eMotorCare

**Hệ thống Quản lý Dịch vụ Bảo dưỡng Xe Điện**

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](#)
[![License](https://img.shields.io/badge/license-MIT-blue)](#giấy-phép)
[![Version](https://img.shields.io/badge/version-1.0.0-blue)](#)
[![.NET Version](https://img.shields.io/badge/.NET-8.0-512BD4)](#)

## Mục lục

- [Giới thiệu](#giới-thiệu)
- [Tính năng chính](#tính-năng-chính)
- [Kiến trúc](#kiến-trúc)
- [Yêu cầu hệ thống](#yêu-cầu-hệ-thống)
- [Cài đặt nhanh](#cài-đặt-nhanh)
- [Cấu hình](#cấu-hình)
- [Sử dụng](#sử-dụng)
- [API Documentation](#api-documentation)
- [Cấu trúc Dự án](#cấu-trúc-dự-án)
- [Đóng góp](#đóng-góp)
- [Hỗ trợ](#hỗ-trợ)
- [Giấy phép](#giấy-phép)

## Giới thiệu

BE_eMotorCare là một ứng dụng backend được phát triển bằng **.NET 8** và **C#**, cung cấp một bộ API toàn diện cho việc quản lý dịch vụ bảo dưỡng và kiểm tra xe điện. Hệ thống được thiết kế theo kiến trúc phân tầng (Layered Architecture) để đảm bảo tính bảo trì, mở rộng và khả năng kiểm tra cao.

Dự án được phát triển như một phần của khóa học Capstone và hướng đến việc tạo ra một nền tảng quản lý dịch vụ ô tô điện toàn diện.

### Tính năng chính

BE_eMotorCare cung cấp các tính năng sau:

- **Quản lý Khách hàng**: Tạo, cập nhật, xóa thông tin khách hàng và theo dõi hồ sơ dịch vụ
- **Quản lý Xe Điện**: Quản lý danh sách xe, thông tin bộ phận, lịch sử bảo dưỡng
- **Đặt lịch Dịch vụ**: Hệ thống đặt lịch thông minh với quản lý slot thời gian
- **Kiểm tra và Bảo dưỡng**: Kiểm tra pin, kiểm tra hệ thống, kế hoạch bảo dưỡng định kỳ
- **Quản lý Trung tâm Dịch vụ**: Quản lý thông tin chi nhánh, nhân viên, kho linh kiện
- **Thanh toán**: Tích hợp cổng thanh toán, quản lý hóa đơn, xử lý RMA
- **Thông báo Real-time**: Sử dụng SignalR và Firebase cho thông báo tức thời
- **Dashboard**: Thống kê, báo cáo hoạt động, theo dõi hiệu suất

## Kiến trúc

Dự án sử dụng **Layered Architecture** (kiến trúc phân tầng) - một mô hình kiến trúc phổ biến giúp tách biệt các mối quan tâm (Separation of Concerns) và cải thiện tính bảo trì của code.

```
BE_eMotoCare/
├── BE_eMotoCare.API/          Tầng API (Controllers, Middlewares, Configuration)
├── eMotoCare.BO/              Tầng Business Objects (DTOs, Entities, Exceptions)
├── eMotoCare.DAL/             Tầng Data Access (Repositories, DbContext)
└── eMototCare.BLL/            Tầng Business Logic (Services, Mappers)
```

### Tầng API (BE_eMotoCare.API)

**Controllers** - Xử lý các HTTP requests
- AccountsController: Quản lý tài khoản người dùng
- AuthController: Xác thực và ủy quyền
- AppointmentsController: Quản lý cuộc hẹn dịch vụ
- VehiclesController: Quản lý dữ liệu xe điện
- CustomersController: Quản lý khách hàng
- BatteryChecksController: Kiểm tra tình trạng pin
- EVChecksController: Kiểm tra toàn bộ hệ thống xe điện
- MaintenancePlansController: Kế hoạch bảo dưỡng định kỳ
- ServiceCentersController: Quản lý trung tâm dịch vụ
- PartsController: Quản lý bộ phận và linh kiện
- CheckoutController: Xử lý thanh toán
- Các controller khác để quản lý các thành phần hệ thống

**Middlewares** - Xử lý logic chung cho tất cả requests
- ExceptionMiddleware: Xử lý exception toàn cục
- JwtMiddleware: Xác thực JWT tokens

**Configuration** - Cấu hình khởi động ứng dụng
- AutoMapperConfiguration: Cấu hình object mapping
- DependencyInjection: Đăng ký dependencies
- SwaggerConfig: Cấu hình API documentation

**Realtime** - Xử lý real-time communication
- SignalR Hubs: Kết nối hai chiều với clients

### Tầng Business Objects (eMotoCare.BO)

- Entities: Các lớp đại diện cho bảng trong cơ sở dữ liệu
- DTOs: Đối tượng truyền tải dữ liệu cho API requests và responses
- Exceptions: Các ngoại lệ tùy chỉnh cho ứng dụng
- Common: Lớp cơ sở và các cấu hình như JwtSettings, AISettings

### Tầng Data Access (eMotoCare.DAL)

- DbContext: Entity Framework Core context để giao tiếp với database
- Repositories: Triển khai các pattern truy cập dữ liệu
- Migrations: Quản lý các phiên bản schema của database
- UnitOfWork: Quản lý các transaction

### Tầng Business Logic (eMototCare.BLL)

- Services: Triển khai logic kinh doanh
- Mappers: Ánh xạ giữa DTOs và Entities
- JwtServices: Tạo và xác thực JWT tokens
## Yêu cầu hệ thống

Để cài đặt và chạy BE_eMotorCare, bạn cần:

| Thành phần | Phiên bản | Ghi chú |
|-----------|----------|--------|
| .NET SDK | 8.0 hoặc cao hơn | Yêu cầu bắt buộc |
| MySQL | 8.0 hoặc cao hơn | Cơ sở dữ liệu |
| Docker | Phiên bản mới nhất | Tùy chọn, dùng cho containerization |
| Visual Studio | 2022 hoặc mới hơn | Tùy chọn, IDE khuyến nghị |
| Git | Phiên bản mới nhất | Để clone repository |

## Cài đặt nhanh

### Yêu cầu tiên quyết

Đảm bảo bạn đã cài đặt các công cụ bắt buộc ở trên.

### Clone Repository

```bash
git clone https://github.com/nghiatlm/BE_eMotorCare.git
cd BE_eMotorCare
```

## Cấu hình

### Cấu hình Cơ sở dữ liệu

Cập nhật connection string trong `appsettings.json`:

```json
{
  "Database": {
    "Server": "your-db-server",
    "Port": 3306,
    "UserId": "your-db-user",
    "Password": "your-db-password",
    "DataName": "eMotoCare"
  }
}
```

Hoặc sử dụng biến môi trường:

```bash
DB_HOST=your-db-server
DB_PORT=3306
DB_NAME=eMotoCare
DB_USER=your-db-user
DB_PASSWORD=your-db-password
```

### Cấu hình các Dịch vụ

Cập nhật trong `appsettings.json`:

```json
{
  "Jwt": {
    "SecretKey": "your-secret-key",
    "Issuer": "eMotoCareAPI",
    "Audience": "eMotoCareClient",
    "ExpiresInMinutes": 10080
  },
  "MailSettings": {
    "Host": "your-smtp-server",
    "Port": 587,
    "FromEmail": "your-email@example.com",
    "Password": "your-password"
  },
  "Firebase": {
    "project_id": "your-project-id",
    "private_key": "your-private-key",
    "client_email": "your-client-email"
  },
  "Vonage": {
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  },
  "PayOS": {
    "ClientId": "your-client-id",
    "ApiKey": "your-api-key",
    "ChecksumKey": "your-checksum-key"
  }
}
```

## Sử dụng

### Chạy bằng Visual Studio

## API Documentation

### Truy cập Swagger UI

Sau khi khởi động ứng dụng, truy cập tài liệu API:

**Local Development**: `https://localhost:7001/swagger`

Swagger UI cung cấp:
- Danh sách đầy đủ tất cả endpoints
- Chi tiết về request/response schemas
- Khả năng test trực tiếp các API
- Mô tả chi tiết cho từng endpoint

### Xác thực (Authentication)

BE_eMotorCare sử dụng **JWT (JSON Web Tokens)** để xác thực:

#### 1. Đăng nhập và nhận Token

```bash
curl -X POST "https://localhost:7001/api/auth/login" \
## Cấu trúc Dự án

### Phân bố Thư mục# 2. Sử dụng Token trong Requests

```bash
curl -X GET "https://localhost:7001/api/customers" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

#### 3. Refresh Token

```bash
curl -X POST "https://localhost:7001/api/auth/refresh-token" \
  -H "Authorization: Bearer your_current_token"
```
# Chạy container
docker-compose up -d

# Xem logs
docker-compose logs -f backend
``` "Port": 3306,
    "UserId": "your-db-user",
    "Password": "your-db-password",
    "DataName": "eMotoCare"
  }
}
```

Hoặc sử dụng các biến môi trường:

```bash
DB_HOST=your-db-server
DB_PORT=3306
DB_NAME=eMotoCare
DB_USER=your-db-user
DB_PASSWORD=your-db-password
```

### 3. Cấu hình các Dịch vụ

Cập nhật các cấu hình trong `appsettings.json`:

- JWT Settings: Đặt secret key, issuer, audience cho xác thực
- Mail Settings: Cấu hình SMTP server để gửi email
- Firebase: Cấu hình Firebase credentials cho notifications
- Vonage: API key cho dịch vụ SMS
- PayOS: Thông tin đăng nhập cổng thanh toán

### 4. Chạy bằng Visual Studio

1. Mở tệp BE_eMotoCare.API.sln trong Visual Studio
2. Đặt BE_eMotoCare.API làm startup project
3. Chạy lệnh update database migrations
4. Nhấn F5 để khởi động ứng dụng

### 5. Chạy bằng Command Line

```bash
cd BE_eMotoCare.API
dotnet restore
dotnet ef database update
dotnet run
```

### 6. Chạy bằng Docker
```
BE_eMotoCare/
├── BE_eMotoCare.sln           Solution file chính
├── docker-compose.yml         Cấu hình Docker Compose
├── Dockerfile                 Cấu hình Docker
├── README.md                  Tài liệu này
│
├── BE_eMotoCare.API/          
│   ├── appsettings.json       Cấu hình ứng dụng
│   ├── Program.cs             Điểm vào chính
│   ├── Controllers/           API endpoints
│   │   ├── AuthController.cs
│   │   ├── CustomersController.cs
│   │   ├── VehiclesController.cs
│   │   ├── AppointmentsController.cs
│   │   └── ...
│   ├── Configuration/         Cấu hình khởi động
│   │   ├── AutoMapperConfiguration.cs
│   │   ├── DependencyInjection.cs
│   │   └── SwaggerConfig.cs
│   ├── Middlewares/           Custom middlewares
│   │   ├── ExceptionMiddleware.cs
│   │   └── JwtMiddleware.cs
│   ├── Extensions/            Extension methods
│   ├── Realtime/              SignalR
│   │   ├── Hubs/
│   │   └── Services/
│   └── Properties/
│       └── launchSettings.json
│
├── eMotoCare.BO/              Business Objects
│   ├── DTOs/                  Data Transfer Objects
│   ├── Entities/              Database entities
│   ├── Enums/                 Enumerations
│   ├── Exceptions/            Custom exceptions
│   └── Common/                Base classes
│
├── eMotoCare.DAL/             Data Access Layer
│   ├── context/               DbContext
│   │   └── ApplicationDbContext.cs
│   ├── Repositories/          Data access patterns
│   ├── Migrations/            EF Core migrations
│   ├── Base/                  Base repositories
│   └── Configuration/         Entity configurations
│
└── eMototCare.BLL/            Business Logic Layer
    ├── Services/              Business logic
    ├── Mappers/               AutoMapper profiles
    ├── JwtServices/           JWT handling
    ├── HashPasswords/         Password hashing
    └── Configuration/         BLL setup
```

### Mô tả các Tầng

#### BE_eMotoCare.API - Tầng Presentation

Chịu trách nhiệm xử lý HTTP requests/responses. Bao gồm:
- **Controllers**: Định tuyến requests đến services
- **Middlewares**: Xử lý cross-cutting concerns
- **Configuration**: Cấu hình dependencies
- **Realtime**: SignalR hubs cho real-time updates

#### eMotoCare.BO - Business Objects

Định nghĩa các đối tượng dùng chung:
- **Entities**: Lớp đại diện cho các bảng database
- **DTOs**: Đối tượng truyền dữ liệu giữa layers
- **Exceptions**: Ngoại lệ tùy chỉnh
- **Common**: Lớp cơ sở và configuration objects

#### eMotoCare.DAL - Data Access Layer

Xử lý tất cả tương tác với database:
- **DbContext**: Cấu hình Entity Framework Core
- **Repositories**: Triển khai Repository pattern
- **Migrations**: Quản lý database schema
- **UnitOfWork**: Quản lý transactions

#### eMototCare.BLL - Business Logic Layer
## Ví dụ API Requests

### Tạo Khách hàng Mới

```bash
POST /api/customers
Content-Type: application/json
Authorization: Bearer {token}

{
  "fullName": "Nguyễn Văn A",
  "email": "customer@example.com",
  "phoneNumber": "0123456789",
  "address": "123 Đường ABC, Thành phố XYZ"
}
```

### Lấy Danh sách Khách hàng

```bash
GET /api/customers?pageNumber=1&pageSize=10
Authorization: Bearer {token}
```

### Đặt Lịch Bảo dưỡng

```bash
POST /api/appointments
Content-Type: application/json
Authorization: Bearer {token}

{
  "customerId": "123e4567-e89b-12d3-a456-426614174000",
  "vehicleId": "223e4567-e89b-12d3-a456-426614174000",
  "serviceCenterId": "323e4567-e89b-12d3-a456-426614174000",
  "appointmentDate": "2025-12-15T10:00:00Z",
  "serviceType": "Kiểm tra Pin"
}
```

## Troubleshooting

### Vấn đề Kết nối Database

```
SqlException: Không thể kết nối tới database
```

**Giải pháp:**
1. Kiểm tra MySQL service đang chạy: `mysql --version`
2. Xác minh connection string trong `appsettings.json`
3. Kiểm tra thông tin đăng nhập database
4. Chạy lại migrations: `dotnet ef database update`

### JWT Token Expired

**Giải pháp:**
1. Gọi endpoint `/api/auth/refresh-token` để lấy token mới
2. Hoặc đăng nhập lại tại `/api/auth/login`

### Migration Conflicts

```bash
dotnet ef migrations remove
dotnet ef migrations add {migration-name}
dotnet ef database update
```

## Đóng góp

Chúng tôi rất hoan nghênh các đóng góp từ cộng đồng. Để đóng góp:

### Quy trình Pull Request

1. Fork repository tại: https://github.com/nghiatlm/BE_eMotorCare

2. Clone fork của bạn:
```bash
git clone https://github.com/your-username/BE_eMotorCare.git
cd BE_eMotorCare
```

3. Tạo nhánh tính năng:
```bash
git checkout -b feature/your-feature-name
```

4. Commit các thay đổi:
```bash
git commit -m "feat: thêm tính năng mới"
```

5. Push nhánh:
```bash
git push origin feature/your-feature-name
```

6. Tạo Pull Request trên GitHub

### Quy tắc Commit

Chúng tôi sử dụng [Conventional Commits](https://www.conventionalcommits.org/):

```
feat: thêm tính năng mới
fix: sửa lỗi
docs: cập nhật tài liệu
style: định dạng code
refactor: tái cấu trúc code
test: thêm tests
chore: cập nhật dependencies
```

## Hỗ trợ

Nếu gặp vấn đề hoặc có câu hỏi:

### Các Tài nguyên

- **API Documentation**: Truy cập Swagger UI sau khi chạy app
- **Issues**: [GitHub Issues](https://github.com/nghiatlm/BE_eMotorCare/issues)
- **Discussions**: [GitHub Discussions](https://github.com/nghiatlm/BE_eMotorCare/discussions)

### Liên hệ

- **Email**: contact@emotocare.com
- **Repository**: https://github.com/nghiatlm/BE_eMotorCare

## Giấy phép

Dự án này được cấp phép dưới MIT License. Xem file [LICENSE](LICENSE) để biết chi tiết.

## Thông tin Dự án

| Thông tin | Chi tiết |
|-----------|---------|
| **Chủ sở hữu** | nghiatlm |
| **Repository** | https://github.com/nghiatlm/BE_eMotorCare |
| **Chi nhánh chính** | main |
| **Chi nhánh hiện tại** | feature/SyncOEM |
| **Framework** | .NET 8.0 |
| **License** | MIT |
| **Status** | Phát triển |

## Roadmap

Kế hoạch phát triển tương lai:

### Phiên bản 1.1 (Q1 2025)
- [ ] Thêm Unit Tests
- [ ] Cải thiện Error Handling
- [ ] Thêm Logging và Monitoring
- [ ] Tối ưu hóa Performance

### Phiên bản 1.2 (Q2 2025)
- [ ] Mobile App Integration
- [ ] Advanced Analytics Dashboard
- [ ] Machine Learning Predictions
- [ ] Multi-language Support

### Phiên bản 2.0 (H2 2025)
- [ ] Microservices Architecture
- [ ] Kubernetes Deployment
- [ ] Advanced Security Features
- [ ] Blockchain Integration

## Acknowledgments

Dự án này được phát triển như một phần của khóa học Capstone. Cảm ơn tất cả các thành viên trong nhóm đã đóng góp.

---

**Phiên bản**: 1.0.0
**Cập nhật lần cuối**: December 7, 2025
**Trạng thái**: Active Development
├── Services/                  Logic kinh doanh
├── Mappers/                   AutoMapper profiles
├── JwtServices/               Xử lý JWT
└── HashPasswords/             Mã hóa mật khẩu
```

## Các Tính năng Chính

### Quản lý Khách hàng và Xe

- Tạo, cập nhật, xóa thông tin khách hàng
- Quản lý danh sách xe điện của từng khách hàng
- Theo dõi thông tin chi tiết về xe

### Quản lý Đặt lịch

- Đặt lịch bảo dưỡng cho xe
- Xem danh sách cuộc hẹn dịch vụ
- Quản lý các slot thời gian sẵn có
- Gửi thông báo tự động cho khách hàng

### Kiểm tra và Bảo dưỡng

- Kiểm tra tình trạng pin xe
- Kiểm tra toàn bộ hệ thống xe điện
- Quản lý kế hoạch bảo dưỡng định kỳ
- Ghi nhận chi tiết kết quả kiểm tra

### Quản lý Trung tâm Dịch vụ

- Quản lý thông tin chi tiết trung tâm
- Quản lý danh sách nhân viên
- Quản lý kho hàng và linh kiện
- Quản lý thời gian hoạt động

### Thanh toán và Hóa đơn

- Tích hợp với cổng thanh toán PayOS
- Tạo và quản lý đơn hàng
- Theo dõi trạng thái thanh toán
- Xử lý yêu cầu trả hàng (RMA)

### Dashboard và Báo cáo

- Thống kê doanh thu theo kỳ
- Theo dõi hiệu suất dịch vụ
- Báo cáo hoạt động hệ thống

### Thông báo Real-time

- Sử dụng SignalR cho kết nối hai chiều
- Thông báo tức thời cho người dùng
- Push notification qua Firebase

## Quy trình Đóng góp

Để đóng góp vào dự án:

1. Fork repository từ GitHub
2. Tạo nhánh tính năng mới (git checkout -b feature/AmazingFeature)
3. Commit các thay đổi (git commit -m 'Add some AmazingFeature')
4. Push nhánh (git push origin feature/AmazingFeature)
5. Tạo Pull Request

## Giấy phép

Dự án này là một phần của khóa học Capstone.

## Thông tin Dự án

- Chủ sở hữu: nghiatlm
- Repository: https://github.com/nghiatlm/BE_eMotorCare
- Chi nhánh hiện tại: feature/SyncOEM

## Hỗ trợ

Nếu gặp vấn đề hoặc có câu hỏi:

1. Kiểm tra phần tài liệu của dự án
2. Xem API documentation tại Swagger UI
3. Liên hệ với nhóm phát triển

## Kế hoạch Phát triển

- Tối ưu hóa hiệu suất ứng dụng
- Thêm các unit tests
- Cải thiện xử lý lỗi
- Thêm logging chi tiết
- Tích hợp ứng dụng mobile
- Dashboard phân tích nâng caonce
- [ ] Thêm unit tests
- [ ] Cải thiện error handling
- [ ] Thêm logging
- [ ] Mobile app integration
- [ ] Analytics dashboard

---

