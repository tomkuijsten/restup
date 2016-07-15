<#

.SYNOPSIS

Builds the WebServer and WebServer.Logging project. Optionally updates the release notes in the .nuspec file.

.DESCRIPTION

This file is meant as an easy way to generate the restup releated packages and test them before release using the file system as a package source.

The loggingVersion & loggingSimpleVersion is optional and set to "1.0.0" since we don't expect it to change any time soon.

The output can be found in the nugetoutput folder.

#>
param(
    [Parameter(Mandatory=$true)] [string]$mainVersion,
    [Parameter(Mandatory=$false)] [string]$loggingVersion = "1.0.0",
	[Parameter(Mandatory=$false)] [string]$loggingSimpleVersion = "1.0.0",
    [Parameter(Mandatory=$false)] [string]$mainReleaseNotes,
    [Parameter(Mandatory=$false)] [string]$loggingReleaseNotes
)

Set-StrictMode -Version Latest

Import-Module "$PSSCriptRoot/BuildFunctions.psm1"

$outputDirectory = "$PSScriptRoot/../nugetoutput"

Remove-Item $outputDirectory -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
New-Item $outputDirectory -ItemType Directory -Force | Out-Null

$outputDirectory = Resolve-Path $outputDirectory

Set-NuSpecFile "$PSScriptRoot/../src/WebServer.Logging/WebServer.Logging.nuspec" $loggingVersion $loggingReleaseNotes
Set-NuSpecFile "$PSScriptRoot/../src/WebServer/WebServer.nuspec" $mainVersion $mainReleaseNotes

Invoke-NuGetPack "$PSScriptRoot/../src/WebServer.Logging/WebServer.Logging.csproj" $outputDirectory
Invoke-NuGetPack "$PSScriptRoot/../src/WebServer/WebServer.csproj" $outputDirectory

Write-Host "The NuGet packages for Restup and Restup.Logging can be found in $outputDirectory"