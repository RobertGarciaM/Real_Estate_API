# Real_Estate_API

Steps to execute the solution

1. Clone this repository (the MASTER BRANCH contains the consolidated changes).

2. Open the Real Estate API/appsettings.json file and make sure to change the connection to your database server.

"ConnectionStrings": {
    "SqlServerConnection": "Server=localhost;Database=RealEstate;User Id=sa;Password=Test123!;TrustServerCertificate=True;"
  }

3. Install the necessary dependencies, this application is built on .Net 6.0

4. Tip: When we think about querying millions of records in a database using multiple filters we can get performance problems. My experience has taught me that theory is not 100% true in practice so for this kind of problems many solutions have been designed, one of them is the creation of indexes in the database, I suggest you to run the following script to improve the performance. However if we abuse the creation of indexes we can get serious problems, so it is important to analyze the application in different environments and with different number of users. I leave the script for you to test which of them you need according to your needs.

----------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Price' AND object_id = OBJECT_ID('Properties'))
BEGIN
    DROP INDEX IX_Price ON Properties;
END
CREATE INDEX IX_Price ON Properties (Price);

-- Índice para Year
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Year' AND object_id = OBJECT_ID('Properties'))
BEGIN
    DROP INDEX IX_Year ON Properties;
END
CREATE INDEX IX_Year ON Properties (Year);

-- Índice para Name
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Name' AND object_id = OBJECT_ID('Properties'))
BEGIN
    DROP INDEX IX_Name ON Properties;
END
CREATE INDEX IX_Name ON Properties (Name);

-- Índice para Address
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Address' AND object_id = OBJECT_ID('Properties'))
BEGIN
    DROP INDEX IX_Address ON Properties;
END
CREATE INDEX IX_Address ON Properties (Address);

-- Índice para CodeInternal
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CodeInternal' AND object_id = OBJECT_ID('Properties'))
BEGIN
    DROP INDEX IX_CodeInternal ON Properties;
END
CREATE INDEX IX_CodeInternal ON Properties (CodeInternal);

-- Índice para IdProperty
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_IdProperty' AND object_id = OBJECT_ID('Properties'))
BEGIN
    DROP INDEX IX_IdProperty ON Properties;
END
CREATE INDEX IX_IdProperty ON Properties (IdProperty);
----------------------------------------------------------------------------------------------------------------


6. launch the application by running the api from visual studio.

At the moment of executing the application the following will happen

1. the migrations will be applied on the database, creating the necessary entities for the execution of the project.

2. An Administrator user will be created with the following user admin@example.com and the following password AdminPassword123!

3. A Standard user will be created with the following user user@example.com and password UserPassword123!

Note: The Administrator user has access to the entire application, while the Standard user can only read information.

Some of the things we can improve in the application:
1. Use of a cache to limit the number of times we go to the database.
2. Use of containers to improve scaling.
3. Differential loading
...
