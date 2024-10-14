$addMigration = Join-Path $PSScriptRoot "..\..\add-migration.ps1"
& $addMigration -Context ChangedByEntityDbContext -OutputDir ChangedByEntity/Persistence/Migrations