using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using X_Service.Login;
using X_Service.Util.SplashScreen;

namespace X_Service.Util {

    public partial class X_Waiting {

        public X_Waiting() {
            Splasher.Show(typeof(X_Splash));
        }

        public void ShowMsg(string msg) {
            Splasher.Show();
            Splasher.Status = msg;
        }

        private void showWaitDlg(object msg) {
            Splasher.Show();
            Splasher.Status = msg.ToString();
        }

        public void CloseMsg() {
            Splasher.Close(false);
        }

        public void Dispose() {
            Splasher.Close();
        }

       



    }
}
