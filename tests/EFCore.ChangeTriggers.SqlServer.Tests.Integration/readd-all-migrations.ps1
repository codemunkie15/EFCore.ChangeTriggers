$global:NoBuild = $false
$persistenceDirs = Get-ChildItem -Recurse -Directory | Where-Object { $_.FullName -like "$PSScriptRoot\*Persistence" }

# Delete current migrations before rebuild
foreach ($persistenceDir in $persistenceDirs) {
    $migrationsPath = Join-Path $persistenceDir.FullName "Migrations"
    
    if (Test-Path $migrationsPath) {
        Write-Output "Deleting Migrations folder: $migrationsPath"
        Remove-Item -Recurse -Force -Path $migrationsPath
    } else {
        Write-Output "Migrations folder not found in: $($persistenceDir.FullName)"
    }
}

# Add migrations
foreach ($persistenceDir in $persistenceDirs) {
    $addMigrationScript = Join-Path $persistenceDir.FullName "add-migration.ps1"
    
    if (Test-Path $addMigrationScript) {
        Write-Output "Running add-migration.ps1 in: $($persistenceDir.FullName)"

        $noBuildArgument = if ($loopCounter > 0) { "-NoBuild" } else { "" }
        & $addMigrationScript $noBuildArgument
    } else {
        Write-Output "add-migration.ps1 not found in: $($persistenceDir.FullName)"
    }

    # Only build once
    $global:NoBuild = $true
}