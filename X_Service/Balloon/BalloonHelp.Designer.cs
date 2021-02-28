namespace X_Service.Balloon
{
    partial class BalloonHelp
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
            this.windowsHook = new X_Service.Balloon.Hooks.WindowsHook(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // windowsHook
            // 
            this.windowsHook.ThreadID = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            // 
            // BalloonHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F , 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(545 , 165);
            this.Name = "BalloonHelp";
            this.Text = "BalloonHelp";
            this.ResumeLayout(false);

        }

        #endregion

        private Hooks.WindowsHook windowsHook;
        private System.Windows.Forms.Timer timer1;
    }
}