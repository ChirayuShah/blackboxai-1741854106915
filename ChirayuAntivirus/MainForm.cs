using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace ChirayuAntivirus
{
    public partial class MainForm : Form
    {
        private readonly AntivirusEngine _antivirusEngine;
        private bool _isScanning;

        public MainForm()
        {
            InitializeComponent();
            _antivirusEngine = new AntivirusEngine();
            
            // Subscribe to engine events
            _antivirusEngine.ScanProgress += OnScanProgress;
            _antivirusEngine.ThreatDetected += OnThreatDetected;

            // Start real-time protection
            _antivirusEngine.StartRealTimeProtection();

            // Initialize system tray icon
            InitializeSystemTray();
        }

        private void InitializeSystemTray()
        {
            notifyIcon.Icon = Properties.Resources.AppIcon;
            notifyIcon.Text = "ChirayuAntivirus";
            notifyIcon.Visible = true;

            // Create context menu
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Open Dashboard", null, (s, e) => 
            {
                Show();
                WindowState = FormWindowState.Normal;
            });
            contextMenu.Items.Add("Quick Scan", null, async (s, e) => await StartQuickScan());
            contextMenu.Items.Add("-"); // Separator
            contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());

            notifyIcon.ContextMenuStrip = contextMenu;
        }

        private async Task StartQuickScan()
        {
            if (_isScanning)
            {
                MessageBox.Show("A scan is already in progress.", "ChirayuAntivirus", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _isScanning = true;
                btnQuickScan.Enabled = false;
                btnFullScan.Enabled = false;
                btnCustomScan.Enabled = false;
                progressBar.Value = 0;
                lblStatus.Text = "Quick Scan in progress...";

                var result = await _antivirusEngine.QuickScanAsync();

                if (result.WasCancelled)
                {
                    lblStatus.Text = "Scan cancelled.";
                }
                else if (result.Error != null)
                {
                    lblStatus.Text = "Scan completed with errors.";
                    MessageBox.Show($"Scan encountered errors: {result.Error.Message}", 
                        "ChirayuAntivirus", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    lblStatus.Text = $"Scan completed. Found {result.DetectedThreats.Count} threats.";
                    UpdateThreatsList(result.DetectedThreats);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError("Quick Scan failed", ex);
                MessageBox.Show("An error occurred during the scan. Check logs for details.", 
                    "ChirayuAntivirus", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isScanning = false;
                btnQuickScan.Enabled = true;
                btnFullScan.Enabled = true;
                btnCustomScan.Enabled = true;
            }
        }

        private async Task StartFullScan()
        {
            if (_isScanning)
            {
                MessageBox.Show("A scan is already in progress.", "ChirayuAntivirus", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _isScanning = true;
                btnQuickScan.Enabled = false;
                btnFullScan.Enabled = false;
                btnCustomScan.Enabled = false;
                progressBar.Value = 0;
                lblStatus.Text = "Full System Scan in progress...";

                var result = await _antivirusEngine.FullScanAsync();

                if (result.WasCancelled)
                {
                    lblStatus.Text = "Scan cancelled.";
                }
                else if (result.Error != null)
                {
                    lblStatus.Text = "Scan completed with errors.";
                    MessageBox.Show($"Scan encountered errors: {result.Error.Message}", 
                        "ChirayuAntivirus", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    lblStatus.Text = $"Scan completed. Found {result.DetectedThreats.Count} threats.";
                    UpdateThreatsList(result.DetectedThreats);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError("Full Scan failed", ex);
                MessageBox.Show("An error occurred during the scan. Check logs for details.", 
                    "ChirayuAntivirus", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isScanning = false;
                btnQuickScan.Enabled = true;
                btnFullScan.Enabled = true;
                btnCustomScan.Enabled = true;
            }
        }

        private async Task StartCustomScan()
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    if (_isScanning)
                    {
                        MessageBox.Show("A scan is already in progress.", "ChirayuAntivirus", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    try
                    {
                        _isScanning = true;
                        btnQuickScan.Enabled = false;
                        btnFullScan.Enabled = false;
                        btnCustomScan.Enabled = false;
                        progressBar.Value = 0;
                        lblStatus.Text = "Custom Scan in progress...";

                        var result = await _antivirusEngine.CustomScanAsync(new[] { folderDialog.SelectedPath });

                        if (result.WasCancelled)
                        {
                            lblStatus.Text = "Scan cancelled.";
                        }
                        else if (result.Error != null)
                        {
                            lblStatus.Text = "Scan completed with errors.";
                            MessageBox.Show($"Scan encountered errors: {result.Error.Message}", 
                                "ChirayuAntivirus", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            lblStatus.Text = $"Scan completed. Found {result.DetectedThreats.Count} threats.";
                            UpdateThreatsList(result.DetectedThreats);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError("Custom Scan failed", ex);
                        MessageBox.Show("An error occurred during the scan. Check logs for details.", 
                            "ChirayuAntivirus", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        _isScanning = false;
                        btnQuickScan.Enabled = true;
                        btnFullScan.Enabled = true;
                        btnCustomScan.Enabled = true;
                    }
                }
            }
        }

        private void OnScanProgress(object sender, ScanProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnScanProgress(sender, e)));
                return;
            }

            lblCurrentFile.Text = $"Scanning: {e.CurrentFile}";
            lblScannedFiles.Text = $"Files Scanned: {e.ScannedFiles}";
            lblThreatsFound.Text = $"Threats Found: {e.ThreatsFound}";
            
            // Update progress bar (simulate progress)
            progressBar.Value = Math.Min(progressBar.Value + 1, progressBar.Maximum);
        }

        private void OnThreatDetected(object sender, ThreatDetectedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnThreatDetected(sender, e)));
                return;
            }

            LogManager.LogWarning($"Threat detected: {e.Threat.ThreatName} in {e.Threat.FilePath}");
            
            // Add to threats list
            listThreats.Items.Add(new ListViewItem(new[]
            {
                e.Threat.DetectionTime.ToString("yyyy-MM-dd HH:mm:ss"),
                e.Threat.ThreatName,
                e.Threat.FilePath,
                e.Threat.Severity.ToString()
            }));

            // Show notification
            notifyIcon.ShowBalloonTip(
                3000,
                "Threat Detected",
                $"Threat '{e.Threat.ThreatName}' detected in {e.Threat.FilePath}",
                ToolTipIcon.Warning
            );
        }

        private void UpdateThreatsList(System.Collections.Generic.List<ThreatInfo> threats)
        {
            listThreats.Items.Clear();
            foreach (var threat in threats)
            {
                listThreats.Items.Add(new ListViewItem(new[]
                {
                    threat.DetectionTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    threat.ThreatName,
                    threat.FilePath,
                    threat.Severity.ToString()
                }));
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                notifyIcon.ShowBalloonTip(
                    3000,
                    "ChirayuAntivirus",
                    "Application minimized to system tray",
                    ToolTipIcon.Info
                );
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.ShowBalloonTip(
                    3000,
                    "ChirayuAntivirus",
                    "Application minimized to system tray",
                    ToolTipIcon.Info
                );
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _antivirusEngine.StopRealTimeProtection();
                notifyIcon?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
