-- =============================================================================
-- 002_seed_reference_data.sql
-- Seed data for Northwind Developer Edition reference/lookup tables.
--
-- IDs are sourced directly from the VBA enums in modGlobal.bas, which the
-- application code uses as integer literals (e.g. enumOrderStatus.osNew = 3).
-- Any future application code must keep those enums in sync with these rows.
--
-- SERIAL sequences are advanced after each block so that the next application-
-- generated INSERT receives the correct next value.
-- =============================================================================

BEGIN;

-- ---------------------------------------------------------------------------
-- TaxStatus
-- Source: enumTaxStatus { txTaxExempt = 0, txTaxable = 1 }
-- TaxStatusID is SMALLINT (not SERIAL) — no sequence to advance.
-- ---------------------------------------------------------------------------
INSERT INTO "TaxStatus" ("TaxStatusID", "TaxStatus") VALUES
    (0, 'Tax Exempt'),
    (1, 'Taxable');

-- ---------------------------------------------------------------------------
-- CompanyTypes
-- Source: enumCompanyType { ctCustomer=1, ctShipper=2, ctVendor=3, ctNorthwind=4 }
-- Note: ctAll=0 is a UI filter sentinel only; it has no corresponding table row.
-- ---------------------------------------------------------------------------
INSERT INTO "CompanyTypes" ("CompanyTypeID", "CompanyType") VALUES
    (1, 'Customer'),
    (2, 'Shipper'),
    (3, 'Vendor'),
    (4, 'Northwind');

SELECT setval(
    pg_get_serial_sequence('"CompanyTypes"', 'CompanyTypeID'),
    (SELECT MAX("CompanyTypeID") FROM "CompanyTypes")
);

-- ---------------------------------------------------------------------------
-- OrderStatus
-- Source: enumOrderStatus { osClosed=1, osInvoiced=2, osNew=3, osShipped=4, osPaid=5 }
-- SortOrder reflects natural order-lifecycle progression.
-- OrderStatusCode is the 2-5 char abbreviation used for display/filtering.
-- ---------------------------------------------------------------------------
INSERT INTO "OrderStatus" ("OrderStatusID", "OrderStatusCode", "OrderStatusName", "SortOrder") VALUES
    (1, 'CL', 'Closed',   5),
    (2, 'IN', 'Invoiced', 2),
    (3, 'NW', 'New',      1),
    (4, 'SH', 'Shipped',  3),
    (5, 'PD', 'Paid',     4);

SELECT setval(
    pg_get_serial_sequence('"OrderStatus"', 'OrderStatusID'),
    (SELECT MAX("OrderStatusID") FROM "OrderStatus")
);

-- ---------------------------------------------------------------------------
-- OrderDetailStatus
-- Source: enumOrderDetailStatus
--   { odsAllocated=1, odsInvoiced=2, odsNew=3, odsNoStock=4, odsOnOrder=5, odsShipped=6 }
-- SortOrder reflects line-item lifecycle: New → Allocated → No Stock → On Order
--   → Shipped → Invoiced.
-- IDs 1, 4, 5 (Allocated, No Stock, On Order) are the "open" statuses queried
--   by modInventory for stock allocation logic.
-- ---------------------------------------------------------------------------
INSERT INTO "OrderDetailStatus" ("OrderDetailStatusID", "OrderDetailStatusName", "SortOrder") VALUES
    (1, 'Allocated', 2),
    (2, 'Invoiced',  6),
    (3, 'New',       1),
    (4, 'No Stock',  3),
    (5, 'On Order',  4),
    (6, 'Shipped',   5);

SELECT setval(
    pg_get_serial_sequence('"OrderDetailStatus"', 'OrderDetailStatusID'),
    (SELECT MAX("OrderDetailStatusID") FROM "OrderDetailStatus")
);

-- ---------------------------------------------------------------------------
-- PurchaseOrderStatus
-- Source: enumPurchaseOrderStatus
--   { posApprove=1, posClosed=2, posNew=3, posSubmitted=4, posReceived=5 }
-- SortOrder reflects PO lifecycle: New → Submitted → Approved → Received → Closed.
-- StatusID=1 ('Approved') is the specific value checked by modInventory when
--   calculating stock on order (posApprove = 1).
-- ---------------------------------------------------------------------------
INSERT INTO "PurchaseOrderStatus" ("StatusID", "StatusName", "SortOrder") VALUES
    (1, 'Approved',  3),
    (2, 'Closed',    5),
    (3, 'New',       1),
    (4, 'Submitted', 2),
    (5, 'Received',  4);

SELECT setval(
    pg_get_serial_sequence('"PurchaseOrderStatus"', 'StatusID'),
    (SELECT MAX("StatusID") FROM "PurchaseOrderStatus")
);

-- ---------------------------------------------------------------------------
-- Privileges
-- Source: enumPrivileges { pApprovePO = 1 }
-- Only one privilege exists in this version of Northwind (modSecurity confirms:
-- "Currently, only one privilege exists: approve Purchase Orders").
-- ---------------------------------------------------------------------------
INSERT INTO "Privileges" ("PrivilegeID", "PrivilegeName") VALUES
    (1, 'Approve Purchase Orders');

SELECT setval(
    pg_get_serial_sequence('"Privileges"', 'PrivilegeID'),
    (SELECT MAX("PrivilegeID") FROM "Privileges")
);

-- ---------------------------------------------------------------------------
-- Titles  (VARCHAR primary key — no sequence)
-- Standard professional and social salutations used on Employees records.
-- ---------------------------------------------------------------------------
INSERT INTO "Titles" ("Title") VALUES
    ('Dr.'),
    ('Miss'),
    ('Mr.'),
    ('Mrs.'),
    ('Ms.'),
    ('Prof.');

-- ---------------------------------------------------------------------------
-- States  (VARCHAR primary key — no sequence)
-- All 50 US states plus the District of Columbia.
-- ---------------------------------------------------------------------------
INSERT INTO "States" ("StateAbbrev", "StateName") VALUES
    ('AL', 'Alabama'),
    ('AK', 'Alaska'),
    ('AZ', 'Arizona'),
    ('AR', 'Arkansas'),
    ('CA', 'California'),
    ('CO', 'Colorado'),
    ('CT', 'Connecticut'),
    ('DC', 'District of Columbia'),
    ('DE', 'Delaware'),
    ('FL', 'Florida'),
    ('GA', 'Georgia'),
    ('HI', 'Hawaii'),
    ('ID', 'Idaho'),
    ('IL', 'Illinois'),
    ('IN', 'Indiana'),
    ('IA', 'Iowa'),
    ('KS', 'Kansas'),
    ('KY', 'Kentucky'),
    ('LA', 'Louisiana'),
    ('ME', 'Maine'),
    ('MD', 'Maryland'),
    ('MA', 'Massachusetts'),
    ('MI', 'Michigan'),
    ('MN', 'Minnesota'),
    ('MS', 'Mississippi'),
    ('MO', 'Missouri'),
    ('MT', 'Montana'),
    ('NE', 'Nebraska'),
    ('NV', 'Nevada'),
    ('NH', 'New Hampshire'),
    ('NJ', 'New Jersey'),
    ('NM', 'New Mexico'),
    ('NY', 'New York'),
    ('NC', 'North Carolina'),
    ('ND', 'North Dakota'),
    ('OH', 'Ohio'),
    ('OK', 'Oklahoma'),
    ('OR', 'Oregon'),
    ('PA', 'Pennsylvania'),
    ('RI', 'Rhode Island'),
    ('SC', 'South Carolina'),
    ('SD', 'South Dakota'),
    ('TN', 'Tennessee'),
    ('TX', 'Texas'),
    ('UT', 'Utah'),
    ('VT', 'Vermont'),
    ('VA', 'Virginia'),
    ('WA', 'Washington'),
    ('WV', 'West Virginia'),
    ('WI', 'Wisconsin'),
    ('WY', 'Wyoming');

COMMIT;
