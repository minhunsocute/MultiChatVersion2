
namespace ClientChat
{
    partial class ClientOnline
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbName = new System.Windows.Forms.Label();
            this.lbCount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.avtClient = new Guna.UI2.WinForms.Guna2PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.avtClient)).BeginInit();
            this.SuspendLayout();
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point(61, 16);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(81, 21);
            this.lbName.TabIndex = 3;
            this.lbName.Text = "Hung Mai";
            this.lbName.MouseEnter += new System.EventHandler(this.lbName_MouseEnter);
            this.lbName.MouseLeave += new System.EventHandler(this.lbName_MouseLeave);
            // 
            // lbCount
            // 
            this.lbCount.AutoSize = false;
            this.lbCount.BackColor = System.Drawing.Color.Red;
            this.lbCount.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCount.ForeColor = System.Drawing.Color.White;
            this.lbCount.Location = new System.Drawing.Point(177, 3);
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new System.Drawing.Size(17, 20);
            this.lbCount.TabIndex = 26;
            this.lbCount.Text = "1";
            this.lbCount.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel1.Location = new System.Drawing.Point(0, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(209, 1);
            this.panel1.TabIndex = 27;
            // 
            // avtClient
            // 
            this.avtClient.BorderRadius = 11;
            this.avtClient.ImageRotate = 0F;
            this.avtClient.Location = new System.Drawing.Point(9, 3);
            this.avtClient.Name = "avtClient";
            this.avtClient.ShadowDecoration.Parent = this.avtClient;
            this.avtClient.Size = new System.Drawing.Size(44, 41);
            this.avtClient.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.avtClient.TabIndex = 28;
            this.avtClient.TabStop = false;
            // 
            // ClientOnline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.avtClient);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbCount);
            this.Controls.Add(this.lbName);
            this.Name = "ClientOnline";
            this.Size = new System.Drawing.Size(199, 48);
            this.MouseEnter += new System.EventHandler(this.ClientOnline_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.ClientOnline_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.avtClient)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label lbName;
        public Guna.UI2.WinForms.Guna2HtmlLabel lbCount;
        private System.Windows.Forms.Panel panel1;
        public Guna.UI2.WinForms.Guna2PictureBox avtClient;
    }
}
