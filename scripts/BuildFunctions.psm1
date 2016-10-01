Set-StrictMode -Version Latest

<#

.SYNOPSIS

Sets the version and optionally the release notes in the nu spec file location.

#>
function Set-NuSpecFile {
    param(
        [Parameter(Mandatory=$true)] [string] $nuSpecLocation,
        [Parameter(Mandatory=$true)] [string] $version,
        [Parameter(Mandatory=$false)] [string] $releaseNotes
    )

    # make sure the path exists and resolve it for the XmlDocument.Save method
    $nuSpecLocation = Resolve-Path $nuSpecLocation

    [xml]$nuSpecXml = Get-Content -Raw $nuSpecLocation
    $nuSpecXml.package.metadata.version = $version
    Write-Host "Version for $($nuspecXml.package.metadata.id) set to $version"
    if ($releaseNotes) {
        $nuSpecXml.package.metadata.releaseNotes = $releaseNotes
        Write-Host "Release notes for $($nuspecXml.package.metadata.id) set to $releaseNotes"
    }
    $nuSpecXml.Save($nuSpecLocation) 
}

<#

.SYNOPSIS

Invokes the NuGet pack function to create a NuGet package file.

.DESCRIPTION

This function exists so that we execute the pack function with the same parameters for all NuGet packages generated.

The -IncludeReferencedProjects is used so that the correct version is referenced in the NuGet package for referenced projects. 

#>
function Invoke-NuGetPack {
    param(
        [Parameter(Mandatory=$true)] [string] $csProjFileLocation,
        [Parameter(Mandatory=$true)] [string] $outputDirectory
    )

    # make sure the path exists
    $csProjFileLocation = Resolve-Path $csProjFileLocation
    & "$PSScriptRoot/nuget.exe" pack $csProjFileLocation -Build -Properties "Configuration=Release;Platform=AnyCPU" -IncludeReferencedProjects -OutputDirectory $outputDirectory
}