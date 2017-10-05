$binRoot = $(Split-Path -parent $MyInvocation.MyCommand.Definition)
$name  = "guidgen-console"
$version = "2.0.0.6"

$md5_net2_0 = "1E108A4438773F18B835E5C440AD214E"
$md5_net3_5 = "5445A8923A8FD28653F0F4E297A3B06F"
$md5_net4_0 = "75D23C938D5FBEAAF1FCF6088137321D"
$md5_net4_5 = "72C986997017084F2DA038F6DDC7DC94"
$md5_net4_6 = "21A479765778AC5710546EDBD3F956BD"
$md5_root = "A1E02CE554B2098F3F8CE17916E1F8C6"

$local = $false

if($local) {
	$rooturl = "" #local
	$rooturl = [System.Uri]$rooturl
} else {
	$rooturl = "https://cdn.rawgit.com/michaelmcdaniel/GuidgenConsole/master/Binaries/versions/$version"
}
$url = "$rooturl/GuidGen.exe" #default as latest
$checksum = $md5_root

Write-Host "Determining version to install..."
$installVersion = ""
$ndpDirectory = 'hklm:\SOFTWARE\Microsoft\NET Framework Setup\NDP\'

if ($pathToLatest -eq $binRoot -and (Test-Path "$ndpDirectory\v2.0.50727")) { # .NET 2
	Write-Debug ".NET version 2.0 is available"
    $url = "$rooturl/dotNet2_0/GuidGen.exe"
	$checksum = $md5_net2_0
	$installVersion = "guidgen.exe for .NET version 2.0"
} 
if (Test-Path "$ndpDirectory\v3.0") { # .NET 3
	Write-Debug ".NET version 3.0 is available"
}
if ($pathToLatest -eq $binRoot -and (Test-Path "$ndpDirectory\v3.5")) { # .NET 3.5
	Write-Debug ".NET version 3.5 is available"
    $url = "$rooturl/dotNet3_5/GuidGen.exe"
	$checksum = $md5_net3_5
	$installVersion = "guidgen.exe for .NET version 3.5"
}
if ($pathToLatest -eq $binRoot -and (Test-Path "$ndpDirectory\v4")) { # .NET 4
	Write-Debug ".NET version 4.0 is available"
    $url = "$rooturl/dotNet4_0/GuidGen.exe"
	$checksum = $md5_net4_0
	$installVersion = "guidgen.exe for .NET version 4.0"
}
if (Test-Path "$ndpDirectory\v4\Full") { # .NET 4.5+  (we could serve up 4_5 version for all these.)
    $rv = (Get-ItemProperty "$ndpDirectory\v4\Full" -name Release).Release
    switch([String]$rv) {
        '378389' { Write-Debug ".NET version 4.5 is available";  $url = "$rooturl/dotNet4_5/GuidGen.exe"; $checksum = $md5_net4_5; $installVersion = "guidgen.exe for .NET version 4.5"; break; } #4.5
        '378758' { Write-Debug ".NET version 4.5.1 is available";  $url = "$rooturl/dotNet4_5/GuidGen.exe"; $checksum = $md5_net4_5; $installVersion = "guidgen.exe for .NET version 4.5.1"; break; } #4.5.1 (win81/ws12r2)
        '378675' { Write-Debug ".NET version 4.5.1 is available";  $url = "$rooturl/dotNet4_5/GuidGen.exe"; $checksum = $md5_net4_5; $installVersion = "guidgen.exe for .NET version 4.5.1"; break; } #4.5.1
        '379893' { Write-Debug ".NET version 4.5.2 is available";  $url = "$rooturl/dotNet4_5/GuidGen.exe"; $checksum = $md5_net4_5; $installVersion = "guidgen.exe for .NET version 4.5.2"; break; } #4.5.2
        '393295' { Write-Debug ".NET version 4.6 is available"; $url = "$rooturl/dotNet4_6/GuidGen.exe"; $checksum = $md5_net4_6; $installVersion = "guidgen.exe for .NET version 4.6"; break; } #4.6 (win10)
        '393297' { Write-Debug ".NET version 4.6 is available"; $url = "$rooturl/dotNet4_6/GuidGen.exe"; $checksum = $md5_net4_6; $installVersion = "guidgen.exe for .NET version 4.6"; break; } #4.6
        '394254' { Write-Debug ".NET version 4.6.1 is available"; $url = "$rooturl/dotNet4_6/GuidGen.exe"; $checksum = $md5_net4_6; $installVersion = "guidgen.exe for .NET version 4.6.1"; break; } #4.6.1 (win10)
        '394271' { Write-Debug ".NET version 4.6.1 is available"; $url = "$rooturl/dotNet4_6/GuidGen.exe"; $checksum = $md5_net4_6; $installVersion = "guidgen.exe for .NET version 4.6.1"; break; } #4.6.1
        '394802' { Write-Debug ".NET version 4.6.2 is available"; $url = "$rooturl/dotNet4_6/GuidGen.exe"; $checksum = $md5_net4_6; $installVersion = "guidgen.exe for .NET version 4.6.2"; break; } #4.6.2 (win10)
        '394806' { Write-Debug ".NET version 4.6.2 is available"; $url = "$rooturl/dotNet4_6/GuidGen.exe"; $checksum = $md5_net4_6; $installVersion = "guidgen.exe for .NET version 4.6.2"; break; } #4.6.2
        '460798' { Write-Debug ".NET version 4.7 is available"; $url = "$rooturl/dotNet4_7/GuidGen.exe"; $checksum = $md5_net4_7; $installVersion = "guidgen.exe for .NET version 4.7"; break; } #4.7 (win10)
        '460805' { Write-Debug ".NET version 4.7 is available"; $url = "$rooturl/dotNet4_7/GuidGen.exe"; $checksum = $md5_net4_7; $installVersion = "guidgen.exe for .NET version 4.7"; break; } #4.7
    }
}

$filePath = ([System.IO.FileInfo](join-path $binRoot "..\GuidGen.exe")).FullName
Write-Host "Downloading $installVersion"
Write-Host "  from`: $url"
write-host "  to`: $filePath"

Get-ChocolateyWebFile -packageName $name -fileFullPath $filePath -url $url -checksum $checksum -checksumType "md5"
