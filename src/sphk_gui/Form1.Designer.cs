namespace sphk_gui
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.generateConfigFileButton = new System.Windows.Forms.Button();
            this.startSpamButton = new System.Windows.Forms.Button();
            this.debugModeCheck = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveTemplateFile = new System.Windows.Forms.SaveFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // generateConfigFileButton
            // 
            this.generateConfigFileButton.Location = new System.Drawing.Point(74, 200);
            this.generateConfigFileButton.Name = "generateConfigFileButton";
            this.generateConfigFileButton.Size = new System.Drawing.Size(135, 66);
            this.generateConfigFileButton.TabIndex = 0;
            this.generateConfigFileButton.Text = "Generate config template file";
            this.generateConfigFileButton.UseVisualStyleBackColor = true;
            this.generateConfigFileButton.Click += new System.EventHandler(this.generateConfigFileButton_Click);
            // 
            // startSpamButton
            // 
            this.startSpamButton.Location = new System.Drawing.Point(74, 283);
            this.startSpamButton.Name = "startSpamButton";
            this.startSpamButton.Size = new System.Drawing.Size(135, 66);
            this.startSpamButton.TabIndex = 1;
            this.startSpamButton.Text = "Start Spamming";
            this.startSpamButton.UseVisualStyleBackColor = true;
            this.startSpamButton.Click += new System.EventHandler(this.startSpamButton_Click_1);
            // 
            // debugModeCheck
            // 
            this.debugModeCheck.Location = new System.Drawing.Point(308, 200);
            this.debugModeCheck.Name = "debugModeCheck";
            this.debugModeCheck.Size = new System.Drawing.Size(129, 23);
            this.debugModeCheck.TabIndex = 2;
            this.debugModeCheck.Text = "Debug Mode";
            this.debugModeCheck.UseVisualStyleBackColor = true;
            this.debugModeCheck.CheckedChanged += new System.EventHandler(this.debugModeCheck_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(504, 188);
            this.label1.TabIndex = 3;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // saveTemplateFile
            // 
            this.saveTemplateFile.DefaultExt = "json";
            this.saveTemplateFile.Filter = "JSON files|.json|All files|*.*";
            this.saveTemplateFile.Title = "Save config template file | sphk GUI";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(308, 250);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 99);
            this.label2.TabIndex = 4;
            this.label2.Text = "Copyright 2020 3reetop.\r\nLicensed under the terms of the MIT License.\r\n\r\nMade wit" + "h love.";
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(308, 349);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(202, 20);
            this.versionLabel.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 378);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.debugModeCheck);
            this.Controls.Add(this.startSpamButton);
            this.Controls.Add(this.generateConfigFileButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "sphk GUI";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.CheckBox debugModeCheck;
        private System.Windows.Forms.Button generateConfigFileButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SaveFileDialog saveTemplateFile;
        private System.Windows.Forms.Button startSpamButton;
        private System.Windows.Forms.Label versionLabel;

        #endregion
    }
}