-- Branch tables (PostgreSQL)
-- Applied via EF Core migration AddBranch. This script is a reference for manual setups.

CREATE TABLE IF NOT EXISTS "Branch" (
    "BranchId" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "AccountId" UUID NULL REFERENCES "Account"("AccountId"),
    "Name" VARCHAR(200) NOT NULL,
    "AddressLine1" VARCHAR(500) NOT NULL,
    "AddressLine2" VARCHAR(500) NULL,
    "City" VARCHAR(100) NOT NULL,
    "State" VARCHAR(100) NOT NULL,
    "Pincode" VARCHAR(20) NOT NULL,
    "Mobile" VARCHAR(20) NOT NULL,
    "Email" VARCHAR(256) NOT NULL,
    "BranchType" VARCHAR(50) NOT NULL,
    "OpeningTime" VARCHAR(10) NOT NULL,
    "ClosingTime" VARCHAR(10) NOT NULL,
    "WeeklyOff" VARCHAR(20) NOT NULL,
    "Status" VARCHAR(20) NOT NULL DEFAULT 'active',
    "Latitude" NUMERIC(9,6) NULL,
    "Longitude" NUMERIC(9,6) NULL,
    "MapsLink" VARCHAR(500) NULL,
    "Logo" VARCHAR(500) NULL,
    "CreatedBy" UUID NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "LastUpdated" TIMESTAMP NULL,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS "BranchService" (
    "BranchId" UUID NOT NULL REFERENCES "Branch"("BranchId") ON DELETE CASCADE,
    "ServiceId" UUID NOT NULL REFERENCES "Services"("ServiceId"),
    PRIMARY KEY ("BranchId", "ServiceId")
);

CREATE TABLE IF NOT EXISTS "BranchEmployee" (
    "BranchId" UUID NOT NULL REFERENCES "Branch"("BranchId") ON DELETE CASCADE,
    "UserId" UUID NOT NULL REFERENCES "User"("UserId"),
    "Photo" VARCHAR(500) NULL,
    PRIMARY KEY ("BranchId", "UserId")
);
