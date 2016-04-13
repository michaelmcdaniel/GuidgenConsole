$chocRoot = [System.Environment]::ExpandEnvironmentVariables("%ALLUSERSPROFILE%\chocolatey\bin");
if (Test-Path "$chocoRoot\guidgen.lnk") {
    Remove-Item "$chocoRoot\guidgen.lnk"
}

Uninstall-ChocolateyZipPackage '$env:chocolateyPackageName' 'guidgen-console.$env:chocolateyPackageVersion.zip'