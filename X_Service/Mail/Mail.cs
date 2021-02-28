using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using X_Service.Util;
using System.Windows.Forms;

namespace X_Service.Mail {
    public class Mail {

        public static void sendMail(string title, string content) {
            //string path = Application.StartupPath + "\\Config\\Setup.ini";
            //INIHelper ini = new INIHelper(path);
            //string toMail = ini.re("默认注册信息", "ToEmail");

            //try {
            //SmtpClient smtpClient = new SmtpClient();
            //smtpClient.Host = "smtp.sina.com";
            //smtpClient.Credentials = new NetworkCredential("zq535228@sina.com", "zqowner");
            //MailMessage message = new MailMessage();
            //message.From = new MailAddress("zq535228@sina.com");

            //message.To.Add(new MailAddress(toMail));
            //message.Subject = title;
            //message.Body = content;
            //smtpClient.Send(message);
            //} catch (Exception ex) {
            //    EchoHelper.EchoException(ex);
            //}

        }
    }
}
