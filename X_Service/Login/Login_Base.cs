using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using X_Service.Util;
using X_Service.Web;
using System.Text;
using System.Net.Sockets;
using X_Service.Db;

namespace X_Service.Login {
    public partial class Login_Base : KryptonForm {

        public INIHelper ini = new INIHelper(Application.StartupPath + "\\Config\\Setup.ini");
        public static IList<Form> frms;

        protected TcpClient m_socketClient;
        protected bool m_stopLoop = true;
        protected byte[] m_receiveBuffer = new byte[2048 * 1024];


        public static ModelMember member = new ModelMember();

        public Login_Base() {
            InitializeComponent();
            kryptonManager.GlobalPalette = PaletteRenZhe;
            if (frms == null) {
                frms = new List<Form>();
            }
        }

        private void Login_Base_Load(object sender, EventArgs e) {

        }



        /// <summary>
        /// 隐藏在右下角的任务托盘中,点击显示出来.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void notiICON_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                if (!Visible) {
                    this.Visible = true;
                    this.WindowState = FormWindowState.Normal;
                    this.TopMost = true;
                    this.TopMost = false;
                }
            }
        }

        protected virtual void Login_Base_SizeChanged(object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                this.Visible = false;
                notiICON.Visible = true;
                this.notiICON.ShowBalloonTip(3000, "我在这里", "忍者站群，点击显示界面！", ToolTipIcon.Info);
            } else {
                notiICON.Visible = false;
            }
        }


        /// <summary>
        /// 撑大LV的行高
        /// </summary>
        protected void listViewHeight(ListView lv, int height) {
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, height);//分别是宽和高
            lv.SmallImageList = imgList;   //这里设置listView的SmallImageList ,用imgList将其撑大
        }

        public static List<Thread> ThreadTaskList = new List<Thread> {
        };

        public static Thread SearchThread(string Name) {
            Thread th = null;
            for (int i = 0; i < ThreadTaskList.Count; i++) {
                if (ThreadTaskList[i].Name.ToString() == Name) {
                    th = ThreadTaskList[i];
                }
            }
            return th;

        }

        protected void ShowWin(Form frm) {
            int has = -1;
            for (int i = 0; i < frms.Count; i++) {
                if (frms[i].Text.Contains(frm.Text)) {
                    has = i;
                }
            }

            if (has > -1 & frms.Count > 0) {
                if (frms[has].Visible == false)
                    frms[has].Show();
            } else {
                frms.Add(frm);
                frms[frms.IndexOf(frm)].Show();
            }
        }




    }
}
