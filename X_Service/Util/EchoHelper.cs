using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using X_Service.Balloon;
using X_Service.Util;
using ComponentFactory.Krypton.Toolkit;
using System.Web;

namespace X_Service.Util {
    /// <summary>
    /// 输出类
    /// </summary>
    public static class EchoHelper {
        /// <summary>
        /// 初始化气泡
        /// </summary>
        private static BalloonHelp Balloon;
        public static bool IsShow = true;
        private static IntPtr ParenthWnd = new IntPtr(0);
        [DllImport("User32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWND);
        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);
        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int type);

        public static void Hide() {
            ParenthWnd = FindWindow(null, Console.Title);
            IsShow = false;
            ShowWindow(ParenthWnd, 0);
        }
        public static void Show() {
            ParenthWnd = FindWindow(null, Console.Title);
            IsShow = true;
            ShowWindow(ParenthWnd, 5);
        }

        public enum EchoType {
            普通信息,
            任务信息,
            错误信息,
            异常信息,
        }
        /// <summary>
        /// 输出控制台字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="i"></param>
        public static void Echo(string str, string title, EchoType echo) {
            string strall = str;
            str = HttpUtility.UrlDecode(str);
            title = HttpUtility.UrlDecode(title);

            lock ("我锁") {
                switch (echo) {
                    case EchoType.普通信息: {
                            str = StringHelper.SubString(str.Replace("\n", ""), 0, 80).Replace("【", "[").Replace("】", "]");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(DateTime.Now.ToString("MM-dd HH:mm:ss") + "|");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("【" + str + "】" + Environment.NewLine);
                            LogHelper.CreateLog("普通信息", title + " " + strall, EchoType.普通信息);
                            break;
                        }
                    case EchoType.任务信息: {
                            str = StringHelper.SubString(str.Replace("\n", ""), 0, 80).Replace("【", "[").Replace("】", "]");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(DateTime.Now.ToString("MM-dd HH:mm:ss") + "|");
                            Console.Write(string.Format("【{0}】：{1}", title, str + Environment.NewLine));
                            LogHelper.CreateLog("任务信息", title + " " + strall, EchoType.任务信息);
                            break;
                        }
                    case EchoType.错误信息: {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(DateTime.Now.ToString("MM-dd HH:mm:ss") + "|");
                            Console.Write(string.Format("【{0}】：{1}", title, str + Environment.NewLine));
                            LogHelper.CreateLog("错误信息", title + " " + strall, EchoType.错误信息);
                            break;
                        }
                    case EchoType.异常信息: {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(DateTime.Now.ToString("MM-dd HH:mm:ss") + "|");
                            Console.Write(string.Format("【{0}】：{1}", title, str + Environment.NewLine));
                            LogHelper.CreateLog("异常信息", title + " " + strall, EchoType.异常信息);
                            break;
                        }
                }
            }
        }

        #region Qin的更改内容
        /// <summary>
        /// 这里定义一个弹出类型的枚举类.比输入1，2，4，5这样的方式更好,更直观.
        /// 
        /// </summary>
        public enum MessageType {
            提示,
            警告,
            错误,
        }
        //然后下面就这样调用了
        public static DialogResult Show(string str, MessageType m) {
            switch (m) {
                case MessageType.警告:
                    return KryptonMessageBox.Show(str, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                case MessageType.错误:
                    return KryptonMessageBox.Show(str, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                default:
                    return KryptonMessageBox.Show(str, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        /// <summary>
        /// 显示气泡
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="text">正文</param>
        /// <param name="ctr">控件</param>
        public static void ShowBalloon(string title, string text, Control ctr) {
            Balloon = new BalloonHelp();
            Balloon.Caption = title;
            Balloon.Content = text;
            Balloon.Icon = SystemIcons.Information;
            Balloon.ShowBalloon(ctr);

        }

        public static void EchoPickStart(string info) {
            EchoHelper.Echo("┏━━━━━" + info + "━━━━━┓", "【分隔符】", EchoHelper.EchoType.普通信息);
        }

        public static void EchoPickEnd(string info) {
            EchoHelper.Echo("┗━━━━━" + info + "━━━━━┛", "【分隔符】", EchoHelper.EchoType.普通信息);
        }


        public static void EchoPickStart() {
            EchoHelper.Echo("=========================================start=========================================", "【分隔符】", EchoHelper.EchoType.普通信息);
        }

        public static void EchoPickEnd() {
            EchoHelper.Echo("========================================= end =========================================", "【分隔符】", EchoHelper.EchoType.普通信息);
        }

        public static string EchoException(Exception ex) {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(DateTime.Now.ToString("MM-dd HH:mm:ss") + "|");
            Console.Write(string.Format("【{0}】：{1}", "系统异常", "系统出现了一个异常，已经记录在案，您可以发送log给忍者官方。" + Environment.NewLine));
            LogHelper.CreateLog("异常信息", ex.Message + " " + ex.StackTrace, EchoType.异常信息);
            return ex.Message;
        }
    }
}
