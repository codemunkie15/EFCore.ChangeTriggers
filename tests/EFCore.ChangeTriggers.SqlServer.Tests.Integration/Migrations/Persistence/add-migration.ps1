$addMigration = Join-Path $PSScriptRoot "..\..\add-migration.ps1"
& $addMigration -Context MigrationsDbContext -OutputDir Migrations/Persistence/Migrations