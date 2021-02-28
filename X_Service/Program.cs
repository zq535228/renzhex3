using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using X_Service.Util;
using X_Service.Login;


namespace X_Service {
    /// <summary>
    /// 主程序,启动运行.
    /// </summary>
    class Program {

        #region Windows Api加载
        /// <summary>
        /// 查找窗体句柄
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport ( "user32.dll", EntryPoint = "FindWindow" )]
        extern static IntPtr FindWindow ( string lpClassName, string lpWindowName );
        /// <summary>
        /// 查找菜单句柄
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="bRevert"></param>
        /// <returns></returns>
        [DllImport ( "user32.dll", EntryPoint = "GetSystemMenu" )]
        extern static IntPtr GetSystemMenu ( IntPtr hWnd, IntPtr bRevert );
        /// <summary>
        /// 移除某个菜单
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="uPosition"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport ( "user32.dll", EntryPoint = "RemoveMenu" )]
        extern static IntPtr RemoveMenu ( IntPtr hMenu, uint uPosition, uint uFlags );
        #endregion

        #region 静态方法

        #region 灰掉控制台上面的X
        /// <summary>
        /// 灰掉控制台上面的X
        /// </summary>
        static void DisClose ( ) {
            IntPtr windowHandle = FindWindow ( null, Console.Title );
            IntPtr closeMenu = GetSystemMenu ( windowHandle, IntPtr.Zero );
            uint SC_CLOSE = 0xF060;
            RemoveMenu ( closeMenu, SC_CLOSE, 0x0 );
        }
        #endregion

        #endregion

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="args">参数</param>
        [STAThread]
        static void Main ( string[] args ) {
            //MainForm f = new MainForm();

#if DEBUG
            //             X_Form_MainFormBrowser f = new X_Form_MainFormBrowser();
            //             f.ShowDialog();
            //             return;
            //X_Form_MainFormBrowser x = new X_Form_MainFormBrowser();
            //x.GotoPage("http://www.baidu.com/");
            //return;

#endif
            int clientWidth = Screen.PrimaryScreen.Bounds.Width;
            if ( clientWidth > 1000 ) {
                Console.SetWindowSize ( 120, 44 / 2 );
            } else {

                EchoHelper.Echo ( "推荐使用的分辨率是：1024*800以上", null, EchoHelper.EchoType.普通信息 );
            }

            //Qin添加的 只开一个实例
            bool Exist = false;
            System.Diagnostics.Process CIP = System.Diagnostics.Process.GetCurrentProcess ( );
            System.Diagnostics.Process[] CIPR = System.Diagnostics.Process.GetProcesses ( );
            foreach ( System.Diagnostics.Process p in CIPR ) {
                if ( p.ProcessName == CIP.ProcessName && p.Id != CIP.Id ) {
                    Exist = true;
                }
            }
            if ( Exist ) {
#if !DEBUG
                KryptonMessageBox.Show("对不起，应用程序正在运行中，请不要重复启动！");
                return;
#endif
            }

            DisClose ( );
            //设置标题
            Console.Title = "忍者X2智能站群系统 运行状态监控中..."; //设置控制台窗口的标题 
            //初始化内容
            Console.ForegroundColor = ConsoleColor.Magenta;

            #region 待更新的，美丽的CMD字体效果。
            Console.WriteLine ( "" );
            Console.WriteLine ( "      ┏━┓　　　　　　┏━┓┏┓　　　　　　　　　　　　　　　" );
            Console.WriteLine ( "      ┃┃┃┏━┓┏━┓┣━┃┃┗┓┏━┓　　┏━┓┏┳┓┏━┓" );
            Console.WriteLine ( "      ┃　┫┃┻┫┃┃┃┃━┫┃┃┃┃┻┫┏┓┃┃┃┃┏┛┃┃┃" );
            Console.WriteLine ( "      ┗┻┛┗━┛┗┻┛┗━┛┗┻┛┗━┛┗┛┗━┛┗┛　┣┓┃" );
            Console.WriteLine ( "      　　　　　　　　　　　　　　　　　　    　　　　　　┗━┛" );


            #endregion

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine ( "                                  --官方网站：www.RenZhe.org" );
            Console.WriteLine ( "" );
            EchoHelper.Echo ( "忍者X2控制台初始化完毕！", null, EchoHelper.EchoType.普通信息 );

            //初始化窗体
            Application.EnableVisualStyles ( );
            Application.SetCompatibleTextRenderingDefault ( false );

            //检查是否具有可写入的权限
            try {
                X_Service.Files.FilesHelper.WriteFile ( Application.StartupPath + @"\Temp\test.txt", "文件写入权限测试！", Encoding.UTF8 );
            } catch {
                EchoHelper.Echo ( "此系统中，忍者站群写入权限不足，请以管理员的身份运行！", "系统权限不足", EchoHelper.EchoType.异常信息 );
            }
            //初始化登陆
            X_Form_Login Login = new X_Form_Login ( );

            DialogResult dr = Login.ShowDialog ( );
            if ( dr == DialogResult.OK ) {

                //    Login.Dispose();//释放
                //    Login.Close();

                Application.Run ( new X_Platform ( ) );//运行主界面

                //    EchoHelper.Echo("保存信息中...", null, 0);
                //    Thread.Sleep(500);
                //    EchoHelper.Echo("保存成功！...系统退出...", null, EchoHelper.EchoType.普通信息);
                //}
                //else
                //{
                //    EchoHelper.Echo("登陆未完成...关闭窗体！", null, EchoHelper.EchoType.普通信息);
            }

            //Thread.Sleep(500);
            //CIP.Kill();
            //Application.ExitThread();
            //Application.Exit();
        }


    }
}

