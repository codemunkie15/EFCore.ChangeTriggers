$addMigration = Join-Path $PSScriptRoot "..\..\add-migration.ps1"
& $addMigration -Context ChangeSourceEntityDbContext -OutputDir ChangeSourceEntity/Persistence/Migrations