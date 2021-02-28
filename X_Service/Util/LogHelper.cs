using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using X_Service.Files;

namespace X_Service.Util {
    public class LogHelper {

        ///   <summary>   
        ///   生成系统日志。   
        ///   </summary>   
        ///   <param   name="Description">所记录日志描述。</param>   
        ///   <param   name="Content">所记录日志内容。</param>   
        public static void CreateLog(string Description, string Content, X_Service.Util.EchoHelper.EchoType etype) {

            //string strTmp = "";
            string FilePath = Application.StartupPath + "\\Log\\";

            if (!Directory.Exists(FilePath)) {
                Directory.CreateDirectory(FilePath);
            }//   文件夹操作完成。   

            string path = Application.StartupPath + "\\Config\\Setup.ini";

            INIHelper ini = new INIHelper(path);
            if (!ini.ExistINIFile()) {
                Console.WriteLine("系统崩溃，读取：\\Config\\Setup.ini失败！请重新下载&覆盖", "日志系统", EchoHelper.EchoType.异常信息);
                return;
            }

            switch (etype) {
                case EchoHelper.EchoType.普通信息:
                    if (!Convert.ToBoolean(ini.re("日志设定", "普通信息")))
                        return;
                    FilePath += "普通信息_log_";
                    break;
                case EchoHelper.EchoType.任务信息:
                    if (!Convert.ToBoolean(ini.re("日志设定", "任务信息")))
                        return;
                    FilePath += "任务信息_log_";
                    break;
                case EchoHelper.EchoType.错误信息:
                    if (!Convert.ToBoolean(ini.re("日志设定", "错误信息")))
                        return;
                    FilePath += "错误信息_log_";
                    break;
                default:
                    FilePath += "异常信息_log_";
                    break;
            }
            FilePath += DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + ".txt";
            //天使更新
            lock ("我锁") {
                StreamWriter sw;
                if (!File.Exists(FilePath)) {
                    sw = File.CreateText(FilePath);
                } else {
                    sw = File.AppendText(FilePath);
                }

                sw.WriteLine("-------------------" + "[System Time]:   " + DateTime.Now.ToString() + "---------------------------");
                sw.WriteLine(Description.ToString() + "：" + Content.ToString());
                sw.WriteLine();

                sw.Close();
            }

        }
    }
}
