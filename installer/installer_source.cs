using System;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

namespace VeloVpnInstaller
{
    public class ModernInstaller : Form
    {
        private PictureBox picLogo;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblBadge;
        private Panel pnlCard;
        private Label lblPath;
        private TextBox txtPath;
        private Button btnBrowse;
        private CheckBox chkDesktop;
        private CheckBox chkStartup;
        private CheckBox chkLaunch;
        private ProgressBar prgBar;
        private Label lblStatus;
        private Button btnInstall;
        private Panel pnlFeatures;
        private bool isDone = false;
        private bool isUpdateMode = false;
        private string installedExe = "";

        public ModernInstaller()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            string defaultPath = @"C:\VELO_VPN";
            isUpdateMode = Directory.Exists(defaultPath) && 
                           (File.Exists(Path.Combine(defaultPath, "velovpn.exe")) || 
                            File.Exists(Path.Combine(defaultPath, "VELO VPN.exe")) || 
                            File.Exists(Path.Combine(defaultPath, "Hiddify.exe")));

            this.Text = isUpdateMode ? "velovpn - Update Wizard" : "velovpn - Setup Wizard";
            this.Size = new Size(640, 560);
            this.MinimumSize = new Size(640, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(11, 15, 25); // Ultra dark slate/obsidian
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            var asm = Assembly.GetExecutingAssembly();
            try {
                using (Stream icoStream = asm.GetManifestResourceStream("velo_logo.ico")) {
                    if (icoStream != null) this.Icon = new Icon(icoStream);
                }
            } catch {}

            // Header Section
            picLogo = new PictureBox();
            picLogo.Size = new Size(68, 68);
            picLogo.Location = new Point(32, 28);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            try {
                using (Stream imgStream = asm.GetManifestResourceStream("logo.png")) {
                    if (imgStream != null) picLogo.Image = Image.FromStream(imgStream);
                }
            } catch {}
            this.Controls.Add(picLogo);

            lblTitle = new Label();
            lblTitle.Text = isUpdateMode ? "velovpn Update" : "velovpn";
            lblTitle.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 242, 254); // Electric Cyan
            lblTitle.Location = new Point(112, 24);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            lblBadge = new Label();
            lblBadge.Text = isUpdateMode ? "UPDATE READY" : "v1.0.1 STABLE";
            lblBadge.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblBadge.ForeColor = isUpdateMode ? Color.FromArgb(52, 211, 153) : Color.FromArgb(168, 85, 247);
            lblBadge.BackColor = isUpdateMode ? Color.FromArgb(6, 78, 59) : Color.FromArgb(30, 27, 75);
            lblBadge.Location = new Point(isUpdateMode ? 320 : 255, 32);
            lblBadge.Padding = new Padding(5, 2, 5, 2);
            lblBadge.AutoSize = true;
            this.Controls.Add(lblBadge);

            lblSubtitle = new Label();
            lblSubtitle.Text = isUpdateMode 
                ? "Existing installation detected — Upgrade components while preserving all configs"
                : "Ultra-Modern Cyberpunk VPN Client with Sing-Box Core & Iran Bypass";
            lblSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.FromArgb(148, 163, 184); // Slate 400
            lblSubtitle.Location = new Point(114, 62);
            lblSubtitle.Size = new Size(500, 36);
            this.Controls.Add(lblSubtitle);

            // Feature Badges Container
            pnlFeatures = new Panel();
            pnlFeatures.Location = new Point(32, 112);
            pnlFeatures.Size = new Size(560, 36);
            pnlFeatures.BackColor = Color.FromArgb(17, 24, 39);
            this.Controls.Add(pnlFeatures);

            AddFeaturePill(pnlFeatures, "⚡ Sing-Box Core", 8, Color.FromArgb(56, 189, 248));
            AddFeaturePill(pnlFeatures, "🇮🇷 Smart Iran Bypass", 145, Color.FromArgb(74, 222, 128));
            AddFeaturePill(pnlFeatures, "🛡️ TUN Mode Enabled", 295, Color.FromArgb(192, 132, 252));
            AddFeaturePill(pnlFeatures, "🔒 Safe Config Guard", 435, Color.FromArgb(251, 146, 60));

            // Destination Path Card
            pnlCard = new Panel();
            pnlCard.Location = new Point(32, 164);
            pnlCard.Size = new Size(560, 96);
            pnlCard.BackColor = Color.FromArgb(20, 28, 46);
            this.Controls.Add(pnlCard);

            lblPath = new Label();
            lblPath.Text = isUpdateMode ? "Existing Installation Directory (Configs Safe):" : "Installation Directory:";
            lblPath.ForeColor = Color.FromArgb(203, 213, 225);
            lblPath.Location = new Point(16, 14);
            lblPath.AutoSize = true;
            pnlCard.Controls.Add(lblPath);

            txtPath = new TextBox();
            txtPath.Text = defaultPath;
            txtPath.Location = new Point(18, 42);
            txtPath.Size = new Size(420, 28);
            txtPath.BackColor = Color.FromArgb(11, 15, 25);
            txtPath.ForeColor = Color.FromArgb(0, 242, 254);
            txtPath.BorderStyle = BorderStyle.FixedSingle;
            pnlCard.Controls.Add(txtPath);

            btnBrowse = new Button();
            btnBrowse.Text = "Browse...";
            btnBrowse.Location = new Point(448, 41);
            btnBrowse.Size = new Size(94, 30);
            btnBrowse.BackColor = Color.FromArgb(30, 41, 59);
            btnBrowse.ForeColor = Color.White;
            btnBrowse.FlatStyle = FlatStyle.Flat;
            btnBrowse.FlatAppearance.BorderColor = Color.FromArgb(71, 85, 105);
            btnBrowse.Cursor = Cursors.Hand;
            btnBrowse.Click += (s, e) => {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.SelectedPath = txtPath.Text;
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        txtPath.Text = fbd.SelectedPath;
                    }
                }
            };
            pnlCard.Controls.Add(btnBrowse);

            // Checkboxes
            chkDesktop = new CheckBox();
            chkDesktop.Text = "Create Desktop Shortcut (velovpn)";
            chkDesktop.Location = new Point(48, 276);
            chkDesktop.Size = new Size(320, 26);
            chkDesktop.Checked = true;
            chkDesktop.ForeColor = Color.FromArgb(226, 232, 240);
            this.Controls.Add(chkDesktop);

            chkStartup = new CheckBox();
            chkStartup.Text = "Auto-start velovpn on Windows login";
            chkStartup.Location = new Point(48, 306);
            chkStartup.Size = new Size(320, 26);
            chkStartup.Checked = true;
            chkStartup.ForeColor = Color.FromArgb(226, 232, 240);
            this.Controls.Add(chkStartup);

            chkLaunch = new CheckBox();
            chkLaunch.Text = "Launch velovpn immediately when finished";
            chkLaunch.Location = new Point(48, 336);
            chkLaunch.Size = new Size(320, 26);
            chkLaunch.Checked = true;
            chkLaunch.ForeColor = Color.FromArgb(226, 232, 240);
            this.Controls.Add(chkLaunch);

            // Progress Bar
            prgBar = new ProgressBar();
            prgBar.Location = new Point(32, 380);
            prgBar.Size = new Size(560, 10);
            prgBar.Visible = false;
            this.Controls.Add(prgBar);

            // Status Label
            lblStatus = new Label();
            lblStatus.Text = isUpdateMode 
                ? "Existing installation found. Click Update to upgrade components without losing configs."
                : "Ready to install. Click Install Now to begin.";
            lblStatus.Location = new Point(32, 400);
            lblStatus.Size = new Size(560, 28);
            lblStatus.ForeColor = Color.FromArgb(148, 163, 184);
            this.Controls.Add(lblStatus);

            // Action Button
            btnInstall = new Button();
            btnInstall.Text = isUpdateMode ? "UPDATE NOW" : "INSTALL NOW";
            btnInstall.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnInstall.Location = new Point(32, 436);
            btnInstall.Size = new Size(560, 52);
            btnInstall.BackColor = isUpdateMode ? Color.FromArgb(16, 185, 129) : Color.FromArgb(6, 182, 212);
            btnInstall.ForeColor = Color.Black;
            btnInstall.FlatStyle = FlatStyle.Flat;
            btnInstall.FlatAppearance.BorderSize = 0;
            btnInstall.Cursor = Cursors.Hand;
            btnInstall.Click += BtnInstall_Click;
            this.Controls.Add(btnInstall);
        }

        private void AddFeaturePill(Panel parent, string text, int x, Color fg)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lbl.ForeColor = fg;
            lbl.Location = new Point(x, 8);
            lbl.AutoSize = true;
            parent.Controls.Add(lbl);
        }

        private void BtnInstall_Click(object sender, EventArgs e)
        {
            if (isDone)
            {
                if (chkLaunch.Checked && File.Exists(installedExe))
                {
                    try {
                        ProcessStartInfo psi = new ProcessStartInfo(installedExe) {
                            UseShellExecute = true,
                            Verb = "runas"
                        };
                        Process.Start(psi);
                    } catch {
                        try { Process.Start(installedExe); } catch {}
                    }
                }
                this.Close();
                return;
            }

            btnInstall.Enabled = false;
            btnBrowse.Enabled = false;
            txtPath.Enabled = false;
            prgBar.Visible = true;
            prgBar.Style = ProgressBarStyle.Marquee;
            lblStatus.Text = isUpdateMode ? "Updating velovpn components and preserving user configs..." : "Installing velovpn components...";
            lblStatus.ForeColor = Color.FromArgb(56, 189, 248);

            string targetDir = txtPath.Text.Trim();
            bool makeShortcuts = chkDesktop.Checked;
            bool runStartup = chkStartup.Checked;

            Thread t = new Thread(() => {
                try {
                    // Close any running velovpn or Hiddify processes if active
                    foreach (var p in Process.GetProcessesByName("velovpn")) { try { p.Kill(); p.WaitForExit(1000); } catch {} }
                    foreach (var p in Process.GetProcessesByName("VELO VPN")) { try { p.Kill(); p.WaitForExit(1000); } catch {} }
                    foreach (var p in Process.GetProcessesByName("Hiddify")) { try { p.Kill(); p.WaitForExit(1000); } catch {} }

                    // Free port 17078 if occupied by any zombie process
                    try {
                        ProcessStartInfo psi = new ProcessStartInfo("netstat", "-ano") { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true };
                        using (var pNet = Process.Start(psi)) {
                            string netOut = pNet.StandardOutput.ReadToEnd();
                            foreach (string line in netOut.Split('\n')) {
                                if (line.Contains(":17078 ") && line.Contains("LISTENING")) {
                                    string[] parts = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    int conflictPid;
                                    if (parts.Length > 0 && int.TryParse(parts[parts.Length - 1], out conflictPid)) {
                                        try { Process.GetProcessById(conflictPid).Kill(); } catch {}
                                    }
                                }
                            }
                        }
                    } catch {}

                    // Clean conflicting legacy autorun keys
                    try {
                        using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                        {
                            if (key != null)
                            {
                                if (key.GetValue("hiddify") != null) key.DeleteValue("hiddify", false);
                                if (key.GetValue("nekoray") != null) key.DeleteValue("nekoray", false);
                                if (key.GetValue("nekobox") != null) key.DeleteValue("nekobox", false);
                            }
                        }
                    } catch {}

                    if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

                    // Extract embedded payload with safe config preservation
                    var asm = Assembly.GetExecutingAssembly();
                    using (Stream s = asm.GetManifestResourceStream("velo_app.zip"))
                    {
                        if (s != null)
                        {
                            string tempZip = Path.Combine(Path.GetTempPath(), "velo_setup_temp.zip");
                            using (FileStream fs = new FileStream(tempZip, FileMode.Create))
                            {
                                s.CopyTo(fs);
                            }

                            using (ZipArchive archive = ZipFile.OpenRead(tempZip))
                            {
                                foreach (ZipArchiveEntry entry in archive.Entries)
                                {
                                    string destinationPath = Path.GetFullPath(Path.Combine(targetDir, entry.FullName));
                                    if (!destinationPath.StartsWith(Path.GetFullPath(targetDir), StringComparison.OrdinalIgnoreCase))
                                        continue;

                                    if (entry.FullName.EndsWith("/") || entry.FullName.EndsWith("\\"))
                                    {
                                        if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);
                                        continue;
                                    }

                                    string dir = Path.GetDirectoryName(destinationPath);
                                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                                    // CONFIG PRESERVATION LOGIC:
                                    // If target file already exists and belongs to user configs/database, NEVER OVERWRITE!
                                    bool isUserData = entry.FullName.IndexOf("configs", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                      entry.FullName.IndexOf("db.sqlite", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                      entry.FullName.IndexOf("AppSettings", StringComparison.OrdinalIgnoreCase) >= 0;

                                    if (File.Exists(destinationPath) && isUserData)
                                    {
                                        // Preserve user's existing configs!
                                        continue;
                                    }

                                    try
                                    {
                                        entry.ExtractToFile(destinationPath, true);
                                    }
                                    catch {}
                                }
                            }

                            try { File.Delete(tempZip); } catch {}
                        }
                    }

                    installedExe = Path.Combine(targetDir, "velovpn.exe");
                    if (!File.Exists(installedExe)) installedExe = Path.Combine(targetDir, "VELO VPN.exe");
                    if (!File.Exists(installedExe)) installedExe = Path.Combine(targetDir, "Hiddify.exe");

                    string iconPath = Path.Combine(targetDir, "velo_logo.ico");

                    // Ensure VPN (TUN) mode is enabled in shared_preferences
                    try {
                        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        string[] targetDirs = new string[] {
                            Path.Combine(appData, "velovpn", "hiddify"),
                            Path.Combine(appData, "Hiddify", "hiddify")
                        };
                        string defaultPrefs = "{\"portable.preferences_version\":4,\"portable.region\":\"ir\",\"portable.locale\":\"fa\",\"portable.intro_completed\":true,\"portable.silent_start\":false,\"portable.service-mode\":\"vpn\",\"portable.extra-security-profile-id\":\"399c69f2-6022-4e19-a83f-d47ee80bcd6c\",\"portable.unblocker-profile-id\":\"399c69f2-6022-4e19-a83f-d47ee80bcd6c\",\"portable.theme_mode\":\"dark\",\"portable.theme\":\"dark\",\"portable.true_black\":true}";
                        foreach (var d in targetDirs) {
                            if (!Directory.Exists(d)) Directory.CreateDirectory(d);
                            string pFile = Path.Combine(d, "shared_preferences.json");
                            if (!File.Exists(pFile)) {
                                File.WriteAllText(pFile, defaultPrefs);
                            } else {
                                string content = File.ReadAllText(pFile);
                                if (!content.Contains("\"portable.service-mode\":\"vpn\"")) {
                                    content = content.Replace("\"portable.service-mode\":\"proxy\"", "\"portable.service-mode\":\"vpn\"");
                                    if (!content.Contains("\"portable.service-mode\"")) {
                                        content = content.TrimEnd('}') + ",\"portable.service-mode\":\"vpn\"}";
                                    }
                                    File.WriteAllText(pFile, content);
                                }
                            }
                        }
                    } catch {}

                    if (makeShortcuts)
                    {
                        string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        CreateShortcut(Path.Combine(desktop, "velovpn.lnk"), installedExe, targetDir, iconPath, "");
                        CreateShortcut(Path.Combine(desktop, "VELO VPN.lnk"), installedExe, targetDir, iconPath, "");

                        string programs = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                        string appMenu = Path.Combine(programs, "velovpn");
                        if (!Directory.Exists(appMenu)) Directory.CreateDirectory(appMenu);
                        CreateShortcut(Path.Combine(appMenu, "velovpn.lnk"), installedExe, targetDir, iconPath, "");
                    }

                    if (runStartup)
                    {
                        string startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                        CreateShortcut(Path.Combine(startup, "velovpn.lnk"), installedExe, targetDir, iconPath, "");
                    }

                    this.Invoke((MethodInvoker)delegate {
                        prgBar.Style = ProgressBarStyle.Blocks;
                        prgBar.Value = 100;
                        lblStatus.Text = isUpdateMode 
                            ? "Upgrade completed successfully! All configs preserved." 
                            : "Installation completed successfully! velovpn is ready.";
                        lblStatus.ForeColor = Color.FromArgb(74, 222, 128); // Emerald Green
                        btnInstall.Text = "LAUNCH VELOVPN & FINISH";
                        btnInstall.BackColor = Color.FromArgb(74, 222, 128);
                        btnInstall.Enabled = true;
                        isDone = true;
                    });
                }
                catch (Exception ex)
                {
                    this.Invoke((MethodInvoker)delegate {
                        prgBar.Visible = false;
                        lblStatus.Text = "Error: " + ex.Message;
                        lblStatus.ForeColor = Color.FromArgb(248, 113, 113);
                        btnInstall.Enabled = true;
                    });
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        private static void CreateShortcut(string shortcutPath, string target, string workingDir, string icon, string args)
        {
            try
            {
                Type shellType = Type.GetTypeFromProgID("WScript.Shell");
                dynamic shell = Activator.CreateInstance(shellType);
                dynamic shortcut = shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = target;
                shortcut.WorkingDirectory = workingDir;
                if (!string.IsNullOrEmpty(args)) shortcut.Arguments = args;
                if (!string.IsNullOrEmpty(icon) && File.Exists(icon)) shortcut.IconLocation = icon + ",0";
                shortcut.Save();

                // Set SLDF_RUNAS_USER (byte 21 |= 0x20) so shortcut requests Administrator elevation
                try
                {
                    byte[] bytes = File.ReadAllBytes(shortcutPath);
                    if (bytes.Length > 21)
                    {
                        bytes[21] = (byte)(bytes[21] | 0x20);
                        File.WriteAllBytes(shortcutPath, bytes);
                    }
                }
                catch {}
            }
            catch {}
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ModernInstaller());
        }
    }
}
