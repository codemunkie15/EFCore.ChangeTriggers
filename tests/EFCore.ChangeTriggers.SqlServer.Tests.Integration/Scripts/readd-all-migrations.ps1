$global:NoBuild = $false

# Find all add-migration.ps1 files at least one level below $PSScriptRoot
$addMigrationScripts = Get-ChildItem -Path "../" -Recurse -Filter "add-migration.ps1" | Where-Object {
    $_.DirectoryName -ne $PSScriptRoot  # Exclude top-level scripts as name could be the same
}

Write-Host "Found $($addMigrationScripts.Count) add-migration.ps1 files..."

# Delete current migrations folders
foreach ($script in $addMigrationScripts) {
    $persistenceDir = Split-Path $script.FullName -Parent
    $migrationsPath = Join-Path $persistenceDir "Migrations"

    if (Test-Path $migrationsPath) {
        Write-Host "Deleting Migrations folder: $migrationsPath"
        Remove-Item -Recurse -Force -Path $migrationsPath
    }
}

# Run add-migration scripts
foreach ($script in $addMigrationScripts) {
    Write-Host "Running add-migration.ps1 in: $($script.DirectoryName)"
    & $script.FullName

    # Only need to build on first loop iteration
    $global:NoBuild = $true
}
