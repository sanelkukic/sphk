using System.ComponentModel;
using System.Windows.Forms;

namespace sphk_gui
{
    partial class SpamForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpamForm));
            this.commandlineOutputBox = new System.Windows.Forms.RichTextBox();
            this.closeWindowButton = new System.Windows.Forms.Button();
            this.openConfigDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // commandlineOutputBox
            // 
            this.commandlineOutputBox.Location = new System.Drawing.Point(12, 12);
            this.commandlineOutputBox.Name = "commandlineOutputBox";
            this.commandlineOutputBox.ReadOnly = true;
            this.commandlineOutputBox.Size = new System.Drawing.Size(776, 388);
            this.commandlineOutputBox.TabIndex = 0;
            this.commandlineOutputBox.Text = "";
            // 
            // closeWindowButton
            // 
            this.closeWindowButton.Location = new System.Drawing.Point(12, 409);
            this.closeWindowButton.Name = "closeWindowButton";
            this.closeWindowButton.Size = new System.Drawing.Size(776, 29);
            this.closeWindowButton.TabIndex = 1;
            this.closeWindowButton.Text = "Close";
            this.closeWindowButton.UseVisualStyleBackColor = true;
            this.closeWindowButton.Click += new System.EventHandler(this.closeWindowButton_Click);
            // 
            // openConfigDialog
            // 
            this.openConfigDialog.DefaultExt = "json";
            this.openConfigDialog.DereferenceLinks = false;
            this.openConfigDialog.Filter = "JSON files|*.json|All files|*.*";
            this.openConfigDialog.Title = "Open config file | sphk GUI";
            // 
            // SpamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.closeWindowButton);
            this.Controls.Add(this.commandlineOutputBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.Name = "SpamForm";
            this.Text = "Output";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpamForm_FormClosing);
            this.Load += new System.EventHandler(this.SpamForm_Load);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button closeWindowButton;
        private System.Windows.Forms.RichTextBox commandlineOutputBox;
        private System.Windows.Forms.OpenFileDialog openConfigDialog;

        #endregion
    }
}