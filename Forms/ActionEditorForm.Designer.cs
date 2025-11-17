using System.Drawing;
using System.Windows.Forms;

namespace MacroMan.Forms
{
    partial class ActionEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        private void InitializeComponent()
        {
            lblHotkey = new Label();
            cmbHotkey = new ComboBox();
            grpModifiers = new GroupBox();
            chkCtrl = new CheckBox();
            chkAlt = new CheckBox();
            chkShift = new CheckBox();
            lblModifierExample = new Label();
            lblWaitTime = new Label();
            numWaitTime = new NumericUpDown();
            lblActionType = new Label();
            cmbActionType = new ComboBox();
            pnlClick = new Panel();
            lblClickX = new Label();
            numClickX = new NumericUpDown();
            lblClickY = new Label();
            numClickY = new NumericUpDown();
            btnCaptureMousePos = new Button();
            lblMouseBtn = new Label();
            cmbMouseButton = new ComboBox();
            lblClickType = new Label();
            cmbClickType = new ComboBox();
            lblMousePosInfo = new Label();
            pnlText = new Panel();
            lblText = new Label();
            txtText = new TextBox();
            btnOk = new Button();
            btnCancel = new Button();
            grpModifiers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numWaitTime).BeginInit();
            pnlClick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numClickX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numClickY).BeginInit();
            pnlText.SuspendLayout();
            SuspendLayout();
            // 
            // lblHotkey
            // 
            lblHotkey.AutoSize = true;
            lblHotkey.Location = new Point(20, 20);
            lblHotkey.Name = "lblHotkey";
            lblHotkey.Size = new Size(28, 15);
            lblHotkey.TabIndex = 0;
            lblHotkey.Text = "Tuş:";
            // 
            // cmbHotkey
            // 
            cmbHotkey.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHotkey.Location = new Point(20, 43);
            cmbHotkey.Name = "cmbHotkey";
            cmbHotkey.Size = new Size(200, 23);
            cmbHotkey.TabIndex = 1;
            // 
            // grpModifiers
            // 
            grpModifiers.Controls.Add(chkCtrl);
            grpModifiers.Controls.Add(chkAlt);
            grpModifiers.Controls.Add(chkShift);
            grpModifiers.Controls.Add(lblModifierExample);
            grpModifiers.Location = new Point(230, 20);
            grpModifiers.Name = "grpModifiers";
            grpModifiers.Size = new Size(240, 70);
            grpModifiers.TabIndex = 2;
            grpModifiers.TabStop = false;
            // 
            // chkCtrl
            // 
            chkCtrl.AutoSize = true;
            chkCtrl.Location = new Point(10, 25);
            chkCtrl.Name = "chkCtrl";
            chkCtrl.Size = new Size(53, 19);
            chkCtrl.TabIndex = 0;
            chkCtrl.Text = "CTRL";
            // 
            // chkAlt
            // 
            chkAlt.AutoSize = true;
            chkAlt.Location = new Point(85, 25);
            chkAlt.Name = "chkAlt";
            chkAlt.Size = new Size(45, 19);
            chkAlt.TabIndex = 1;
            chkAlt.Text = "ALT";
            // 
            // chkShift
            // 
            chkShift.AutoSize = true;
            chkShift.Location = new Point(165, 25);
            chkShift.Name = "chkShift";
            chkShift.Size = new Size(56, 19);
            chkShift.TabIndex = 2;
            chkShift.Text = "SHIFT";
            // 
            // lblModifierExample
            // 
            lblModifierExample.Font = new Font("Segoe UI", 7F);
            lblModifierExample.ForeColor = Color.Gray;
            lblModifierExample.Location = new Point(10, 47);
            lblModifierExample.Name = "lblModifierExample";
            lblModifierExample.Size = new Size(220, 15);
            lblModifierExample.TabIndex = 3;
            lblModifierExample.Text = "Örnek: Ctrl+F1, Alt+A, Shift+Tab";
            // 
            // lblWaitTime
            // 
            lblWaitTime.AutoSize = true;
            lblWaitTime.Location = new Point(20, 105);
            lblWaitTime.Name = "lblWaitTime";
            lblWaitTime.Size = new Size(116, 15);
            lblWaitTime.TabIndex = 3;
            lblWaitTime.Text = "Bekleme Süresi (ms):";
            // 
            // numWaitTime
            // 
            numWaitTime.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numWaitTime.Location = new Point(20, 130);
            numWaitTime.Maximum = new decimal(new int[] { 60000, 0, 0, 0 });
            numWaitTime.Name = "numWaitTime";
            numWaitTime.Size = new Size(200, 23);
            numWaitTime.TabIndex = 4;
            numWaitTime.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // lblActionType
            // 
            lblActionType.AutoSize = true;
            lblActionType.Location = new Point(240, 105);
            lblActionType.Name = "lblActionType";
            lblActionType.Size = new Size(60, 15);
            lblActionType.TabIndex = 5;
            lblActionType.Text = "İşlem Tipi:";
            // 
            // cmbActionType
            // 
            cmbActionType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbActionType.Items.AddRange(new object[] { "TuşaBas", "Click", "Yaz" });
            cmbActionType.Location = new Point(240, 130);
            cmbActionType.Name = "cmbActionType";
            cmbActionType.Size = new Size(230, 23);
            cmbActionType.TabIndex = 6;
            cmbActionType.SelectedIndexChanged += CmbActionType_SelectedIndexChanged;
            // 
            // pnlClick
            // 
            pnlClick.BorderStyle = BorderStyle.FixedSingle;
            pnlClick.Controls.Add(lblClickX);
            pnlClick.Controls.Add(numClickX);
            pnlClick.Controls.Add(lblClickY);
            pnlClick.Controls.Add(numClickY);
            pnlClick.Controls.Add(btnCaptureMousePos);
            pnlClick.Controls.Add(lblMouseBtn);
            pnlClick.Controls.Add(cmbMouseButton);
            pnlClick.Controls.Add(lblClickType);
            pnlClick.Controls.Add(cmbClickType);
            pnlClick.Controls.Add(lblMousePosInfo);
            pnlClick.Location = new Point(20, 160);
            pnlClick.Name = "pnlClick";
            pnlClick.Size = new Size(450, 165);
            pnlClick.TabIndex = 7;
            pnlClick.Visible = false;
            // 
            // lblClickX
            // 
            lblClickX.AutoSize = true;
            lblClickX.Location = new Point(10, 10);
            lblClickX.Name = "lblClickX";
            lblClickX.Size = new Size(17, 15);
            lblClickX.TabIndex = 0;
            lblClickX.Text = "X:";
            // 
            // numClickX
            // 
            numClickX.Location = new Point(40, 8);
            numClickX.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numClickX.Name = "numClickX";
            numClickX.Size = new Size(80, 23);
            numClickX.TabIndex = 1;
            // 
            // lblClickY
            // 
            lblClickY.AutoSize = true;
            lblClickY.Location = new Point(130, 10);
            lblClickY.Name = "lblClickY";
            lblClickY.Size = new Size(17, 15);
            lblClickY.TabIndex = 2;
            lblClickY.Text = "Y:";
            // 
            // numClickY
            // 
            numClickY.Location = new Point(160, 8);
            numClickY.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numClickY.Name = "numClickY";
            numClickY.Size = new Size(80, 23);
            numClickY.TabIndex = 3;
            // 
            // btnCaptureMousePos
            // 
            btnCaptureMousePos.Location = new Point(250, 6);
            btnCaptureMousePos.Name = "btnCaptureMousePos";
            btnCaptureMousePos.Size = new Size(180, 25);
            btnCaptureMousePos.TabIndex = 4;
            btnCaptureMousePos.Text = "Mouse Pos. Öğren (5sn)";
            btnCaptureMousePos.Click += BtnCaptureMousePos_Click;
            // 
            // lblMouseBtn
            // 
            lblMouseBtn.AutoSize = true;
            lblMouseBtn.Location = new Point(10, 45);
            lblMouseBtn.Name = "lblMouseBtn";
            lblMouseBtn.Size = new Size(74, 15);
            lblMouseBtn.TabIndex = 5;
            lblMouseBtn.Text = "Mouse Tuşu:";
            // 
            // cmbMouseButton
            // 
            cmbMouseButton.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMouseButton.Items.AddRange(new object[] { "Sol", "Sag" });
            cmbMouseButton.Location = new Point(100, 43);
            cmbMouseButton.Name = "cmbMouseButton";
            cmbMouseButton.Size = new Size(99, 23);
            cmbMouseButton.TabIndex = 6;
            // 
            // lblClickType
            // 
            lblClickType.AutoSize = true;
            lblClickType.Location = new Point(10, 75);
            lblClickType.Name = "lblClickType";
            lblClickType.Size = new Size(58, 15);
            lblClickType.TabIndex = 7;
            lblClickType.Text = "Click Tipi:";
            // 
            // cmbClickType
            // 
            cmbClickType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClickType.Items.AddRange(new object[] { "Tek", "Cift" });
            cmbClickType.Location = new Point(100, 73);
            cmbClickType.Name = "cmbClickType";
            cmbClickType.Size = new Size(100, 23);
            cmbClickType.TabIndex = 8;
            // 
            // lblMousePosInfo
            // 
            lblMousePosInfo.ForeColor = Color.Gray;
            lblMousePosInfo.Location = new Point(10, 105);
            lblMousePosInfo.Name = "lblMousePosInfo";
            lblMousePosInfo.Size = new Size(430, 46);
            lblMousePosInfo.TabIndex = 9;
            lblMousePosInfo.Text = "Mouse pozisyonunu öğrenmek için butona tıklayın\nve 5 saniye içinde istediğiniz noktaya gidin.";
            // 
            // pnlText
            // 
            pnlText.BorderStyle = BorderStyle.FixedSingle;
            pnlText.Controls.Add(lblText);
            pnlText.Controls.Add(txtText);
            pnlText.Location = new Point(20, 331);
            pnlText.Name = "pnlText";
            pnlText.Size = new Size(450, 100);
            pnlText.TabIndex = 8;
            pnlText.Visible = false;
            // 
            // lblText
            // 
            lblText.AutoSize = true;
            lblText.Location = new Point(10, 10);
            lblText.Name = "lblText";
            lblText.Size = new Size(91, 15);
            lblText.TabIndex = 0;
            lblText.Text = "Yazılacak Metin:";
            // 
            // txtText
            // 
            txtText.Location = new Point(10, 35);
            txtText.Multiline = true;
            txtText.Name = "txtText";
            txtText.Size = new Size(430, 50);
            txtText.TabIndex = 1;
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.Green;
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnOk.ForeColor = Color.White;
            btnOk.Location = new Point(280, 450);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(90, 35);
            btnOk.TabIndex = 9;
            btnOk.Text = "Tamam";
            btnOk.UseVisualStyleBackColor = false;
            btnOk.Click += BtnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.IndianRed;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(380, 450);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 35);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "İptal";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // ActionEditorForm
            // 
            AcceptButton = btnOk;
            CancelButton = btnCancel;
            ClientSize = new Size(484, 491);
            Controls.Add(lblHotkey);
            Controls.Add(cmbHotkey);
            Controls.Add(grpModifiers);
            Controls.Add(lblWaitTime);
            Controls.Add(numWaitTime);
            Controls.Add(lblActionType);
            Controls.Add(cmbActionType);
            Controls.Add(pnlClick);
            Controls.Add(pnlText);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ActionEditorForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Makro Action Ekle/Düzenle";
            grpModifiers.ResumeLayout(false);
            grpModifiers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numWaitTime).EndInit();
            pnlClick.ResumeLayout(false);
            pnlClick.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numClickX).EndInit();
            ((System.ComponentModel.ISupportInitialize)numClickY).EndInit();
            pnlText.ResumeLayout(false);
            pnlText.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion
        private ComboBox cmbHotkey;
        private CheckBox chkCtrl;
        private CheckBox chkAlt;
        private CheckBox chkShift;
        private NumericUpDown numWaitTime;
        private ComboBox cmbActionType;
        private Panel pnlClick;
        private NumericUpDown numClickX;
        private NumericUpDown numClickY;
        private Button btnCaptureMousePos;
        private ComboBox cmbMouseButton;
        private ComboBox cmbClickType;
        private Label lblMousePosInfo;
        private Panel pnlText;
        private TextBox txtText;
        private Button btnOk;
        private Button btnCancel;


        private Label lblHotkey;
        private GroupBox grpModifiers;
        private Label lblModifierExample;
        private Label lblWaitTime;
        private Label lblActionType;
        private Label lblClickX;
        private Label lblClickY;
        private Label lblMouseBtn;
        private Label lblClickType;
        private Label lblText;


    }
}