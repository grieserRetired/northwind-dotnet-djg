-- =============================================================================
-- 003_seed_transactional_data.sql
-- Sample transactional data for Northwind Developer Edition.
--
-- Company type IDs (from 002_seed_reference_data.sql):
--   1=Customer  2=Shipper  3=Vendor  4=Northwind
-- Tax status IDs:  0=Tax Exempt  1=Taxable
-- Order status IDs:   1=Closed  2=Invoiced  3=New  4=Shipped  5=Paid
-- OrderDetail status: 1=Allocated  2=Invoiced  3=New  4=No Stock  5=On Order  6=Shipped
-- PurchaseOrder status: 1=Approved  2=Closed  3=New  4=Submitted  5=Received
--
-- Insertion order respects all FK dependencies:
--   ProductCategories → Companies → Employees → EmployeePrivileges
--   → Products → ProductVendors → Contacts
--   → Orders → OrderDetails
--   → PurchaseOrders → PurchaseOrderDetails
-- =============================================================================

BEGIN;

-- ---------------------------------------------------------------------------
-- ProductCategories  (5 categories, IDs 1–5)
-- ---------------------------------------------------------------------------
INSERT INTO "ProductCategories"
    ("ProductCategoryID", "ProductCategoryName", "ProductCategoryCode", "ProductCategoryDesc")
VALUES
    (1, 'Beverages',            'BEV', 'Soft drinks, coffees, teas, beers, and ales'),
    (2, 'Condiments',           'CON', 'Sweet and savory sauces, relishes, spreads, and seasonings'),
    (3, 'Dried Goods & Grains', 'DRY', 'Breads, crackers, pasta, and cereal'),
    (4, 'Seafood',              'SEA', 'Seaweed and fish'),
    (5, 'Dairy Products',       'DAI', 'Cheeses and dairy');

SELECT setval(
    pg_get_serial_sequence('"ProductCategories"', 'ProductCategoryID'),
    (SELECT MAX("ProductCategoryID") FROM "ProductCategories")
);

-- ---------------------------------------------------------------------------
-- Companies  (15 rows: 1 Northwind, 6 Customers, 3 Shippers, 5 Vendors)
-- Unique constraint is (CompanyName, CompanyTypeID) — all names are distinct.
-- ---------------------------------------------------------------------------
INSERT INTO "Companies"
    ("CompanyID", "CompanyName", "CompanyTypeID", "BusinessPhone",
     "Address", "City", "StateAbbrev", "Zip", "StandardTaxStatusID")
VALUES
    ( 1, 'Northwind Traders',          4, '(206) 555-0100', '507 20th Ave E',       'Seattle',       'WA', '98122', 0),
    ( 2, 'Wide World Importers',       1, '(212) 555-0101', '123 Broadway',         'New York',      'NY', '10001', 1),
    ( 3, 'Contoso Ltd.',               1, '(415) 555-0102', '456 Market St',        'San Francisco', 'CA', '94105', 1),
    ( 4, 'Adventure Works',            1, '(512) 555-0103', '789 Congress Ave',     'Austin',        'TX', '78701', 1),
    ( 5, 'Tailspin Toys',              1, '(312) 555-0104', '321 N Michigan Ave',   'Chicago',       'IL', '60601', 1),
    ( 6, 'Fabrikam Inc.',              1, '(305) 555-0105', '1000 Brickell Ave',    'Miami',         'FL', '33131', 1),
    ( 7, 'Litware Inc.',               1, '(617) 555-0106', '200 State St',         'Boston',        'MA', '02109', 0),
    ( 8, 'Blue Yonder Airlines',       2, '(206) 555-0107', '1 Airport Way',        'Seattle',       'WA', '98188', 0),
    ( 9, 'Speedy Shipping Co.',        2, '(503) 555-0108', '88 Industrial Blvd',   'Portland',      'OR', '97201', 0),
    (10, 'Swift Cargo Express',        2, '(323) 555-0109', '4500 Alameda St',      'Los Angeles',   'CA', '90023', 0),
    (11, 'Alpine Goods Supply',        3, '(720) 555-0110', '3200 Blake St',        'Denver',        'CO', '80205', 0),
    (12, 'Pacific Coast Imports',      3, '(650) 555-0111', '900 El Camino Real',   'Menlo Park',    'CA', '94025', 0),
    (13, 'Great Plains Distributors',  3, '(785) 555-0112', '550 Kansas Ave',       'Topeka',        'KS', '66603', 0),
    (14, 'Eastern Provisions Group',   3, '(718) 555-0113', '44 Fulton St',         'Brooklyn',      'NY', '11201', 0),
    (15, 'Southern Heritage Foods',    3, '(615) 555-0114', '180 Second Ave N',     'Nashville',     'TN', '37201', 0);

SELECT setval(
    pg_get_serial_sequence('"Companies"', 'CompanyID'),
    (SELECT MAX("CompanyID") FROM "Companies")
);

-- ---------------------------------------------------------------------------
-- Employees  (5 rows, IDs 1–5)
-- Title FK references Titles.Title (VARCHAR PK seeded in 002).
-- SupervisorID self-references Employees — Fuller has no supervisor.
-- WindowsUserName must be unique.
-- ---------------------------------------------------------------------------
INSERT INTO "Employees"
    ("EmployeeID", "FirstName", "LastName", "EmailAddress", "JobTitle",
     "PrimaryPhone", "Title", "SupervisorID", "WindowsUserName")
VALUES
    (1, 'Andrew',   'Fuller',    'afuller@northwind.com',    'VP, Sales',            '(206) 555-0150', 'Mr.', NULL, 'northwind\afuller'),
    (2, 'Nancy',    'Davolio',   'ndavolio@northwind.com',   'Sales Representative', '(206) 555-0151', 'Ms.', 1,    'northwind\ndavolio'),
    (3, 'Janet',    'Leverling', 'jleverling@northwind.com', 'Sales Representative', '(206) 555-0152', 'Ms.', 1,    'northwind\jleverling'),
    (4, 'Steven',   'Buchanan',  'sbuchanan@northwind.com',  'Sales Manager',        '(206) 555-0153', 'Mr.', 1,    'northwind\sbuchanan'),
    (5, 'Margaret', 'Peacock',   'mpeacock@northwind.com',   'Sales Representative', '(206) 555-0154', 'Ms.', 4,    'northwind\mpeacock');

SELECT setval(
    pg_get_serial_sequence('"Employees"', 'EmployeeID'),
    (SELECT MAX("EmployeeID") FROM "Employees")
);

-- ---------------------------------------------------------------------------
-- EmployeePrivileges
-- Fuller and Buchanan can approve purchase orders (pApprovePO = 1).
-- ---------------------------------------------------------------------------
INSERT INTO "EmployeePrivileges"
    ("EmployeePrivilegeID", "EmployeeID", "PrivilegeID")
VALUES
    (1, 1, 1),
    (2, 4, 1);

SELECT setval(
    pg_get_serial_sequence('"EmployeePrivileges"', 'EmployeePrivilegeID'),
    (SELECT MAX("EmployeePrivilegeID") FROM "EmployeePrivileges")
);

-- ---------------------------------------------------------------------------
-- Products  (20 rows, IDs 1–20, 4 per category)
-- ---------------------------------------------------------------------------
INSERT INTO "Products"
    ("ProductID", "ProductCode", "ProductName", "ProductDescription",
     "StandardUnitCost", "UnitPrice", "ReorderLevel", "TargetLevel",
     "QuantityPerUnit", "Discontinued", "MinimumReorderQuantity", "ProductCategoryID")
VALUES
    -- Beverages (CategoryID = 1)
    ( 1, 'CHAI', 'Chai Tea',                    '10 boxes x 20 bags',        7.5000, 18.0000, 10, 40, '10 boxes x 20 bags',  false, 10, 1),
    ( 2, 'CHAN', 'Chang Beer',                   '24 - 12 oz bottles',        9.2000, 19.0000, 10, 30, '24 - 12 oz bottles',  false, 10, 1),
    ( 3, 'GUAR', 'Guarana Fantastica',           '12 - 355 ml cans',          2.5000,  4.5000, 20, 60, '12 - 355 ml cans',    false, 10, 1),
    ( 4, 'CHRT', 'Chartreuse verte',             '750 cc per bottle',        12.0000, 18.0000,  5, 20, '750 cc per bottle',   false,  5, 1),
    -- Condiments (CategoryID = 2)
    ( 5, 'ANSY', 'Aniseed Syrup',               '12 - 550 ml bottles',       6.0000, 10.0000, 25, 70, '12 - 550 ml bottles', false, 25, 2),
    ( 6, 'CACS', 'Chef Anton''s Cajun Seasoning','48 - 6 oz jars',           16.5000, 22.0000,  0,  0, '48 - 6 oz jars',      false, 10, 2),
    ( 7, 'GBOY', 'Grandma''s Boysenberry Spread','12 - 8 oz jars',           17.5000, 25.0000, 25, 50, '12 - 8 oz jars',      false, 10, 2),
    ( 8, 'NWCR', 'Northwoods Cranberry Sauce',  '12 - 12 oz jars',          28.0000, 40.0000, 10, 40, '12 - 12 oz jars',     false,  5, 2),
    -- Dried Goods & Grains (CategoryID = 3)
    ( 9, 'GNOC', 'Gnocchi di nonna Alice',      '24 - 250 g pkgs.',         26.5000, 38.0000, 30, 80, '24 - 250 g pkgs.',    false, 10, 3),
    (10, 'IPOH', 'Ipoh Coffee',                 '16 - 500 g tins',          32.0000, 46.0000, 25, 50, '16 - 500 g tins',     false,  5, 3),
    (11, 'LGFT', 'Longlife Tofu',               '5 kg pkg.',                 7.0000, 10.0000,  5, 20, '5 kg pkg.',           false,  5, 3),
    (12, 'WGSK', 'Wimmers gute Semmelknodel',   '20 bags x 4 pieces',       23.0000, 33.2500, 20, 60, '20 bags x 4 pieces',  false, 10, 3),
    -- Seafood (CategoryID = 4)
    (13, 'BCMR', 'Boston Crab Meat',            '24 - 4 oz tins',           12.8000, 18.4000, 30, 70, '24 - 4 oz tins',      false, 20, 4),
    (14, 'JACC', 'Jack''s Clam Chowder',        '12 - 12 oz cans',           6.5000,  9.6500, 20, 60, '12 - 12 oz cans',     false, 10, 4),
    (15, 'INSL', 'Inlagd Sill',                 '24 - 250 g jars',          13.0000, 19.0000, 10, 40, '24 - 250 g jars',     false, 10, 4),
    (16, 'GVLX', 'Gravad lax',                  '12 - 500 g pkgs.',         18.0000, 26.0000,  5, 30, '12 - 500 g pkgs.',    false,  5, 4),
    -- Dairy Products (CategoryID = 5)
    (17, 'CAMP', 'Camembert Pierrot',           '15 - 300 g rounds',        23.5000, 34.0000, 20, 60, '15 - 300 g rounds',   false, 10, 5),
    (18, 'RACL', 'Raclette Courdavault',        '5 kg pkg.',                38.5000, 55.0000, 15, 50, '5 kg pkg.',           false,  5, 5),
    (19, 'GEIT', 'Geitost',                     '500 g',                     1.5000,  2.5000,  5, 40, '500 g',               false, 20, 5),
    (20, 'MOZZ', 'Mozzarella di Giovanni',      '24 - 200 g pkgs.',         24.5000, 34.8000, 15, 40, '24 - 200 g pkgs.',    false, 10, 5);

SELECT setval(
    pg_get_serial_sequence('"Products"', 'ProductID'),
    (SELECT MAX("ProductID") FROM "Products")
);

-- ---------------------------------------------------------------------------
-- ProductVendors  (one vendor per product, IDs 1–20)
-- Alpine (11) → Beverages | Pacific Coast (12) → Condiments
-- Great Plains (13) → Dried Goods | Eastern (14) → Seafood
-- Southern Heritage (15) → Dairy
-- ---------------------------------------------------------------------------
INSERT INTO "ProductVendors"
    ("ProductVendorID", "ProductID", "VendorID")
VALUES
    ( 1,  1, 11), ( 2,  2, 11), ( 3,  3, 11), ( 4,  4, 11),
    ( 5,  5, 12), ( 6,  6, 12), ( 7,  7, 12), ( 8,  8, 12),
    ( 9,  9, 13), (10, 10, 13), (11, 11, 13), (12, 12, 13),
    (13, 13, 14), (14, 14, 14), (15, 15, 14), (16, 16, 14),
    (17, 17, 15), (18, 18, 15), (19, 19, 15), (20, 20, 15);

SELECT setval(
    pg_get_serial_sequence('"ProductVendors"', 'ProductVendorID'),
    (SELECT MAX("ProductVendorID") FROM "ProductVendors")
);

-- ---------------------------------------------------------------------------
-- Contacts  (7 rows: one per customer company + one vendor contact)
-- ---------------------------------------------------------------------------
INSERT INTO "Contacts"
    ("ContactID", "CompanyID", "LastName", "FirstName", "EmailAddress", "JobTitle", "PrimaryPhone")
VALUES
    (1,  2, 'Berg',      'Karen',    'k.berg@wideworldimporters.com',  'Sales Manager',        '(212) 555-0201'),
    (2,  3, 'Martinez',  'Michael',  'm.martinez@contoso.com',         'Procurement Director', '(415) 555-0202'),
    (3,  4, 'Lee',       'Jennifer', 'j.lee@adventureworks.com',       'Purchasing Agent',     '(512) 555-0203'),
    (4,  5, 'Kim',       'Robert',   'r.kim@tailspintoys.com',         'VP, Operations',       '(312) 555-0204'),
    (5,  6, 'Thompson',  'Patricia', 'p.thompson@fabrikam.com',        'Buyer',                '(305) 555-0205'),
    (6,  7, 'Chen',      'David',    'd.chen@litware.com',             'Supply Chain Manager', '(617) 555-0206'),
    (7, 11, 'Huang',     'Victor',   'v.huang@alpinegoods.com',        'Account Manager',      '(720) 555-0207');

SELECT setval(
    pg_get_serial_sequence('"Contacts"', 'ContactID'),
    (SELECT MAX("ContactID") FROM "Contacts")
);

-- ---------------------------------------------------------------------------
-- Orders  (15 rows, IDs 1–15)
-- Customers: companies 2–7 (type=1)
-- Shippers:  companies 8–10 (type=2)
-- Employees: 1–5
-- Mix of all five order statuses across a ~6-month window in 2024.
-- ---------------------------------------------------------------------------
INSERT INTO "Orders"
    ("OrderID", "EmployeeID", "CustomerID", "OrderDate", "InvoiceDate",
     "ShippedDate", "ShipperID", "ShippingFee", "TaxRate", "TaxStatusID",
     "PaymentMethod", "PaidDate", "OrderStatusID")
VALUES
    -- Paid orders (OrderStatusID = 5)
    ( 1, 2, 2, '2024-01-15', '2024-01-18', '2024-01-19',  8, 12.5000, 0.08, 1, 'Credit Card',   '2024-01-22', 5),
    ( 2, 3, 3, '2024-01-22', '2024-01-25', '2024-01-26',  9,  8.7500, 0.08, 1, 'Credit Card',   '2024-02-01', 5),
    (15, 3, 4, '2024-02-28', '2024-03-04', '2024-03-05',  8, 11.5000, 0.08, 1, 'Wire Transfer', '2024-03-07', 5),
    -- Closed orders (OrderStatusID = 1)
    ( 3, 2, 4, '2024-02-03', '2024-02-07', '2024-02-08',  8, 15.0000, 0.00, 0, 'Check',          NULL,        1),
    ( 4, 4, 5, '2024-02-14', '2024-02-18', '2024-02-19', 10,  9.5000, 0.08, 1, 'Check',          NULL,        1),
    -- Shipped orders (OrderStatusID = 4)
    ( 5, 5, 6, '2024-03-05',  NULL,         '2024-03-10',  9, 11.0000, 0.08, 1,  NULL,            NULL,        4),
    ( 6, 3, 7, '2024-03-12',  NULL,         '2024-03-18',  8,  7.2500, 0.00, 0,  NULL,            NULL,        4),
    ( 7, 2, 2, '2024-03-20',  NULL,         '2024-03-26',  9, 13.7500, 0.08, 1,  NULL,            NULL,        4),
    ( 8, 4, 3, '2024-04-02',  NULL,         '2024-04-08', 10, 18.5000, 0.00, 0,  NULL,            NULL,        4),
    -- Invoiced orders (OrderStatusID = 2)
    ( 9, 5, 4, '2024-04-15', '2024-04-22',  NULL,          8, 10.0000, 0.08, 1,  NULL,            NULL,        2),
    (10, 2, 5, '2024-04-28', '2024-05-05',  NULL,          9, 14.2500, 0.08, 1,  NULL,            NULL,        2),
    (11, 3, 6, '2024-05-10', '2024-05-17',  NULL,         10, 16.0000, 0.08, 1,  NULL,            NULL,        2),
    -- New orders (OrderStatusID = 3)
    (12, 4, 7, '2024-05-22',  NULL,          NULL,          8,  0.0000, 0.00, 0,  NULL,            NULL,        3),
    (13, 5, 2, '2024-06-01',  NULL,          NULL,          9,  0.0000, 0.08, 1,  NULL,            NULL,        3),
    (14, 2, 3, '2024-06-10',  NULL,          NULL,         10,  0.0000, 0.08, 1,  NULL,            NULL,        3);

SELECT setval(
    pg_get_serial_sequence('"Orders"', 'OrderID'),
    (SELECT MAX("OrderID") FROM "Orders")
);

-- ---------------------------------------------------------------------------
-- OrderDetails  (37 rows)
-- Status aligns with parent order: Paid/Closed/Invoiced → 2 (Invoiced)
--                                  Shipped → 6 (Shipped)
--                                  New     → 3 (New)
-- ---------------------------------------------------------------------------
INSERT INTO "OrderDetails"
    ("OrderDetailID", "OrderID", "ProductID", "Quantity", "UnitPrice", "Discount", "OrderDetailStatusID")
VALUES
    -- Order 1 – Paid (3 lines)
    ( 1,  1,  1, 12, 18.0000, 0.00, 2),
    ( 2,  1,  5, 10, 10.0000, 0.05, 2),
    ( 3,  1, 17,  5, 34.0000, 0.00, 2),
    -- Order 2 – Paid (2 lines)
    ( 4,  2,  2, 20, 19.0000, 0.10, 2),
    ( 5,  2,  9,  8, 38.0000, 0.00, 2),
    -- Order 3 – Closed (2 lines)
    ( 6,  3, 13, 15, 18.4000, 0.00, 2),
    ( 7,  3, 19, 30,  2.5000, 0.00, 2),
    -- Order 4 – Closed (3 lines)
    ( 8,  4,  6, 12, 22.0000, 0.05, 2),
    ( 9,  4, 10,  6, 46.0000, 0.00, 2),
    (10,  4, 20,  4, 34.8000, 0.00, 2),
    -- Order 5 – Shipped (2 lines)
    (11,  5,  3, 24,  4.5000, 0.00, 6),
    (12,  5,  7,  6, 25.0000, 0.10, 6),
    -- Order 6 – Shipped (3 lines)
    (13,  6,  4,  8, 18.0000, 0.00, 6),
    (14,  6, 11, 20, 10.0000, 0.00, 6),
    (15,  6, 14, 15,  9.6500, 0.05, 6),
    -- Order 7 – Shipped (2 lines)
    (16,  7, 15, 12, 19.0000, 0.00, 6),
    (17,  7, 18,  3, 55.0000, 0.10, 6),
    -- Order 8 – Shipped (3 lines)
    (18,  8,  8, 10, 40.0000, 0.00, 6),
    (19,  8, 12, 14, 33.2500, 0.00, 6),
    (20,  8, 16,  6, 26.0000, 0.05, 6),
    -- Order 9 – Invoiced (2 lines)
    (21,  9,  1, 15, 18.0000, 0.00, 2),
    (22,  9, 13, 20, 18.4000, 0.05, 2),
    -- Order 10 – Invoiced (3 lines)
    (23, 10,  2, 30, 19.0000, 0.10, 2),
    (24, 10,  6,  8, 22.0000, 0.00, 2),
    (25, 10, 17,  6, 34.0000, 0.00, 2),
    -- Order 11 – Invoiced (2 lines)
    (26, 11, 10,  4, 46.0000, 0.00, 2),
    (27, 11, 18,  5, 55.0000, 0.10, 2),
    -- Order 12 – New (2 lines)
    (28, 12,  5,  8, 10.0000, 0.00, 3),
    (29, 12,  9, 10, 38.0000, 0.00, 3),
    -- Order 13 – New (3 lines)
    (30, 13,  3, 36,  4.5000, 0.00, 3),
    (31, 13, 14, 12,  9.6500, 0.00, 3),
    (32, 13, 19, 20,  2.5000, 0.00, 3),
    -- Order 14 – New (2 lines)
    (33, 14,  4,  5, 18.0000, 0.00, 3),
    (34, 14, 11, 25, 10.0000, 0.05, 3),
    -- Order 15 – Paid (3 lines)
    (35, 15,  7,  8, 25.0000, 0.00, 2),
    (36, 15, 15, 10, 19.0000, 0.00, 2),
    (37, 15, 20,  6, 34.8000, 0.05, 2);

SELECT setval(
    pg_get_serial_sequence('"OrderDetails"', 'OrderDetailID'),
    (SELECT MAX("OrderDetailID") FROM "OrderDetails")
);

-- ---------------------------------------------------------------------------
-- PurchaseOrders  (8 rows, IDs 1–8)
-- Vendors: 11=Alpine  12=Pacific Coast  13=Great Plains  14=Eastern  15=Southern
-- ApprovedBy must have PrivilegeID=1: employees 1 (Fuller) and 4 (Buchanan).
-- Submitted PO (status=4) has no approver yet.
-- New/draft PO (status=3) has no submitted date.
-- Payment amounts for Closed POs reflect line-item costs + shipping.
-- ---------------------------------------------------------------------------
INSERT INTO "PurchaseOrders"
    ("PurchaseOrderID", "VendorID", "SubmittedByID", "SubmittedDate",
     "ApprovedByID", "ApprovedDate", "StatusID", "ReceivedDate",
     "ShippingFee", "TaxAmount", "PaymentDate", "PaymentAmount", "PaymentMethod", "Notes")
VALUES
    (1, 11, 2, '2024-01-05', 1, '2024-01-07', 2, '2024-01-15', 15.0000, 0.0000, '2024-01-20',  942.0000, 'Check',         'Initial beverages restock'),
    (2, 12, 3, '2024-01-20', 1, '2024-01-22', 2, '2024-01-30', 12.0000, 0.0000, '2024-02-05', 1184.5000, 'Check',         'Initial condiments restock'),
    (3, 13, 4, '2024-02-10', 1, '2024-02-12', 5, '2024-03-01', 18.0000, 0.0000, NULL,           NULL,     NULL,            'Dried goods Q1 restock'),
    (4, 14, 2, '2024-02-25', 1, '2024-02-27', 5, '2024-03-10', 20.0000, 0.0000, NULL,           NULL,     NULL,            'Seafood Q1 restock'),
    (5, 15, 5, '2024-04-01', 4, '2024-04-03', 1, NULL,         10.0000, 0.0000, NULL,           NULL,     NULL,            'Dairy products spring order'),
    (6, 11, 3, '2024-05-15', NULL, NULL,        4, NULL,          8.0000, 0.0000, NULL,           NULL,     NULL,            'Beverage restock for summer'),
    (7, 12, 2, NULL,         NULL, NULL,        3, NULL,          0.0000, 0.0000, NULL,           NULL,     NULL,            'Condiments Q3 planning draft'),
    (8, 13, 4, '2024-03-01', 1, '2024-03-03', 2, '2024-03-15', 14.0000, 0.0000, '2024-03-20', 1596.5000, 'Wire Transfer', 'Dried goods Q1 supplemental');

SELECT setval(
    pg_get_serial_sequence('"PurchaseOrders"', 'PurchaseOrderID'),
    (SELECT MAX("PurchaseOrderID") FROM "PurchaseOrders")
);

-- ---------------------------------------------------------------------------
-- PurchaseOrderDetails  (18 rows)
-- ReceivedDate is NULL for POs that have not yet been received (status 1,3,4).
-- ---------------------------------------------------------------------------
INSERT INTO "PurchaseOrderDetails"
    ("PurchaseOrderDetailID", "PurchaseOrderID", "ProductID", "Quantity", "UnitCost", "ReceivedDate")
VALUES
    -- PO 1 – Alpine, Beverages (Closed)
    ( 1, 1,  1, 50,  7.5000, '2024-01-15'),
    ( 2, 1,  2, 60,  9.2000, '2024-01-15'),
    -- PO 2 – Pacific Coast, Condiments (Closed)
    ( 3, 2,  5, 40,  6.0000, '2024-01-30'),
    ( 4, 2,  6, 30, 16.5000, '2024-01-30'),
    ( 5, 2,  7, 25, 17.5000, '2024-01-30'),
    -- PO 3 – Great Plains, Dried Goods (Received)
    ( 6, 3,  9, 35, 26.5000, '2024-03-01'),
    ( 7, 3, 10, 20, 32.0000, '2024-03-01'),
    ( 8, 3, 11, 60,  7.0000, '2024-03-01'),
    -- PO 4 – Eastern, Seafood (Received)
    ( 9, 4, 13, 80, 12.8000, '2024-03-10'),
    (10, 4, 14, 60,  6.5000, '2024-03-10'),
    (11, 4, 15, 40, 13.0000, '2024-03-10'),
    -- PO 5 – Southern Heritage, Dairy (Approved — not yet received)
    (12, 5, 17, 30, 23.5000, NULL),
    (13, 5, 18, 20, 38.5000, NULL),
    -- PO 6 – Alpine, Beverages (Submitted — not yet received)
    (14, 6,  1, 40,  7.5000, NULL),
    (15, 6,  4, 30, 12.0000, NULL),
    -- PO 7 – Pacific Coast, Condiments (New draft)
    (16, 7,  8, 20, 28.0000, NULL),
    -- PO 8 – Great Plains, Dried Goods (Closed)
    (17, 8, 12, 40, 23.0000, '2024-03-15'),
    (18, 8,  9, 25, 26.5000, '2024-03-15');

SELECT setval(
    pg_get_serial_sequence('"PurchaseOrderDetails"', 'PurchaseOrderDetailID'),
    (SELECT MAX("PurchaseOrderDetailID") FROM "PurchaseOrderDetails")
);

COMMIT;
