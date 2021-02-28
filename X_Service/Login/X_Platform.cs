using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Plugin;
using System.ComponentModel.Design;
using System.Reflection;
using System.IO;
using System.Threading;

namespace X_Service.Login {
    public partial class X_Platform : Login_Base, IApplication {
        public X_Platform ( ) {
            InitializeComponent ( );
            panel5.Visible = false;
        }

        #region IApplication Members
        public FlowLayoutPanel MyFLP { get { return flowLayoutPanel2; } }
        #endregion

        #region IServiceContainer Members
        private ServiceContainer serviceContainer = new ServiceContainer ( );

        public void AddService ( Type serviceType, System.ComponentModel.Design.ServiceCreatorCallback callback, bool promote ) {
            serviceContainer.AddService ( serviceType, callback, promote );
        }

        public void AddService ( Type serviceType, System.ComponentModel.Design.ServiceCreatorCallback callback ) {
            serviceContainer.AddService ( serviceType, callback );
        }

        public void AddService ( Type serviceType, object serviceInstance, bool promote ) {
            serviceContainer.AddService ( serviceType, serviceInstance, promote );
        }

        public void AddService ( Type serviceType, object serviceInstance ) {
            serviceContainer.AddService ( serviceType, serviceInstance );
        }

        public void RemoveService ( Type serviceType, bool promote ) {
            serviceContainer.RemoveService ( serviceType, promote );
        }

        public void RemoveService ( Type serviceType ) {
            serviceContainer.RemoveService ( serviceType );
        }

        #endregion


        private void X_Platform_Load ( object sender, EventArgs e ) {
            string pluginpath = Path.GetDirectoryName ( Application.ExecutablePath ) + "\\Plugin";

            Assembly assembly = Assembly.LoadFile ( Path.Combine ( pluginpath, "DEDE网站助手.dll" ) );
            IPlugin instance = ( IPlugin ) assembly.CreateInstance ( "DEDE网站助手.Main" );
            instance.Application = this;
            instance.load ( );

            //assembly = Assembly.LoadFile ( Path.Combine ( pluginpath, "复件 DEDE网站助手.dll" ) );
            //instance = ( IPlugin ) assembly.CreateInstance ( "DEDE网站助手.Main" );
            //instance.Application = this;
            //instance.load ( );

            // 
            // panel5
            // 
            Panel panel5 = new Panel ( );
            panel5.BackgroundImage = global::X_Service.Properties.Resources.add;
            panel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel5.Location = new System.Drawing.Point ( 3, 3 );
            panel5.Name = "panel5";
            panel5.Size = new System.Drawing.Size ( 50, 50 );
            panel5.TabIndex = 999;
            this.flowLayoutPanel2.Controls.Add ( panel5 );
        }

        //由于Form类型本身间接的继承了IServiceProvider接口，所以我们要覆盖掉Form本身的实现
        //所以我们使用了new关键字
        public new object GetService ( Type serviceType ) {
            return serviceContainer.GetService ( serviceType );
        }

        private void panel1_MouseEnter ( object sender, EventArgs e ) {
        }

        private void panel1_MouseLeave ( object sender, EventArgs e ) {
        }

        private void panel1_MouseHover ( object sender, EventArgs e ) {

        }

        private void panel5_MouseEnter ( object sender, EventArgs e ) {
        }

    }
}
