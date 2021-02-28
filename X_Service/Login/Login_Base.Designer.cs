namespace X_Service.Login {
    partial class Login_Base {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login_Base));
            this.notiICON = new System.Windows.Forms.NotifyIcon(this.components);
            this.忍者站群引擎XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.忍者WordPress淘宝客建站ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.忍者用户论坛ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.忍者平台退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PaletteRenZhe = new ComponentFactory.Krypton.Toolkit.KryptonPalette(this.components);
            this.kryptonManager = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.TaskDialog = new ComponentFactory.Krypton.Toolkit.KryptonTaskDialog();
            this.SuspendLayout();
            // 
            // notiICON
            // 
            this.notiICON.Icon = ((System.Drawing.Icon)(resources.GetObject("notiICON.Icon")));
            this.notiICON.Text = "忍者X2站群引擎";
            this.notiICON.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notiICON_MouseClick);
            // 
            // 忍者站群引擎XToolStripMenuItem
            // 
            this.忍者站群引擎XToolStripMenuItem.Name = "忍者站群引擎XToolStripMenuItem";
            this.忍者站群引擎XToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // 忍者WordPress淘宝客建站ToolStripMenuItem
            // 
            this.忍者WordPress淘宝客建站ToolStripMenuItem.Name = "忍者WordPress淘宝客建站ToolStripMenuItem";
            this.忍者WordPress淘宝客建站ToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(6, 6);
            // 
            // 忍者用户论坛ToolStripMenuItem
            // 
            this.忍者用户论坛ToolStripMenuItem.Name = "忍者用户论坛ToolStripMenuItem";
            this.忍者用户论坛ToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // 忍者平台退出ToolStripMenuItem
            // 
            this.忍者平台退出ToolStripMenuItem.Name = "忍者平台退出ToolStripMenuItem";
            this.忍者平台退出ToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // PaletteRenZhe
            // 
            this.PaletteRenZhe.BasePaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Black;
            this.PaletteRenZhe.FormStyles.FormMain.StateCommon.Border.Color1 = System.Drawing.Color.Black;
            this.PaletteRenZhe.FormStyles.FormMain.StateCommon.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Rounding4;
            this.PaletteRenZhe.FormStyles.FormMain.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.PaletteRenZhe.FormStyles.FormMain.StateCommon.Border.Rounding = 2;
            this.PaletteRenZhe.HeaderStyles.HeaderForm.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.PaletteRenZhe.HeaderStyles.HeaderForm.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.PaletteRenZhe.ToolMenuStatus.ToolStrip.ToolStripBorder = System.Drawing.Color.LightGray;
            this.PaletteRenZhe.ToolMenuStatus.ToolStrip.ToolStripGradientBegin = System.Drawing.Color.Transparent;
            this.PaletteRenZhe.ToolMenuStatus.ToolStrip.ToolStripGradientEnd = System.Drawing.Color.LightGray;
            this.PaletteRenZhe.ToolMenuStatus.ToolStrip.ToolStripGradientMiddle = System.Drawing.Color.WhiteSmoke;
            this.PaletteRenZhe.ToolMenuStatus.ToolStrip.ToolStripText = System.Drawing.Color.Black;
            this.PaletteRenZhe.ToolMenuStatus.UseRoundedEdges = ComponentFactory.Krypton.Toolkit.InheritBool.False;
            // 
            // kryptonManager
            // 
            this.kryptonManager.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Custom;
            // 
            // TaskDialog
            // 
            this.TaskDialog.CheckboxText = null;
            this.TaskDialog.Content = null;
            this.TaskDialog.DefaultButton = ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK;
            this.TaskDialog.DefaultRadioButton = null;
            this.TaskDialog.FooterHyperlink = "http://www.renzhe.org";
            this.TaskDialog.FooterText = null;
            this.TaskDialog.Icon = System.Windows.Forms.MessageBoxIcon.Asterisk;
            this.TaskDialog.MainInstruction = null;
            this.TaskDialog.WindowTitle = "忍者软件提示：";
            // 
            // Login_Base
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 272);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(500, 300);
            this.MaximizeBox = false;
            this.Name = "Login_Base";
            this.Palette = this.PaletteRenZhe;
            this.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "忍者X2软件平台";
            this.Load += new System.EventHandler(this.Login_Base_Load);
            this.SizeChanged += new System.EventHandler(this.Login_Base_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem 忍者站群引擎XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 忍者WordPress淘宝客建站ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 忍者用户论坛ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 忍者平台退出ToolStripMenuItem;
        private ComponentFactory.Krypton.Toolkit.KryptonPalette PaletteRenZhe;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager;
        public ComponentFactory.Krypton.Toolkit.KryptonTaskDialog TaskDialog;
        protected System.Windows.Forms.NotifyIcon notiICON;







    }
}