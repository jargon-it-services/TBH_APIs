using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TheBeautyHubData.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AccountCode = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    AccountName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AccountType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Mode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsUnderTrial = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TrialStartedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrialDuration = table.Column<int>(type: "integer", nullable: true),
                    TrialExpiredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.CheckConstraint("CK_Account_AccountType", "\"AccountType\" IN ('FirmOwner', 'Customer')");
                    table.CheckConstraint("CK_Account_Mode", "\"Mode\" IN ('subscription', 'one_time')");
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    PlanName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PlanDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PlanCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsPlanActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlanAppliedTo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ReportName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                });

            migrationBuilder.CreateTable(
                name: "TransactionType",
                columns: table => new
                {
                    TransactionTypeId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsTransactionTypeActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType", x => x.TransactionTypeId);
                    table.CheckConstraint("CK_TransactionType_Type", "\"Type\" IN ('Service', 'Expenses')");
                });

            migrationBuilder.CreateTable(
                name: "Firm",
                columns: table => new
                {
                    FirmId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirmName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FirmAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FirmGstin = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    FirmContact = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FirmEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FirmPhoto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FirmOwnerName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    FirmType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FirmRegistration = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FirmLogo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firm", x => x.FirmId);
                    table.ForeignKey(
                        name: "FK_Firm_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Partner",
                columns: table => new
                {
                    PartnerId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Mobile = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partner", x => x.PartnerId);
                    table.CheckConstraint("CK_Partner_Gender", "\"Gender\" IS NULL OR \"Gender\" IN ('Male', 'Female', 'Other')");
                    table.ForeignKey(
                        name: "FK_Partner_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserRole = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UserName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    UserEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UserMobile = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    UserPasswordHash = table.Column<byte[]>(type: "bytea", maxLength: 64, nullable: false),
                    EmailVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MobileVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    WorkerPaymentType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.CheckConstraint("CK_User_UserRole", "\"UserRole\" IN ('Admin', 'Manager', 'Employee')");
                    table.CheckConstraint("CK_User_WorkerPaymentType", "\"WorkerPaymentType\" IS NULL OR \"WorkerPaymentType\" IN ('Fix Pay', 'FP + Incentive', 'Incentive')");
                    table.ForeignKey(
                        name: "FK_User_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_User_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    WalletType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.WalletId);
                    table.CheckConstraint("CK_Wallet_WalletType", "\"WalletType\" IN ('ReferralBonus', 'Promotional', 'Cashback')");
                    table.ForeignKey(
                        name: "FK_Wallet_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ExpiryOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    SubscriptionAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    DiscountedAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    SubscriptionAmountAfterDiscount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    DiscountType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.SubscriptionId);
                    table.CheckConstraint("CK_Subscription_DiscountType", "\"DiscountType\" IS NULL OR \"DiscountType\" IN ('Wallet', 'Coupon')");
                    table.CheckConstraint("CK_Subscription_Status", "\"Status\" IN ('Active', 'Expired', 'Cancelled', 'Pending')");
                    table.ForeignKey(
                        name: "FK_Subscription_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscription_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportsForAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsForAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportsForAccount_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportsForAccount_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpensesType",
                columns: table => new
                {
                    ExpensesTypeId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpensesTypeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    FirmId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensesType", x => x.ExpensesTypeId);
                    table.ForeignKey(
                        name: "FK_ExpensesType_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpensesType_Firm_FirmId",
                        column: x => x.FirmId,
                        principalTable: "Firm",
                        principalColumn: "FirmId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ServiceName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ServiceDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ServicePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    ServiceTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirmId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsIncentiveApplicable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IncentiveAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    IncentivePercentage = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceId);
                    table.CheckConstraint("CK_Services_IncentivePercentage", "\"IncentivePercentage\" IS NULL OR (\"IncentivePercentage\" >= 0 AND \"IncentivePercentage\" <= 100)");
                    table.ForeignKey(
                        name: "FK_Services_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_Firm_FirmId",
                        column: x => x.FirmId,
                        principalTable: "Firm",
                        principalColumn: "FirmId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_TransactionType_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "TransactionType",
                        principalColumn: "TransactionTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionRules",
                columns: table => new
                {
                    TransactionRuleId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    RuleName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    FirmId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionRules", x => x.TransactionRuleId);
                    table.ForeignKey(
                        name: "FK_TransactionRules_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionRules_Firm_FirmId",
                        column: x => x.FirmId,
                        principalTable: "Firm",
                        principalColumn: "FirmId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Draft"),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirmId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    PostedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CheckInTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CheckOutTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.CheckConstraint("CK_Transaction_Status", "\"Status\" IN ('Draft', 'Posted', 'Cancelled')");
                    table.ForeignKey(
                        name: "FK_Transactions_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Firm_FirmId",
                        column: x => x.FirmId,
                        principalTable: "Firm",
                        principalColumn: "FirmId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: false),
                    DeviceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExceptionLogs_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FirmDetails",
                columns: table => new
                {
                    FirmDetailsId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirmId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirmDetails", x => x.FirmDetailsId);
                    table.ForeignKey(
                        name: "FK_FirmDetails_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FirmDetails_Firm_FirmId",
                        column: x => x.FirmId,
                        principalTable: "Firm",
                        principalColumn: "FirmId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FirmDetails_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DeviceId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    AccessTokenJti = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshTokenHash = table.Column<byte[]>(type: "bytea", maxLength: 32, nullable: false),
                    RefreshTokenExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevocationReason = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_UserSessions_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionsDetails",
                columns: table => new
                {
                    TransactionDetailsId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpensesTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ServiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IncentiveAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TransactionRuleId = table.Column<Guid>(type: "uuid", nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    FirmId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionsDetails", x => x.TransactionDetailsId);
                    table.ForeignKey(
                        name: "FK_TransactionsDetails_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionsDetails_ExpensesType_ExpensesTypeId",
                        column: x => x.ExpensesTypeId,
                        principalTable: "ExpensesType",
                        principalColumn: "ExpensesTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionsDetails_Firm_FirmId",
                        column: x => x.FirmId,
                        principalTable: "Firm",
                        principalColumn: "FirmId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionsDetails_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionsDetails_TransactionRules_TransactionRuleId",
                        column: x => x.TransactionRuleId,
                        principalTable: "TransactionRules",
                        principalColumn: "TransactionRuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionsDetails_TransactionType_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "TransactionType",
                        principalColumn: "TransactionTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionsDetails_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_AccountCode",
                table: "Account",
                column: "AccountCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionLogs_UserId",
                table: "ExceptionLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesType_AccountId",
                table: "ExpensesType",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesType_FirmId",
                table: "ExpensesType",
                column: "FirmId");

            migrationBuilder.CreateIndex(
                name: "IX_Firm_AccountId",
                table: "Firm",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FirmDetails_AccountId",
                table: "FirmDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FirmDetails_FirmId",
                table: "FirmDetails",
                column: "FirmId");

            migrationBuilder.CreateIndex(
                name: "IX_FirmDetails_UserId",
                table: "FirmDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_AccountId",
                table: "Partner",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_Email",
                table: "Partner",
                column: "Email",
                unique: true,
                filter: "\"Email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_Mobile",
                table: "Partner",
                column: "Mobile",
                unique: true,
                filter: "\"Mobile\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsForAccount_AccountId",
                table: "ReportsForAccount",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsForAccount_ReportId",
                table: "ReportsForAccount",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_AccountId",
                table: "Services",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_FirmId",
                table: "Services",
                column: "FirmId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_AccountId",
                table: "Subscription",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_PlanId",
                table: "Subscription",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRules_AccountId",
                table: "TransactionRules",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRules_FirmId",
                table: "TransactionRules",
                column: "FirmId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FirmId",
                table: "Transactions",
                column: "FirmId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsDetails_AccountId",
                table: "TransactionsDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsDetails_ExpensesTypeId",
                table: "TransactionsDetails",
                column: "ExpensesTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsDetails_FirmId",
                table: "TransactionsDetails",
                column: "FirmId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsDetails_ServiceId",
                table: "TransactionsDetails",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsDetails_TransactionId",
                table: "TransactionsDetails",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsDetails_TransactionRuleId",
                table: "TransactionsDetails",
                column: "TransactionRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsDetails_TransactionTypeId",
                table: "TransactionsDetails",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AccountId",
                table: "User",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ManagerId",
                table: "User",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserEmail",
                table: "User",
                column: "UserEmail",
                unique: true,
                filter: "\"UserEmail\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserMobile",
                table: "User",
                column: "UserMobile",
                unique: true,
                filter: "\"UserMobile\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_AccountId",
                table: "Wallet",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionLogs");

            migrationBuilder.DropTable(
                name: "FirmDetails");

            migrationBuilder.DropTable(
                name: "Partner");

            migrationBuilder.DropTable(
                name: "ReportsForAccount");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "TransactionsDetails");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "ExpensesType");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "TransactionRules");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "TransactionType");

            migrationBuilder.DropTable(
                name: "Firm");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
