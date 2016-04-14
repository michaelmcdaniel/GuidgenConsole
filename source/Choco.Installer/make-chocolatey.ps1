$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$testPackageInstall = $true

Set-Location -path $scriptPath
$nuspecFile = (join-path $scriptPath "chocolatey.guidgen-console.nuspec")
$xml = [Xml](Get-Content $nuspecFile)
$version = $xml.package.metadata.version

 Write-Host "Setting installer to web install"
((Get-Content (join-path $scriptPath "tools\chocolateyInstall.ps1")) -replace '\$local\s*=\s*\$\w+$', "`$local = `$false") | out-file  (join-path $scriptPath "tools\chocolateyInstall.ps1")

 Write-Host "Updating Version`: $version"
((Get-Content (join-path $scriptPath "tools\chocolateyInstall.ps1")) -replace '\$version\s*=\s*".*?"', "`$version = ""$version""") | out-file  (join-path $scriptPath "tools\chocolateyInstall.ps1")

$rootFile = "$scriptPath\..\..\binaries\versions\$version\guidgen.exe"
$farg = "-f=""$rootFile"""
$targ = "-t=md5"
$checksum = (checksum $targ $farg | out-string).Replace("`r|`n","").Trim()
((Get-Content (join-path $scriptPath "tools\chocolateyInstall.ps1")) -replace "\`$md5_root\s*=\s*"".*?""", "`$md5_root = ""$checksum""") | out-file  (join-path $scriptPath "tools\chocolateyInstall.ps1")

Get-ChildItem -Path "$scriptPath\..\..\binaries\versions\$version\*\guidgen.exe" | ForEach-Object {
	$fileName =  $_.FullName
	$farg = "-f=""$fileName"""
	$checksum = (checksum $targ $farg | out-string).Replace("`r|`n","").Trim()
	$folder = [regex]::match($fileName, "\\(?'netv'[^\\]+)\\guidgen\.exe", "IgnoreCase").Groups["netv"].Value.ToLower()
	if ($folder -match ".+")
	{
		 Write-Host "setting md5 checksum ($checksum) for $folder\guidgen.exe"
		((Get-Content (join-path $scriptPath "tools\chocolateyInstall.ps1")) -replace "\`$md5_$folder\s*=\s*"".*?""", "`$md5_$folder = ""$checksum""") | out-file  (join-path $scriptPath "tools\chocolateyInstall.ps1")
	}
}
Write-Host "Creating Package from`: $nuspecFile"
choco pack $nuspecFile
$pkg = join-path $scriptPath "guidgen-console.$version`.nupkg"
if (Test-path $pkg) {
    if (Test-Path "$scriptPath\..\..\binaries\Chocolatey-Packages\guidgen-console.$version`.nupkg") { remove-item "$scriptPath\..\..\binaries\Chocolatey-Packages\guidgen-console.$version`.nupkg" }
	Write-Host "moving package to Binaries\Chocolatey-Packges"
    move-item $pkg "$scriptPath\..\..\binaries\Chocolatey-Packages\"
} 
else 
{ 
    write-host "Package not found`: $pkg" 
}

if ($testPackageInstall) {
	Write-Host "Creating Test Package with local install"
	((Get-Content (join-path $scriptPath "tools\chocolateyInstall.ps1")) -replace '\$local\s*=\s*\$\w+$', "`$local = `$true") | out-file  (join-path $scriptPath "tools\chocolateyInstall.ps1")
	#$rooturl = "" #local
	((Get-Content (join-path $scriptPath "tools\chocolateyInstall.ps1")) -replace '\$rooturl\s*=\s*".*"\s*#local$', "`$rooturl = ""$scriptPath\..\..\binaries\versions\$version"" #local") | out-file  (join-path $scriptPath "tools\chocolateyInstall.ps1")
	choco pack $nuspecFile
	((Get-Content (join-path $scriptPath "tools\chocolateyInstall.ps1")) -replace '\$rooturl\s*=\s*".*"\s*#local$', "`$rooturl = """" #local") | out-file  (join-path $scriptPath "tools\chocolateyInstall.ps1")
	$pkg = join-path $scriptPath "guidgen-console.$version`.nupkg"
	if (Test-path $pkg) {
		Write-Host "Installing package`: $pkg"
		choco install $pkg --version $version --force
		Write-Host "removing package`: $pkg"
		Remove-Item $pkg
	}
	else
	{
		Write-Host "Package not found`: $pkg"
	}
}
Write-Host "Complete"
