# VELO VPN - Automated Installer Builder
$ErrorActionPreference = "Stop"

$Root = Split-Path -Parent $PSScriptRoot
$SourceCs = Join-Path $PSScriptRoot "installer_source.cs"
$OutExe = Join-Path $Root "VELO_VPN_Setup.exe"
$PayloadZip = "C:\nekoray\velo_app.zip"
$Icon = Join-Path $Root "assets\velo_logo.ico"
$LogoPng = Join-Path $Root "assets\velo_logo.png"

$Csc = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"

Write-Host "Compiling VELO VPN Standalone Setup Wizard..." -ForegroundColor Cyan

& $Csc /target:winexe `
    /win32icon:"$Icon" `
    /resource:"$PayloadZip",velo_app.zip `
    /resource:"$LogoPng",logo.png `
    /r:System.IO.Compression.FileSystem.dll `
    /r:System.IO.Compression.dll `
    /r:System.Windows.Forms.dll `
    /r:System.Drawing.dll `
    /out:"$OutExe" `
    "$SourceCs"

Write-Host "Build complete: $OutExe" -ForegroundColor Green
