using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using X_Service.Web;
using System.Net;
using System.Threading;
using Winista.Text.HtmlParser.Util;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Tags;

namespace X_Service.Util {
    public class SeoHelper {


        public static bool isWWW(string url) {
            bool re = false;
            try {
                if (url.Trim().Length > 0) {
                    if (new Uri(url).Host.Contains("www.") || (new Uri(url).Host.Split('.').Length < 3 && new Uri(url).Host.Split('.').Length > 1)) {
                        re = true;
                    }
                }
            } catch {
            }
            return re;
        }
        /// <summary>
        /// 获取某域名的IP
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getIp(string url) {
            string result = "";
            try {
                url = new Uri(url).Host;
                IPAddress[] IPs = Dns.GetHostAddresses(url);

                for (int i = 0; i < IPs.Length; i++) {
                    string ip = IPs[i].ToString();
                    string v_ip = RegexHelper.getMatch(ip, @"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+");
                    if (!string.IsNullOrEmpty(v_ip)) {
                        result += v_ip + ",";
                    }
                }
                result = result.Substring(0, result.IndexOf(","));
            } catch (Exception) {
                result = "000.000.000.000";
            }
            return result;
        }

        /// <summary>
        ///  搜索引擎查询
        /// </summary>
        public enum EnumSearchEngine {
            Google = 1,
            Baidu = 2,
        }


        /// <summary>
        ///  获取某站点关键字在某搜索引擎的排名
        /// </summary>
        /// <param name="_engine"></param>
        /// <param name="Url"></param>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        public static string engineKeyWordinfo(EnumSearchEngine _engine, string Url, string KeyWord) {
            string re = "200+";
            string Html = "";
            string searchUrl = "";
            switch (_engine) {
                case EnumSearchEngine.Baidu:
                    EchoHelper.Echo("关键词【" + KeyWord + "】的排名查询中，请稍后...", "", EchoHelper.EchoType.普通信息);
                    searchUrl = "http://i.linkhelper.cn/searchkeywords.asp";
                    string postData = StringHelper.urlencode("url=" + Url + "&querywords=" + KeyWord + "&btnsubmit=%B2%E9+%D1%AF", "gb2312");
                    CookieCollection cookies = new CookieCollection();
                    Html = new xkHttp().httpPost(searchUrl, postData, ref cookies, searchUrl, Encoding.GetEncoding("gb2312"));
                    re = RegexHelper.getMatch(Html, "百度排名：(.*?)&nbsp;&nbsp;", 1);
                    if (re.Contains("排名在")) {
                        re = "200+";
                    }
                    break;
                case EnumSearchEngine.Google:
                    break;
            }
            return re;
        }

        private void engineKeyWordinfoTest() {
            engineKeyWordinfo(EnumSearchEngine.Baidu, "www.langmei-bao.info", "浪美包包");
        }

        /// <summary>
        ///  站点收录信息
        /// </summary>
        /// <param name="_engine"></param>
        /// <returns></returns>
        public static int getBaiduNum(EnumSearchEngine _engine, string url) {
            //EchoHelper.Echo("网站【" + url + "】的收录查询中，请稍后...", "", EchoHelper.EchoType.普通信息);
            string regStart = "找到相关结果数";
            string regEnd = "个";
            string siteUrl = "http://www.baidu.com/s?wd=site%3A" + url;

            CookieCollection cookies = new CookieCollection();
            string html = new xkHttp().httpGET(siteUrl, ref cookies, siteUrl);
            string Result = StringHelper.GetMetaString(html, regStart, regEnd, true);
            int re = 9999;
            try {
                re = Convert.ToInt32(Result);
            } catch {
                re = 0;
            }
            if (html.Contains("没有找到与")) {
                re = 0;
            }
            return re;
        }


        public static int checkDomain(string domain) {
            if (domain.Contains("http://")) {
                domain = new Uri(domain.Trim()).Host.Replace("www.", "");
            } else {
                domain = domain.Trim();
            }
            int re = 0;
            CookieCollection cookies = new CookieCollection();
            string html = new xkHttp().httpGET("http://whoissoft.com/" + domain, ref cookies);
            if (html.Contains("No match for domain")) {
                re = 1;
            }
            return re;
        }

        public static DateTime getDomainExpired(string domain) {
            if (domain.Contains("http://")) {
                domain = new Uri(domain.Trim()).Host.Replace("www.", "");
            } else {
                domain = domain.Trim().Replace("www.", "");
            }
            int l = domain.Split('.').Length;
            if (l > 2) {
                domain = domain.Split('.')[l - 2] + "." + domain.Split('.')[l - 1];
            }
            DateTime re = DateTime.Now.AddDays(-1);
            CookieCollection cookies = new CookieCollection();
            string html = new xkHttp().httpGET("http://tool.chinaz.com/DomainDel/?wd=" + domain, ref cookies);

            //循环5次，去抓取域名信息
            int i = 0;
            while (html.Contains("没有查询到相应的信息.")) {
                html = new xkHttp().httpGET("http://tool.chinaz.com/DomainDel/?wd=" + domain, ref cookies);
                i++;
                if (i > 10) {
                    break;
                }
            }
            if (html.Contains("域名到期时间")) {
                string edate = RegexHelper.getHtmlRegexText(html, "{域名到期时间</td><td class=\"deltd1\">(.*?)</td></tr>}");
                DateTime dt = new DateTime(Convert.ToInt32(edate.Split('-')[0]), Convert.ToInt32(edate.Split('-')[1]), Convert.ToInt32(edate.Split('-')[2]));
                re = dt;
            }

            return re;
        }

        //获取域名是否已经备案
        public static int getDomainISBeiAN(string domain) {
            int re = 0;
            if (domain.Contains("http://")) {
                domain = new Uri(domain.Trim()).Host.Replace("www.", "");
            } else {
                domain = domain.Trim().Replace("www.", "");
            }
            int l = domain.Split('.').Length;
            if (l > 2) {
                domain = domain.Split('.')[l - 2] + "." + domain.Split('.')[l - 1];
            }
            CookieCollection cookies = new CookieCollection();
            string html = new xkHttp().httpGET("http://www.15so.com/baidu/index.html?type=0&kword=" + domain, ref cookies);
            if (html.Contains("查看详细备案信息")) {
                re = 1;
            }
            return re;
        }


        public void testdate() {
            getDomainExpired("renzhe.org");
        }



    }
}
