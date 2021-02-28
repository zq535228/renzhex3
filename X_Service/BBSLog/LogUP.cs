using System;
using System.Collections.Generic;
using System.Text;
using X_Service.Web;
using X_Service.Util;
using X_Service.Login;
using X_Service.Files;
using System.Windows.Forms;

namespace X_Service.BBSLog {
    public class LogUP {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title">文章标题</param>
        /// <param name="turl">文章地址</param>
        /// <param name="siteName">站点名</param>
        /// <param name="taskName">任务名</param>
        /// <param name="siteUrl">站点地址</param>
        public static void upToRenzheBBS(string title, string turl, string siteName, string taskName, string siteUrl) {

            if (turl.Contains("127.0.0.1") || turl.Contains("localhost")) {
                return;//如果是本机测试用的话，就不要去上传到论坛上了。
            }

            title = string.Format("【{0}-{1}】：{2}", Login_Base.member.group, Login_Base.member.netname, title);
            string content = string.Format("授权会员：{0}\n用户组别：{1}\n当前金币：{7}\n\n网站名称：{2}\n任务名称：{3}\n[hide=99999]网站地址：{4}\n\n文章标题：{5}\n文章地址：{6}[/hide]",
                                     Login_Base.member.netname,
                                    Login_Base.member.group,
                                     siteName,
                                     taskName,
                                     siteUrl,
                                     title,
                                     turl,
                                     Login_Base.member.userMoney.ToString()
                                     );
            if (title.Length > 40) {
                title = StringHelper.SubString(title, 0, 38);
            }
            content = content.Replace("&", "-");
            string purl = "http://www.renzhe.org/forum.php?mod=post&action=newthread&fid=37&extra=&topicsubmit=yes";
            string pdata = string.Format("formhash={0}&posttime=&wysiwyg=0&subject={1}&checkbox=0&message={2}&replycredit_extcredits=0&replycredit_times=1&replycredit_membertimes=1&replycredit_random=100&readperm=&price=&save=&usesig=1&allownoticeauthor=1", Login_Base.member.formhash, title, content);
            string reffer = "http://www.renzhe.org/forum.php";

            string html = new xkHttp().httpPost(purl, pdata, ref Login_Base.member.cookies, reffer, Encoding.UTF8);
            if (html.Contains("非常感谢，您的主题已发布")) {
                Login_Base.member.userMoney--;
                EchoHelper.Echo("上传日志成功！金币-1", "忍者X2日志系统", EchoHelper.EchoType.任务信息);
            } else {
                FilesHelper.WriteFile(Application.StartupPath + @"\Log\未知标识\【忍者X2日志】发布没有找到标志_" + DateTime.Now.Millisecond.ToString() + ".html", html, Encoding.UTF8);
                EchoHelper.Echo("跳过上传日志：随机抽取未选中。", "忍者X2日志系统", EchoHelper.EchoType.普通信息);
            }
        }

    }
}
