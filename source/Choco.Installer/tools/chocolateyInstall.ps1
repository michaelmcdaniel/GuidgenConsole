$name  = 'guidgen-console'
$url   = 'https://cdn.rawgit.com/michaelmcdaniel/GuidgenConsole/master/Binaries/guidgen-console.$env:chocolateyPackageVersion.zip'
$binRoot = $(Split-Path -parent $MyInvocation.MyCommand.Definition)

Install-ChocolateyZipPackage '$env:chocolateyPackageName' $url $binRoot

#now put a link to the highest .net version in %ALLUSERSPROFILE%\chocolatey\bin
$pathToLatest = $binRoot

$ndpDirectory = 'hklm:\SOFTWARE\Microsoft\NET Framework Setup\NDP\'
if (Test-Path "$ndpDirectory\v4\Full") { # .NET 4.5+
    $rv = (Get-ItemProperty "$ndpDirectory\v4\Full" -name Release).Release
    switch([String]$rv) {
        '378389' { break; } #4.5
        '378758' { break; } #4.5.1 (win81/ws12r2)
        '378675' { break; } #4.5.1
        '379893' { $pathToLatest = join-path $pathToLatest "..\dotNET4_5\guidgen.exe"; break; } #4.5.2
        '393295' { break; } #4.6 (win10)
        '393297' { break; } #4.6
        '394254' { $pathToLatest = join-path $pathToLatest "..\dotNET4_6\guidgen.exe"; break; } #4.6.1 (win10)
        '394271' { $pathToLatest = join-path $pathToLatest "..\dotNET4_6\guidgen.exe"; break; } #4.6.1
    }
}
if ($pathToLatest -eq $binRoot -and (Test-Path "$ndpDirectory\v4")) { # .NET 4
    $pathToLatest = join-path $pathToLatest "..\dotNET4_0\guidgen.exe"
}
if ($pathToLatest -eq $binRoot -and (Test-Path "$ndpDirectory\v3.5")) { # .NET 3.5
    $pathToLatest = join-path $pathToLatest "..\dotNET3_5\guidgen.exe"
}
#if (Test-Path "$ndpDirectory\v3.0") { # .NET 3
#}
if ($pathToLatest -eq $binRoot -and (Test-Path "$ndpDirectory\v2.0.50727")) { # .NET 2
    $pathToLatest = join-path $pathToLatest "..\dotNET2_0\guidgen.exe"
}

if ($pathToLatest -match "guidgen\.exe$") {
    
    $chocRoot = [System.Environment]::ExpandEnvironmentVariables("%ALLUSERSPROFILE%\chocolatey\bin");
    if (Test-Path $pathToLatest -and Test-Path $chocRoot) {
        Install-ChocolateyShortcut -shortcutFilePath "$chocoRoot\guidgen.lnk" -targetPath $pathToLatest
    }
}