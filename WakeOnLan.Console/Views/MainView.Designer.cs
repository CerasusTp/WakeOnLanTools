namespace WakeOnLan.Console.Views
{
    partial class MainView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
            btnLocal = new Controls.CustomButton();
            btnRemote = new Controls.CustomButton();
            grpCommon = new GroupBox();
            btnHostName = new Button();
            txtSubnetMask = new TextBox();
            label5 = new Label();
            cmbConnection = new ComboBox();
            label4 = new Label();
            txtHostName = new TextBox();
            label3 = new Label();
            txtIPAddress = new TextBox();
            label2 = new Label();
            txtMacAddress = new TextBox();
            label1 = new Label();
            btnBoot = new Button();
            MainViewError = new ErrorProvider(components);
            grpCommon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainViewError).BeginInit();
            SuspendLayout();
            // 
            // btnLocal
            // 
            btnLocal.BackColor = Color.PaleGreen;
            btnLocal.Clicked = false;
            btnLocal.Location = new Point(12, 12);
            btnLocal.Name = "btnLocal";
            btnLocal.Size = new Size(100, 40);
            btnLocal.TabIndex = 3;
            btnLocal.Text = "ローカル";
            btnLocal.UseVisualStyleBackColor = true;
            btnLocal.Click += SwitchModeButton;
            // 
            // btnRemote
            // 
            btnRemote.BackColor = Color.PaleGreen;
            btnRemote.Clicked = false;
            btnRemote.Location = new Point(118, 12);
            btnRemote.Name = "btnRemote";
            btnRemote.Size = new Size(100, 40);
            btnRemote.TabIndex = 4;
            btnRemote.Text = "リモート";
            btnRemote.UseVisualStyleBackColor = true;
            btnRemote.Click += SwitchModeButton;
            // 
            // grpCommon
            // 
            grpCommon.Controls.Add(btnHostName);
            grpCommon.Controls.Add(txtSubnetMask);
            grpCommon.Controls.Add(label5);
            grpCommon.Controls.Add(cmbConnection);
            grpCommon.Controls.Add(label4);
            grpCommon.Controls.Add(txtHostName);
            grpCommon.Controls.Add(label3);
            grpCommon.Controls.Add(txtIPAddress);
            grpCommon.Controls.Add(label2);
            grpCommon.Controls.Add(txtMacAddress);
            grpCommon.Controls.Add(label1);
            grpCommon.Location = new Point(12, 58);
            grpCommon.Name = "grpCommon";
            grpCommon.Size = new Size(486, 180);
            grpCommon.TabIndex = 5;
            grpCommon.TabStop = false;
            grpCommon.Text = "宛先";
            // 
            // btnHostName
            // 
            btnHostName.Location = new Point(405, 132);
            btnHostName.Name = "btnHostName";
            btnHostName.Size = new Size(55, 29);
            btnHostName.TabIndex = 10;
            btnHostName.Text = "取得";
            btnHostName.UseVisualStyleBackColor = true;
            btnHostName.Click += GetIPAddressButton;
            // 
            // txtSubnetMask
            // 
            txtSubnetMask.Location = new Point(405, 97);
            txtSubnetMask.Name = "txtSubnetMask";
            txtSubnetMask.Size = new Size(55, 29);
            txtSubnetMask.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(383, 100);
            label5.Name = "label5";
            label5.Size = new Size(16, 21);
            label5.TabIndex = 8;
            label5.Text = "/";
            // 
            // cmbConnection
            // 
            cmbConnection.FormattingEnabled = true;
            cmbConnection.Location = new Point(120, 62);
            cmbConnection.Name = "cmbConnection";
            cmbConnection.Size = new Size(178, 29);
            cmbConnection.TabIndex = 7;
            cmbConnection.SelectionChangeCommitted += ChangeConnectionCombobox;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 65);
            label4.Name = "label4";
            label4.Size = new Size(79, 21);
            label4.TabIndex = 6;
            label4.Text = "ネットワーク";
            // 
            // txtHostName
            // 
            txtHostName.Location = new Point(120, 132);
            txtHostName.Name = "txtHostName";
            txtHostName.Size = new Size(257, 29);
            txtHostName.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 135);
            label3.Name = "label3";
            label3.Size = new Size(62, 21);
            label3.TabIndex = 4;
            label3.Text = "ホスト名";
            // 
            // txtIPAddress
            // 
            txtIPAddress.Location = new Point(120, 97);
            txtIPAddress.Name = "txtIPAddress";
            txtIPAddress.Size = new Size(257, 29);
            txtIPAddress.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 100);
            label2.Name = "label2";
            label2.Size = new Size(40, 21);
            label2.TabIndex = 2;
            label2.Text = "IPv4";
            // 
            // txtMacAddress
            // 
            txtMacAddress.Location = new Point(120, 27);
            txtMacAddress.Name = "txtMacAddress";
            txtMacAddress.Size = new Size(178, 29);
            txtMacAddress.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 30);
            label1.Name = "label1";
            label1.Size = new Size(90, 21);
            label1.TabIndex = 0;
            label1.Text = "MACアドレス";
            // 
            // btnBoot
            // 
            btnBoot.Location = new Point(358, 12);
            btnBoot.Name = "btnBoot";
            btnBoot.Size = new Size(140, 40);
            btnBoot.TabIndex = 8;
            btnBoot.Text = "起動";
            btnBoot.UseVisualStyleBackColor = true;
            btnBoot.Click += ClickBootButton;
            // 
            // MainViewError
            // 
            MainViewError.ContainerControl = this;
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(514, 251);
            Controls.Add(btnBoot);
            Controls.Add(grpCommon);
            Controls.Add(btnRemote);
            Controls.Add(btnLocal);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            Name = "MainView";
            Text = "WakeOnLan Console";
            grpCommon.ResumeLayout(false);
            grpCommon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MainViewError).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Controls.CustomButton btnLocal;
        private Controls.CustomButton btnRemote;
        private GroupBox grpCommon;
        private Label label1;
        private TextBox txtMacAddress;
        private TextBox txtIPAddress;
        private Label label2;
        private TextBox txtHostName;
        private Label label3;
        private Button btnBoot;
        private ComboBox cmbConnection;
        private Label label4;
        private TextBox txtSubnetMask;
        private Label label5;
        private Button btnHostName;
        private ErrorProvider MainViewError;
    }
}
