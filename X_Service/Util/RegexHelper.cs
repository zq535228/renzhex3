using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace X_Service.Util {
    public class RegexHelper {

        /// <summary>
        /// 在HTML中查找text，其中Text就是正则表达式{asdf(.*)asdf},返回()中的内容
        /// 例如传入的是html
        /// 查找的是 {abcd(.*?)efg}
        /// 那么返回的就是(.*?)中的内容。
        /// 相当于getMatch( string html , string pattern , 1 )
        /// </summary>
        /// <param name="html"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string getHtmlRegexText(string html, string text) {
            if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(text)) {
                return "";
            }
            html = html.Replace("\n", "`");
            Regex r = new Regex("{(.*?)}", RegexOptions.Multiline);
            MatchCollection m = r.Matches(text);
            int i = 0;
            foreach (Match math in m) {
                Match m1 = Regex.Match(html, math.Groups[1].Value, RegexOptions.Multiline);
                if (m1.Success) {
                    text = text.Replace("{" + math.Groups[1].Value + "}", m1.Groups[1].Value);
                }
                i++;
            }
            return text;

        }

        //这里的所有匹配,正则的写法中 前面的字符一定要唯一  否则就是最大化匹配
        public static string getMatch(string html, string pattern) {
            return getMatch(html, pattern, 0);
        }
        public static string getMatch(string html, string pattern, int groupid) {
            return getMatch(html, pattern, groupid, false);
        }
        public static string getMatch(string html, string pattern, int groupid, bool mutilLine) {
            if (mutilLine) {
                html = html.Replace("\n", "`");
            }
            pattern = getFixPattern(pattern);

            string re = "";
            try {
                Match mc = Regex.Match(html, pattern, RegexOptions.IgnoreCase);
                re = mc.Groups[groupid].Value;
            } catch (Exception ex) {
                EchoHelper.EchoException(ex);
            }
            return re;

        }
        public static string getMatchs(string html, string pattern, int groupid, string split) {
            string re = "";
            pattern = getFixPattern(pattern);

            ArrayList al = getMatchs(html, pattern, groupid);
            for (int i = 0; i < al.Count; i++) {
                if (i == 1) {
                    re += al[i].ToString();
                } else {
                    re += split + al[i].ToString();
                }
            }
            return re;
        }


        public static ArrayList getMatchs(string html, string pattern) {
            pattern = getFixPattern(pattern);

            ArrayList al = new ArrayList();
            try {
                MatchCollection mcs = Regex.Matches(html, pattern, RegexOptions.IgnoreCase);
                foreach (Match mc in mcs) {
                    al.Add(mc.Value);
                }

            } catch { }
            return al;
        }

        public static ArrayList getMatchs(string html, string pattern, int groupid) {
            pattern = getFixPattern(pattern);
            ArrayList al = new ArrayList();
            try {
                if (!string.IsNullOrEmpty(html) && !string.IsNullOrEmpty(pattern)) {
                    MatchCollection mcs = Regex.Matches(html, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    foreach (Match mc in mcs) {
                        string tmp = mc.Groups[groupid].Value.Replace("`", "");
                        al.Add(tmp);
                    }
                }

            } catch { }
            return al;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pattern"></param>
        /// <param name="newStr"></param>
        /// <returns></returns>
        public static string regReplace(string html, string pattern, string newStr) {
            pattern = getFixPattern(pattern);

            string oldStr = getMatch(html, pattern);
            if (oldStr.Length > 0) {
                html = html.Replace(oldStr, newStr);
            }
            return html;
        }

        /// <summary>
        /// 多个内容替换
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pattern"></param>
        /// <param name="newStr"></param>
        /// <returns></returns>
        public static string regReplaces(string html, string pattern, string newStr) {
            pattern = getFixPattern(pattern);

            ArrayList al = getMatchs(html, pattern);
            for (int i = 0; i < al.Count; i++) {
                if (al[i].ToString().Length > 0) {
                    html = html.Replace(al[i].ToString(), newStr);
                }
            }
            return html;
        }

        /// <summary>
        /// 对pattern，进行转义处理。
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private static string getFixPattern(string pattern) {
            if (!pattern.Contains("\\\"")) {
                pattern = pattern.Replace("\"", "\\\"");
            }
            return pattern;
        }


    }
}
