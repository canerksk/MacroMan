using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace MacroMan.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            cmbClients = new ComboBox();
            grpClient = new GroupBox();
            btnScanClients = new Button();
            grpSettings = new GroupBox();
            lblRepeat = new Label();
            numRepeatCount = new NumericUpDown();
            lblRepeatInfo = new Label();
            chkScheduled = new CheckBox();
            dtpScheduledDate = new DateTimePicker();
            dtpScheduledTime = new DateTimePicker();
            lblScheduledInfo = new Label();
            lblCompletion = new Label();
            cmbCompletionAction = new ComboBox();
            lblTitle = new Label();
            pnlButtons = new Panel();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnMoveUp = new Button();
            btnMoveDown = new Button();
            btnClear = new Button();
            menuStrip = new MenuStrip();
            menuFile = new ToolStripMenuItem();
            menuSave = new ToolStripMenuItem();
            menuLoad = new ToolStripMenuItem();
            menuExit = new ToolStripMenuItem();
            menuView = new ToolStripMenuItem();
            menuMinimizeToTray = new ToolStripMenuItem();
            menuTools = new ToolStripMenuItem();
            menuChangeTitle = new ToolStripMenuItem();
            dgvActions = new DataGridView();
            colIndex = new DataGridViewTextBoxColumn();
            colHotkey = new DataGridViewTextBoxColumn();
            colWait = new DataGridViewTextBoxColumn();
            colType = new DataGridViewTextBoxColumn();
            colDetail = new DataGridViewTextBoxColumn();
            colProgress = new DataGridViewTextBoxColumn();
            pnlStatus = new Panel();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            btnStart = new Button();
            btnStop = new Button();
            _notifyIcon = new NotifyIcon(components);
            grpClient.SuspendLayout();
            grpSettings.SuspendLayout();
            ((ISupportInitialize)numRepeatCount).BeginInit();
            pnlButtons.SuspendLayout();
            menuStrip.SuspendLayout();
            ((ISupportInitialize)dgvActions).BeginInit();
            pnlStatus.SuspendLayout();
            SuspendLayout();
            // 
            // cmbClients
            // 
            cmbClients.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClients.Items.AddRange(new object[] { "Client bulunamadı - Scan butonuna tıklayın" });
            cmbClients.Location = new Point(8, 22);
            cmbClients.Name = "cmbClients";
            cmbClients.Size = new Size(260, 23);
            cmbClients.TabIndex = 1;
            // 
            // grpClient
            // 
            grpClient.Controls.Add(cmbClients);
            grpClient.Controls.Add(btnScanClients);
            grpClient.ForeColor = Color.FromArgb(192, 255, 255);
            grpClient.Location = new Point(12, 27);
            grpClient.Name = "grpClient";
            grpClient.Size = new Size(355, 60);
            grpClient.TabIndex = 1;
            grpClient.TabStop = false;
            grpClient.Text = "UO Client Seçimi";
            // 
            // btnScanClients
            // 
            btnScanClients.Location = new Point(274, 20);
            btnScanClients.Name = "btnScanClients";
            btnScanClients.Size = new Size(68, 25);
            btnScanClients.TabIndex = 2;
            btnScanClients.Text = "🔍 Tara";
            btnScanClients.Click += BtnScanClients_Click;
            // 
            // grpSettings
            // 
            grpSettings.Controls.Add(lblRepeat);
            grpSettings.Controls.Add(numRepeatCount);
            grpSettings.Controls.Add(lblRepeatInfo);
            grpSettings.Controls.Add(chkScheduled);
            grpSettings.Controls.Add(dtpScheduledDate);
            grpSettings.Controls.Add(dtpScheduledTime);
            grpSettings.Controls.Add(lblScheduledInfo);
            grpSettings.ForeColor = Color.FromArgb(192, 255, 255);
            grpSettings.Location = new Point(373, 27);
            grpSettings.Name = "grpSettings";
            grpSettings.Size = new Size(298, 97);
            grpSettings.TabIndex = 2;
            grpSettings.TabStop = false;
            grpSettings.Text = "Makro Ayarları";
            // 
            // lblRepeat
            // 
            lblRepeat.AutoSize = true;
            lblRepeat.ForeColor = Color.Aqua;
            lblRepeat.Location = new Point(10, 20);
            lblRepeat.Name = "lblRepeat";
            lblRepeat.Size = new Size(41, 15);
            lblRepeat.TabIndex = 0;
            lblRepeat.Text = "Tekrar:";
            // 
            // numRepeatCount
            // 
            numRepeatCount.Location = new Point(10, 38);
            numRepeatCount.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            numRepeatCount.Name = "numRepeatCount";
            numRepeatCount.Size = new Size(81, 23);
            numRepeatCount.TabIndex = 1;
            numRepeatCount.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // lblRepeatInfo
            // 
            lblRepeatInfo.AutoSize = true;
            lblRepeatInfo.ForeColor = Color.Gray;
            lblRepeatInfo.Location = new Point(10, 72);
            lblRepeatInfo.Name = "lblRepeatInfo";
            lblRepeatInfo.Size = new Size(72, 15);
            lblRepeatInfo.TabIndex = 2;
            lblRepeatInfo.Text = "(0 = Sonsuz)";
            // 
            // chkScheduled
            // 
            chkScheduled.AutoSize = true;
            chkScheduled.Location = new Point(108, 20);
            chkScheduled.Name = "chkScheduled";
            chkScheduled.Size = new Size(142, 19);
            chkScheduled.TabIndex = 5;
            chkScheduled.Text = "Zamanlanmış Bitirme:";
            chkScheduled.CheckedChanged += ChkScheduled_CheckedChanged;
            // 
            // dtpScheduledDate
            // 
            dtpScheduledDate.Enabled = false;
            dtpScheduledDate.Format = DateTimePickerFormat.Short;
            dtpScheduledDate.Location = new Point(108, 38);
            dtpScheduledDate.Name = "dtpScheduledDate";
            dtpScheduledDate.Size = new Size(97, 23);
            dtpScheduledDate.TabIndex = 6;
            // 
            // dtpScheduledTime
            // 
            dtpScheduledTime.Enabled = false;
            dtpScheduledTime.Format = DateTimePickerFormat.Time;
            dtpScheduledTime.Location = new Point(211, 38);
            dtpScheduledTime.Name = "dtpScheduledTime";
            dtpScheduledTime.ShowUpDown = true;
            dtpScheduledTime.Size = new Size(81, 23);
            dtpScheduledTime.TabIndex = 7;
            // 
            // lblScheduledInfo
            // 
            lblScheduledInfo.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblScheduledInfo.ForeColor = Color.Gray;
            lblScheduledInfo.Location = new Point(108, 64);
            lblScheduledInfo.Name = "lblScheduledInfo";
            lblScheduledInfo.Size = new Size(184, 30);
            lblScheduledInfo.TabIndex = 8;
            lblScheduledInfo.Text = "(Belirtilen tarih/saatte 'Bitince' eylemini gerçekleştirir)";
            // 
            // lblCompletion
            // 
            lblCompletion.AutoSize = true;
            lblCompletion.ForeColor = Color.FromArgb(192, 255, 255);
            lblCompletion.Location = new Point(190, 100);
            lblCompletion.Name = "lblCompletion";
            lblCompletion.Size = new Size(46, 15);
            lblCompletion.TabIndex = 3;
            lblCompletion.Text = "Bitince:";
            // 
            // cmbCompletionAction
            // 
            cmbCompletionAction.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCompletionAction.Items.AddRange(new object[] { "Hiçbir Şey Yapma", "Client'ı Kapat", "Programı Kapat", "PC'yi Kapat" });
            cmbCompletionAction.Location = new Point(242, 95);
            cmbCompletionAction.Name = "cmbCompletionAction";
            cmbCompletionAction.Size = new Size(125, 23);
            cmbCompletionAction.TabIndex = 4;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(192, 255, 255);
            lblTitle.Location = new Point(20, 95);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(164, 21);
            lblTitle.TabIndex = 3;
            lblTitle.Text = "Makro Action Listesi";
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnAdd);
            pnlButtons.Controls.Add(btnEdit);
            pnlButtons.Controls.Add(btnDelete);
            pnlButtons.Controls.Add(btnMoveUp);
            pnlButtons.Controls.Add(btnMoveDown);
            pnlButtons.Controls.Add(btnClear);
            pnlButtons.Location = new Point(558, 130);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(113, 271);
            pnlButtons.TabIndex = 5;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = SystemColors.Control;
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.ForeColor = Color.OliveDrab;
            btnAdd.Location = new Point(0, 0);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(113, 40);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "+ Ekle";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += BtnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = SystemColors.Control;
            btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEdit.ForeColor = Color.DarkCyan;
            btnEdit.Location = new Point(0, 50);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(113, 40);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Düzenle";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += BtnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = SystemColors.Control;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.ForeColor = Color.Maroon;
            btnDelete.Location = new Point(0, 100);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(113, 40);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "- Sil";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnMoveUp
            // 
            btnMoveUp.BackColor = SystemColors.Control;
            btnMoveUp.ForeColor = Color.Black;
            btnMoveUp.Location = new Point(0, 150);
            btnMoveUp.Name = "btnMoveUp";
            btnMoveUp.Size = new Size(113, 24);
            btnMoveUp.TabIndex = 3;
            btnMoveUp.Text = "↑ Yukarı";
            btnMoveUp.UseVisualStyleBackColor = false;
            btnMoveUp.Click += BtnMoveUp_Click;
            // 
            // btnMoveDown
            // 
            btnMoveDown.BackColor = SystemColors.Control;
            btnMoveDown.ForeColor = Color.Black;
            btnMoveDown.Location = new Point(0, 180);
            btnMoveDown.Name = "btnMoveDown";
            btnMoveDown.Size = new Size(113, 24);
            btnMoveDown.TabIndex = 4;
            btnMoveDown.Text = "↓ Aşağı";
            btnMoveDown.UseVisualStyleBackColor = false;
            btnMoveDown.Click += BtnMoveDown_Click;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.IndianRed;
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(-3, 231);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(116, 40);
            btnClear.TabIndex = 5;
            btnClear.Text = "Temizle";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += BtnClear_Click;
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { menuFile, menuView, menuTools });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(685, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // menuFile
            // 
            menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuSave, menuLoad, menuExit });
            menuFile.Name = "menuFile";
            menuFile.Size = new Size(51, 20);
            menuFile.Text = "Dosya";
            // 
            // menuSave
            // 
            menuSave.Name = "menuSave";
            menuSave.ShortcutKeys = Keys.Control | Keys.S;
            menuSave.Size = new Size(150, 22);
            menuSave.Text = "Kaydet";
            menuSave.Click += BtnSave_Click;
            // 
            // menuLoad
            // 
            menuLoad.Name = "menuLoad";
            menuLoad.ShortcutKeys = Keys.Control | Keys.O;
            menuLoad.Size = new Size(150, 22);
            menuLoad.Text = "Yükle";
            menuLoad.Click += BtnLoad_Click;
            // 
            // menuExit
            // 
            menuExit.Name = "menuExit";
            menuExit.Size = new Size(150, 22);
            menuExit.Text = "Çıkış";
            menuExit.Click += menuExitClick;
            // 
            // menuView
            // 
            menuView.DropDownItems.AddRange(new ToolStripItem[] { menuMinimizeToTray });
            menuView.Name = "menuView";
            menuView.Size = new Size(70, 20);
            menuView.Text = "Görünüm";
            // 
            // menuMinimizeToTray
            // 
            menuMinimizeToTray.Name = "menuMinimizeToTray";
            menuMinimizeToTray.ShortcutKeys = Keys.Control | Keys.M;
            menuMinimizeToTray.Size = new Size(241, 22);
            menuMinimizeToTray.Text = "Sistem Tepsisine Küçült";
            menuMinimizeToTray.Click += menuMinimizeToTrayClick;
            // 
            // menuTools
            // 
            menuTools.DropDownItems.AddRange(new ToolStripItem[] { menuChangeTitle });
            menuTools.Name = "menuTools";
            menuTools.Size = new Size(56, 20);
            menuTools.Text = "Araçlar";
            // 
            // menuChangeTitle
            // 
            menuChangeTitle.Name = "menuChangeTitle";
            menuChangeTitle.Size = new Size(195, 22);
            menuChangeTitle.Text = "Window Title Değiştir...";
            menuChangeTitle.Click += menuChangeTitleClick;
            // 
            // dgvActions
            // 
            dgvActions.AllowUserToAddRows = false;
            dgvActions.AllowUserToDeleteRows = false;
            dgvActions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvActions.BackgroundColor = Color.FromArgb(64, 64, 64);
            dgvActions.Columns.AddRange(new DataGridViewColumn[] { colIndex, colHotkey, colWait, colType, colDetail, colProgress });
            dgvActions.Location = new Point(20, 130);
            dgvActions.MultiSelect = false;
            dgvActions.Name = "dgvActions";
            dgvActions.ReadOnly = true;
            dgvActions.RowHeadersVisible = false;
            dgvActions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvActions.Size = new Size(532, 271);
            dgvActions.TabIndex = 4;
            // 
            // colIndex
            // 
            colIndex.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colIndex.HeaderText = "#";
            colIndex.Name = "colIndex";
            colIndex.ReadOnly = true;
            colIndex.Width = 40;
            // 
            // colHotkey
            // 
            colHotkey.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colHotkey.HeaderText = "Tuş";
            colHotkey.Name = "colHotkey";
            colHotkey.ReadOnly = true;
            // 
            // colWait
            // 
            colWait.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colWait.HeaderText = "Bekleme";
            colWait.Name = "colWait";
            colWait.ReadOnly = true;
            colWait.Width = 80;
            // 
            // colType
            // 
            colType.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colType.HeaderText = "Tip";
            colType.Name = "colType";
            colType.ReadOnly = true;
            colType.Width = 80;
            // 
            // colDetail
            // 
            colDetail.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colDetail.HeaderText = "Detay";
            colDetail.Name = "colDetail";
            colDetail.ReadOnly = true;
            // 
            // colProgress
            // 
            colProgress.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colProgress.HeaderText = "İlerleme";
            colProgress.Name = "colProgress";
            colProgress.ReadOnly = true;
            colProgress.Width = 120;
            // 
            // pnlStatus
            // 
            pnlStatus.BackColor = Color.FromArgb(64, 64, 64);
            pnlStatus.BorderStyle = BorderStyle.FixedSingle;
            pnlStatus.Controls.Add(lblStatus);
            pnlStatus.Controls.Add(progressBar);
            pnlStatus.Controls.Add(btnStart);
            pnlStatus.Controls.Add(btnStop);
            pnlStatus.Location = new Point(20, 407);
            pnlStatus.Name = "pnlStatus";
            pnlStatus.Size = new Size(651, 80);
            pnlStatus.TabIndex = 6;
            // 
            // lblStatus
            // 
            lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(192, 255, 255);
            lblStatus.Location = new Point(10, 7);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(634, 20);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Hazır (Client seçiniz.)";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(10, 35);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(433, 30);
            progressBar.TabIndex = 1;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.Transparent;
            btnStart.BackgroundImage = Properties.Resources.Baslat;
            btnStart.BackgroundImageLayout = ImageLayout.Center;
            btnStart.Enabled = false;
            btnStart.FlatStyle = FlatStyle.Popup;
            btnStart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(475, 35);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(73, 30);
            btnStart.TabIndex = 2;
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += BtnStart_Click;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.Transparent;
            btnStop.BackgroundImage = Properties.Resources.Stop;
            btnStop.BackgroundImageLayout = ImageLayout.Center;
            btnStop.Enabled = false;
            btnStop.FlatStyle = FlatStyle.Popup;
            btnStop.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStop.ForeColor = Color.White;
            btnStop.Location = new Point(554, 35);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(90, 30);
            btnStop.TabIndex = 3;
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += BtnStop_Click;
            // 
            // _notifyIcon
            // 
            _notifyIcon.BalloonTipTitle = "MacroMan V3";
            _notifyIcon.Icon = (Icon)resources.GetObject("_notifyIcon.Icon");
            _notifyIcon.Text = "MacroMan V3";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(685, 508);
            Controls.Add(menuStrip);
            Controls.Add(grpClient);
            Controls.Add(grpSettings);
            Controls.Add(lblCompletion);
            Controls.Add(lblTitle);
            Controls.Add(cmbCompletionAction);
            Controls.Add(dgvActions);
            Controls.Add(pnlButtons);
            Controls.Add(pnlStatus);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Macroman V3.0 by Caner";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            Resize += MainForm_Resize;
            grpClient.ResumeLayout(false);
            grpSettings.ResumeLayout(false);
            grpSettings.PerformLayout();
            ((ISupportInitialize)numRepeatCount).EndInit();
            pnlButtons.ResumeLayout(false);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ((ISupportInitialize)dgvActions).EndInit();
            pnlStatus.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // === FIELDS ===
        private DataGridView dgvActions;
        private ComboBox cmbClients;
        private Button btnScanClients;
        private NumericUpDown numRepeatCount;
        private ComboBox cmbCompletionAction;
        private CheckBox chkScheduled;
        private DateTimePicker dtpScheduledDate;
        private DateTimePicker dtpScheduledTime;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnMoveUp;
        private Button btnMoveDown;
        private Button btnClear;
        private Label lblStatus;
        private ProgressBar progressBar;
        private Button btnStart;
        private Button btnStop;

        private GroupBox grpClient;
        private GroupBox grpSettings;
        private Label lblRepeat;
        private Label lblRepeatInfo;
        private Label lblCompletion;
        private Label lblScheduledInfo;
        private Label lblTitle;
        private Panel pnlButtons;
        private MenuStrip menuStrip;
        private Panel pnlStatus;

        private ToolStripMenuItem menuFile;
        private ToolStripMenuItem menuView;
        private ToolStripMenuItem menuTools;
        private ToolStripMenuItem menuSave;
        private ToolStripMenuItem menuLoad;
        private ToolStripMenuItem menuExit;
        private ToolStripMenuItem menuMinimizeToTray;
        private ToolStripMenuItem menuChangeTitle;
        private NotifyIcon _notifyIcon;
        private DataGridViewTextBoxColumn colIndex;
        private DataGridViewTextBoxColumn colHotkey;
        private DataGridViewTextBoxColumn colWait;
        private DataGridViewTextBoxColumn colType;
        private DataGridViewTextBoxColumn colDetail;
        private DataGridViewTextBoxColumn colProgress;
    }
}
