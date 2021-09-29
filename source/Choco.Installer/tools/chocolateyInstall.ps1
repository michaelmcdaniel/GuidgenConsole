$binRoot = $(Split-Path -parent $MyInvocation.MyCommand.Definition)
$name  = "guidgen-console"
$version = "2.0.0.7"

$md5_net2_0 = "697525F8C8A68E4A6EC8B9056F337625"
$md5_net3_5 = "CDDC25848D866BD5C32AF1AE141E017E"
$md5_net4_0 = "CF5B535CBF80C362C03AB23D69F3EB17"
$md5_net4_5 = "F049C2B45D1F309DFC93E9EE914AC397"
$md5_net4_6 = "FA82999104F2C1A2A8E68DCFF054F017"
$md5_net4_7 = "88F9BC0FC52940AED9A8A811FE730103"
$md5_net4_8 = "916A2236F448D257413042C8BB637788"
$md5_root = "916A2236F448D257413042C8BB637788"

$local = $false

if($local) {
	$rooturl = "" #local
	$rooturl = [System.Uri]$rooturl
} else {
	$rooturl = "https://github.com/michaelmcdaniel/GuidgenConsole/raw/master/Binaries/versions/$version"
}
$url = "$rooturl/GuidGen.exe" #default as latest
$checksum = $md5_root

Write-Host "Determining version to install..."
$installVersion = ""
$ndpDirectory = 'hklm:\SOFTWARE\Microsoft\NET Framework Setup\NDP\'

if ($pathToLatest -eq $binRoot -and (Test-Path "$ndpDirectory\v2.0.50727")) { # .NET 2
	Write-Debug ".NET version 2.0 is available"
    $url = "$rooturl/NET2_0/GuidGen.exe"
	$checksum = $md5_net2_0
	$installVersion = "guidgen.exe for .NET version 2.0"
} 
if (Test-Path "$ndpDirectory\v3.0") { # .NET 3
	Write-Debug ".NET version 3.0 is available"
}
if ($pathToLatest -eq $binRoot -and (Test-Path "$ndpDirectory\v3.5")) { # .NET 3.5
	Write-Debug ".NET version 3.5 is available"
    $url = "$rooturl/NET3_5/GuidGen.exe"
	$checksum = $md5_net3_5
	$installVersion = "guidgen.exe for .NET version 3.5"
}
if ($pathToLatest -eq $binRoot -and (Test-Path "$ndpDirectory\v4")) { # .NET 4
	Write-Debug ".NET version 4.0 is available"
    $url = "$rooturl/NET4_0/GuidGen.exe"
	$checksum = $md5_net4_0
	$installVersion = "guidgen.exe for .NET version 4.0"
}
if (Test-Path "$ndpDirectory\v4\Full") { # .NET 4.5+  (we could serve up 4_5 version for all these.)
    $rv = (Get-ItemProperty "$ndpDirectory\v4\Full" -name Release).Release
    switch([String]$rv) {
        '378389' { Write-Debug ".NET version 4.5 is available";  $url = "$rooturl/NET4_5/GuidGen.exe"; $checksum = $md5_net4_5; $installVersion = "guidgen.exe for .NET version 4.5"; break; } #4.5
        '378758' { Write-Debug ".NET version 4.5.1 is available";  $url = "$rooturl/NET4_5/GuidGen.exe"; $checksum = $md5_net4_5; $installVersion = "guidgen.exe for .NET version 4.5.1"; break; } #4.5.1 (win81/ws12r2)
        '378675' { Write-Debug ".NET version 4.5.1 is available";  $url = "$rooturl/NET4_5/GuidGen.exe"; $checksum = $md5_net4_5; $installVersion = "guidgen.exe for .NET version 4.5.1"; break; } #4.5.1
        '379893' { Write-Debug ".NET version 4.5.2 is available";  $url = "$rooturl/NET4_5/GuidGen.exe"; $checksum = $md5_net4_5; $installVersion = "guidgen.exe for .NET version 4.5.2"; break; } #4.5.2
        '393295' { Write-Debug ".NET version 4.6 is available"; $url = "$rooturl/NET4_6/GuidGen.exe"; $checksum = $md5_net4_6; $installVersion = "guidgen.exe for .NET version 4.6"; break; } #4.6 (win10)
        '393297' { Write-Debug ".NET version 4.6 is available"; $url = "$rooturl/NET4_6/GuidGen.exe"; $checksum = $md5_net4_6; $installVersion = "guidgen.exe for .NET version 4.6"; break; } #4.6
        '394254' { Write-Debug ".NET version 4.6.1 is available"; $url = "$rooturl/NET4_6/GuidGen.exe"; $checksum = $md5_net4_6; $installVersion = "guidgen.exe for .NET version 4.6.1"; break; } #4.6.1 (win10)
        '394271' { Write-Debug ".NET version 4.6.1 is available"; $url = "$rooturl/NET4_6/GuidGen.exe"; $checksum = $md5_net4_6; $installVersion = "guidgen.exe for .NET version 4.6.1"; break; } #4.6.1
        '394802' { Write-Debug ".NET version 4.6.2 is available"; $url = "$rooturl/NET4_6/GuidGen.exe"; $checksum = $md5_net4_6; $installVersion = "guidgen.exe for .NET version 4.6.2"; break; } #4.6.2 (win10)
        '394806' { Write-Debug ".NET version 4.6.2 is available"; $url = "$rooturl/NET4_6/GuidGen.exe"; $checksum = $md5_net4_6; $installVersion = "guidgen.exe for .NET version 4.6.2"; break; } #4.6.2
        '460798' { Write-Debug ".NET version 4.7 is available"; $url = "$rooturl/NET4_7/GuidGen.exe"; $checksum = $md5_net4_7; $installVersion = "guidgen.exe for .NET version 4.7"; break; } #4.7 (win10)
        '461308' { Write-Debug ".NET version 4.7 is available"; $url = "$rooturl/NET4_7/GuidGen.exe"; $checksum = $md5_net4_7; $installVersion = "guidgen.exe for .NET version 4.7"; break; } #4.7
        '460805' { Write-Debug ".NET version 4.7 is available"; $url = "$rooturl/NET4_7/GuidGen.exe"; $checksum = $md5_net4_7; $installVersion = "guidgen.exe for .NET version 4.7"; break; } #4.7.1
        '461808' { Write-Debug ".NET version 4.7 is available"; $url = "$rooturl/NET4_7/GuidGen.exe"; $checksum = $md5_net4_7; $installVersion = "guidgen.exe for .NET version 4.7"; break; } #4.7.2
        '528040' { Write-Debug ".NET version 4.8 is available"; $url = "$rooturl/NET4_8/GuidGen.exe"; $checksum = $md5_net4_8; $installVersion = "guidgen.exe for .NET version 4.8"; break; } #4.8
    }
}

$filePath = ([System.IO.FileInfo](join-path $binRoot "..\GuidGen.exe")).FullName
Write-Host "Downloading $installVersion"
Write-Host "  from`: $url"
write-host "  to`: $filePath"

Get-ChocolateyWebFile -packageName $name -fileFullPath $filePath -url $url -checksum $checksum -checksumType "md5"
