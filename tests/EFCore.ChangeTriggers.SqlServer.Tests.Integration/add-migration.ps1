param(
	[Parameter(Mandatory=$true)]
    [string]$Context,

    [string]$OutputDir = (Join-Path $MyInvocation.PSScriptRoot "Migrations")
)

$noBuildArgument = if ($Global:NoBuild) { "--no-build" } else { "" }

if (Test-Path $OutputDir)
{
	Write-Host "Removing existing migrations..."
	Remove-Item -Recurse -Force -Path $OutputDir
}

Write-Host "Scaffolding new migration for $Context..."
dotnet ef migrations add Initial `
	--context $Context `
	--output-dir $OutputDir `
	--project (Join-Path $PSScriptRoot "EFCore.ChangeTriggers.SqlServer.Tests.Integration.csproj") `
	$noBuildArgument