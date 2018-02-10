to add migrations, in Visual Studio navigate to Tools->NuGet Package Manager->Package Manager Console

in console change the project to current and run following comands:

for the first time:
Enable-Migrations -ContextTypeName StocksData.Contexts.StockEfMigrationContext
Add-Migration Initial
Update-Database -Script -SourceMigration:0 to generate sql file
Update-Database -SourceMigration:Initial - to run migration on db