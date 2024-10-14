param(
	[Parameter(Mandatory=$true)]
    [string]$Context,

	[Parameter(Mandatory=$true)]
    [string]$OutputDir
)

$noBuildArgument = if ($Global:NoBuild) { "--no-build" } else { "" }

dotnet ef migrations add Initial `
	--context $Context `
	--output-dir $OutputDir `
	--project (Join-Path $PSScriptRoot "EFCore.ChangeTriggers.SqlServer.Tests.Integration.csproj") `
	$noBuildArgument