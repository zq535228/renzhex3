using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DEDE网站助手 {

    [Plugin.PluginInfo ( "测试使用情况", "1.1.1.212", "王军", "www.baidu.com", false )]
    public class Main : IPlugin {

        void IPlugin.load ( ) {
            if ( application != null && application.MyFLP != null ) {
                Panel dedepanel = new Panel ( );
                dedepanel.Text = "hello word";
                dedepanel.Name = "test1";
                dedepanel.Size = new System.Drawing.Size ( global::DEDE网站助手.Resource.youdao.Width, global::DEDE网站助手.Resource.youdao.Height );
                dedepanel.BackgroundImage = global::DEDE网站助手.Resource.youdao;
                dedepanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
                dedepanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                application.MyFLP.Controls.Add ( dedepanel );
            }
        }

        void IPlugin.dispose ( ) {
            foreach ( Panel ts in application.MyFLP.Controls ) {
                if ( ts.Name == "test1" ) {
                    application.MyFLP.Controls.Remove ( ts );
                    break;
                }
            }
        }

        private IApplication application = null;
        public IApplication Application {
            get {
                return application;
            }
            set {
                application = value;
            }
        }
    }
}
