namespace ImgurUploader
{
    partial class ProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.fileListProgressBar = new System.Windows.Forms.ProgressBar();
            this.uploadProgressBar = new System.Windows.Forms.ProgressBar();
            this.uploadName = new System.Windows.Forms.Label();
            this.uploadPercent = new System.Windows.Forms.Label();
            this.fileListName = new System.Windows.Forms.Label();
            this.fileListPercent = new System.Windows.Forms.Label();
            this.decorationLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.linkListLabel = new System.Windows.Forms.Label();
            this.linksPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.clipboardCopyLabel = new System.Windows.Forms.LinkLabel();
            this.formatCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // fileListProgressBar
            // 
            this.fileListProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fileListProgressBar.Location = new System.Drawing.Point(12, 92);
            this.fileListProgressBar.Name = "fileListProgressBar";
            this.fileListProgressBar.Size = new System.Drawing.Size(396, 23);
            this.fileListProgressBar.TabIndex = 0;
            // 
            // uploadProgressBar
            // 
            this.uploadProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.uploadProgressBar.Location = new System.Drawing.Point(12, 44);
            this.uploadProgressBar.Name = "uploadProgressBar";
            this.uploadProgressBar.Size = new System.Drawing.Size(396, 23);
            this.uploadProgressBar.TabIndex = 0;
            // 
            // uploadName
            // 
            this.uploadName.AutoSize = true;
            this.uploadName.Location = new System.Drawing.Point(12, 70);
            this.uploadName.Name = "uploadName";
            this.uploadName.Size = new System.Drawing.Size(159, 13);
            this.uploadName.TabIndex = 1;
            this.uploadName.Text = "Checking for validity / uploading";
            // 
            // uploadPercent
            // 
            this.uploadPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uploadPercent.Location = new System.Drawing.Point(345, 70);
            this.uploadPercent.Name = "uploadPercent";
            this.uploadPercent.Size = new System.Drawing.Size(63, 13);
            this.uploadPercent.TabIndex = 1;
            this.uploadPercent.Text = "1%";
            this.uploadPercent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // fileListName
            // 
            this.fileListName.AutoSize = true;
            this.fileListName.Location = new System.Drawing.Point(12, 118);
            this.fileListName.Name = "fileListName";
            this.fileListName.Size = new System.Drawing.Size(88, 13);
            this.fileListName.TabIndex = 1;
            this.fileListName.Text = "Doing Something";
            // 
            // fileListPercent
            // 
            this.fileListPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fileListPercent.Location = new System.Drawing.Point(333, 118);
            this.fileListPercent.Name = "fileListPercent";
            this.fileListPercent.Size = new System.Drawing.Size(75, 13);
            this.fileListPercent.TabIndex = 1;
            this.fileListPercent.Text = "27%";
            this.fileListPercent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // decorationLabel
            // 
            this.decorationLabel.AutoSize = true;
            this.decorationLabel.Location = new System.Drawing.Point(12, 9);
            this.decorationLabel.Name = "decorationLabel";
            this.decorationLabel.Size = new System.Drawing.Size(358, 13);
            this.decorationLabel.TabIndex = 1;
            this.decorationLabel.Text = "Thank you for using iamhrh\'s imgur uploader - and thanks MrGrim for imgur!";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(333, 141);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 500;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // linkListLabel
            // 
            this.linkListLabel.AutoSize = true;
            this.linkListLabel.Location = new System.Drawing.Point(9, 154);
            this.linkListLabel.Name = "linkListLabel";
            this.linkListLabel.Size = new System.Drawing.Size(41, 13);
            this.linkListLabel.TabIndex = 4;
            this.linkListLabel.Text = "So Far:";
            // 
            // linksPanel
            // 
            this.linksPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.linksPanel.AutoScroll = true;
            this.linksPanel.BackColor = System.Drawing.Color.White;
            this.linksPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.linksPanel.Location = new System.Drawing.Point(12, 170);
            this.linksPanel.Name = "linksPanel";
            this.linksPanel.Size = new System.Drawing.Size(396, 158);
            this.linksPanel.TabIndex = 5;
            // 
            // clipboardCopyLabel
            // 
            this.clipboardCopyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clipboardCopyLabel.Location = new System.Drawing.Point(183, 401);
            this.clipboardCopyLabel.Name = "clipboardCopyLabel";
            this.clipboardCopyLabel.Size = new System.Drawing.Size(225, 23);
            this.clipboardCopyLabel.TabIndex = 6;
            this.clipboardCopyLabel.TabStop = true;
            this.clipboardCopyLabel.Text = "copy upload information to clipboard";
            this.clipboardCopyLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.clipboardCopyLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.clipboardCopyLabel_LinkClicked);
            // 
            // formatCombo
            // 
            this.formatCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.formatCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatCombo.FormattingEnabled = true;
            this.formatCombo.Items.AddRange(new object[] {
            "Html List (Large Thumnails)",
            "Html List (Small Thumnails)",
            "Message Board List (BBCode) (Large Thumbs)",
            "Message Board List (BBCode) (Small Thumbs)",
            "Message Board Lines (BBCode) (Large Thumbs)",
            "Message Board Lines (BBCode) (Small Thumbs)",
            "Markdown List (Reddit friendly)",
            "Simple List (Imgur Pages)",
            "Simple List (Originals)",
            "Detail List"});
            this.formatCombo.Location = new System.Drawing.Point(175, 355);
            this.formatCombo.Name = "formatCombo";
            this.formatCombo.Size = new System.Drawing.Size(233, 21);
            this.formatCombo.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(322, 339);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "choose a format:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // linkCheck
            // 
            this.linkCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkCheck.AutoSize = true;
            this.linkCheck.Location = new System.Drawing.Point(318, 381);
            this.linkCheck.Name = "linkCheck";
            this.linkCheck.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.linkCheck.Size = new System.Drawing.Size(90, 17);
            this.linkCheck.TabIndex = 9;
            this.linkCheck.Text = "?include links";
            this.linkCheck.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkCheck.UseVisualStyleBackColor = true;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 433);
            this.Controls.Add(this.linkCheck);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.formatCombo);
            this.Controls.Add(this.clipboardCopyLabel);
            this.Controls.Add(this.linksPanel);
            this.Controls.Add(this.linkListLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.fileListPercent);
            this.Controls.Add(this.uploadPercent);
            this.Controls.Add(this.fileListName);
            this.Controls.Add(this.decorationLabel);
            this.Controls.Add(this.uploadName);
            this.Controls.Add(this.uploadProgressBar);
            this.Controls.Add(this.fileListProgressBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProgressForm";
            this.Text = "Imgur Upload Progress";
            this.Load += new System.EventHandler(this.ProgressForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar fileListProgressBar;
        private System.Windows.Forms.ProgressBar uploadProgressBar;
        private System.Windows.Forms.Label uploadName;
        private System.Windows.Forms.Label uploadPercent;
        private System.Windows.Forms.Label fileListName;
        private System.Windows.Forms.Label fileListPercent;
        private System.Windows.Forms.Label decorationLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Label linkListLabel;
        private System.Windows.Forms.FlowLayoutPanel linksPanel;
        private System.Windows.Forms.LinkLabel clipboardCopyLabel;
        private System.Windows.Forms.ComboBox formatCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox linkCheck;
    }
}