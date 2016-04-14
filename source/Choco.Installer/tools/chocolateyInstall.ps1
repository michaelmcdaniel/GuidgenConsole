$binRoot = $(Split-Path -parent $MyInvocation.MyCommand.Definition)
$name  = "guidgen-console"
$version = "2.0.0.3"

$md5_net2_0 = "FB654B376A3E6E5B6E814242DF3D244F"
$md5_net3_5 = "7B460B1E7E12B9833478E8DC854CD79E"
$md5_net4_0 = "2A59CAA345DC80BF2B2FDD6C4A360DF1"
$md5_net4_5_2 = "A89EA0433B012751B321F2F12F99CD2A"
$md5_net4_6_1 = "144ABC5D74B9824E045702A559E218DA"
$md5_root = "144ABC5D74B9824E045702A559E218DA"

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
if (Test-Path "$ndpDirectory\v4\Full") { # .NET 4.5+
    $rv = (Get-ItemProperty "$ndpDirectory\v4\Full" -name Release).Release
    switch([String]$rv) {
        '378389' { Write-Debug ".NET version 4.5 is available"; break; } #4.5
        '378758' { Write-Debug ".NET version 4.5.1 is available"; break; } #4.5.1 (win81/ws12r2)
        '378675' { Write-Debug ".NET version 4.5.1 is available"; break; } #4.5.1
        '379893' { Write-Debug ".NET version 4.5.2 is available";  $url = "$rooturl/NET4_5_2/GuidGen.exe"; $checksum = $md5_net4_5_2; $installVersion = "guidgen.exe for .NET version 4.5.2"; break; } #4.5.2
        '393295' { Write-Debug ".NET version 4.6 is available"; break; } #4.6 (win10)
        '393297' { Write-Debug ".NET version 4.6 is available"; break; } #4.6
        '394254' { Write-Debug ".NET version 4.6.1 is available"; $url = "$rooturl/NET4_6_1/GuidGen.exe"; $checksum = $md5_net4_6_1; $installVersion = "guidgen.exe for .NET version 4.6.1"; break; } #4.6.1 (win10)
        '394271' { Write-Debug ".NET version 4.6.1 is available"; $url = "$rooturl/NET4_6_1/GuidGen.exe"; $checksum = $md5_net4_6_1; $installVersion = "guidgen.exe for .NET version 4.6.1"; break; } #4.6.1
    }
}

$filePath = ([System.IO.FileInfo](join-path $binRoot "..\GuidGen.exe")).FullName
Write-Host "Downloading $installVersion"
Write-Host "  from`: $url"
write-host "  to`: $filePath"

Get-ChocolateyWebFile -packageName $name -fileFullPath $filePath -url $url -checksum $checksum -checksumType "md5"
