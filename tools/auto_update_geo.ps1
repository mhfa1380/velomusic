# NekoRay & Hiddify Automated Geo-Databases Updater
# Keeps Iranian domains, banks, and IP lists 100% updated automatically.
$ErrorActionPreference = "SilentlyContinue"

$destDir = "C:\nekoray"
$geoipUrl = "https://github.com/Chocolate4U/Iran-v2ray-rules/releases/latest/download/geoip.dat"
$geositeUrl = "https://github.com/Chocolate4U/Iran-v2ray-rules/releases/latest/download/geosite.dat"
$geoipDbUrl = "https://github.com/Chocolate4U/Iran-sing-box-rules/releases/latest/download/geoip.db"
$geositeDbUrl = "https://github.com/Chocolate4U/Iran-sing-box-rules/releases/latest/download/geosite.db"

$tempDir = "$destDir\temp_auto_geo"
New-Item -ItemType Directory -Force -Path $tempDir | Out-Null

curl.exe -s -L -o "$tempDir\geoip.dat" $geoipUrl
curl.exe -s -L -o "$tempDir\geosite.dat" $geositeUrl
curl.exe -s -L -o "$tempDir\geoip.db" $geoipDbUrl
curl.exe -s -L -o "$tempDir\geosite.db" $geositeDbUrl

if (Test-Path "$tempDir\geoip.dat") {
    Copy-Item "$tempDir\geoip.dat" "$destDir\geoip.dat" -Force
    Copy-Item "$tempDir\geosite.dat" "$destDir\geosite.dat" -Force
    Copy-Item "$tempDir\geoip.db" "$destDir\geoip.db" -Force
    Copy-Item "$tempDir\geosite.db" "$destDir\geosite.db" -Force
}

Remove-Item $tempDir -Recurse -Force
