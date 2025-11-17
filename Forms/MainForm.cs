using MacroMan.Models;
using MacroMan.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MacroMan.Forms
{
    public partial class MainForm : Form
    {
        private List<MacroAction> _macroActions;
        private MacroSettings _macroSettings;
        private MacroExecutor _macroExecutor;
        private ClientScanner _clientScanner;
        private List<UOClient> _uoClients;
        private int _currentExecutingIndex = -1;
        private Timer _scheduledTimer;
        private bool _isMinimizedToTray;
        private InputSimulator _inputSimulator;

        public MainForm()
        {
            InitializeComponent();
            InitializeSystemTray();

            _macroActions = new List<MacroAction>();
            _macroSettings = new MacroSettings();
            _inputSimulator = new InputSimulator(this);
            _macroExecutor = new MacroExecutor(_inputSimulator);
            _clientScanner = new ClientScanner();
            _uoClients = new List<UOClient>();
            _macroExecutor.ProgressChanged += MacroExecutor_ProgressChanged;
            _macroExecutor.MacroCompleted += MacroExecutor_MacroCompleted;
            _macroExecutor.ErrorOccurred += MacroExecutor_ErrorOccurred;

            _scheduledTimer = new Timer();
            _scheduledTimer.Interval = 1000;
            _scheduledTimer.Tick += ScheduledTimer_Tick;

            RefreshDataGrid();
        }

        private void InitializeSystemTray()
        {
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            var contextMenu = new ContextMenuStrip();

            var menuShow = new ToolStripMenuItem("Göster");
            menuShow.Click += (s, e) => ShowFromTray();

            var menuExit = new ToolStripMenuItem("Çıkış");
            menuExit.Click += (s, e) => Application.Exit();

            contextMenu.Items.AddRange(new ToolStripItem[] { menuShow, new ToolStripSeparator(), menuExit });
            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowFromTray();
        }

        private void MinimizeToTray()
        {
            Hide();
            _notifyIcon.Visible = true;
            _isMinimizedToTray = true;

            _notifyIcon.ShowBalloonTip(
                2000,
                "MacroMan",
                "Program sistem tepsisine küçültüldü.\nÇift tıklayarak geri getirebilirsiniz.",
                ToolTipIcon.Info
            );
        }

        private void ShowFromTray()
        {
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            _notifyIcon.Visible = false;
            _isMinimizedToTray = false;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            /*
            if (WindowState == FormWindowState.Minimized)
            {
                MinimizeToTray();
            }
            */
        }

        private void ChangeWindowTitle()
        {
            var inputForm = new Form
            {
                Text = "Window Title Değiştir",
                Size = new Size(400, 150),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblTitle = new Label
            {
                Text = "Yeni Window Title:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            var txtTitle = new TextBox
            {
                Text = Text,
                Location = new Point(20, 45),
                Width = 340
            };

            var btnOk = new Button
            {
                Text = "Tamam",
                DialogResult = DialogResult.OK,
                Location = new Point(200, 75),
                Size = new Size(75, 30)
            };

            var btnCancel = new Button
            {
                Text = "İptal",
                DialogResult = DialogResult.Cancel,
                Location = new Point(285, 75),
                Size = new Size(75, 30)
            };

            inputForm.Controls.AddRange(new Control[] { lblTitle, txtTitle, btnOk, btnCancel });
            inputForm.AcceptButton = btnOk;
            inputForm.CancelButton = btnCancel;

            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                Text = txtTitle.Text;
                _notifyIcon.Text = txtTitle.Text;
            }
        }

        private void ChkScheduled_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkScheduled.Checked;
            dtpScheduledDate.Enabled = isChecked;
            dtpScheduledTime.Enabled = isChecked;

            if (isChecked)
            {
                DateTime defaultTime = DateTime.Now.AddHours(1);
                dtpScheduledDate.Value = defaultTime;
                dtpScheduledTime.Value = defaultTime;
            }
        }

        private void ScheduledTimer_Tick(object sender, EventArgs e)
        {
            if (_macroSettings.IsScheduled && DateTime.Now >= _macroSettings.ScheduledCompletionTime.Value)
            {
                UpdateScheduledTimeDisplay();
            }
        }

        private void UpdateScheduledTimeDisplay()
        {
            if (_macroSettings.IsScheduled && _macroExecutor.IsRunning)
            {
                TimeSpan remaining = _macroSettings.ScheduledCompletionTime.Value - DateTime.Now;
                if (remaining.TotalSeconds > 0)
                {
                    lblStatus.Text = $"Makro çalışıyor... | Kalan: {remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
                }
            }
        }

        private void BtnScanClients_Click(object sender, EventArgs e)
        {
            btnScanClients.Enabled = false;
            btnScanClients.Text = "Taranıyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                _uoClients = _clientScanner.ScanForClients();

                cmbClients.Items.Clear();

                if (_uoClients.Count == 0)
                {
                    cmbClients.Items.Add("Hiç UO client bulunamadı!");
                    cmbClients.SelectedIndex = 0;
                    cmbClients.Enabled = false;
                    btnStart.Enabled = false;
                    MessageBox.Show(
                        "Hiçbir Ultima Online client bulunamadı!\n\n" +
                        "Desteklenen clientler:\n" +
                        "• Official UO (client.exe)\n" +
                        "• Enhanced Client (UOSA.exe)\n" +
                        "• Orion (OrionUO.exe)\n" +
                        "• AssistUO/Razor/UOSteam üzerinden",
                        "Client Bulunamadı",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                else
                {
                    foreach (var client in _uoClients)
                    {
                        cmbClients.Items.Add(client);
                    }
                    cmbClients.SelectedIndex = 0;
                    cmbClients.Enabled = true;

                    UpdateButtonStates();

                    /*
                    MessageBox.Show(
                        $"{_uoClients.Count} adet UO client bulundu!",
                        "Scan Tamamlandı",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    */

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Client tarama hatası: {ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                btnScanClients.Enabled = true;
                btnScanClients.Text = "🔍 Tara";
                Cursor = Cursors.Default;
            }
        }

        private void RefreshDataGrid()
        {
            dgvActions.Rows.Clear();

            for (int i = 0; i < _macroActions.Count; i++)
            {
                var action = _macroActions[i];
                string hotkey = GetHotkeyString(action);
                string detail = GetActionDetail(action);

                dgvActions.Rows.Add(
                    (i + 1).ToString(),
                    hotkey,
                    action.WaitTimeMs + "ms",
                    action.ActionType.ToString(),
                    detail,
                    ""
                );
            }

            UpdateButtonStates();
        }

        private string GetHotkeyString(MacroAction action)
        {
            string modifiers = "";
            if (action.UseCtrl) modifiers += "Ctrl+";
            if (action.UseAlt) modifiers += "Alt+";
            if (action.UseShift) modifiers += "Shift+";

            return modifiers + action.HotkeyName;
        }

        private string GetActionDetail(MacroAction action)
        {
            return action.ActionType switch
            {
                ActionType.TusaBas => "Tuşa Bas",
                ActionType.Click => $"Click: ({action.ClickX},{action.ClickY}) {action.MouseButton} {action.ClickType}",
                ActionType.Yaz => $"Yaz: \"{action.TextToWrite}\"",
                _ => ""
            };
        }

        private void UpdateButtonStates(bool ignoreIsRunning = false)
        {
            bool hasSelection = dgvActions.SelectedRows.Count > 0;
            bool hasItems = _macroActions.Count > 0;
            bool hasClient = _uoClients.Count > 0 && cmbClients.SelectedIndex >= 0;

            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnMoveUp.Enabled = hasSelection;
            btnMoveDown.Enabled = hasSelection;
            btnClear.Enabled = hasItems;

            if (ignoreIsRunning)
                btnStart.Enabled = hasItems && hasClient;
            else
                btnStart.Enabled = hasItems && hasClient && !_macroExecutor.IsRunning;
            // btnStart.Enabled = hasItems && hasClient && !_macroExecutor.IsRunning;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new ActionEditorForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _macroActions.Add(form.MacroAction);
                    RefreshDataGrid();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvActions.SelectedRows.Count == 0) return;

            int index = dgvActions.SelectedRows[0].Index;
            var action = _macroActions[index];

            using (var form = new ActionEditorForm(action))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _macroActions[index] = form.MacroAction;
                    RefreshDataGrid();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvActions.SelectedRows.Count == 0) return;

            var result = MessageBox.Show(
                "Seçili action'ı silmek istediğinize emin misiniz?",
                "Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                int index = dgvActions.SelectedRows[0].Index;
                _macroActions.RemoveAt(index);
                RefreshDataGrid();
            }
        }

        private void BtnMoveUp_Click(object sender, EventArgs e)
        {
            if (dgvActions.SelectedRows.Count == 0) return;

            int index = dgvActions.SelectedRows[0].Index;
            if (index == 0) return;

            var temp = _macroActions[index];
            _macroActions[index] = _macroActions[index - 1];
            _macroActions[index - 1] = temp;

            RefreshDataGrid();
            dgvActions.Rows[index - 1].Selected = true;
        }

        private void BtnMoveDown_Click(object sender, EventArgs e)
        {
            if (dgvActions.SelectedRows.Count == 0) return;

            int index = dgvActions.SelectedRows[0].Index;
            if (index >= _macroActions.Count - 1) return;

            var temp = _macroActions[index];
            _macroActions[index] = _macroActions[index + 1];
            _macroActions[index + 1] = temp;

            RefreshDataGrid();
            dgvActions.Rows[index + 1].Selected = true;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Tüm action'ları silmek istediğinize emin misiniz?",
                "Temizleme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                _macroActions.Clear();
                RefreshDataGrid();
            }
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            if (_macroActions.Count == 0)
            {
                MessageBox.Show("Hiç makro action eklenmemiş!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_uoClients.Count == 0 || cmbClients.SelectedIndex < 0)
            {
                MessageBox.Show("Lütfen önce bir UO client seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _macroSettings.RepeatCount = (int)numRepeatCount.Value;
            _macroSettings.CompletionAction = (CompletionAction)cmbCompletionAction.SelectedIndex;

            var selectedClient = _uoClients[cmbClients.SelectedIndex];
            IntPtr handleToUse = selectedClient.Handle;

            if (selectedClient.Type == ClientType.Assistant)
            {
                try
                {
                    var p = Process.GetProcessById(selectedClient.ProcessId);
                    if (p != null && p.MainWindowHandle != IntPtr.Zero)
                    {
                        handleToUse = p.MainWindowHandle;
                    }
                }
                catch
                {
                    // ignore
                }
            }

            _macroSettings.SelectedClientHandle = handleToUse.ToString();

            if (chkScheduled.Checked)
            {
                DateTime scheduledDate = dtpScheduledDate.Value.Date;
                DateTime scheduledTime = dtpScheduledTime.Value;
                DateTime combined = scheduledDate.Add(scheduledTime.TimeOfDay);

                if (combined <= DateTime.Now)
                {
                    MessageBox.Show(
                        "Zamanlanmış tarih/saat geçmiş bir zamanda olamaz!",
                        "Geçersiz Tarih",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                _macroSettings.ScheduledCompletionTime = combined;
                _scheduledTimer.Start();
            }
            else
            {
                _macroSettings.ScheduledCompletionTime = null;
            }

            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnScanClients.Enabled = false;
            cmbClients.Enabled = false;
            numRepeatCount.Enabled = false;
            cmbCompletionAction.Enabled = false;
            chkScheduled.Enabled = false;
            dtpScheduledDate.Enabled = false;
            dtpScheduledTime.Enabled = false;

            string repeatText = _macroSettings.RepeatCount == 0 ? "∞" : _macroSettings.RepeatCount.ToString();

            if (_macroSettings.IsScheduled)
            {
                lblStatus.Text = $"Makro çalışıyor... (Tekrar: {repeatText}) | Bitiş: {_macroSettings.ScheduledCompletionTime:dd.MM.yyyy HH:mm}";
            }
            else
            {
                lblStatus.Text = $"Makro çalışıyor... (Tekrar: {repeatText})";
            }

            //lblStatus.ForeColor = Color.Green;
            lblStatus.ForeColor = Color.Aqua;

            await _macroExecutor.ExecuteMacroAsync(_macroActions, _macroSettings);
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            _macroExecutor.Stop();
            _scheduledTimer.Stop();
            lblStatus.Text = "Makro durduruldu";
            lblStatus.ForeColor = Color.OrangeRed;

            ResetUI();
        }

        private void MacroExecutor_ProgressChanged(object sender, MacroProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => MacroExecutor_ProgressChanged(sender, e)));
                return;
            }

            if (e.ScheduledTimeReached)
            {
                lblStatus.Text = "Zamanlanmış süre doldu! Makro tamamlandı.";
                //lblStatus.ForeColor = Color.Blue;
                lblStatus.ForeColor = Color.Lime;
                return;
            }

            if (e.CurrentAction != null)
            {
                _currentExecutingIndex = e.CurrentIndex;

                string repeatInfo = e.TotalRepeats == 0
                    ? $"Tekrar: {e.CurrentRepeat} / ∞"
                    : $"Tekrar: {e.CurrentRepeat} / {e.TotalRepeats}";

                string statusText = $"{repeatInfo} | İşlem {e.CurrentIndex + 1}/{e.TotalActions}: {GetHotkeyString(e.CurrentAction)}";

                if (e.ScheduledTime.HasValue)
                {
                    TimeSpan remaining = e.ScheduledTime.Value - DateTime.Now;
                    if (remaining.TotalSeconds > 0)
                    {
                        statusText += $" | Kalan: {remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
                    }
                }

                lblStatus.Text = statusText;

                foreach (DataGridViewRow row in dgvActions.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }

                if (_currentExecutingIndex >= 0 && _currentExecutingIndex < dgvActions.Rows.Count)
                {
                    dgvActions.Rows[_currentExecutingIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }

            if (e.WaitTimeMs > 0)
            {
                int percentage = (int)((double)e.WaitElapsed / e.WaitTimeMs * 100);
                progressBar.Value = Math.Min(percentage, 100);

                if (_currentExecutingIndex >= 0 && _currentExecutingIndex < dgvActions.Rows.Count)
                {
                    dgvActions.Rows[_currentExecutingIndex].Cells["colProgress"].Value = $"{percentage}%";
                }
            }
        }

        private void MacroExecutor_MacroCompleted(object sender, MacroCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => MacroExecutor_MacroCompleted(sender, e)));
                return;
            }

            _scheduledTimer.Stop();

            lblStatus.Text = e.WasScheduled ? "Makro zamanlanmış sürede tamamlandı!" : "Makro tamamlandı!";
            //lblStatus.ForeColor = Color.Blue;
            lblStatus.ForeColor = Color.Lime;
            progressBar.Value = 100;

            ResetUI();

            switch (e.CompletionAction)
            {
                case CompletionAction.CloseClient:
                    if (e.ClientHandle != IntPtr.Zero)
                    {
                        ClientScanner.CloseClient(e.ClientHandle);
                        MessageBox.Show("Client kapatıldı!", "Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case CompletionAction.CloseProgram:
                    MessageBox.Show("Program kapatılıyor...", "Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                    break;

                case CompletionAction.ShutdownPC:
                    var confirmShutdown = MessageBox.Show(
                        "Makro tamamlandı. PC'yi kapatmak istediğinize emin misiniz?",
                        "PC Kapatma Onayı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (confirmShutdown == DialogResult.Yes)
                    {
                        Process.Start("shutdown", "/s /t 10");
                    }
                    break;

                case CompletionAction.DoNothing:
                default:
                    MessageBox.Show("Makro tamamlandı!", "Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void MacroExecutor_ErrorOccurred(object sender, string error)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => MacroExecutor_ErrorOccurred(sender, error)));
                return;
            }

            _scheduledTimer.Stop();
            MessageBox.Show(error, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = $"Hata: {error}";
            lblStatus.ForeColor = Color.Red;

            ResetUI();
        }

        private void ResetUI()
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnAdd.Enabled = true;
            btnScanClients.Enabled = true;
            cmbClients.Enabled = true;
            numRepeatCount.Enabled = true;
            cmbCompletionAction.Enabled = true;
            chkScheduled.Enabled = true;
            dtpScheduledDate.Enabled = chkScheduled.Checked;
            dtpScheduledTime.Enabled = chkScheduled.Checked;
            UpdateButtonStates(ignoreIsRunning: true);
            progressBar.Value = 0;
            _currentExecutingIndex = -1;

            foreach (DataGridViewRow row in dgvActions.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
                row.Cells["colProgress"].Value = "";
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Makro Dosyası (*.xml)|*.xml";
                dialog.DefaultExt = "xml";
                dialog.Title = "Makroyu Kaydet";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var serializer = new XmlSerializer(typeof(List<MacroAction>));
                        using (var stream = new FileStream(dialog.FileName, FileMode.Create))
                        {
                            serializer.Serialize(stream, _macroActions);
                        }

                        MessageBox.Show("Makro başarıyla kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Kaydetme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void menuChangeTitleClick(object sender, EventArgs e)
        {
            ChangeWindowTitle();
        }

        private void menuExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuMinimizeToTrayClick(object sender, EventArgs e)
        {
            MinimizeToTray();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Makro Dosyası (*.xml)|*.xml";
                dialog.Title = "Makro Yükle";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var serializer = new XmlSerializer(typeof(List<MacroAction>));
                        using (var stream = new FileStream(dialog.FileName, FileMode.Open))
                        {
                            _macroActions = (List<MacroAction>)serializer.Deserialize(stream);
                        }

                        RefreshDataGrid();
                        MessageBox.Show("Makro başarıyla yüklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Yükleme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_macroExecutor.IsRunning)
            {
                var result = MessageBox.Show(
                    "Makro hala çalışıyor! Çıkmak istediğinize emin misiniz?",
                    "Makro Çalışıyor",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

                _macroExecutor.Stop();
            }

            _scheduledTimer?.Stop();
            _scheduledTimer?.Dispose();
            _notifyIcon?.Dispose();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            BtnScanClients_Click(btnScanClients, EventArgs.Empty);
        }


    }
}
