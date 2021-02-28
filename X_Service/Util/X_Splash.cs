using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace X_Service.Util.SplashScreen {
    public partial class X_Splash : Form, ISplashForm {
        public X_Splash() {
            InitializeComponent();
        }

        #region ISplashForm

        void ISplashForm.SetStatusInfo(string NewStatusInfo) {
            lblMsg.Text = NewStatusInfo;
        }

        #endregion

        private void X_Splash_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 27) {
                this.Hide();
            }
        }

        private void close_Click(object sender, EventArgs e) {
            this.Hide();
        }

        private void X_Splash_Load(object sender, EventArgs e) {
            this.TopMost = true;
        }

    }
}