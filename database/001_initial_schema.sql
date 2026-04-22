-- =============================================================================
-- 001_initial_schema.sql
-- PostgreSQL schema migrated from Microsoft Access Northwind Developer Edition
--
-- Access → PostgreSQL type mappings applied:
--   AUTOINCREMENT  → SERIAL
--   LONG           → INTEGER
--   SHORT          → SMALLINT
--   BYTE           → SMALLINT  (no unsigned byte in PostgreSQL)
--   VARCHAR(n)     → VARCHAR(n)
--   VARCHAR        → TEXT
--   LONGTEXT       → TEXT
--   DATETIME       → TIMESTAMP
--   CURRENCY       → NUMERIC(19,4)
--   SINGLE         → REAL
--   BIT            → BOOLEAN
--
-- Table creation order respects all foreign key dependencies.
-- Cascade behaviour is preserved from the Access relation attributes
--   (Attributes=4096 → ON DELETE CASCADE).
-- Composite unique index on Companies(CompanyName, CompanyTypeID) is
--   preserved from the original CustomerName index in the Access schema.
-- =============================================================================

BEGIN;

-- ---------------------------------------------------------------------------
-- TIER 1: Lookup / reference tables (no FK dependencies)
-- ---------------------------------------------------------------------------

CREATE TABLE "States" (
    "StateAbbrev" VARCHAR(2)  NOT NULL,
    "StateName"   VARCHAR(50) UNIQUE,
    CONSTRAINT "PK_States" PRIMARY KEY ("StateAbbrev")
);

CREATE TABLE "CompanyTypes" (
    "CompanyTypeID" SERIAL       NOT NULL,
    "CompanyType"   VARCHAR(50)  UNIQUE,
    "AddedBy"       VARCHAR(255),
    "AddedOn"       TIMESTAMP,
    "ModifiedBy"    VARCHAR(255),
    "ModifiedOn"    TIMESTAMP,
    CONSTRAINT "PK_CompanyTypes" PRIMARY KEY ("CompanyTypeID")
);

-- TaxStatusID uses BYTE in Access (0-255); SMALLINT is the closest PostgreSQL type.
CREATE TABLE "TaxStatus" (
    "TaxStatusID" SMALLINT     NOT NULL,
    "TaxStatus"   VARCHAR(50)  UNIQUE,
    "AddedBy"     VARCHAR(255),
    "AddedOn"     TIMESTAMP,
    "ModifiedBy"  VARCHAR(255),
    "ModifiedOn"  TIMESTAMP,
    CONSTRAINT "PK_TaxStatus" PRIMARY KEY ("TaxStatusID")
);

CREATE TABLE "Titles" (
    "Title"      VARCHAR(20)  NOT NULL,
    "AddedBy"    VARCHAR(255),
    "AddedOn"    TIMESTAMP,
    "ModifiedBy" VARCHAR(255),
    "ModifiedOn" TIMESTAMP,
    CONSTRAINT "PK_Titles" PRIMARY KEY ("Title")
);

CREATE TABLE "OrderStatus" (
    "OrderStatusID"   SERIAL      NOT NULL,
    "OrderStatusCode" VARCHAR(5)  UNIQUE,
    "OrderStatusName" VARCHAR(50) UNIQUE,
    "SortOrder"       SMALLINT    UNIQUE,
    "AddedBy"         VARCHAR(255),
    "AddedOn"         TIMESTAMP,
    "ModifiedBy"      VARCHAR(255),
    "ModifiedOn"      TIMESTAMP,
    CONSTRAINT "PK_OrderStatus" PRIMARY KEY ("OrderStatusID")
);

CREATE TABLE "OrderDetailStatus" (
    "OrderDetailStatusID"   SERIAL      NOT NULL,
    "OrderDetailStatusName" VARCHAR(50) UNIQUE,
    "SortOrder"             SMALLINT    UNIQUE,
    "AddedBy"               VARCHAR(255),
    "AddedOn"               TIMESTAMP,
    "ModifiedBy"            VARCHAR(255),
    "ModifiedOn"            TIMESTAMP,
    CONSTRAINT "PK_OrderDetailStatus" PRIMARY KEY ("OrderDetailStatusID")
);

CREATE TABLE "PurchaseOrderStatus" (
    "StatusID"   SERIAL      NOT NULL,
    "StatusName" VARCHAR(50) UNIQUE,
    "SortOrder"  SMALLINT    UNIQUE,
    "AddedBy"    VARCHAR(255),
    "AddedOn"    TIMESTAMP,
    "ModifiedBy" VARCHAR(255),
    "ModifiedOn" TIMESTAMP,
    CONSTRAINT "PK_PurchaseOrderStatus" PRIMARY KEY ("StatusID")
);

CREATE TABLE "Privileges" (
    "PrivilegeID"   SERIAL      NOT NULL,
    "PrivilegeName" VARCHAR(50) UNIQUE,
    "AddedBy"       VARCHAR(255),
    "AddedOn"       TIMESTAMP,
    "ModifiedBy"    VARCHAR(255),
    "ModifiedOn"    TIMESTAMP,
    CONSTRAINT "PK_Privileges" PRIMARY KEY ("PrivilegeID")
);

CREATE TABLE "ProductCategories" (
    "ProductCategoryID"    SERIAL       NOT NULL,
    "ProductCategoryName"  VARCHAR(255) UNIQUE,
    "ProductCategoryCode"  VARCHAR(3)   UNIQUE,
    "ProductCategoryDesc"  VARCHAR(255),
    "ProductCategoryImage" TEXT,
    "AddedBy"              VARCHAR(255),
    "AddedOn"              TIMESTAMP,
    "ModifiedBy"           VARCHAR(255),
    "ModifiedOn"           TIMESTAMP,
    CONSTRAINT "PK_ProductCategories" PRIMARY KEY ("ProductCategoryID")
);

-- ---------------------------------------------------------------------------
-- TIER 1: Application metadata / UI tables (no domain FK dependencies)
-- ---------------------------------------------------------------------------

CREATE TABLE "Catalog_TableOfContents" (
    "TocTitle" VARCHAR(255) NOT NULL,
    "TocPage"  SMALLINT,
    CONSTRAINT "PK_Catalog_TableOfContents" PRIMARY KEY ("TocTitle")
);

CREATE TABLE "Learn" (
    "ID"          SERIAL   NOT NULL,
    "SectionNo"   SMALLINT UNIQUE,
    "SectionText" TEXT,
    CONSTRAINT "PK_Learn" PRIMARY KEY ("ID")
);

CREATE TABLE "NorthwindFeatures" (
    "NorthwindFeaturesID" SERIAL       NOT NULL,
    "ItemName"            VARCHAR(255),
    "Description"         VARCHAR(255),
    "Navigation"          VARCHAR(255),
    "LearnMore"           TEXT,
    "HelpKeywords"        VARCHAR(255),
    "OpenMethod"          INTEGER,
    CONSTRAINT "PK_NorthwindFeatures" PRIMARY KEY ("NorthwindFeaturesID")
);

CREATE TABLE "SystemSettings" (
    "SettingID"    SERIAL       NOT NULL,
    "SettingName"  VARCHAR(50)  UNIQUE,
    "SettingValue" VARCHAR(255),
    "Notes"        VARCHAR(255),
    CONSTRAINT "PK_SystemSettings" PRIMARY KEY ("SettingID")
);

CREATE TABLE "UserSettings" (
    "SettingID"    SERIAL       NOT NULL,
    "SettingName"  VARCHAR(50)  UNIQUE,
    "SettingValue" VARCHAR(255),
    "Notes"        VARCHAR(255),
    CONSTRAINT "PK_UserSettings" PRIMARY KEY ("SettingID")
);

CREATE TABLE "USysRibbons" (
    "ID"         SERIAL       NOT NULL,
    "RibbonName" VARCHAR(255),
    "RibbonXML"  TEXT,
    CONSTRAINT "PK_USysRibbons" PRIMARY KEY ("ID")
);

CREATE TABLE "Welcome" (
    "ID"        SERIAL NOT NULL,
    "Welcome"   TEXT,
    "Learn"     TEXT,
    "DataMacro" TEXT,
    CONSTRAINT "PK_Welcome" PRIMARY KEY ("ID")
);

CREATE TABLE "Strings" (
    "StringID"   SERIAL NOT NULL,
    "StringData" TEXT,
    "AddedBy"    VARCHAR(255),
    "AddedOn"    TIMESTAMP,
    "ModifiedBy" VARCHAR(255),
    "ModifiedOn" TIMESTAMP,
    CONSTRAINT "PK_Strings" PRIMARY KEY ("StringID")
);

-- ---------------------------------------------------------------------------
-- TIER 2: Core business tables (depend on Tier 1 lookup tables)
-- ---------------------------------------------------------------------------

CREATE TABLE "Companies" (
    "CompanyID"           SERIAL       NOT NULL,
    "CompanyName"         VARCHAR(50),
    "CompanyTypeID"       INTEGER,
    "BusinessPhone"       VARCHAR(20),
    "Address"             VARCHAR(255),
    "City"                VARCHAR(255),
    "StateAbbrev"         VARCHAR(2),
    "Zip"                 VARCHAR(10),
    "Website"             TEXT,
    "Notes"               TEXT,
    "StandardTaxStatusID" SMALLINT,
    "AddedBy"             VARCHAR(255),
    "AddedOn"             TIMESTAMP,
    "ModifiedBy"          VARCHAR(255),
    "ModifiedOn"          TIMESTAMP,
    CONSTRAINT "PK_Companies"              PRIMARY KEY ("CompanyID"),
    CONSTRAINT "FK_Companies_CompanyTypes" FOREIGN KEY ("CompanyTypeID")       REFERENCES "CompanyTypes" ("CompanyTypeID"),
    CONSTRAINT "FK_Companies_States"       FOREIGN KEY ("StateAbbrev")          REFERENCES "States"       ("StateAbbrev"),
    CONSTRAINT "FK_Companies_TaxStatus"    FOREIGN KEY ("StandardTaxStatusID")  REFERENCES "TaxStatus"    ("TaxStatusID"),
    -- Original Access index: CustomerName (CompanyName, CompanyTypeID) UNIQUE
    CONSTRAINT "UQ_Companies_CustomerName" UNIQUE ("CompanyName", "CompanyTypeID")
);

CREATE INDEX "IX_Companies_CompanyTypeID"       ON "Companies" ("CompanyTypeID");
CREATE INDEX "IX_Companies_StateAbbrev"         ON "Companies" ("StateAbbrev");
CREATE INDEX "IX_Companies_StandardTaxStatusID" ON "Companies" ("StandardTaxStatusID");

CREATE TABLE "Contacts" (
    "ContactID"      SERIAL       NOT NULL,
    "CompanyID"      INTEGER,
    "LastName"       VARCHAR(30),
    "FirstName"      VARCHAR(20),
    "EmailAddress"   VARCHAR(255),
    "JobTitle"       VARCHAR(50),
    "PrimaryPhone"   VARCHAR(20),
    "SecondaryPhone" VARCHAR(20),
    "Notes"          TEXT,
    "AddedBy"        VARCHAR(255),
    "AddedOn"        TIMESTAMP,
    "ModifiedBy"     VARCHAR(255),
    "ModifiedOn"     TIMESTAMP,
    CONSTRAINT "PK_Contacts"           PRIMARY KEY ("ContactID"),
    CONSTRAINT "FK_Contacts_Companies" FOREIGN KEY ("CompanyID") REFERENCES "Companies" ("CompanyID")
);

CREATE INDEX "IX_Contacts_CompanyID" ON "Contacts" ("CompanyID");

CREATE TABLE "Employees" (
    "EmployeeID"      SERIAL       NOT NULL,
    "FirstName"       VARCHAR(20),
    "LastName"        VARCHAR(30),
    "EmailAddress"    VARCHAR(255),
    "JobTitle"        VARCHAR(50),
    "PrimaryPhone"    VARCHAR(20),
    "SecondaryPhone"  VARCHAR(20),
    "Title"           VARCHAR(20),
    "Notes"           TEXT,
    "Attachments"     TEXT,
    "SupervisorID"    INTEGER,
    "WindowsUserName" VARCHAR(50)  UNIQUE,
    "AddedBy"         VARCHAR(255),
    "AddedOn"         TIMESTAMP,
    "ModifiedBy"      VARCHAR(255),
    "ModifiedOn"      TIMESTAMP,
    CONSTRAINT "PK_Employees"            PRIMARY KEY ("EmployeeID"),
    CONSTRAINT "FK_Employees_Titles"     FOREIGN KEY ("Title")        REFERENCES "Titles"    ("Title"),
    CONSTRAINT "FK_Employees_Supervisor" FOREIGN KEY ("SupervisorID") REFERENCES "Employees" ("EmployeeID")
);

CREATE INDEX "IX_Employees_Title"        ON "Employees" ("Title");
CREATE INDEX "IX_Employees_SupervisorID" ON "Employees" ("SupervisorID");

CREATE TABLE "EmployeePrivileges" (
    "EmployeePrivilegeID" SERIAL   NOT NULL,
    "EmployeeID"          INTEGER,
    "PrivilegeID"         INTEGER,
    "AddedBy"             VARCHAR(255),
    "AddedOn"             TIMESTAMP,
    "ModifiedBy"          VARCHAR(255),
    "ModifiedOn"          TIMESTAMP,
    CONSTRAINT "PK_EmployeePrivileges"            PRIMARY KEY ("EmployeePrivilegeID"),
    CONSTRAINT "FK_EmployeePrivileges_Employees"  FOREIGN KEY ("EmployeeID")  REFERENCES "Employees"  ("EmployeeID") ON DELETE CASCADE,
    CONSTRAINT "FK_EmployeePrivileges_Privileges" FOREIGN KEY ("PrivilegeID") REFERENCES "Privileges" ("PrivilegeID")
);

CREATE INDEX "IX_EmployeePrivileges_EmployeeID"  ON "EmployeePrivileges" ("EmployeeID");
CREATE INDEX "IX_EmployeePrivileges_PrivilegeID" ON "EmployeePrivileges" ("PrivilegeID");

CREATE TABLE "MRU" (
    "MRU_ID"     SERIAL      NOT NULL,
    "EmployeeID" INTEGER,
    "TableName"  VARCHAR(50),
    "PKValue"    INTEGER,
    "DateAdded"  TIMESTAMP,
    CONSTRAINT "PK_MRU"          PRIMARY KEY ("MRU_ID"),
    CONSTRAINT "FK_MRU_Employees" FOREIGN KEY ("EmployeeID") REFERENCES "Employees" ("EmployeeID") ON DELETE CASCADE
);

CREATE INDEX "IX_MRU_EmployeeID" ON "MRU" ("EmployeeID");

CREATE TABLE "Products" (
    "ProductID"              SERIAL         NOT NULL,
    "ProductCode"            VARCHAR(20)    UNIQUE,
    "ProductName"            VARCHAR(50)    UNIQUE,
    "ProductDescription"     TEXT,
    "StandardUnitCost"       NUMERIC(19,4),
    "UnitPrice"              NUMERIC(19,4),
    "ReorderLevel"           SMALLINT,
    "TargetLevel"            SMALLINT,
    "QuantityPerUnit"        VARCHAR(50),
    "Discontinued"           BOOLEAN,
    "MinimumReorderQuantity" SMALLINT,
    "ProductCategoryID"      INTEGER,
    "AddedBy"                VARCHAR(255),
    "AddedOn"                TIMESTAMP,
    "ModifiedBy"             VARCHAR(255),
    "ModifiedOn"             TIMESTAMP,
    CONSTRAINT "PK_Products"                  PRIMARY KEY ("ProductID"),
    CONSTRAINT "FK_Products_ProductCategories" FOREIGN KEY ("ProductCategoryID") REFERENCES "ProductCategories" ("ProductCategoryID")
);

CREATE INDEX "IX_Products_ProductCategoryID" ON "Products" ("ProductCategoryID");

CREATE TABLE "ProductVendors" (
    "ProductVendorID" SERIAL   NOT NULL,
    "ProductID"       INTEGER,
    "VendorID"        INTEGER,
    "AddedBy"         VARCHAR(255),
    "AddedOn"         TIMESTAMP,
    "ModifiedBy"      VARCHAR(255),
    "ModifiedOn"      TIMESTAMP,
    CONSTRAINT "PK_ProductVendors"            PRIMARY KEY ("ProductVendorID"),
    CONSTRAINT "FK_ProductVendors_Products"   FOREIGN KEY ("ProductID") REFERENCES "Products"  ("ProductID"),
    CONSTRAINT "FK_ProductVendors_Companies"  FOREIGN KEY ("VendorID")  REFERENCES "Companies" ("CompanyID")
);

CREATE INDEX "IX_ProductVendors_ProductID" ON "ProductVendors" ("ProductID");
CREATE INDEX "IX_ProductVendors_VendorID"  ON "ProductVendors" ("VendorID");

CREATE TABLE "StockTake" (
    "StockTakeID"      SERIAL    NOT NULL,
    "StockTakeDate"    TIMESTAMP,
    "ProductID"        INTEGER,
    "QuantityOnHand"   SMALLINT,
    "ExpectedQuantity" INTEGER,
    "AddedBy"          VARCHAR(255),
    "AddedOn"          TIMESTAMP,
    "ModifiedBy"       VARCHAR(255),
    "ModifiedOn"       TIMESTAMP,
    CONSTRAINT "PK_StockTake"          PRIMARY KEY ("StockTakeID"),
    CONSTRAINT "FK_StockTake_Products" FOREIGN KEY ("ProductID") REFERENCES "Products" ("ProductID")
);

CREATE INDEX "IX_StockTake_ProductID" ON "StockTake" ("ProductID");

-- ---------------------------------------------------------------------------
-- TIER 3: Order tables (depend on Companies, Employees, Products, TaxStatus)
-- ---------------------------------------------------------------------------

-- CustomerID and ShipperID both reference Companies.CompanyID per the
-- New_New_CompaniesOrders and New_New_CompaniesOrders1 relation files.
CREATE TABLE "Orders" (
    "OrderID"       SERIAL         NOT NULL,
    "EmployeeID"    INTEGER,
    "CustomerID"    INTEGER,
    "OrderDate"     TIMESTAMP,
    "InvoiceDate"   TIMESTAMP,
    "ShippedDate"   TIMESTAMP,
    "ShipperID"     INTEGER,
    "ShippingFee"   NUMERIC(19,4),
    "TaxRate"       REAL,
    "TaxStatusID"   SMALLINT,
    "PaymentMethod" VARCHAR(50),
    "PaidDate"      TIMESTAMP,
    "Notes"         TEXT,
    "OrderStatusID" INTEGER,
    "AddedBy"       VARCHAR(255),
    "AddedOn"       TIMESTAMP,
    "ModifiedBy"    VARCHAR(255),
    "ModifiedOn"    TIMESTAMP,
    CONSTRAINT "PK_Orders"             PRIMARY KEY ("OrderID"),
    CONSTRAINT "FK_Orders_Employees"   FOREIGN KEY ("EmployeeID")   REFERENCES "Employees"   ("EmployeeID"),
    CONSTRAINT "FK_Orders_Customer"    FOREIGN KEY ("CustomerID")   REFERENCES "Companies"   ("CompanyID"),
    CONSTRAINT "FK_Orders_Shipper"     FOREIGN KEY ("ShipperID")    REFERENCES "Companies"   ("CompanyID"),
    CONSTRAINT "FK_Orders_TaxStatus"   FOREIGN KEY ("TaxStatusID")  REFERENCES "TaxStatus"   ("TaxStatusID"),
    CONSTRAINT "FK_Orders_OrderStatus" FOREIGN KEY ("OrderStatusID") REFERENCES "OrderStatus" ("OrderStatusID")
);

CREATE INDEX "IX_Orders_EmployeeID"    ON "Orders" ("EmployeeID");
CREATE INDEX "IX_Orders_CustomerID"    ON "Orders" ("CustomerID");
CREATE INDEX "IX_Orders_ShipperID"     ON "Orders" ("ShipperID");
CREATE INDEX "IX_Orders_TaxStatusID"   ON "Orders" ("TaxStatusID");
CREATE INDEX "IX_Orders_OrderStatusID" ON "Orders" ("OrderStatusID");
CREATE INDEX "IX_Orders_OrderDate"     ON "Orders" ("OrderDate");

CREATE TABLE "OrderDetails" (
    "OrderDetailID"       SERIAL         NOT NULL,
    "OrderID"             INTEGER,
    "ProductID"           INTEGER,
    "Quantity"            SMALLINT,
    "UnitPrice"           NUMERIC(19,4),
    "Discount"            REAL,
    "OrderDetailStatusID" INTEGER,
    "AddedBy"             VARCHAR(255),
    "AddedOn"             TIMESTAMP,
    "ModifiedBy"          VARCHAR(255),
    "ModifiedOn"          TIMESTAMP,
    CONSTRAINT "PK_OrderDetails"                        PRIMARY KEY ("OrderDetailID"),
    CONSTRAINT "FK_OrderDetails_Orders"                 FOREIGN KEY ("OrderID")             REFERENCES "Orders"            ("OrderID") ON DELETE CASCADE,
    CONSTRAINT "FK_OrderDetails_Products"               FOREIGN KEY ("ProductID")           REFERENCES "Products"          ("ProductID"),
    CONSTRAINT "FK_OrderDetails_OrderDetailStatus"      FOREIGN KEY ("OrderDetailStatusID") REFERENCES "OrderDetailStatus" ("OrderDetailStatusID")
);

CREATE INDEX "IX_OrderDetails_OrderID"             ON "OrderDetails" ("OrderID");
CREATE INDEX "IX_OrderDetails_ProductID"           ON "OrderDetails" ("ProductID");
CREATE INDEX "IX_OrderDetails_OrderDetailStatusID" ON "OrderDetails" ("OrderDetailStatusID");

-- ---------------------------------------------------------------------------
-- TIER 3: Purchase order tables (depend on Companies, Employees, Products)
-- ---------------------------------------------------------------------------

-- VendorID references Companies.CompanyID per New_New_CompaniesPurchaseOrders.
-- SubmittedByID and ApprovedByID reference Employees per
--   New_New_EmployeesPurchaseOrders2 and New_New_EmployeesPurchaseOrders1.
CREATE TABLE "PurchaseOrders" (
    "PurchaseOrderID" SERIAL         NOT NULL,
    "VendorID"        INTEGER,
    "SubmittedByID"   INTEGER,
    "SubmittedDate"   TIMESTAMP,
    "ApprovedByID"    INTEGER,
    "ApprovedDate"    TIMESTAMP,
    "StatusID"        INTEGER,
    "ReceivedDate"    TIMESTAMP,
    "ShippingFee"     NUMERIC(19,4),
    "TaxAmount"       NUMERIC(19,4),
    "PaymentDate"     TIMESTAMP,
    "PaymentAmount"   NUMERIC(19,4),
    "PaymentMethod"   VARCHAR(50),
    "Notes"           TEXT,
    "AddedBy"         VARCHAR(255),
    "AddedOn"         TIMESTAMP,
    "ModifiedBy"      VARCHAR(255),
    "ModifiedOn"      TIMESTAMP,
    CONSTRAINT "PK_PurchaseOrders"              PRIMARY KEY ("PurchaseOrderID"),
    CONSTRAINT "FK_PurchaseOrders_Vendor"       FOREIGN KEY ("VendorID")      REFERENCES "Companies"          ("CompanyID"),
    CONSTRAINT "FK_PurchaseOrders_SubmittedBy"  FOREIGN KEY ("SubmittedByID") REFERENCES "Employees"          ("EmployeeID"),
    CONSTRAINT "FK_PurchaseOrders_ApprovedBy"   FOREIGN KEY ("ApprovedByID")  REFERENCES "Employees"          ("EmployeeID"),
    CONSTRAINT "FK_PurchaseOrders_Status"       FOREIGN KEY ("StatusID")      REFERENCES "PurchaseOrderStatus" ("StatusID")
);

CREATE INDEX "IX_PurchaseOrders_VendorID"      ON "PurchaseOrders" ("VendorID");
CREATE INDEX "IX_PurchaseOrders_SubmittedByID" ON "PurchaseOrders" ("SubmittedByID");
CREATE INDEX "IX_PurchaseOrders_ApprovedByID"  ON "PurchaseOrders" ("ApprovedByID");
CREATE INDEX "IX_PurchaseOrders_StatusID"      ON "PurchaseOrders" ("StatusID");

CREATE TABLE "PurchaseOrderDetails" (
    "PurchaseOrderDetailID" SERIAL         NOT NULL,
    "PurchaseOrderID"       INTEGER,
    "ProductID"             INTEGER,
    "Quantity"              SMALLINT,
    "UnitCost"              NUMERIC(19,4),
    "ReceivedDate"          TIMESTAMP,
    "AddedBy"               VARCHAR(255),
    "AddedOn"               TIMESTAMP,
    "ModifiedBy"            VARCHAR(255),
    "ModifiedOn"            TIMESTAMP,
    CONSTRAINT "PK_PurchaseOrderDetails"                   PRIMARY KEY ("PurchaseOrderDetailID"),
    CONSTRAINT "FK_PurchaseOrderDetails_PurchaseOrders"    FOREIGN KEY ("PurchaseOrderID") REFERENCES "PurchaseOrders" ("PurchaseOrderID") ON DELETE CASCADE,
    CONSTRAINT "FK_PurchaseOrderDetails_Products"          FOREIGN KEY ("ProductID")       REFERENCES "Products"       ("ProductID")
);

CREATE INDEX "IX_PurchaseOrderDetails_PurchaseOrderID" ON "PurchaseOrderDetails" ("PurchaseOrderID");
CREATE INDEX "IX_PurchaseOrderDetails_ProductID"       ON "PurchaseOrderDetails" ("ProductID");

COMMIT;
