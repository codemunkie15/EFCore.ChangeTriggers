$global:NoBuild = $false
$persistenceDirs = Get-ChildItem -Recurse -Directory | Where-Object { $_.FullName -like (Join-Path $PSScriptRoot "*Persistence") }

Write-Host "Found $($persistenceDirs.Count) directories to process..."

# Delete current migrations in-case running locally
foreach ($persistenceDir in $persistenceDirs) {
    $migrationsPath = Join-Path $persistenceDir.FullName "Migrations"
    
    if (Test-Path $migrationsPath) {
        Write-Host "Deleting Migrations folder: $migrationsPath"
        Remove-Item -Recurse -Force -Path $migrationsPath
    } else {
        Write-Host "Migrations folder not found in: $($persistenceDir.FullName)"
    }
}

# Add migrations
foreach ($persistenceDir in $persistenceDirs) {
    $addMigrationScript = Join-Path $persistenceDir.FullName "add-migration.ps1"
    
    if (Test-Path $addMigrationScript) {
        Write-Host "Running add-migration.ps1 in: $($persistenceDir.FullName)"

        & $addMigrationScript
    } else {
        Write-Host "add-migration.ps1 not found in: $($persistenceDir.FullName)"
    }

    # Only build once
    $global:NoBuild = $true
}