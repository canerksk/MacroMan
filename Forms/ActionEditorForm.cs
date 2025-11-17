using System;
using System.Drawing;
using System.Windows.Forms;
using MacroMan.Models;

namespace MacroMan.Forms
{
    public partial class ActionEditorForm : Form
    {
        public MacroAction MacroAction { get; private set; }
        private Point _selectedPoint;

        public ActionEditorForm(MacroAction existingAction = null)
        {
            InitializeComponent();
            PopulateHotkeyComboBox();

            MacroAction = existingAction ?? new MacroAction();
            LoadData();

        }


        private void PopulateHotkeyComboBox()
        {
            // F1-F12
            for (int i = 1; i <= 12; i++)
                cmbHotkey.Items.Add($"F{i}");

            // Yön tuşları
            cmbHotkey.Items.Add("Sol");
            cmbHotkey.Items.Add("Sag");
            cmbHotkey.Items.Add("Yukari");
            cmbHotkey.Items.Add("Asagi");

            // Harf tuşları
            for (char c = 'A'; c <= 'Z'; c++)
                cmbHotkey.Items.Add(c.ToString());

            // Rakam tuşları
            for (int i = 0; i <= 9; i++)
                cmbHotkey.Items.Add(i.ToString());

            cmbHotkey.Items.Add("Enter");
            cmbHotkey.Items.Add("Space");
            cmbHotkey.Items.Add("Tab");
            cmbHotkey.Items.Add("Esc");

            cmbHotkey.SelectedIndex = 0;
        }

        private void CmbActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlClick.Visible = false;
            pnlText.Visible = false;

            switch (cmbActionType.SelectedIndex)
            {
                case 0: // TuşaBas
                    break;
                case 1: // Click
                    pnlClick.Visible = true;
                    break;
                case 2: // Yaz
                    pnlText.Visible = true;
                    break;
            }
        }

        private async void BtnCaptureMousePos_Click(object sender, EventArgs e)
        {
            btnCaptureMousePos.Enabled = false;
            lblMousePosInfo.Text = "5 saniye içinde mouse'u istediğiniz pozisyona götürün...";
            lblMousePosInfo.ForeColor = Color.Red;

            await System.Threading.Tasks.Task.Delay(5000);

            _selectedPoint = Cursor.Position;
            numClickX.Value = _selectedPoint.X;
            numClickY.Value = _selectedPoint.Y;

            lblMousePosInfo.Text = $"Pozisyon yakalandı: {_selectedPoint.X}, {_selectedPoint.Y}";
            lblMousePosInfo.ForeColor = Color.Green;
            btnCaptureMousePos.Enabled = true;
        }

        private void LoadData()
        {
            // Mevcut action varsa verileri yükle
            cmbHotkey.SelectedItem = MacroAction.HotkeyName ?? "F1";
            numWaitTime.Value = MacroAction.WaitTimeMs;
            cmbActionType.SelectedIndex = (int)MacroAction.ActionType;

            // Modifier keys
            chkCtrl.Checked = MacroAction.UseCtrl;
            chkAlt.Checked = MacroAction.UseAlt;
            chkShift.Checked = MacroAction.UseShift;

            if (MacroAction.ActionType == ActionType.Click)
            {
                numClickX.Value = MacroAction.ClickX ?? 0;
                numClickY.Value = MacroAction.ClickY ?? 0;
                cmbMouseButton.SelectedItem = MacroAction.MouseButton?.ToString() ?? "Sol";
                cmbClickType.SelectedItem = MacroAction.ClickType?.ToString() ?? "Tek";
            }
            else if (MacroAction.ActionType == ActionType.Yaz)
            {
                txtText.Text = MacroAction.TextToWrite ?? "";
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            // Validation
            if (cmbHotkey.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir hotkey seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save data
            MacroAction.HotkeyName = cmbHotkey.SelectedItem.ToString();
            MacroAction.WaitTimeMs = (int)numWaitTime.Value;
            MacroAction.ActionType = (ActionType)cmbActionType.SelectedIndex;

            // Modifier keys
            MacroAction.UseCtrl = chkCtrl.Checked;
            MacroAction.UseAlt = chkAlt.Checked;
            MacroAction.UseShift = chkShift.Checked;

            if (MacroAction.ActionType == ActionType.Click)
            {
                MacroAction.ClickX = (int)numClickX.Value;
                MacroAction.ClickY = (int)numClickY.Value;
                MacroAction.MouseButton = (MouseButton)Enum.Parse(typeof(MouseButton), cmbMouseButton.SelectedItem.ToString());
                MacroAction.ClickType = (ClickType)Enum.Parse(typeof(ClickType), cmbClickType.SelectedItem.ToString());
            }
            else if (MacroAction.ActionType == ActionType.Yaz)
            {
                if (string.IsNullOrWhiteSpace(txtText.Text))
                {
                    MessageBox.Show("Lütfen yazılacak metni girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                MacroAction.TextToWrite = txtText.Text;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
