using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Mozilla.NUniversalCharDet;
using X_Service.Util;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Tags;

namespace X_Service.Web {

    /// <summary>
    /// 忍者POST类,验证码用这个读取
    /// </summary>
    public class xkHttp {

        //private string useragent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)";
        private string useragent = "Mozilla/5.0 (Windows NT 6.1; rv:5.0) Gecko/20100101 Firefox/5.0";

        #region 获取验证码


        public Image httpPic(string url, string postDataStr, ref CookieCollection cookie) {
            byte[] codeByte = new byte[1];
            try {

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url + (postDataStr == "" ? "" : "?") + postDataStr);
                httpWebRequest.CookieContainer = xkCookies.CookieContainer(cookie, url);
                httpWebRequest.UserAgent = useragent;
                httpWebRequest.Method = "GET";
                httpWebRequest.Referer = url;
                httpWebRequest.Timeout = 30000;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=gb2312";
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                xkCookies.UpCookie(ref cookie, url, response.Headers["Set-Cookie"], response.Cookies);
                Stream myResponseStream = response.GetResponseStream();
                byte[] outBytes = new byte[response.ContentLength];
                codeByte = ReadFully(myResponseStream);
            } catch (Exception ex) {
                LogHelper.CreateLog(this.ToString(), url + "\n" + postDataStr + "\n" + "失败!" + ex.Message, EchoHelper.EchoType.异常信息);
            }
            return ByteToImg(codeByte);
        }
        private Image ByteToImg(byte[] imgData) {
            Image img = null;
            if (imgData.Length > 1) {
                MemoryStream ms = new MemoryStream(imgData);
                img = Image.FromStream(ms);
            }
            return img;
        }
        byte[] ReadFully(Stream stream) {
            byte[] buffer = new byte[128];
            using (MemoryStream ms = new MemoryStream()) {
                while (true) {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public byte[] httpBytes(string url, byte[] byteRequest, ref CookieCollection cookie) {
            url = getDealUrl(url);
            long contentLength;
            HttpWebRequest httpWebRequest;
            HttpWebResponse httpWebResponse;
            Stream getStream;
            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpWebRequest.CookieContainer = xkCookies.CookieContainer(cookie, url);
            httpWebRequest.ContentType = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, */*";
            httpWebRequest.Method = "GET";
            httpWebRequest.Referer = url;
            httpWebRequest.Timeout = 30000;
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            xkCookies.UpCookie(ref cookie, url, httpWebResponse.Headers["Set-Cookie"], httpWebResponse.Cookies);
            getStream = httpWebResponse.GetResponseStream();
            contentLength = httpWebResponse.ContentLength;
            byte[] outBytes = new byte[1000];
            outBytes = ReadFully(getStream);
            getStream.Close();
            return outBytes;
        }

        #endregion


        #region POST方法，提交数据，获取内容。
        /// <summary>
        /// 通过POST方式发送数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="postDataStr">Post数据</param>
        /// <param name="cookies">Cookie容器</param>
        /// <returns></returns>
        public string httpPost(string url, string postDataStr, ref CookieCollection cookies) {
            return httpPost(url, postDataStr, ref cookies, "");
        }
        public string httpPost(string url, string postDataStr, ref CookieCollection cookies, string referer) {
            return httpPost(url, postDataStr, ref cookies, referer, Encoding.Default);
        }

        public string httpPost(string url, string postDataStr, ref CookieCollection cookies, string referer, Encoding encode) {
            return httpPost(url, postDataStr, ref cookies, referer, Encoding.UTF8 == encode, false, 30, false, true);
        }

        public string httpPost(string url, string postDataStr, ref CookieCollection cookies, string referer, bool isutf8, bool sendmode, int timeout, bool isPostOnGzip, bool isRedirect) {
            url = getDealUrl(url);
            Stream stream = null;
            HttpWebResponse httpWebResponse = null;
            HttpWebRequest httpWebRequest = null;
            string result;
            try {
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.DefaultConnectionLimit = 1000;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.ServicePoint.UseNagleAlgorithm = false;
                httpWebRequest.AllowWriteStreamBuffering = false;
                httpWebRequest.CookieContainer = xkCookies.CookieContainer(cookies, url);
                httpWebRequest.Method = "POST";
                if (isPostOnGzip) {
                    httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                }
                if (sendmode) {
                    string text = "X" + DateTime.Now.Ticks;
                    httpWebRequest.ContentType = "multipart/form-data; boundary=---------------------------" + text;
                    postDataStr = postDataStr.Replace("7da38e26991192", text);
                } else {
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                }
                httpWebRequest.Referer = referer;
                httpWebRequest.Timeout = timeout * 1000;
                httpWebRequest.AllowAutoRedirect = false;
                httpWebRequest.Accept = "*/*";
                httpWebRequest.Headers.Add("Accept-Language", "zh-CN");
                httpWebRequest.Headers.Add("Cache-Control", "no-cache");
                httpWebRequest.UserAgent = useragent;
                httpWebRequest.KeepAlive = true;
                httpWebRequest.ProtocolVersion = HttpVersion.Version10;
                byte[] array = isutf8 ? Encoding.UTF8.GetBytes(postDataStr) : Encoding.Default.GetBytes(postDataStr);
                httpWebRequest.ContentLength = (long)array.Length;
                string text2 = httpWebRequest.Headers.ToString();
                Stream requestStream = httpWebRequest.GetRequestStream();
                using (Stream stream2 = requestStream) {
                    stream2.Write(array, 0, array.Length);
                }
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                stream = httpWebResponse.GetResponseStream();
                if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip")) {
                    stream = new GZipStream(stream, CompressionMode.Decompress);
                } else {
                    if (httpWebResponse.ContentEncoding.ToLower().Contains("deflate")) {
                        stream = new DeflateStream(stream, CompressionMode.Decompress);
                    }
                }
                string text3 = isutf8 ? new StreamReader(stream, Encoding.UTF8).ReadToEnd() : new StreamReader(stream, Encoding.Default).ReadToEnd();
                text3 = string.Concat(new object[]
				{
					text3, 
					"\r\n\r\n=================================================\r\n\r\n本次请求：", 
					url, 
					" 响应结果：", 
					httpWebResponse.StatusCode, 
					"\r\n\r\nCookie数量", 
					httpWebRequest.CookieContainer.Count, 
					"\r\n", 
					httpWebRequest.CookieContainer.GetCookieHeader(new Uri(url)), 
					"\r\nrequest:\r\n", 
					text2, 
					"\r\nresponse:\r\n", 
					httpWebResponse.Headers.ToString(), 
					"\r\n\r\n=================================================\r\n\r\n"
				});
                xkCookies.UpCookie(ref cookies, url, httpWebResponse.Headers["Set-Cookie"], httpWebResponse.Cookies);
                if (isRedirect) {
                    if (httpWebResponse.Headers["Location"] != null && httpWebResponse.Headers["Location"].Length > 2) {
                        if (httpWebResponse.Headers["Location"].ToLower().Contains("http://")) {
                            url = httpWebResponse.Headers["Location"];
                        } else {
                            url = geturl(httpWebResponse.Headers["Location"], url);
                        }
                        text3 = httpGET(url, ref cookies, url, 3, 30, isRedirect) + text3;
                    } else {
                        if (httpWebResponse.Headers["Refresh"] != null && httpWebResponse.Headers["Refresh"].Length > 2) {
                            url = httpWebResponse.Headers["Refresh"].ToLower().Replace("url=", "`").Split('`')[1];
                            if (!url.Contains("http://")) {
                                url = geturl(url, url);
                            }
                            text3 = httpGET(url, ref cookies, url, 3, 30, isRedirect) + text3;
                        }
                    }
                }
                result = text3;
            } catch (Exception ex) {
                result = ex.Message;
            } finally {
                if (stream != null) {
                    stream.Close();
                }
                if (httpWebResponse != null) {
                    httpWebResponse.Close();
                }
                if (httpWebRequest != null) {
                    httpWebRequest.Abort();
                }
            }
            return result;


        }



        #endregion

        #region GET，获取内容。
        /// <summary>
        /// 通过GET方式发送数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="cookies">GET容器</param>
        /// <param name="reffer">GET数据</param>
        /// <returns></returns>
        public string httpGET(string url, ref CookieCollection cookies) {
            return httpGET(url, ref cookies, url, 3, 30, true);
        }

        /// <summary>
        /// 通过GET方式发送数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="cookies">GET容器</param>
        /// <param name="reffer">GET数据</param>
        /// <returns></returns>
        public string httpGET(string url, ref CookieCollection cookies, string reffer) {
            return httpGET(url, ref cookies, reffer, 3, 30, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookies"></param>
        /// <param name="refrere"></param>
        /// <param name="encoding">1gbk,2utf8,3auto</param>
        /// <param name="timeout"></param>
        /// <param name="isRedirect"></param>
        /// <returns></returns>
        public string httpGET(string url, ref CookieCollection cookies, string refrere, int encoding, int timeout, bool isRedirect) {
            url = getDealUrl(url);
            Stream stream = null;
            HttpWebResponse httpWebResponse = null;
            HttpWebRequest httpWebRequest = null;
            string result;
            try {
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.DefaultConnectionLimit = 1000;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Headers.Clear();
                httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                httpWebRequest.CookieContainer = xkCookies.CookieContainer(cookies, url);
                httpWebRequest.KeepAlive = true;
                httpWebRequest.ProtocolVersion = HttpVersion.Version10;
                httpWebRequest.Method = "GET";
                httpWebRequest.Referer = refrere;
                httpWebRequest.Timeout = timeout * 1000;
                httpWebRequest.AllowAutoRedirect = false;
                httpWebRequest.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                httpWebRequest.Headers.Add("Accept-Language", "zh-cn");
                httpWebRequest.UserAgent = useragent;
                string text = httpWebRequest.Headers.ToString();
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                stream = httpWebResponse.GetResponseStream();
                xkCookies.UpCookie(ref cookies, url, httpWebResponse.Headers["Set-Cookie"], httpWebResponse.Cookies);
                string tmp_result = "";
                if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip")) {
                    stream = new GZipStream(stream, CompressionMode.Decompress);
                } else {
                    if (httpWebResponse.ContentEncoding.ToLower().Contains("deflate")) {
                        stream = new DeflateStream(stream, CompressionMode.Decompress);
                    }
                }

                Stream mystream = httpWebResponse.GetResponseStream();
                MemoryStream msTemp = new MemoryStream();
                int len = 0;
                byte[] buff = new byte[512];

                while ((len = mystream.Read(buff, 0, 512)) > 0) {
                    msTemp.Write(buff, 0, len);
                }
                httpWebResponse.Close();

                if (msTemp.Length > 0) {
                    msTemp.Seek(0, SeekOrigin.Begin);
                    byte[] PageBytes = new byte[msTemp.Length];
                    msTemp.Read(PageBytes, 0, PageBytes.Length);

                    msTemp.Seek(0, SeekOrigin.Begin);
                    int DetLen = 0;
                    byte[] DetectBuff = new byte[4096];
                    UniversalDetector Det = new UniversalDetector(null);
                    while ((DetLen = msTemp.Read(DetectBuff, 0, DetectBuff.Length)) > 0 && !Det.IsDone()) {
                        Det.HandleData(DetectBuff, 0, DetectBuff.Length);
                    }
                    Det.DataEnd();
                    if (Det.GetDetectedCharset() != null) {
                        tmp_result = System.Text.Encoding.GetEncoding(Det.GetDetectedCharset()).GetString(PageBytes);
                    } else {
                        tmp_result = System.Text.Encoding.GetEncoding("GBK").GetString(PageBytes);
                    }
                }

                tmp_result = string.Concat(new object[]
				    {
					    tmp_result, 
					    "\r\n\r\n=================================================\r\n\r\n本次请求：", 
					    url, 
					    " 响应结果：", 
					    httpWebResponse.StatusCode, 
					    "\r\n\r\nCookie数量", 
					    httpWebRequest.CookieContainer.Count, 
					    "\r\n", 
					    httpWebRequest.CookieContainer.GetCookieHeader(new Uri(url)), 
					    "\r\nrequest:\r\n", 
					    text, 
					    "\r\nresponse:\r\n", 
					    httpWebResponse.Headers.ToString(), 
					    "\r\n\r\n=================================================\r\n\r\n"
				    });
                if (isRedirect) {
                    if (httpWebResponse.Headers["Location"] != null && httpWebResponse.Headers["Location"].Length > 2) {
                        string url_redirect = "";
                        if (httpWebResponse.Headers["Location"].ToLower().Contains("http://")) {
                            url_redirect = httpWebResponse.Headers["Location"];
                        } else {
                            url_redirect = geturl(httpWebResponse.Headers["Location"], url);
                        }
                        tmp_result = httpGET(url_redirect, ref cookies, url, 3, 10, isRedirect) + tmp_result;
                    } else {
                        if (httpWebResponse.Headers["Refresh"] != null && httpWebResponse.Headers["Refresh"].Length > 2) {
                            string text3 = httpWebResponse.Headers["Refresh"].ToLower().Replace("url=", "`").Split('`')[1];
                            if (!text3.Contains("http://")) {
                                text3 = geturl(text3, url);
                            }
                            tmp_result = httpGET(text3, ref cookies, url, 3, 10, isRedirect) + tmp_result;
                        }
                    }
                    if (tmp_result.Contains("Refresh")) {
                        Winista.Text.HtmlParser.Util.NodeList htmlNodes = new Parser(new Lexer(tmp_result)).Parse(new TagNameFilter("meta"));
                        if (htmlNodes.Count > 1) {
                            for (int i = 0; i < htmlNodes.Count; i++) {
                                MetaTag option = (MetaTag)htmlNodes.ElementAt(i);
                                if (option.GetAttribute("http-equiv") == "Refresh") {
                                    string content = option.GetAttribute("content");
                                    string text3 = content.ToLower().Replace("url=", "`").Split('`')[1];

                                    if (!text3.Contains("http://")) {
                                        text3 = geturl(text3, url);
                                    }
                                    tmp_result = httpGET(text3, ref cookies, url, 3, 10, isRedirect) + tmp_result;
                                }
                            }
                        }

                    }
                }
                httpWebResponse.Close();
                httpWebRequest.Abort();
                result = tmp_result;

                if (!url.Contains(":8888") && !url.Contains("renzhe") && !url.Contains("zq535228") && !url.Contains("whoissoft") && !url.Contains("chinaz")) {
                    EchoHelper.Echo(string.Format("成功获取：{0}的HTML内容。", url), null, EchoHelper.EchoType.普通信息);
                }

            } catch (Exception ex) {
                result = ex.Message;
            } finally {
                if (stream != null) {
                    stream.Close();
                }
                if (httpWebResponse != null) {
                    httpWebResponse.Close();
                }
                if (httpWebRequest != null) {
                    httpWebRequest.Abort();
                }
            }
            return result;

        }

        #endregion

        #region 其他方法

        /// <summary>
        /// 获取一张随机图片。
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>返回一张>300-600宽的图片，若没有找到，返回空</returns>
        public string getImage(string keyword) {
            string re = "";
            CookieCollection cookies = new CookieCollection();
            string url = string.Format("http://image.baidu.com/i?ct=201326592&cl=2&lm=-1&tn=baiduimage&istype=2&fm=index&pv=&z=0&word={0}&s=0#z=2&width=&height=&pn=0", HttpUtility.UrlEncode(keyword, Encoding.Default));
            string html = new xkHttp().httpGET(url, ref cookies);
            ArrayList al = RegexHelper.getMatchs(html, "\"objURL\":\"(.*?)     \"", 1);
            string str = StringHelper.getRandStr(ArrayHelper.getStrs(al));
            Image img = new xkHttp().httpPic(str, "", ref cookies);
            if (img.Width > 300 && img.Width < 600) {
                re = string.Format("<img name='renzhex2' src=\"{0}\" alt=\"\" border=\"0\" />", str);
            } else {
                re = string.Format("<img name='renzhex2' src=\"{0}\" alt=\"\" border=\"0\" width=\"500\" height=\"{1}\" />", str, img.Height * 500 / img.Width);
            }

            return re;
        }


        public Encoding getEncoding(string url) {
            lock ("getEncoding") {
                try {
                    Thread.Sleep(700);
                    if (!url.Contains("www.renzhe.org")) {
                        EchoHelper.Echo("自动分析网页编码，请稍等...", null, EchoHelper.EchoType.普通信息);
                    }
                    Encoding encode;
                    //建立HTTP请求 
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    request.Timeout = 30000;
                    request.ContentType = "text/plain";
                    //获取HTTP响应 
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    //根据http应答的http头来判断编码
                    string characterSet = response.CharacterSet;

                    if (characterSet != "") {
                        if (characterSet == "ISO-8859-1") {
                            characterSet = "gb2312";
                        }
                        encode = Encoding.GetEncoding(characterSet);
                    } else {
                        encode = Encoding.Default;
                    }
                    //获取HTTP响应流 
                    Stream stream = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream, encode);
                    string text = sr.ReadToEnd();//获取到html代码到text中 

                    //第二次判断网页编码
                    Regex reg = new Regex(@"<meta[\s\S]+?charset=(.*?)""[\s\S]+?>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    MatchCollection mc = reg.Matches(text);
                    if (mc.Count > 0) {
                        string tempCharSet = mc[0].Result("$1");
                        if (string.Compare(tempCharSet, characterSet, true) != 0) {
                            encode = Encoding.GetEncoding(tempCharSet);
                        }
                    }

                    stream.Close();
                    if (!url.Contains("www.renzhe.org")) {
                        EchoHelper.Echo("网页编码分析成功，该URL的编码是：" + encode.EncodingName + "，停顿1秒后继续...", null, EchoHelper.EchoType.普通信息);
                    }
                    Thread.Sleep(700);
                    return encode;
                } catch {
                    return Encoding.Default;
                }
            }
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) {
            return true;
        }

        //定义（引用）API函数  
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

        /// <summary>
        /// urlpart，就是获取到的路径，如果这个路径不是全路径，那么通过此函数将自动补全。
        /// </summary>
        /// <param name="url"></param>
        /// <param name="urlpart"></param>
        /// <returns></returns>
        public string getDealUrl(string url, string urlpart) {
            if (!new Regex(@"^http://").IsMatch(urlpart) && !string.IsNullOrEmpty(url)) {
                if (!new Regex(@"^/").IsMatch(urlpart)) {//不是/开头，那么
                    string tmppath = new Uri(url).AbsolutePath;
                    urlpart = "/" + tmppath.Remove(tmppath.LastIndexOf("/"), tmppath.Length - tmppath.LastIndexOf("/")) + "/" + urlpart;
                    urlpart = RegexHelper.regReplace(urlpart, "/./", "/");
                    urlpart = RegexHelper.regReplace(urlpart, "(?>/.+?/)\\.\\./", "/");
                    urlpart = RegexHelper.regReplace(urlpart, "(?>/.+?/)\\.\\./", "/");
                    urlpart = RegexHelper.regReplace(urlpart, "(?>/.+?/)\\.\\./", "/");
                    urlpart = RegexHelper.regReplace(urlpart, "(?>/.+?/)\\.\\./", "/");
                }
                urlpart = new Uri(url).GetLeftPart(UriPartial.Authority) + urlpart;
            }
            urlpart = getDealUrl(urlpart);
            return urlpart;
        }


        public string getDealUrl(string url) {
            if (!string.IsNullOrEmpty(url)) {
                url = url.Replace("//", "/").Replace(":/", "://");
            }
            return url;
        }

        //判断当前是否连接Internet
        public bool httpStatus() {
            int lfag = 0;
            bool IsInternet;
            if (InternetGetConnectedState(out lfag, 0))
                IsInternet = true;
            else
                IsInternet = false;
            return IsInternet;
        }

        public bool httpStatus(string url) {
            bool result = false;
            lock ("httpStatus") {
                try {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    request.Referer = url;
                    request.Timeout = 3000;
                    request.ContentType = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    result = response.StatusCode == HttpStatusCode.OK;
                } catch {
                    return false;
                }

            }

            return result;
        }

        private string geturl(string newurl, string oldurl) {
            newurl = newurl.Trim();
            if (newurl.Length > 7 && (newurl.ToLower().Substring(0, 7) == "http://" || newurl.ToLower().Substring(0, 8) == "https://")) {
                return newurl;
            }
            if (newurl == "") {
                return oldurl;
            }
            string result = "";
            Uri uri = new Uri(oldurl);
            string port = uri.Port == 80 ? "" : ":" + uri.Port;
            if (newurl.Substring(0, 1) == "/") {

                result = "http://" + uri.Host + port + newurl;
            } else {
                string text = uri.PathAndQuery;
                if (text.Substring(text.Length - 1, 1) == "/") {
                    result = "http://" + uri.Host + port + text + newurl;
                } else {
                    if (uri.PathAndQuery.Contains("/")) {
                        text = text.Replace(uri.PathAndQuery.Split(new char[]
						{
							'/'
						})[uri.PathAndQuery.Split(new char[]
						{
							'/'
						}).Length - 1], "");
                    } else {
                        text = "/";
                    }
                    result = "http://" + uri.Host + port + text + newurl;
                }
            }
            return result;
        }


        #endregion


        public void test2() {
            string tmp = "";
            //tmp = getDealUrl("http://www.163.com/111.html", "ad.html");
            //tmp = getDealUrl("http://www.163.com/111.html", "/ad.html");
            //tmp = getDealUrl("http://www.163.com/111.html", "http://www.163.com/111.html");

            //tmp = getDealUrl("http://www.163.com/111/111.html", "ad.html");
            //tmp = getDealUrl("http://www.163.com/111/111.html", "/ad.html");
            //tmp = getDealUrl("http://www.163.com/111/111.html", "http://www.163.com//111111.html");
            //tmp = getDealUrl("", "http://www.163.com//111111.html");
            //tmp = getDealUrl("http://www.163.com//111111.html", "");

            //tmp = getDealUrl("", "");
            //tmp = getDealUrl("http://www.163.com/111/111.html", "./222.html");
            //tmp = getDealUrl("http://www.163.com/111/111.html", "../333/444.html");
            //tmp = getDealUrl("http://www.163.com/aaa/bbb/ccc/ddd.html", "./333/444.html");
            //tmp = getDealUrl("http://www.163.com/aaa/bbb/ccc/ddd.html", "../333/444.html");
            tmp = getDealUrl("http://www.163.com/aaa/bbb/ccc/ddd.html", "../../333/444.html");
            tmp = getDealUrl("http://www.163.com/aaa/bbb/ccc/ddd.html", "../../../333/444.html");

            getImage("奥巴马");
        }
    }
}
