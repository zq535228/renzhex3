using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;

namespace X_Service.Util {
    public partial class X_Lable : UserControl {

        public X_Lable() {
            InitializeComponent();
        }

        private Control showcontrol = null;
        [Browsable(true), Category("指定的控件"), Description("请选择要显示的控件")]
        public Control ShowControl {
            get {
                return showcontrol;
            }
            set {
                showcontrol = value;
            }
        }

        private string showTitle = string.Empty;
        [Browsable(true), Category("显示的标题"), Description("这里表示显示的标题。")]
        public string ShowTitle {
            get {
                return showTitle;
            }
            set {
                showTitle = value;
            }
        }
        private string showText = string.Empty;

        [Browsable(true), Category("显示的正文"), Description("这里表示显示的正文。")]
        [SettingsBindable(true)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string ShowText {
            get {
                return showText;
            }
            set {
                showText = value;
            }
        }

        private void picHelp_Click(object sender, EventArgs e) {
            if (showcontrol != null & showText.Trim() != string.Empty & showTitle.Trim() != string.Empty) {
                EchoHelper.ShowBalloon(showTitle, showText, showcontrol);
            }
        }

    }
}
