# Unified VELO VPN & Proxy Control Center
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$Host.UI.RawUI.WindowTitle = "⚡ مرکز جامع مدیریت VELO VPN"

function Show-Menu {
    Clear-Host
    Write-Host "===================================================================" -ForegroundColor Cyan
    Write-Host "                    ⚡ مرکز جامع مدیریت VELO VPN ⚡" -ForegroundColor Yellow
    Write-Host "===================================================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "   [1]  🚀 اجرای کلاینت مدرن هیدیفای (Run Hiddify)" -ForegroundColor Green
    Write-Host "   [2]  🐱 اجرای کلاینت اصلی ولو (Run VELO VPN)" -ForegroundColor Green
    Write-Host "   [3]  🎯 تفکیک برنامه‌ها / حالت لیست سفید (Per-App Whitelist Mode)" -ForegroundColor Yellow
    Write-Host "   [4]  ✈️ تنظیم و اتصال خودکار تلگرام به پروکسی (Auto-Set Telegram)" -ForegroundColor Cyan
    Write-Host "   [5]  📊 باز کردن داشبورد زنده ترافیک و کانکشن‌ها (Live Dashboard)" -ForegroundColor White
    Write-Host "   [6]  🔍 تست وضعیت اتصال، آی‌پی و لوکیشن واقعی (Check IP & Location)" -ForegroundColor DarkCyan
    Write-Host "   [7]  🛠️ تعمیر، ریست پروکسی و پاکسازی شبکه و DNS (Network Repair)" -ForegroundColor Magenta
    Write-Host "   [8]  💾 پشتیبان‌گیری سریع از کانفیگ‌ها (Backup Configs)" -ForegroundColor DarkYellow
    Write-Host "   [9]  🔄 بروزرسانی فوری دیتابیس دامنه‌ها و بانک‌های ایران (Update Rules)" -ForegroundColor Blue
    Write-Host "   [10] 📋 مشاهده مشخصات پروکسی و کانفیگ‌ها (Proxy Info & Configs)" -ForegroundColor DarkGreen
    Write-Host ""
    Write-Host "   [0]  ❌ خروج" -ForegroundColor Red
    Write-Host "===================================================================" -ForegroundColor Cyan
}

function Configure-PerApp {
    Clear-Host
    Write-Host "===================================================================" -ForegroundColor Cyan
    Write-Host "           🎯 مدیریت تفکیک برنامه‌ها (Per-App Split Tunneling)" -ForegroundColor Yellow
    Write-Host "===================================================================" -ForegroundColor Cyan
    Write-Host ""
    
    $cfgPath = "C:\nekoray\config\groups\nekoray.json"
    $cfg = Get-Content $cfgPath -Raw -Encoding UTF8 | ConvertFrom-Json
    $isWhite = $cfg.vpn_rule_white
    $procs = $cfg.vpn_bypass_process

    if ($isWhite) {
        Write-Host "وضعیت فعلی: [ فعال - فقط برنامه‌های لیست زیر از فیلترشکن رد می‌شوند ]" -ForegroundColor Green
    } else {
        Write-Host "وضعیت فعلی: [ غیرفعال - تمام سیستم از فیلترشکن عبور می‌کند ]" -ForegroundColor Gray
    }
    Write-Host "برنامه‌های ثبت شده: " -NoNewline
    if ($procs) { Write-Host $procs -ForegroundColor Cyan } else { Write-Host "(هیچ برنامه‌ای ثبت نشده)" -ForegroundColor Yellow }
    Write-Host ""
    Write-Host "  [1] فعال‌سازی حالت لیست سفید (فقط برنامه‌های مشخص‌شده از VPN رد شوند)"
    Write-Host "  [2] غیرفعال‌سازی (بازگشت به حالت عبور تمام برنامه‌های ویندوز)"
    Write-Host "  [3] ویرایش و افزودن برنامه‌های مجاز (مثلاً Telegram.exe, chrome.exe)"
    Write-Host "  [0] بازگشت به منوی اصلی"
    Write-Host ""
    $sub = Read-Host "انتخاب شما [0-3]"

    if ($sub -eq "1") {
        $cfg.vpn_rule_white = $true
        if (-not $cfg.vpn_bypass_process) {
            $cfg.vpn_bypass_process = "Telegram.exe, chrome.exe, msedge.exe, discord.exe"
        }
        $cfg | ConvertTo-Json -Depth 10 | Set-Content $cfgPath -Encoding UTF8
        Write-Host "[✓] حالت لیست سفید فعال شد! از این پس فقط برنامه‌های ثبت شده از فیلترشکن عبور می‌کنند." -ForegroundColor Green
        Start-Sleep -Seconds 2
    } elseif ($sub -eq "2") {
        $cfg.vpn_rule_white = $false
        $cfg | ConvertTo-Json -Depth 10 | Set-Content $cfgPath -Encoding UTF8
        Write-Host "[✓] حالت لیست سفید غیرفعال شد (تمام سیستم از فیلترشکن استفاده می‌کند)." -ForegroundColor Yellow
        Start-Sleep -Seconds 2
    } elseif ($sub -eq "3") {
        Write-Host "نام فایل‌های اجرایی را با کاما جدا کنید (مثال: Telegram.exe, chrome.exe, msedge.exe)" -ForegroundColor Yellow
        $newProcs = Read-Host "لیست برنامه‌ها"
        if ($newProcs.Trim()) {
            $cfg.vpn_bypass_process = $newProcs.Trim()
            $cfg | ConvertTo-Json -Depth 10 | Set-Content $cfgPath -Encoding UTF8
            Write-Host "[✓] لیست برنامه‌ها با موفقیت ذخیره شد." -ForegroundColor Green
            Start-Sleep -Seconds 2
        }
    }
}

do {
    Show-Menu
    $opt = Read-Host "لطفاً شماره گزینه مورد نظر را وارد کنید [0-10]"
    switch ($opt) {
        "1" { Start-Process "C:\Hiddify\Hiddify.exe" }
        "2" { Start-Process "C:\nekoray\nekoray.exe" }
        "3" { Configure-PerApp }
        "4" {
            Clear-Host
            Write-Host "در حال ارسال تنظیمات پروکسی به تلگرام..." -ForegroundColor Yellow
            Start-Process "tg://socks?server=127.0.0.1&port=2080"
            Start-Sleep -Seconds 1
            Start-Process "https://t.me/socks?server=127.0.0.1&port=2080"
            Write-Host "[✓] دستور به تلگرام ارسال شد! در تلگرام روی دکمه Enable Proxy بزنید." -ForegroundColor Green
            Start-Sleep -Seconds 3
        }
        "5" { Start-Process "C:\nekoray\dashboard\index.html" }
        "6" { & "C:\nekoray\tools\بررسی_وضعیت_اتصال.bat" }
        "7" { & "C:\nekoray\tools\تعمیر_و_پاکسازی_شبکه.bat" }
        "8" { & "C:\nekoray\tools\پشتیبان‌گیری_سریع_کانفیگ‌ها.bat" }
        "9" {
            Clear-Host
            Write-Host "در حال بروزرسانی پایگاه داده ایران..." -ForegroundColor Yellow
            & "C:\nekoray\auto_update_geo.ps1"
            Write-Host "[✓] دیتابیس دامنه‌ها و آی‌پی‌های ایران با موفقیت آپدیت شد!" -ForegroundColor Green
            Start-Sleep -Seconds 2
        }
        "10" {
            Clear-Host
            Write-Host "==================================================" -ForegroundColor Cyan
            Write-Host "          اطلاعات پروکسی داخلی (Local Proxy)" -ForegroundColor Yellow
            Write-Host "==================================================" -ForegroundColor Cyan
            Write-Host "نوع اول (SOCKS5):" -ForegroundColor Green
            Write-Host "  آدرس سرور: 127.0.0.1"
            Write-Host "  پورت: 2080"
            Write-Host ""
            Write-Host "نوع دوم (HTTP):" -ForegroundColor Green
            Write-Host "  آدرس سرور: 127.0.0.1"
            Write-Host "  پورت: 2081"
            Write-Host ""
            Write-Host "لینک خودکار تلگرام:" -ForegroundColor Cyan
            Write-Host "  tg://socks?server=127.0.0.1&port=2080"
            Write-Host "==================================================" -ForegroundColor Cyan
            Write-Host ""
            Start-Process "notepad.exe" "C:\Hiddify\my_configs.txt"
            Read-Host "برای بازگشت به منو اینتر بزنید"
        }
        "0" { break }
        Default { Write-Host "گزینه نامعتبر است." -ForegroundColor Red; Start-Sleep -Seconds 1 }
    }
} while ($opt -ne "0")
