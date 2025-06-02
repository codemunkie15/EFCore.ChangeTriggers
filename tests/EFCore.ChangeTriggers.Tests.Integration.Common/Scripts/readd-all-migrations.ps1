# Find all initial-migration scripts in test projects
$scripts = Get-ChildItem -Path "..\..\" -Recurse -Filter "*-initial-migration.ps1" | Where-Object {
    $_.DirectoryName -ne $PSScriptRoot  # Exclude scripts in this directory
}

Write-Host "Found $($scripts.Count) scripts to process..."

# Group by test project path
$scriptsByProject = $scripts | Group-Object { (Resolve-Path (Join-Path $_.Directory.FullName "..\")).Path }

foreach ($group in $scriptsByProject) {
    $projectPath = $group.Name
    $projectName = Split-Path $projectPath -Leaf
    Write-Host "`nCleaning and building test project $projectName`n"

    # 1. Clean all Migrations folders under this test project
    foreach ($script in $group.Group) {
        $migrationDir = Join-Path $script.Directory.Parent.FullName "Migrations"
        if (Test-Path $migrationDir) {
            Write-Host "Removing $migrationDir"
            Remove-Item $migrationDir -Recurse -Force
        }
    }

    # 2. Build the project once
    dotnet build $projectPath

    # 3. Re-add each migration (skip EF build)
    foreach ($script in $group.Group) {
        Write-Host "Running $($script.FullName)"
        & $script.FullName -SkipBuild
    }
}

Write-Host "`nAll integration test migrations rebuilt."
