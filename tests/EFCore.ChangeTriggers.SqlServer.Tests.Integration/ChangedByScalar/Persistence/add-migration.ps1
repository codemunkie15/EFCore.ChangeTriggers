$addMigration = Join-Path $PSScriptRoot "..\..\add-migration.ps1"
& $addMigration -Context ChangedByScalarDbContext -OutputDir ChangedByScalar/Persistence/Migrations