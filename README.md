# ⚡ VELO VPN

<p align="center">
  <img src="assets/hero_banner.png" alt="VELO VPN Hero Banner" width="850" />
</p>

<p align="center">
  <a href="https://github.com/mhfa1380/velovpn/releases/latest"><img src="https://img.shields.io/badge/Release-v1.0.0-00F5FF?style=for-the-badge&logo=github" alt="Release" /></a>
  <a href="#"><img src="https://img.shields.io/badge/Platform-Windows%2010%20%7C%2011%20x64-7B2CBF?style=for-the-badge&logo=windows" alt="Platform" /></a>
  <a href="#"><img src="https://img.shields.io/badge/Core-Sing--Box%20%2B%20Hiddify-FF007A?style=for-the-badge" alt="Core" /></a>
  <a href="#"><img src="https://img.shields.io/badge/UI-Modern%20Flutter-10B981?style=for-the-badge&logo=flutter" alt="UI" /></a>
  <a href="#"><img src="https://img.shields.io/badge/Status-Ultra--Fast%20%26%20Stable-3B82F6?style=for-the-badge" alt="Status" /></a>
</p>

---

## 🌟 Overview

**VELO VPN** is a next-generation, high-performance VPN and proxy client engineered for zero-friction censorship circumvention, peak security, and maximum speed. Built upon the modern **Hiddify / Sing-Box** foundation with custom **VELO** styling, it combines an ultra-modern responsive dark interface with battle-tested routing and tunnel resilience.

### 📥 Instant Download
👉 **[Download Official Installer: VELO_VPN_Setup.exe](https://github.com/mhfa1380/velovpn/releases/download/v1.0.0/VELO_VPN_Setup.exe)**

---

## 🚀 Key Features

- 🎨 **Modern Cyber-Dark Flutter UI:** Ultra-smooth animations, responsive layouts, neon accent palettes, and effortless navigation.
- 🛡️ **Intelligent Iran Direct Bypass:** Zero speed-loss for domestic services. Automatically detects `.ir` domains, Iranian banking, payment gateways, and domestic CIDR blocks without sending them through the VPN.
- ⚡ **Dual Engine (TUN / VPN + System Proxy):** Run high-speed system-wide TUN tunnel and local proxy simultaneously.
- 📱 **1-Click Telegram Proxy:** Instant SOCKS5 / HTTP proxy integration with automatic desktop Telegram binding.
- 🎯 **Split Tunneling & Per-App Routing:** Choose exactly which applications route through the VPN and which stay on your normal local connection.
- 🔄 **Auto-Updating GeoIP & GeoSite:** Daily background synchronization with Chocolate4U's curated Iranian rule sets.
- 🔒 **Zero QUIC/UDP Slice Panics:** Completely eliminated memory slice boundary bugs and connection drops.
- 📦 **Single-File Setup Wizard:** Elegant standalone C# setup installer that handles shortcuts, startup, and dependencies in seconds.

---

## 📸 Screenshots

<p align="center">
  <img src="assets/screenshot_installer.png" alt="VELO VPN Setup Wizard" width="410" />
  <img src="assets/screenshot_dashboard.png" alt="VELO VPN Modern UI" width="410" />
</p>

---

## ⚡ Quick Start

1. **Download & Install:**
   - Grab `VELO_VPN_Setup.exe` from [Releases](https://github.com/mhfa1380/velovpn/releases/latest).
   - Run the installer and click **Install Now**.
2. **Launch:**
   - Open **VELO VPN** from your Desktop or Start Menu.
3. **Connect:**
   - Select your preferred high-speed server (🇩🇪 Germany, 🇺🇸 United States, 🇹🇷 Turkey, etc.) and hit **Connect**.
4. **Telegram Proxy:**
   - SOCKS5 is ready at `127.0.0.1:2080`.
   - Click the 1-click proxy button or open `tg://socks?server=127.0.0.1&port=2080`.

---

## 🛠️ Project Structure

```text
├── assets/                     # Branding, icons, logos, screenshots
│   ├── velo_logo.ico           # Application & tray icons
│   ├── velo_logo.png           # High-resolution cyber emblem
│   └── hero_banner.png         # GitHub hero presentation
├── config/                     # Outbounds and server definitions
│   └── my_configs.txt          # Preloaded VIP nodes
├── installer/                  # Standalone C# Setup Wizard
│   ├── installer_source.cs     # English dark-themed installer source
│   └── build_installer.ps1     # Automated MSBuild compiler script
├── tools/                      # Automation & Control Center
│   ├── Control_Center.ps1      # PowerShell interactive utility
│   ├── Control_Center.bat      # Fast launcher
│   └── auto_update_geo.ps1     # Automated daily geo-rules updater
└── README.md
```

---

## 👤 Author & Credits

- Developed by **velomusic** / [mhfa1380](https://github.com/mhfa1380)
- Core powered by **Sing-Box** & **Hiddify**
- Routing rules powered by **Chocolate4U / Iran-sing-box-rules**
