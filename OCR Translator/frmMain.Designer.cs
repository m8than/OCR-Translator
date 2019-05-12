namespace OCR_Translator
{
    partial class frmMain
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
            this.btnSelectScreen = new System.Windows.Forms.Button();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.lblTextColour_ = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblEnd_ = new System.Windows.Forms.Label();
            this.lblStart_ = new System.Windows.Forms.Label();
            this.btnTextColour = new System.Windows.Forms.Button();
            this.grpSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectScreen
            // 
            this.btnSelectScreen.Location = new System.Drawing.Point(119, 72);
            this.btnSelectScreen.Name = "btnSelectScreen";
            this.btnSelectScreen.Size = new System.Drawing.Size(75, 23);
            this.btnSelectScreen.TabIndex = 0;
            this.btnSelectScreen.Text = "Select Area";
            this.btnSelectScreen.UseVisualStyleBackColor = true;
            this.btnSelectScreen.Click += new System.EventHandler(this.btnSelectScreen_Click);
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.btnTextColour);
            this.grpSettings.Controls.Add(this.lblTextColour_);
            this.grpSettings.Controls.Add(this.btnStartStop);
            this.grpSettings.Controls.Add(this.lblEnd);
            this.grpSettings.Controls.Add(this.lblStart);
            this.grpSettings.Controls.Add(this.lblEnd_);
            this.grpSettings.Controls.Add(this.lblStart_);
            this.grpSettings.Controls.Add(this.btnSelectScreen);
            this.grpSettings.Location = new System.Drawing.Point(12, 12);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(200, 101);
            this.grpSettings.TabIndex = 1;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Controls";
            // 
            // lblTextColour_
            // 
            this.lblTextColour_.AutoSize = true;
            this.lblTextColour_.Location = new System.Drawing.Point(6, 53);
            this.lblTextColour_.Name = "lblTextColour_";
            this.lblTextColour_.Size = new System.Drawing.Size(64, 13);
            this.lblTextColour_.TabIndex = 6;
            this.lblTextColour_.Text = "Text Colour:";
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(6, 72);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 5;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.BtnStartStop_Click);
            // 
            // lblEnd
            // 
            this.lblEnd.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblEnd.Location = new System.Drawing.Point(107, 33);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(87, 13);
            this.lblEnd.TabIndex = 4;
            this.lblEnd.Text = "-, -";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStart
            // 
            this.lblStart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStart.Location = new System.Drawing.Point(110, 20);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(84, 13);
            this.lblStart.TabIndex = 3;
            this.lblStart.Text = "-, -";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEnd_
            // 
            this.lblEnd_.AutoSize = true;
            this.lblEnd_.Location = new System.Drawing.Point(6, 33);
            this.lblEnd_.Name = "lblEnd_";
            this.lblEnd_.Size = new System.Drawing.Size(69, 13);
            this.lblEnd_.TabIndex = 2;
            this.lblEnd_.Text = "End Position:";
            this.lblEnd_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStart_
            // 
            this.lblStart_.AutoSize = true;
            this.lblStart_.Location = new System.Drawing.Point(6, 20);
            this.lblStart_.Name = "lblStart_";
            this.lblStart_.Size = new System.Drawing.Size(75, 13);
            this.lblStart_.TabIndex = 1;
            this.lblStart_.Text = "Start Position: ";
            this.lblStart_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTextColour
            // 
            this.btnTextColour.Location = new System.Drawing.Point(175, 51);
            this.btnTextColour.Name = "btnTextColour";
            this.btnTextColour.Size = new System.Drawing.Size(19, 17);
            this.btnTextColour.TabIndex = 7;
            this.btnTextColour.UseVisualStyleBackColor = true;
            this.btnTextColour.Click += new System.EventHandler(this.BtnTextColour_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 125);
            this.Controls.Add(this.grpSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMain";
            this.Text = "OCR Translator - by github.com/m8than";
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSelectScreen;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.Label lblStart_;
        private System.Windows.Forms.Label lblEnd_;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Label lblTextColour_;
        private System.Windows.Forms.Button btnTextColour;
    }
}

