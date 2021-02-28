namespace X_Update
{
    partial class FrmUpdate
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUpdate));
            this.tmStart = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnHulve = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.pbDownFile = new System.Windows.Forms.ProgressBar();
            this.lvUpdateList = new System.Windows.Forms.ListView();
            this.chFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtUpdateInfo = new System.Windows.Forms.TextBox();
            this.Pic_Login = new System.Windows.Forms.PictureBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Login)).BeginInit();
            this.SuspendLayout();
            // 
            // tmStart
            // 
            this.tmStart.Enabled = true;
            this.tmStart.Interval = 1;
            this.tmStart.Tick += new System.EventHandler(this.tmStart_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 50);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(478, 259);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnHulve);
            this.tabPage1.Controls.Add(this.btnNext);
            this.tabPage1.Controls.Add(this.pbDownFile);
            this.tabPage1.Controls.Add(this.lvUpdateList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(470, 233);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "更新";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnHulve
            // 
            this.btnHulve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHulve.Location = new System.Drawing.Point(238, 201);
            this.btnHulve.Name = "btnHulve";
            this.btnHulve.Size = new System.Drawing.Size(82, 23);
            this.btnHulve.TabIndex = 11;
            this.btnHulve.Text = "忽略";
            this.btnHulve.UseVisualStyleBackColor = true;
            this.btnHulve.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnNext
            // 
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Location = new System.Drawing.Point(150, 201);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(82, 23);
            this.btnNext.TabIndex = 12;
            this.btnNext.Text = "开始";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // pbDownFile
            // 
            this.pbDownFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbDownFile.Location = new System.Drawing.Point(3, 172);
            this.pbDownFile.Name = "pbDownFile";
            this.pbDownFile.Size = new System.Drawing.Size(464, 23);
            this.pbDownFile.TabIndex = 10;
            // 
            // lvUpdateList
            // 
            this.lvUpdateList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFileName,
            this.chVersion,
            this.chProgress});
            this.lvUpdateList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lvUpdateList.Location = new System.Drawing.Point(3, 3);
            this.lvUpdateList.Name = "lvUpdateList";
            this.lvUpdateList.Size = new System.Drawing.Size(464, 169);
            this.lvUpdateList.TabIndex = 9;
            this.lvUpdateList.UseCompatibleStateImageBehavior = false;
            this.lvUpdateList.View = System.Windows.Forms.View.Details;
            // 
            // chFileName
            // 
            this.chFileName.Text = "组件名";
            this.chFileName.Width = 125;
            // 
            // chVersion
            // 
            this.chVersion.Text = "版本号";
            this.chVersion.Width = 127;
            // 
            // chProgress
            // 
            this.chProgress.Text = "进度";
            this.chProgress.Width = 73;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtUpdateInfo);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(470, 233);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "更新说明";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtUpdateInfo
            // 
            this.txtUpdateInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUpdateInfo.Location = new System.Drawing.Point(3, 3);
            this.txtUpdateInfo.Multiline = true;
            this.txtUpdateInfo.Name = "txtUpdateInfo";
            this.txtUpdateInfo.Size = new System.Drawing.Size(464, 227);
            this.txtUpdateInfo.TabIndex = 0;
            // 
            // Pic_Login
            // 
            this.Pic_Login.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pic_Login.Image = global::X_Update.Properties.Resources.X_Form_Login;
            this.Pic_Login.Location = new System.Drawing.Point(0, 0);
            this.Pic_Login.Name = "Pic_Login";
            this.Pic_Login.Size = new System.Drawing.Size(478, 50);
            this.Pic_Login.TabIndex = 5;
            this.Pic_Login.TabStop = false;
            // 
            // FrmUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 310);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.Pic_Login);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "软件更新";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Login)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmStart;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnHulve;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.ProgressBar pbDownFile;
        private System.Windows.Forms.ListView lvUpdateList;
        private System.Windows.Forms.ColumnHeader chFileName;
        private System.Windows.Forms.ColumnHeader chVersion;
        private System.Windows.Forms.ColumnHeader chProgress;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtUpdateInfo;
        private System.Windows.Forms.PictureBox Pic_Login;
    }
}

