using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Context
{
    /// <summary>
    /// Database context for The Beauty Hub application.
    /// Manages all entity sets and database configuration.
    /// </summary>
    public class BeautyHubDbContext : DbContext
    {
        public BeautyHubDbContext(DbContextOptions<BeautyHubDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Accounts table
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Users table
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Firms table
        /// </summary>
        public DbSet<Firm> Firms { get; set; }

        /// <summary>
        /// FirmDetails table
        /// </summary>
        public DbSet<FirmDetails> FirmDetails { get; set; }

        /// <summary>
        /// Plans table
        /// </summary>
        public DbSet<Plans> Plans { get; set; }

        /// <summary>
        /// Subscriptions table
        /// </summary>
        public DbSet<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Wallets table
        /// </summary>
        public DbSet<Wallet> Wallets { get; set; }

        /// <summary>
        /// ExpensesTypes table
        /// </summary>
        public DbSet<ExpensesType> ExpensesTypes { get; set; }

        /// <summary>
        /// Services table
        /// </summary>
        public DbSet<Services> Services { get; set; }

        /// <summary>
        /// TransactionTypes table
        /// </summary>
        public DbSet<TransactionType> TransactionTypes { get; set; }

        /// <summary>
        /// TransactionRules table
        /// </summary>
        public DbSet<TransactionRules> TransactionRules { get; set; }

        /// <summary>
        /// Transactions table
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// TransactionDetails table
        /// </summary>
        public DbSet<TransactionDetail> TransactionDetails { get; set; }

        /// <summary>
        /// Reports table
        /// </summary>
        public DbSet<Report> Reports { get; set; }

        /// <summary>
        /// ReportsForAccount table
        /// </summary>
        public DbSet<ReportForAccount> ReportsForAccount { get; set; }

        /// <summary>
        /// Partners table
        /// </summary>
        public DbSet<Partner> Partners { get; set; }

        /// <summary>
        /// UserSessions table
        /// </summary>
        public DbSet<UserSession> UserSessions { get; set; }

        /// <summary>
        /// ExceptionLogs table
        /// </summary>
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }

        /// <summary>
        /// Branches table
        /// </summary>
        public DbSet<Branch> Branches { get; set; }

        /// <summary>
        /// BranchService junction table
        /// </summary>
        public DbSet<BranchService> BranchServices { get; set; }

        /// <summary>
        /// BranchEmployee junction table
        /// </summary>
        public DbSet<BranchEmployee> BranchEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Account entity
            modelBuilder.Entity<Account>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.AccountId);

                // Default value for AccountId - PostgreSQL
                entity.Property(e => e.AccountId)
                    .HasDefaultValueSql("gen_random_uuid()");

                // Unique constraint on AccountCode
                entity.HasIndex(e => e.AccountCode)
                    .IsUnique();

                // Check constraints - PostgreSQL syntax
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Account_AccountType",
                        "\"AccountType\" IN ('FirmOwner', 'Customer')");
                    t.HasCheckConstraint("CK_Account_Mode",
                        "\"Mode\" IN ('subscription', 'one_time')");
                });

                // Default value for CreatedAt - PostgreSQL
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Default value for IsDeleted
                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                // Default value for IsUnderTrial
                entity.Property(e => e.IsUnderTrial)
                    .HasDefaultValue(false);
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.UserId);

                // Default value for UserId - PostgreSQL
                entity.Property(e => e.UserId)
                    .HasDefaultValueSql("gen_random_uuid()");

                // Unique constraint on UserEmail
                entity.HasIndex(e => e.UserEmail)
                    .IsUnique()
                    .HasFilter("\"UserEmail\" IS NOT NULL");

                // Unique constraint on UserMobile
                entity.HasIndex(e => e.UserMobile)
                    .IsUnique()
                    .HasFilter("\"UserMobile\" IS NOT NULL");

                // Check constraints - PostgreSQL syntax
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_User_UserRole",
                        "\"UserRole\" IN ('Admin', 'Manager', 'Employee')");
                    t.HasCheckConstraint("CK_User_WorkerPaymentType",
                        "\"WorkerPaymentType\" IS NULL OR \"WorkerPaymentType\" IN ('Fix Pay', 'FP + Incentive', 'Incentive')");
                });

                // Default value for CreatedAt - PostgreSQL
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Default value for IsDeleted
                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                // Default value for EmailVerified
                entity.Property(e => e.EmailVerified)
                    .HasDefaultValue(false);

                // Default value for MobileVerified
                entity.Property(e => e.MobileVerified)
                    .HasDefaultValue(false);

                // Default value for Status
                entity.Property(e => e.Status)
                    .HasDefaultValue("Active");

                // Foreign key relationship with Account
                entity.HasOne(e => e.Account)
                    .WithMany(a => a.Users)
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Self-referencing relationship for Manager
                entity.HasOne(e => e.Manager)
                    .WithMany(u => u.ManagedUsers)
                    .HasForeignKey(e => e.ManagerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Firm entity
            modelBuilder.Entity<Firm>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.FirmId);

                // Default value for FirmId - PostgreSQL
                entity.Property(e => e.FirmId)
                    .HasDefaultValueSql("gen_random_uuid()");

                // Default value for CreatedAt - PostgreSQL
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Default value for IsDeleted
                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                // Foreign key relationship with Account
                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure FirmDetails entity
            modelBuilder.Entity<FirmDetails>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.FirmDetailsId);

                // Default value for FirmDetailsId - PostgreSQL
                entity.Property(e => e.FirmDetailsId)
                    .HasDefaultValueSql("gen_random_uuid()");

                // Default value for CreatedAt - PostgreSQL
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Foreign key relationship with User
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Foreign key relationship with Account
                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Foreign key relationship with Firm
                entity.HasOne(e => e.Firm)
                    .WithMany(f => f.FirmDetails)
                    .HasForeignKey(e => e.FirmId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Plans entity
            modelBuilder.Entity<Plans>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.PlanId);

                // Default value for PlanId - PostgreSQL
                entity.Property(e => e.PlanId)
                    .HasDefaultValueSql("gen_random_uuid()");

                // Default value for CreatedAt - PostgreSQL
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Default value for IsPlanActive
                entity.Property(e => e.IsPlanActive)
                    .HasDefaultValue(true);
            });

            // Configure Subscription entity
            modelBuilder.Entity<Subscription>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.SubscriptionId);

                // Default value for SubscriptionId - PostgreSQL
                entity.Property(e => e.SubscriptionId)
                    .HasDefaultValueSql("gen_random_uuid()");

                // Default value for CreatedAt - PostgreSQL
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Default values for amounts
                entity.Property(e => e.SubscriptionAmount)
                    .HasDefaultValue(0);

                entity.Property(e => e.DiscountedAmount)
                    .HasDefaultValue(0);

                entity.Property(e => e.SubscriptionAmountAfterDiscount)
                    .HasDefaultValue(0);

                // Check constraints - PostgreSQL syntax
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Subscription_Status",
                        "\"Status\" IN ('Active', 'Expired', 'Cancelled', 'Pending')");
                    t.HasCheckConstraint("CK_Subscription_DiscountType",
                        "\"DiscountType\" IS NULL OR \"DiscountType\" IN ('Wallet', 'Coupon')");
                });

                // Foreign key relationship with Account
                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Foreign key relationship with Plan
                entity.HasOne(e => e.Plan)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(e => e.PlanId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Wallet entity
            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(e => e.WalletId);

                entity.Property(e => e.WalletId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.Amount)
                    .HasDefaultValue(0);

                entity.Property(e => e.IsUsed)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Wallet_WalletType",
                        "\"WalletType\" IN ('ReferralBonus', 'Promotional', 'Cashback')");
                });

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure ExpensesType entity
            modelBuilder.Entity<ExpensesType>(entity =>
            {
                entity.HasKey(e => e.ExpensesTypeId);

                entity.Property(e => e.ExpensesTypeId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Firm)
                    .WithMany()
                    .HasForeignKey(e => e.FirmId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Services entity
            modelBuilder.Entity<Services>(entity =>
            {
                entity.HasKey(e => e.ServiceId);

                entity.Property(e => e.ServiceId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.ServicePrice)
                    .HasDefaultValue(0);

                entity.Property(e => e.IsIncentiveApplicable)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Services_IncentivePercentage",
                        "\"IncentivePercentage\" IS NULL OR (\"IncentivePercentage\" >= 0 AND \"IncentivePercentage\" <= 100)");
                });

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Firm)
                    .WithMany()
                    .HasForeignKey(e => e.FirmId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ServiceType)
                    .WithMany(t => t.Services)
                    .HasForeignKey(e => e.ServiceTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure TransactionType entity
            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.HasKey(e => e.TransactionTypeId);

                entity.Property(e => e.TransactionTypeId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsTransactionTypeActive)
                    .HasDefaultValue(true);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_TransactionType_Type",
                        "\"Type\" IN ('Service', 'Expenses')");
                });
            });

            // Configure TransactionRules entity
            modelBuilder.Entity<TransactionRules>(entity =>
            {
                entity.HasKey(e => e.TransactionRuleId);

                entity.Property(e => e.TransactionRuleId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Firm)
                    .WithMany()
                    .HasForeignKey(e => e.FirmId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Transaction entity
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.Property(e => e.TotalAmount)
                    .HasDefaultValue(0);

                entity.Property(e => e.Status)
                    .HasDefaultValue("Draft");

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Transaction_Status",
                        "\"Status\" IN ('Draft', 'Posted', 'Cancelled')");
                });

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Firm)
                    .WithMany()
                    .HasForeignKey(e => e.FirmId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure TransactionDetail entity
            modelBuilder.Entity<TransactionDetail>(entity =>
            {
                entity.HasKey(e => e.TransactionDetailsId);

                entity.Property(e => e.TransactionDetailsId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(e => e.Transaction)
                    .WithMany(t => t.TransactionDetails)
                    .HasForeignKey(e => e.TransactionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TransactionType)
                    .WithMany()
                    .HasForeignKey(e => e.TransactionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ExpensesType)
                    .WithMany()
                    .HasForeignKey(e => e.ExpensesTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Service)
                    .WithMany()
                    .HasForeignKey(e => e.ServiceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TransactionRule)
                    .WithMany()
                    .HasForeignKey(e => e.TransactionRuleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Firm)
                    .WithMany()
                    .HasForeignKey(e => e.FirmId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Report entity
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.ReportId);

                entity.Property(e => e.ReportId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);
            });

            // Configure ReportForAccount entity
            modelBuilder.Entity<ReportForAccount>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.HasOne(e => e.Report)
                    .WithMany(r => r.ReportsForAccounts)
                    .HasForeignKey(e => e.ReportId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Partner entity
            modelBuilder.Entity<Partner>(entity =>
            {
                entity.HasKey(e => e.PartnerId);

                entity.Property(e => e.PartnerId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasIndex(e => e.Mobile)
                    .IsUnique()
                    .HasFilter("\"Mobile\" IS NOT NULL");

                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasFilter("\"Email\" IS NOT NULL");

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Partner_Gender",
                        "\"Gender\" IS NULL OR \"Gender\" IN ('Male', 'Female', 'Other')");
                });

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure UserSession entity
            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.HasKey(e => e.SessionId);

                entity.Property(e => e.SessionId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure ExceptionLog entity
            modelBuilder.Entity<ExceptionLog>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Branch entity
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(e => e.BranchId);

                entity.Property(e => e.BranchId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.Property(e => e.Status)
                    .HasDefaultValue("active");

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure BranchService entity
            modelBuilder.Entity<BranchService>(entity =>
            {
                entity.HasKey(e => new { e.BranchId, e.ServiceId });

                entity.HasOne(e => e.Branch)
                    .WithMany(b => b.BranchServices)
                    .HasForeignKey(e => e.BranchId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Service)
                    .WithMany()
                    .HasForeignKey(e => e.ServiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure BranchEmployee entity
            modelBuilder.Entity<BranchEmployee>(entity =>
            {
                entity.HasKey(e => new { e.BranchId, e.UserId });

                entity.HasOne(e => e.Branch)
                    .WithMany(b => b.BranchEmployees)
                    .HasForeignKey(e => e.BranchId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
