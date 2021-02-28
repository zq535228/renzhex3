namespace X_Service.Util
{
    partial class X_Lable
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.picHelp = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
            this.SuspendLayout();
            // 
            // picHelp
            // 
            this.picHelp.Image = global::X_Service.Properties.Resources.question;
            this.picHelp.Location = new System.Drawing.Point(0, 0);
            this.picHelp.Name = "picHelp";
            this.picHelp.Size = new System.Drawing.Size(16, 16);
            this.picHelp.TabIndex = 16;
            this.picHelp.TabStop = false;
            this.picHelp.Click += new System.EventHandler(this.picHelp_Click);
            // 
            // X_Lable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.picHelp);
            this.MaximumSize = new System.Drawing.Size(16, 16);
            this.MinimumSize = new System.Drawing.Size(22, 17);
            this.Name = "X_Lable";
            this.Size = new System.Drawing.Size(22, 17);
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picHelp;
    }
}
