param (
    [switch]$SkipBuild
)

& (Join-Path $PSScriptRoot "..\..\EFCore.ChangeTriggers.Tests.Integration.Common\Scripts\initial-migration.ps1") `
    -ContextName "ChangedByScalarDbContext" `
    -SkipBuild:$SkipBuild