namespace X_Service.Login {
    partial class X_Form_Login {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing ) {
            if ( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( ) {
            this.TabC_Login = new System.Windows.Forms.TabControl ( );
            this.TabP_Login = new System.Windows.Forms.TabPage ( );
            this.groupBox_Login = new System.Windows.Forms.GroupBox ( );
            this.Text_PWD = new ComponentFactory.Krypton.Toolkit.KryptonTextBox ( );
            this.btnTxtPWD = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny ( );
            this.Text_PID = new ComponentFactory.Krypton.Toolkit.KryptonMaskedTextBox ( );
            this.btnTxtPid = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny ( );
            this.L_Pwd = new System.Windows.Forms.Label ( );
            this.L_Pid = new System.Windows.Forms.Label ( );
            this.TabP_Set = new System.Windows.Forms.TabPage ( );
            this.groupBox_Info = new System.Windows.Forms.GroupBox ( );
            this.x_Lable1 = new X_Service.Util.X_Lable ( );
            this.txtHardInfo = new System.Windows.Forms.TextBox ( );
            this.x_Lable2 = new X_Service.Util.X_Lable ( );
            this.ck提示更新 = new System.Windows.Forms.CheckBox ( );
            this.label7 = new System.Windows.Forms.Label ( );
            this.cVer = new System.Windows.Forms.Label ( );
            this.label3 = new System.Windows.Forms.Label ( );
            this.Ch_IsSave = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox ( );
            this.Pic_Login = new System.Windows.Forms.PictureBox ( );
            this.linkGetPs = new System.Windows.Forms.LinkLabel ( );
            this.btn_Login = new ComponentFactory.Krypton.Toolkit.KryptonButton ( );
            this.TabC_Login.SuspendLayout ( );
            this.TabP_Login.SuspendLayout ( );
            this.groupBox_Login.SuspendLayout ( );
            this.TabP_Set.SuspendLayout ( );
            this.groupBox_Info.SuspendLayout ( );
            ( ( System.ComponentModel.ISupportInitialize ) ( this.Pic_Login ) ).BeginInit ( );
            this.SuspendLayout ( );
            // 
            // TabC_Login
            // 
            this.TabC_Login.Controls.Add ( this.TabP_Login );
            this.TabC_Login.Controls.Add ( this.TabP_Set );
            this.TabC_Login.Location = new System.Drawing.Point ( 12, 56 );
            this.TabC_Login.Name = "TabC_Login";
            this.TabC_Login.SelectedIndex = 0;
            this.TabC_Login.Size = new System.Drawing.Size ( 357, 126 );
            this.TabC_Login.TabIndex = 2;
            this.TabC_Login.SelectedIndexChanged += new System.EventHandler ( this.TabC_Login_SelectedIndexChanged );
            // 
            // TabP_Login
            // 
            this.TabP_Login.BackColor = System.Drawing.SystemColors.Control;
            this.TabP_Login.Controls.Add ( this.groupBox_Login );
            this.TabP_Login.Location = new System.Drawing.Point ( 4, 21 );
            this.TabP_Login.Name = "TabP_Login";
            this.TabP_Login.Padding = new System.Windows.Forms.Padding ( 3 );
            this.TabP_Login.Size = new System.Drawing.Size ( 349, 101 );
            this.TabP_Login.TabIndex = 0;
            this.TabP_Login.Text = "登陆";
            // 
            // groupBox_Login
            // 
            this.groupBox_Login.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_Login.Controls.Add ( this.Text_PWD );
            this.groupBox_Login.Controls.Add ( this.Text_PID );
            this.groupBox_Login.Controls.Add ( this.L_Pwd );
            this.groupBox_Login.Controls.Add ( this.L_Pid );
            this.groupBox_Login.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Login.Location = new System.Drawing.Point ( 3, 3 );
            this.groupBox_Login.Name = "groupBox_Login";
            this.groupBox_Login.Size = new System.Drawing.Size ( 343, 91 );
            this.groupBox_Login.TabIndex = 5;
            this.groupBox_Login.TabStop = false;
            this.groupBox_Login.Text = "登录信息填写";
            // 
            // Text_PWD
            // 
            this.Text_PWD.ButtonSpecs.AddRange ( new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            this.btnTxtPWD} );
            this.Text_PWD.Location = new System.Drawing.Point ( 77, 54 );
            this.Text_PWD.Name = "Text_PWD";
            this.Text_PWD.PasswordChar = '*';
            this.Text_PWD.Size = new System.Drawing.Size ( 248, 21 );
            this.Text_PWD.TabIndex = 11;
            this.Text_PWD.UseSystemPasswordChar = true;
            // 
            // btnTxtPWD
            // 
            this.btnTxtPWD.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Close;
            this.btnTxtPWD.UniqueName = "83F268296CCD4CDD56A982B1E3D447CB";
            this.btnTxtPWD.Click += new System.EventHandler ( this.btnTxtPWD_Click );
            // 
            // Text_PID
            // 
            this.Text_PID.ButtonSpecs.AddRange ( new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            this.btnTxtPid} );
            this.Text_PID.Location = new System.Drawing.Point ( 77, 20 );
            this.Text_PID.Name = "Text_PID";
            this.Text_PID.Size = new System.Drawing.Size ( 248, 21 );
            this.Text_PID.TabIndex = 2;
            // 
            // btnTxtPid
            // 
            this.btnTxtPid.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Close;
            this.btnTxtPid.UniqueName = "4C243899C9EA47F2F0B67D6227AF4CC1";
            this.btnTxtPid.Click += new System.EventHandler ( this.btnTxtPid_Click );
            // 
            // L_Pwd
            // 
            this.L_Pwd.AutoSize = true;
            this.L_Pwd.Location = new System.Drawing.Point ( 6, 58 );
            this.L_Pwd.Name = "L_Pwd";
            this.L_Pwd.Size = new System.Drawing.Size ( 65, 12 );
            this.L_Pwd.TabIndex = 9;
            this.L_Pwd.Text = "论坛密码：";
            // 
            // L_Pid
            // 
            this.L_Pid.AutoSize = true;
            this.L_Pid.Location = new System.Drawing.Point ( 6, 24 );
            this.L_Pid.Name = "L_Pid";
            this.L_Pid.Size = new System.Drawing.Size ( 65, 12 );
            this.L_Pid.TabIndex = 10;
            this.L_Pid.Text = "论坛账户：";
            // 
            // TabP_Set
            // 
            this.TabP_Set.BackColor = System.Drawing.SystemColors.Control;
            this.TabP_Set.Controls.Add ( this.groupBox_Info );
            this.TabP_Set.Location = new System.Drawing.Point ( 4, 21 );
            this.TabP_Set.Name = "TabP_Set";
            this.TabP_Set.Padding = new System.Windows.Forms.Padding ( 3 );
            this.TabP_Set.Size = new System.Drawing.Size ( 349, 101 );
            this.TabP_Set.TabIndex = 1;
            this.TabP_Set.Text = "选项";
            // 
            // groupBox_Info
            // 
            this.groupBox_Info.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_Info.Controls.Add ( this.x_Lable1 );
            this.groupBox_Info.Controls.Add ( this.x_Lable2 );
            this.groupBox_Info.Controls.Add ( this.txtHardInfo );
            this.groupBox_Info.Controls.Add ( this.ck提示更新 );
            this.groupBox_Info.Controls.Add ( this.label7 );
            this.groupBox_Info.Controls.Add ( this.cVer );
            this.groupBox_Info.Controls.Add ( this.label3 );
            this.groupBox_Info.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Info.Location = new System.Drawing.Point ( 3, 3 );
            this.groupBox_Info.Name = "groupBox_Info";
            this.groupBox_Info.Size = new System.Drawing.Size ( 343, 88 );
            this.groupBox_Info.TabIndex = 0;
            this.groupBox_Info.TabStop = false;
            this.groupBox_Info.Text = "软件登录信息设定";
            // 
            // x_Lable1
            // 
            this.x_Lable1.BackColor = System.Drawing.Color.Transparent;
            this.x_Lable1.Location = new System.Drawing.Point ( 317, 56 );
            this.x_Lable1.MaximumSize = new System.Drawing.Size ( 16, 16 );
            this.x_Lable1.MinimumSize = new System.Drawing.Size ( 22, 17 );
            this.x_Lable1.Name = "x_Lable1";
            this.x_Lable1.ShowControl = this.txtHardInfo;
            this.x_Lable1.ShowText = "根据您的电脑CPU，硬盘等信息，生成的固定的ID值。俗称机器码。";
            this.x_Lable1.ShowTitle = "本地信息";
            this.x_Lable1.Size = new System.Drawing.Size ( 22, 17 );
            this.x_Lable1.TabIndex = 14;
            // 
            // txtHardInfo
            // 
            this.txtHardInfo.Location = new System.Drawing.Point ( 77, 54 );
            this.txtHardInfo.Name = "txtHardInfo";
            this.txtHardInfo.ReadOnly = true;
            this.txtHardInfo.Size = new System.Drawing.Size ( 233, 21 );
            this.txtHardInfo.TabIndex = 10;
            this.txtHardInfo.Click += new System.EventHandler ( this.txtHardInfo_Click );
            // 
            // x_Lable2
            // 
            this.x_Lable2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.x_Lable2.BackColor = System.Drawing.Color.Transparent;
            this.x_Lable2.Location = new System.Drawing.Point ( 333, -55 );
            this.x_Lable2.MaximumSize = new System.Drawing.Size ( 22, 17 );
            this.x_Lable2.MinimumSize = new System.Drawing.Size ( 22, 17 );
            this.x_Lable2.Name = "x_Lable2";
            this.x_Lable2.ShowControl = null;
            this.x_Lable2.ShowText = "登录后，会直接进入您选择的应用。根据您是否具有该应用的权限来进行选择，否者可能无法登录。";
            this.x_Lable2.ShowTitle = "选择入口";
            this.x_Lable2.Size = new System.Drawing.Size ( 22, 17 );
            this.x_Lable2.TabIndex = 13;
            // 
            // ck提示更新
            // 
            this.ck提示更新.AutoSize = true;
            this.ck提示更新.Enabled = false;
            this.ck提示更新.Location = new System.Drawing.Point ( 271, 22 );
            this.ck提示更新.Name = "ck提示更新";
            this.ck提示更新.Size = new System.Drawing.Size ( 72, 16 );
            this.ck提示更新.TabIndex = 9;
            this.ck提示更新.Text = "提示更新";
            this.ck提示更新.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point ( 7, 58 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size ( 65, 12 );
            this.label7.TabIndex = 7;
            this.label7.Text = "本地信息：";
            // 
            // cVer
            // 
            this.cVer.AutoSize = true;
            this.cVer.Location = new System.Drawing.Point ( 76, 24 );
            this.cVer.Name = "cVer";
            this.cVer.Size = new System.Drawing.Size ( 53, 12 );
            this.cVer.TabIndex = 3;
            this.cVer.Text = ".NET版本";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point ( 6, 24 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size ( 65, 12 );
            this.label3.TabIndex = 2;
            this.label3.Text = ".NET版本：";
            // 
            // Ch_IsSave
            // 
            this.Ch_IsSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Ch_IsSave.Checked = true;
            this.Ch_IsSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Ch_IsSave.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalControl;
            this.Ch_IsSave.Location = new System.Drawing.Point ( 200, 192 );
            this.Ch_IsSave.Name = "Ch_IsSave";
            this.Ch_IsSave.Size = new System.Drawing.Size ( 78, 18 );
            this.Ch_IsSave.TabIndex = 16;
            this.Ch_IsSave.Text = "记住密码";
            this.Ch_IsSave.Values.Text = "记住密码";
            // 
            // Pic_Login
            // 
            this.Pic_Login.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pic_Login.Image = global::X_Service.Properties.Resources.X_Form_Login;
            this.Pic_Login.Location = new System.Drawing.Point ( 0, 0 );
            this.Pic_Login.Name = "Pic_Login";
            this.Pic_Login.Size = new System.Drawing.Size ( 381, 50 );
            this.Pic_Login.TabIndex = 4;
            this.Pic_Login.TabStop = false;
            // 
            // linkGetPs
            // 
            this.linkGetPs.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.linkGetPs.AutoSize = true;
            this.linkGetPs.Location = new System.Drawing.Point ( 12, 194 );
            this.linkGetPs.Name = "linkGetPs";
            this.linkGetPs.Size = new System.Drawing.Size ( 125, 12 );
            this.linkGetPs.TabIndex = 7;
            this.linkGetPs.TabStop = true;
            this.linkGetPs.Text = "还没有账户？马上注册";
            this.linkGetPs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler ( this.linkGetPs_LinkClicked );
            // 
            // btn_Login
            // 
            this.btn_Login.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_Login.Location = new System.Drawing.Point ( 279, 188 );
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size ( 90, 25 );
            this.btn_Login.TabIndex = 0;
            this.btn_Login.Values.Text = "登陆";
            this.btn_Login.Click += new System.EventHandler ( this.Btn_Login_Click );
            // 
            // X_Form_Login
            // 
            this.AcceptButton = this.btn_Login;
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 381, 222 );
            this.Controls.Add ( this.Ch_IsSave );
            this.Controls.Add ( this.btn_Login );
            this.Controls.Add ( this.linkGetPs );
            this.Controls.Add ( this.Pic_Login );
            this.Controls.Add ( this.TabC_Login );
            this.Location = new System.Drawing.Point ( 0, 0 );
            this.MinimizeBox = false;
            this.Name = "X_Form_Login";
            this.Text = "忍者X2站群引擎登录 - 用户授权论坛 RenZhe.org";
            this.Load += new System.EventHandler ( this.X_Form_Login_Load );
            this.TabC_Login.ResumeLayout ( false );
            this.TabP_Login.ResumeLayout ( false );
            this.groupBox_Login.ResumeLayout ( false );
            this.groupBox_Login.PerformLayout ( );
            this.TabP_Set.ResumeLayout ( false );
            this.groupBox_Info.ResumeLayout ( false );
            this.groupBox_Info.PerformLayout ( );
            ( ( System.ComponentModel.ISupportInitialize ) ( this.Pic_Login ) ).EndInit ( );
            this.ResumeLayout ( false );
            this.PerformLayout ( );

        }

        #endregion

        private System.Windows.Forms.TabControl TabC_Login;
        private System.Windows.Forms.TabPage TabP_Login;
        private System.Windows.Forms.TabPage TabP_Set;
        private System.Windows.Forms.GroupBox groupBox_Info;
        private System.Windows.Forms.Label cVer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox_Login;
        private System.Windows.Forms.Label L_Pwd;
        private System.Windows.Forms.Label L_Pid;
        private System.Windows.Forms.TextBox txtHardInfo;
        private System.Windows.Forms.PictureBox Pic_Login;
        private X_Service.Util.X_Lable x_Lable2;
        private System.Windows.Forms.CheckBox ck提示更新;
        private System.Windows.Forms.LinkLabel linkGetPs;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btn_Login;
        private ComponentFactory.Krypton.Toolkit.KryptonMaskedTextBox Text_PID;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny btnTxtPid;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox Ch_IsSave;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox Text_PWD;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny btnTxtPWD;
        private Util.X_Lable x_Lable1;
    }
}