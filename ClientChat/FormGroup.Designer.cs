
namespace ClientChat
{
    partial class FormGroup
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2ControlBox2 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.flpListClient = new System.Windows.Forms.FlowLayoutPanel();
            this.nameGroup = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnConnectServer = new Guna.UI2.WinForms.Guna2GradientTileButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(132)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.guna2ControlBox2);
            this.panel1.Controls.Add(this.guna2ControlBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 40);
            this.panel1.TabIndex = 0;
            // 
            // guna2ControlBox2
            // 
            this.guna2ControlBox2.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.guna2ControlBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.guna2ControlBox2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(132)))), ((int)(((byte)(255)))));
            this.guna2ControlBox2.HoverState.Parent = this.guna2ControlBox2;
            this.guna2ControlBox2.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox2.Location = new System.Drawing.Point(160, 0);
            this.guna2ControlBox2.Name = "guna2ControlBox2";
            this.guna2ControlBox2.ShadowDecoration.Parent = this.guna2ControlBox2;
            this.guna2ControlBox2.Size = new System.Drawing.Size(40, 40);
            this.guna2ControlBox2.TabIndex = 3;
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.guna2ControlBox1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(132)))), ((int)(((byte)(255)))));
            this.guna2ControlBox1.HoverState.Parent = this.guna2ControlBox1;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(200, 0);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.ShadowDecoration.Parent = this.guna2ControlBox1;
            this.guna2ControlBox1.Size = new System.Drawing.Size(40, 40);
            this.guna2ControlBox1.TabIndex = 2;
            // 
            // guna2DragControl1
            // 
            this.guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2DragControl1.TargetControl = this.panel1;
            this.guna2DragControl1.UseTransparentDrag = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(49, 9);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 21);
            this.label5.TabIndex = 8;
            this.label5.Text = "Group";
            // 
            // flpListClient
            // 
            this.flpListClient.AutoScroll = true;
            this.flpListClient.BackColor = System.Drawing.SystemColors.Control;
            this.flpListClient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpListClient.Location = new System.Drawing.Point(12, 106);
            this.flpListClient.Name = "flpListClient";
            this.flpListClient.Size = new System.Drawing.Size(209, 273);
            this.flpListClient.TabIndex = 12;
            // 
            // nameGroup
            // 
            this.nameGroup.BorderColor = System.Drawing.Color.Black;
            this.nameGroup.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nameGroup.DefaultText = "";
            this.nameGroup.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.nameGroup.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.nameGroup.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.nameGroup.DisabledState.Parent = this.nameGroup;
            this.nameGroup.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.nameGroup.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.nameGroup.FocusedState.Parent = this.nameGroup;
            this.nameGroup.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.nameGroup.ForeColor = System.Drawing.Color.Black;
            this.nameGroup.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.nameGroup.HoverState.Parent = this.nameGroup;
            this.nameGroup.Location = new System.Drawing.Point(12, 63);
            this.nameGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nameGroup.Name = "nameGroup";
            this.nameGroup.PasswordChar = '\0';
            this.nameGroup.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.nameGroup.PlaceholderText = "Name group";
            this.nameGroup.SelectedText = "";
            this.nameGroup.ShadowDecoration.Parent = this.nameGroup;
            this.nameGroup.Size = new System.Drawing.Size(209, 36);
            this.nameGroup.TabIndex = 13;
            // 
            // btnConnectServer
            // 
            this.btnConnectServer.BorderRadius = 11;
            this.btnConnectServer.CheckedState.Parent = this.btnConnectServer;
            this.btnConnectServer.CustomImages.Parent = this.btnConnectServer;
            this.btnConnectServer.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnConnectServer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnConnectServer.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnConnectServer.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnConnectServer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnConnectServer.DisabledState.Parent = this.btnConnectServer;
            this.btnConnectServer.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(132)))), ((int)(((byte)(255)))));
            this.btnConnectServer.FillColor2 = System.Drawing.Color.Cyan;
            this.btnConnectServer.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnConnectServer.ForeColor = System.Drawing.Color.White;
            this.btnConnectServer.HoverState.Parent = this.btnConnectServer;
            this.btnConnectServer.Location = new System.Drawing.Point(149, 385);
            this.btnConnectServer.Name = "btnConnectServer";
            this.btnConnectServer.ShadowDecoration.Parent = this.btnConnectServer;
            this.btnConnectServer.Size = new System.Drawing.Size(72, 37);
            this.btnConnectServer.TabIndex = 14;
            this.btnConnectServer.Text = "Create";
            this.btnConnectServer.Click += new System.EventHandler(this.btnConnectServer_Click);
            // 
            // panel5
            // 
            this.panel5.BackgroundImage = global::ClientChat.Properties.Resources.create_group_button1;
            this.panel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel5.Location = new System.Drawing.Point(14, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(30, 29);
            this.panel5.TabIndex = 9;
            // 
            // FormGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(240, 434);
            this.Controls.Add(this.btnConnectServer);
            this.Controls.Add(this.nameGroup);
            this.Controls.Add(this.flpListClient);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormGroup";
            this.Text = "FormGroup";
            this.Load += new System.EventHandler(this.FormGroup_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox2;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.FlowLayoutPanel flpListClient;
        private Guna.UI2.WinForms.Guna2TextBox nameGroup;
        private Guna.UI2.WinForms.Guna2GradientTileButton btnConnectServer;
    }
}