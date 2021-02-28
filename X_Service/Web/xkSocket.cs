using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace X_Service.Web {

    /// <summary>
    /// 这是Socket的请求类,要是出现了验证码,不知道用这个是否可以.
    /// </summary>
    public class xkSocket {

        //定义（引用）API函数  
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

        //判断当前是否连接Internet
        public bool IsLinkInternet() {
            int lfag = 0;
            bool IsInternet;

            if (InternetGetConnectedState(out lfag, 0))
                IsInternet = true;
            else
                IsInternet = false;

            return IsInternet;
        }

        /// <summary>
        /// 用Socket的方式去发送数据,包括get,post
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="method">get/post</param>
        /// <param name="data">postdata</param>
        /// <param name="cookies">xkcookies.getCookieStr()</param>
        /// <param name="reffer">Referer</param>
        /// <param name="userAgent">UserAgent</param>
        /// <param name="encoding">En</param>
        /// <returns></returns>
        public string SendData(Uri uri, string method, string data, string cookies, string reffer, string userAgent, Encoding encoding) {
            string formatString = string.Empty;
            string sendString = string.Empty;
            string _method = method.ToUpper();

            //以下是拼接的HTTP头信息
            if (_method == "GET") {
                formatString = "";
                formatString += "{0} {1} HTTP/1.1\r\n";
                formatString += "Accept: */*\r\n";
                formatString += "User-Agent: " + userAgent + "\r\n";
                formatString += "Host: {2}\r\n";
                formatString += "Connection: Keep-Alive\r\n";
                formatString += "Keep-Alive: 1000\r\n";
                formatString += "Cookie: {3}\r\n\r\n";
                sendString = string.Format(formatString, _method, uri.PathAndQuery, uri.Host, cookies);
            } else {
                formatString = "";
                formatString += "{0} {1} HTTP/1.1\r\n";
                formatString += "Accept: */*\r\n";
                formatString += "Content-Type: application/x-www-form-urlencoded\r\n";
                formatString += "User-Agent: " + userAgent + "\r\n";
                formatString += "Host: {2}\r\n";
                formatString += "Content-Type: application/x-www-form-urlencoded\r\n";
                formatString += "Content-Length: {3}\r\n";
                formatString += "Referer: " + reffer + "\r\n";
                formatString += "Keep-Alive: 300\r\n";
                formatString += "Connection: Keep-Alive\r\n";
                formatString += "Cookie: {4}\r\n\r\n";
                formatString += "{5}\r\n";
                sendString = string.Format(formatString, _method, uri.PathAndQuery, uri.Host, encoding.GetByteCount(data), cookies, data);
            }

            Byte[] ByteGet = encoding.GetBytes(sendString);
            Byte[] RecvBytes = new Byte[1024 * 64];
            String html = null;
            try {
                IPAddress hostadd = Dns.Resolve(uri.Host).AddressList[0];
                IPEndPoint EPhost = new IPEndPoint(hostadd, uri.Port);
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                s.Connect(EPhost);
                if (!s.Connected) {
                    html = "链接主机失败";
                    return html;
                }
                s.Send(ByteGet, ByteGet.Length, SocketFlags.None);
                html = Recv(s, encoding);
                //xkCookies.UpCookie(uri, html);
                return html;
            } catch {
                html = "链接主机失败";
                return html;
            }

        }

        private String Recv(Socket sock, Encoding encode) {
            Byte[] buffer = new Byte[1000 * 64];
            StringBuilder sb = new StringBuilder();

            Thread.Sleep(300);//根据页面响应时间进行微调..经测试 这个最快 如果这个是60 那么下边的sleep会很多,第一次是取300毫秒的内容
            int len = sock.Receive(buffer);
            sb.Append(encode.GetString(buffer, 0, len));

            while (sock.Available > 0) {
                Thread.Sleep(10);//也可以视情况微调  zheli 10???这是每一次 的值 一般没用 最好弄少点 这里是取剩下的
                Array.Clear(buffer, 0, buffer.Length);
                len = sock.Receive(buffer);
                sb.Append(encode.GetString(buffer, 0, len));
                string ss = encode.GetString(buffer, 0, len);
            }
            sock.Close();
            return sb.ToString();
        }

        /// <summary>
        /// 获取HTML内容
        /// </summary>
        /// <returns></returns>
        public string getHtml(string html) {
            int index = html.IndexOf("\r\n\r\n");//去除http头,明白 4字节....感觉用socket要自己去处理很多string..挺快的,有利有弊吧
            if (index != -1) {
                string head = string.Empty;
                head = html.Substring(0, index);//这里是截取 0 到 html开头...结尾head  +4 才是开始
                html = html.Substring(index + 4);//html

                if (head.Substring(9, 3) != "200") {
                    string back = head.Substring(9, 3);
                    string url = GetLocationUrl(head);
                    if (back == "302" || back == "301") {
                        return back + url;
                    } else {
                        return url;
                    }
                }
                return html;
            } else {
                return html;
            }
        }

        private string GetLocationUrl(string head) {
            string url = string.Empty;
            string[] arr = head.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in arr) {
                if (str.StartsWith("Location: ")) {
                    url = str.Substring(10);
                }
            }
            return url;
        }


    }
}
