namespace ChirayuAntivirus
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button btnQuickScan;
        private System.Windows.Forms.Button btnFullScan;
        private System.Windows.Forms.Button btnCustomScan;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCurrentFile;
        private System.Windows.Forms.Label lblScannedFiles;
        private System.Windows.Forms.Label lblThreatsFound;
        private System.Windows.Forms.ListView listThreats;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Panel panelMain;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            
            // Initialize main form
            this.Text = "ChirayuAntivirus";
            this.Size = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);

            // Menu Strip
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuStrip.Items.Add("File");
            this.menuStrip.Items.Add("Tools");
            this.menuStrip.Items.Add("Help");
            this.Controls.Add(this.menuStrip);

            // Header Panel
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 100;
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.Controls.Add(this.panelHeader);

            // Quick Scan Button
            this.btnQuickScan = new System.Windows.Forms.Button();
            this.btnQuickScan.Text = "Quick Scan";
            this.btnQuickScan.Size = new System.Drawing.Size(120, 40);
            this.btnQuickScan.Location = new System.Drawing.Point(20, 30);
            this.btnQuickScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickScan.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnQuickScan.ForeColor = System.Drawing.Color.White;
            this.btnQuickScan.Click += async (s, e) => await StartQuickScan();
            this.panelHeader.Controls.Add(this.btnQuickScan);

            // Full Scan Button
            this.btnFullScan = new System.Windows.Forms.Button();
            this.btnFullScan.Text = "Full Scan";
            this.btnFullScan.Size = new System.Drawing.Size(120, 40);
            this.btnFullScan.Location = new System.Drawing.Point(150, 30);
            this.btnFullScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFullScan.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnFullScan.ForeColor = System.Drawing.Color.White;
            this.btnFullScan.Click += async (s, e) => await StartFullScan();
            this.panelHeader.Controls.Add(this.btnFullScan);

            // Custom Scan Button
            this.btnCustomScan = new System.Windows.Forms.Button();
            this.btnCustomScan.Text = "Custom Scan";
            this.btnCustomScan.Size = new System.Drawing.Size(120, 40);
            this.btnCustomScan.Location = new System.Drawing.Point(280, 30);
            this.btnCustomScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomScan.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnCustomScan.ForeColor = System.Drawing.Color.White;
            this.btnCustomScan.Click += async (s, e) => await StartCustomScan();
            this.panelHeader.Controls.Add(this.btnCustomScan);

            // Status Panel
            this.panelStatus = new System.Windows.Forms.Panel();
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatus.Height = 100;
            this.panelStatus.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            this.Controls.Add(this.panelStatus);

            // Status Label
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblStatus.Text = "System Protected";
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
            this.lblStatus.Location = new System.Drawing.Point(20, 10);
            this.lblStatus.AutoSize = true;
            this.panelStatus.Controls.Add(this.lblStatus);

            // Current File Label
            this.lblCurrentFile = new System.Windows.Forms.Label();
            this.lblCurrentFile.Text = "Ready to scan";
            this.lblCurrentFile.ForeColor = System.Drawing.Color.LightGray;
            this.lblCurrentFile.Location = new System.Drawing.Point(20, 40);
            this.lblCurrentFile.AutoSize = true;
            this.panelStatus.Controls.Add(this.lblCurrentFile);

            // Scanned Files Label
            this.lblScannedFiles = new System.Windows.Forms.Label();
            this.lblScannedFiles.Text = "Files Scanned: 0";
            this.lblScannedFiles.ForeColor = System.Drawing.Color.LightGray;
            this.lblScannedFiles.Location = new System.Drawing.Point(20, 60);
            this.lblScannedFiles.AutoSize = true;
            this.panelStatus.Controls.Add(this.lblScannedFiles);

            // Threats Found Label
            this.lblThreatsFound = new System.Windows.Forms.Label();
            this.lblThreatsFound.Text = "Threats Found: 0";
            this.lblThreatsFound.ForeColor = System.Drawing.Color.LightGray;
            this.lblThreatsFound.Location = new System.Drawing.Point(200, 60);
            this.lblThreatsFound.AutoSize = true;
            this.panelStatus.Controls.Add(this.lblThreatsFound);

            // Progress Bar
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressBar.Location = new System.Drawing.Point(20, 85);
            this.progressBar.Size = new System.Drawing.Size(760, 10);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.panelStatus.Controls.Add(this.progressBar);

            // Main Panel
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.Controls.Add(this.panelMain);

            // Threats ListView
            this.listThreats = new System.Windows.Forms.ListView();
            this.listThreats.View = System.Windows.Forms.View.Details;
            this.listThreats.FullRowSelect = true;
            this.listThreats.GridLines = true;
            this.listThreats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listThreats.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.listThreats.ForeColor = System.Drawing.Color.White;

            // Add columns to ListView
            this.listThreats.Columns.Add("Time", 150);
            this.listThreats.Columns.Add("Threat", 200);
            this.listThreats.Columns.Add("Location", 300);
            this.listThreats.Columns.Add("Severity", 100);
            
            this.panelMain.Controls.Add(this.listThreats);
        }
    }
}
