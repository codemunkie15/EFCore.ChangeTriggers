param (
    [Parameter(Mandatory = $true)]
    [string]$ContextName,

    [switch]$SkipBuild
)

$scriptRoot = $MyInvocation.PSScriptRoot
$projectDir = Join-Path $scriptRoot "..\.."
$migrationDir = Join-Path $scriptRoot "..\Migrations"

# Clean out old migrations
if (Test-Path $migrationDir) {
    Remove-Item -Recurse -Force $migrationDir
}

$noBuildFlag = ""
if ($SkipBuild) {
    $noBuildFlag = "--no-build"
}

Write-Host "`nScaffolding Initial migration for $ContextName into $migrationDir..."
dotnet ef migrations add Initial `
    --context $ContextName `
    --project $projectDir `
    --startup-project $projectDir `
    --output-dir $migrationDir `
    $noBuildFlag