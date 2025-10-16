namespace StickyLite
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.txtNote = new System.Windows.Forms.RichTextBox();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuFontIncrease = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFontDecrease = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuOpacity = new System.Windows.Forms.ToolStripMenuItem();
            this.trackOpacity = new System.Windows.Forms.TrackBar();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuBold = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItalic = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUnderline = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1_2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuColorChange = new System.Windows.Forms.ToolStripMenuItem();
            this.menuColorPresets = new System.Windows.Forms.ToolStripMenuItem();
            this.menuColorCustom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTopMost = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.autoSaveTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.btnPin = new System.Windows.Forms.Button();
            this.btnNewNote = new System.Windows.Forms.Button();
            this.btnNoteList = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.opacitySlider = new System.Windows.Forms.TrackBar();
            this.lblTitle = new System.Windows.Forms.Label();
            this.contextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackOpacity)).BeginInit();
            this.pnlTitleBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacitySlider)).BeginInit();
            this.SuspendLayout();
            // 
            // txtNote
            // 
            this.txtNote.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(224)))));
            this.txtNote.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNote.ContextMenuStrip = this.contextMenu;
            this.txtNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNote.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNote.ForeColor = System.Drawing.Color.Black;
            this.txtNote.Location = new System.Drawing.Point(0, 0);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.txtNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtNote.DetectUrls = false;
            this.txtNote.EnableAutoDragDrop = false;
            this.txtNote.Size = new System.Drawing.Size(300, 200);
            this.txtNote.TabIndex = 0;
            this.txtNote.TextChanged += new System.EventHandler(this.txtNote_TextChanged);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFontIncrease,
            this.menuFontDecrease,
            this.toolStripSeparator1,
            this.menuOpacity,
            this.toolStripSeparator2,
            this.menuBold,
            this.menuItalic,
            this.menuUnderline,
            this.toolStripSeparator1_2,
            this.menuColorChange,
            this.toolStripSeparator3,
            this.menuTopMost,
            this.menuSave,
            this.menuExit});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(181, 236);
            // 
            // menuFontIncrease
            // 
            this.menuFontIncrease.Name = "menuFontIncrease";
            this.menuFontIncrease.Size = new System.Drawing.Size(180, 22);
            this.menuFontIncrease.Text = "폰트 크기 증가 (+)";
            this.menuFontIncrease.Click += new System.EventHandler(this.menuFontIncrease_Click);
            // 
            // menuFontDecrease
            // 
            this.menuFontDecrease.Name = "menuFontDecrease";
            this.menuFontDecrease.Size = new System.Drawing.Size(180, 22);
            this.menuFontDecrease.Text = "폰트 크기 감소 (-)";
            this.menuFontDecrease.Click += new System.EventHandler(this.menuFontDecrease_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // menuOpacity
            // 
            this.menuOpacity.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            new System.Windows.Forms.ToolStripControlHost(this.trackOpacity)});
            this.menuOpacity.Name = "menuOpacity";
            this.menuOpacity.Size = new System.Drawing.Size(180, 22);
            this.menuOpacity.Text = "투명도";
            // 
            // trackOpacity
            // 
            this.trackOpacity.AutoSize = false;
            this.trackOpacity.LargeChange = 10;
            this.trackOpacity.Location = new System.Drawing.Point(0, 0);
            this.trackOpacity.Maximum = 100;
            this.trackOpacity.Minimum = 70;
            this.trackOpacity.Name = "trackOpacity";
            this.trackOpacity.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.trackOpacity.Size = new System.Drawing.Size(150, 45);
            this.trackOpacity.SmallChange = 5;
            this.trackOpacity.TabIndex = 0;
            this.trackOpacity.TickFrequency = 10;
            this.trackOpacity.Value = 90;
            this.trackOpacity.ValueChanged += new System.EventHandler(this.trackOpacity_ValueChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // menuBold
            // 
            this.menuBold.Name = "menuBold";
            this.menuBold.Size = new System.Drawing.Size(180, 22);
            this.menuBold.Text = "굵게 (Ctrl+B)";
            this.menuBold.Click += new System.EventHandler(this.menuBold_Click);
            // 
            // menuItalic
            // 
            this.menuItalic.Name = "menuItalic";
            this.menuItalic.Size = new System.Drawing.Size(180, 22);
            this.menuItalic.Text = "기울임 (Ctrl+I)";
            this.menuItalic.Click += new System.EventHandler(this.menuItalic_Click);
            // 
            // menuUnderline
            // 
            this.menuUnderline.Name = "menuUnderline";
            this.menuUnderline.Size = new System.Drawing.Size(180, 22);
            this.menuUnderline.Text = "밑줄 (Ctrl+U)";
            this.menuUnderline.Click += new System.EventHandler(this.menuUnderline_Click);
            // 
            // toolStripSeparator1_2
            // 
            this.toolStripSeparator1_2.Name = "toolStripSeparator1_2";
            this.toolStripSeparator1_2.Size = new System.Drawing.Size(177, 6);
            // 
            // menuColorChange
            // 
            this.menuColorChange.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuColorPresets,
            this.menuColorCustom});
            this.menuColorChange.Name = "menuColorChange";
            this.menuColorChange.Size = new System.Drawing.Size(180, 22);
            this.menuColorChange.Text = "색상 변경 ▶";
            // 
            // menuColorPresets
            // 
            this.menuColorPresets.Name = "menuColorPresets";
            this.menuColorPresets.Size = new System.Drawing.Size(180, 22);
            this.menuColorPresets.Text = "프리셋 색상";
            this.menuColorPresets.Click += new System.EventHandler(this.menuColorPresets_Click);
            // 
            // menuColorCustom
            // 
            this.menuColorCustom.Name = "menuColorCustom";
            this.menuColorCustom.Size = new System.Drawing.Size(180, 22);
            this.menuColorCustom.Text = "🎨 커스텀 색상...";
            this.menuColorCustom.Click += new System.EventHandler(this.menuColorCustom_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // menuTopMost
            // 
            this.menuTopMost.Checked = true;
            this.menuTopMost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuTopMost.Name = "menuTopMost";
            this.menuTopMost.Size = new System.Drawing.Size(180, 22);
            this.menuTopMost.Text = "항상 위 (Ctrl+T)";
            this.menuTopMost.Click += new System.EventHandler(this.menuTopMost_Click);
            // 
            // menuSave
            // 
            this.menuSave.Name = "menuSave";
            this.menuSave.Size = new System.Drawing.Size(180, 22);
            this.menuSave.Text = "저장 (Ctrl+S)";
            this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(180, 22);
            this.menuExit.Text = "종료 (Ctrl+Q)";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // autoSaveTimer
            // 
            this.autoSaveTimer.Interval = 1200;
            this.autoSaveTimer.Tick += new System.EventHandler(this.autoSaveTimer_Tick);
            // 
            // pnlTitleBar
            // 
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(224)))));
            this.pnlTitleBar.Controls.Add(this.lblTitle);
            this.pnlTitleBar.Controls.Add(this.btnPin);
            this.pnlTitleBar.Controls.Add(this.btnNewNote);
            this.pnlTitleBar.Controls.Add(this.btnNoteList);
            this.pnlTitleBar.Controls.Add(this.opacitySlider);
            this.pnlTitleBar.Controls.Add(this.btnClose);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Height = 35;
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(300, 35);
            this.pnlTitleBar.TabIndex = 1;
            this.pnlTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlTitleBar_MouseDown);
            // 
            // btnPin
            // 
            this.btnPin.BackColor = System.Drawing.Color.Transparent;
            this.btnPin.FlatAppearance.BorderSize = 0;
            this.btnPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPin.Font = new System.Drawing.Font("Segoe UI Emoji", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnPin.Location = new System.Drawing.Point(5, 5);
            this.btnPin.Name = "btnPin";
            this.btnPin.Size = new System.Drawing.Size(24, 24);
            this.btnPin.TabIndex = 0;
            this.btnPin.Text = "📌";
            this.btnPin.UseVisualStyleBackColor = false;
            this.btnPin.Click += new System.EventHandler(this.btnPin_Click);
            // 
            // btnNewNote
            // 
            this.btnNewNote.BackColor = System.Drawing.Color.Transparent;
            this.btnNewNote.FlatAppearance.BorderSize = 0;
            this.btnNewNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewNote.Font = new System.Drawing.Font("Segoe UI Emoji", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNewNote.Location = new System.Drawing.Point(35, 5);
            this.btnNewNote.Name = "btnNewNote";
            this.btnNewNote.Size = new System.Drawing.Size(24, 24);
            this.btnNewNote.TabIndex = 1;
            this.btnNewNote.Text = "➕";
            this.btnNewNote.UseVisualStyleBackColor = false;
            this.btnNewNote.Click += new System.EventHandler(this.btnNewNote_Click);
            // 
            // btnNoteList
            // 
            this.btnNoteList.BackColor = System.Drawing.Color.Transparent;
            this.btnNoteList.FlatAppearance.BorderSize = 0;
            this.btnNoteList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNoteList.Font = new System.Drawing.Font("Segoe UI Emoji", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNoteList.Location = new System.Drawing.Point(65, 5);
            this.btnNoteList.Name = "btnNoteList";
            this.btnNoteList.Size = new System.Drawing.Size(24, 24);
            this.btnNoteList.TabIndex = 2;
            this.btnNoteList.Text = "📋";
            this.btnNoteList.UseVisualStyleBackColor = false;
            this.btnNoteList.Click += new System.EventHandler(this.btnNoteList_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Emoji", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnClose.Location = new System.Drawing.Point(271, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(24, 24);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "✕";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(100, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(60, 15);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "StickyLite";
            this.lblTitle.DoubleClick += new System.EventHandler(this.lblTitle_DoubleClick);
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseDown);
            // 
            // opacitySlider
            // 
            this.opacitySlider.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opacitySlider.AutoSize = false;
            this.opacitySlider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(224)))));
            this.opacitySlider.Location = new System.Drawing.Point(200, 8);
            this.opacitySlider.Maximum = 100;
            this.opacitySlider.Minimum = 70;
            this.opacitySlider.Name = "opacitySlider";
            this.opacitySlider.Size = new System.Drawing.Size(60, 20);
            this.opacitySlider.TabIndex = 0;
            this.opacitySlider.TickFrequency = 10;
            this.opacitySlider.Value = 90;
            this.opacitySlider.ValueChanged += new System.EventHandler(this.opacitySlider_ValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.pnlTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 150);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "StickyLite";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.contextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackOpacity)).EndInit();
            this.pnlTitleBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.opacitySlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtNote;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuFontIncrease;
        private System.Windows.Forms.ToolStripMenuItem menuFontDecrease;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuOpacity;
        private System.Windows.Forms.TrackBar trackOpacity;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuBold;
        private System.Windows.Forms.ToolStripMenuItem menuItalic;
        private System.Windows.Forms.ToolStripMenuItem menuUnderline;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1_2;
        private System.Windows.Forms.ToolStripMenuItem menuColorChange;
        private System.Windows.Forms.ToolStripMenuItem menuColorPresets;
        private System.Windows.Forms.ToolStripMenuItem menuColorCustom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuTopMost;
        private System.Windows.Forms.ToolStripMenuItem menuSave;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.Timer autoSaveTimer;
        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Button btnPin;
        private System.Windows.Forms.Button btnNewNote;
        private System.Windows.Forms.Button btnNoteList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TrackBar opacitySlider;
        private System.Windows.Forms.Label lblTitle;
    }
}
