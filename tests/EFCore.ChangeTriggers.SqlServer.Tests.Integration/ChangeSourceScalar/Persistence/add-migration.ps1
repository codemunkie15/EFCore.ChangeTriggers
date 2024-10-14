$addMigration = Join-Path $PSScriptRoot "..\..\add-migration.ps1"
& $addMigration -Context ChangeSourceScalarDbContext -OutputDir ChangeSourceScalar/Persistence/Migrations