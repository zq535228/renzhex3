using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Util;
using Winista.Text.HtmlParser.Tags;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Lex;
using System.Web;
using X_Service.Web;
using X_Service.Util;
using System.Collections;
using Winista.Text.HtmlParser.Visitors;

namespace X_Service.Fetch {
    public class FetchContent {

        /// <summary>
        /// 把内容中的图片，相对地址变为绝对地址。
        /// </summary>
        /// <param name="tmp_content"></param>
        /// <param name="url"></param>
        public static void ImageSrc(ref string tmp_content, string url) {
            //如果遇到使用绝对路径的图片，转换为全路径。
            NodeList htmlNodes = new Parser(new Lexer(tmp_content.Replace("<IMG", "<img").Replace("<Img", "<img"))).Parse(new TagNameFilter("img"));

            for (int j = htmlNodes.Count - 1; j >= 0; j--) {
                ImageTag link = (ImageTag)htmlNodes.ElementAt(j);

                string urlpart = link.GetAttribute("src");

                if (!string.IsNullOrEmpty(urlpart) && !new Regex(@"^http:").IsMatch(urlpart)) {

                    urlpart = new xkHttp().getDealUrl(url, urlpart);
                    string oldlink = link.ToHtml();

                    link.RemoveAttribute("src");
                    link.RemoveAttribute("onclick");
                    string newsrc = "src=\"" + urlpart + "\" ";
                    string newlink = link.ToHtml();
                    newlink = newlink.Insert(5, newsrc);
                    tmp_content = tmp_content.Replace(oldlink, newlink);
                    //EchoHelper.Echo("成功转换了一个图片的SRC属性！", "", EchoHelper.EchoType.普通信息);
                }
                if (!string.IsNullOrEmpty(urlpart) && new Regex(@"^\.\.").IsMatch(urlpart)) {
                    string oldlink = link.ToHtml();
                    tmp_content = tmp_content.Replace(oldlink, "");
                }
            }


        }

        public static void Filter(ref string tmp_title, ref string tmp_content, string strreps, bool noimg, bool no2br) {
            //内容过滤循环
            string strrep = "<!--.*?-->→\n";
            strrep += "<div.*?>→\n";
            strrep += "</div>→\n";
            strrep += "<span.*?>→\n";
            strrep += "</span>→\n";
            strrep += "<li.*?>→\n";
            strrep += "</li>→\n";
            strrep += "<ul.*?>→\n";
            strrep += "</ul>→\n";
            strrep += "<font.*?>→\n";
            strrep += "</font>→\n";
            strrep += "<iframe.*?frame>→\n";
            strrep += "<.*?form.*?>→\n";
            strrep += "<.*?input.*?>→\n";
            strrep += "<.*?textarea.*?>→\n";
            strrep += "%26[a-zA-Z]+→\n";
            strrep += "===→\n";
            strrep += "///→\n";
            strrep += "重要提醒.*?封闭→\n";
            strrep += "网易博客安全提醒.*?封闭→\n";
            strrep += "重要提醒.*?关闭→\n";
            strrep += "网易博客安全提醒.*?关闭→\n";


            if (no2br) {
                strrep += "<a.*?>→\n";
                strrep += "</a>→\n";
                //表格内容
                strrep += "<tr>\n";
                strrep += "<tr.*?>→\n";
                strrep += "</tr>→\n";
                strrep += "<td.*?>→\n";
                strrep += "</td>→\n";
                strrep += "<table.*?>→\n";
                strrep += "</table>→\n";
                strrep += "<tbody.*?>→\n";
                strrep += "</tbody>→\n";
            }

            strrep += @"<script[^>]*?>([\s|\S])*?</script>→" + Environment.NewLine;
            strrep += @"<style[^>]*?>([\s|\S])*?</style>→" + Environment.NewLine;
            if (noimg) {
                strrep += "<img.*?>→\n";
            }
            strrep += strreps;

            if (!string.IsNullOrEmpty(strrep)) {
                string[] StrReplace = strrep.Split('\n');
                for (int i = 0; i < StrReplace.Length; i++) {
                    string tmpreg = StrReplace[i].ToString();
                    tmpreg = tmpreg.Trim();
                    if (!tmpreg.Contains("→") && !string.IsNullOrEmpty(tmpreg)) {
                        tmp_title = tmp_title.Replace(tmpreg, "");
                        tmp_content = tmp_content.Replace(tmpreg, "");
                    } else if (!string.IsNullOrEmpty(tmpreg)) {
                        string tmp01 = tmpreg.Split('→')[0].ToString();
                        string tmp02 = tmpreg.Split('→')[1].ToString();
                        tmp_title = RegexHelper.regReplaces(tmp_title, tmp01, tmp02);
                        tmp_content = RegexHelper.regReplaces(tmp_content, tmp01, tmp02);
                    }
                }
            }

            tmp_title = StringHelper.FormatTitle(tmp_title).Replace("(组图", "").Replace("(图", "");
            if (no2br) {
                tmp_content = StringHelper.FormatHtml(tmp_content);
            } else {
                tmp_content = StringHelper.NoHtml(tmp_content);
            }

        }

        public static string GetKeywords(string url) {
            string re = "";
            try {
                CookieCollection cookies = new CookieCollection();
                string html = new xkHttp().httpGET(url, ref cookies).ToLower();
                NodeFilter filter = new HasAttributeFilter("name", "keywords");
                NodeList htmlNodes = new Parser(new Lexer(html.ToLower())).Parse(filter);
                ITag t = (MetaTag)htmlNodes[0];

                if (t.Attributes != null && t.Attributes.Count > 0) {
                    re = t.Attributes["CONTENT"].ToString();
                }
            } catch {
            }
            return re;
        }

        public static void GetContentFromUrl(string url, ref string title, ref string content) {
            EchoHelper.EchoPickStart();
            try {
                url = HttpUtility.UrlDecode(url);
                nextPages = new ArrayList();
                while (url != "") {
                    string html = FetchContent.GetDataFromUrl(url);
                    nextPages.Add(url);
                    if (string.IsNullOrEmpty(title)) {
                        title = RegexHelper.getHtmlRegexText(html, "{<title>(.*?)</title>}");
                        title = RegexHelper.regReplace(title, "_.*", "");
                        title = RegexHelper.regReplace(title, "-.*", "");
                        title = title.Replace("&nbsp;", "");
                    }
                    content += FetchContent.GetMainContent(html);
                    url = FetchContent.GetNextPageUrl(html, url);
                    url.Trim();
                }
                if (title.Contains("<title>(.*")) {
                    title = StringHelper.SubString(content, 0, 50);
                }
            } catch {
                title = "";
                content = "";
                EchoHelper.Echo("采集跳过，原因可能是：该文章设置了密码、被删除、乱码等。", "采集出错", EchoHelper.EchoType.普通信息);
            }
            EchoHelper.EchoPickEnd();
        }

        public static void GetContentFromUrl(string url, ref string tmp_title, ref string tmp_content, string treg, string creg) {
            EchoHelper.EchoPickStart();

            tmp_title = tmp_title.Replace("[标题]", "(.*?)");
            tmp_content = tmp_content.Replace("[正文]", "(.*?)");
            nextPages = new ArrayList();

            while (url != "") {
                string html = FetchContent.GetDataFromUrl(url);
                nextPages.Add(url);
                if (string.IsNullOrEmpty(tmp_title)) {
                    tmp_title = RegexHelper.getMatch(html, treg, 1);
                }
                //内容正则循环
                if (!string.IsNullOrEmpty(creg)) {
                    string[] contentRegexs = creg.Split('\n');
                    for (int i = 0; i < contentRegexs.Length; i++) {
                        string tmp = RegexHelper.getMatchs(html.Replace("\n", "`"), contentRegexs[i].ToString().Trim(), 1, "\r\n").Replace("`", "\n");
                        tmp_content += tmp;
                        tmp_content += Environment.NewLine;
                    }
                }
                url = FetchContent.GetNextPageUrl(html, url);

            }
            EchoHelper.EchoPickEnd();
        }



        public static string GetTitleFromUrl(string url) {
            url = HttpUtility.UrlDecode(url);
            string result = string.Empty;
            string html = FetchContent.GetDataFromUrl(url);
            return GetTitleFromHTML(result);
        }

        public static string GetTitleFromHTML(string html) {
            string result = string.Empty;
            string reg = "{<title>(.*?)</title>}";
            result = RegexHelper.getHtmlRegexText(html, reg);

            int indexof = result.IndexOf("_") < result.IndexOf("-") ? result.IndexOf("_") : result.IndexOf("-");
            if (indexof == -1) {
                indexof = result.IndexOf("-");
            }
            if (indexof > 0) {
                result = result.Remove(indexof, result.Length - indexof - 1);
            }
            return result;
        }


        /// <summary>
        /// 函数名称：GetDataFromUrl
        /// 功能说明：获取url指定的网页的源码
        /// 参数：string url用于指定 url
        /// 参数：ref Encoding encode用来获取网页中的字符集编码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string GetDataFromUrl(string url) {
            EchoHelper.Echo("尝试链接：" + url + "，请稍等...", "内容下载", EchoHelper.EchoType.任务信息);
            CookieCollection cookies = new CookieCollection();
            string html = new xkHttp().httpGET(url, ref cookies);
            return html;
        }

        /// <summary>
        /// 函数名称：ModifyRawText
        /// 功能说明：将html源码中的字符串进行转义以便正常显示
        /// 参数： ref string rawtext表示待修改的html源码
        /// </summary>
        /// <param name="rawtext"></param>
        public static void ModifyRawText(ref string rawtext) {
            string[] aryReg ={
         
          @"&(quot;|#34;)",
          @"&(amp;|#38;)",
          @"&(lt;|#60;)",
          @"&(gt;|#62;)", 
          @"&(nbsp;|#160;)", 
          @"&(iexcl;|#161;)",
          @"&(cent;|#162;)",
          @"&(pound;|#163;)",
          @"&(copy|;#169;)",
          @"&#(\d+);",
         
         
         };
            string[] aryRep = {
          
           "\"",
           "&",
           "<",
           ">",
           "  ",
           "\xa1",//chr(161),
           "\xa2",//chr(162),
           "\xa3",//chr(163),
           "\xa9",//chr(169),
           "",
          
          };
            //转义字符置换
            for (int i = 0; i < aryReg.Length; i++) {
                Regex regexfinal = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                rawtext = regexfinal.Replace(rawtext, aryRep[i]);
            }



        }

        /// <summary>
        /// 函数名称：ItemRetrival_1
        /// 功能说明：用于提取帖子列表页面的url,帖子标题，帖子时间
        /// 参数：string url表示帖子列表url
        /// 参数 ref Encoding encode 用于获取网页字符集编码
        /// 参数： ref List<string> listUrl,listTitle,listTime用于存放提取出的各项信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encode"></param>
        /// <param name="listurl"></param>
        /// <param name="listtitle"></param>
        /// <param name="listtime"></param>
        public static void ItemRetrival_1(string url, ref Encoding encode, ref List<string> listUrl, ref List<string> listTitle,
                                          ref List<string> listTime) {
            //获取网页源码；
            string rawtext = GetDataFromUrl(url);
            //将无关的style，script等标签去掉；
            string reg1 = @"<style[\s\S]+?/style>|<select[\s\S]+?/select>|<script[\s\S]+?/script>|<\!\-\-[\s\S]*?\-\->";
            rawtext = new Regex(reg1, RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(rawtext, "");


            //以下用htmlparser提取源码中的目标table;

            Lexer lexer = new Lexer(rawtext);
            //解析出其中的table元素
            Parser parser = new Parser(lexer);
            NodeFilter filter = new TagNameFilter("table");
            NodeList htmlNodes = parser.Parse(filter);
            //去除嵌套式table
            Regex f1 = new Regex(@"<table.*?>");
            for (int i = htmlNodes.Count - 1; i >= 0; i--) {
                MatchCollection myCollection = f1.Matches(htmlNodes[i].ToHtml());
                if (myCollection.Count > 1)
                    htmlNodes.Remove(i);

            }

            //去除没有时间的table，认为这种table是无效table
            Regex f2 = new Regex(@"\d\d:\d\d");
            for (int i = htmlNodes.Count - 1; i >= 0; i--) {
                if (!f2.IsMatch(htmlNodes[i].ToHtml()))
                    htmlNodes.Remove(i);

            }



            //以下程序解析出以上三种目标信息

            string final = htmlNodes.ToHtml();
            Lexer lex2 = new Lexer(final);
            Parser par2 = new Parser(lex2);
            NodeFilter filter2 = new TagNameFilter("tr");
            NodeList finalNodes = par2.Parse(filter2);
            //提取发帖时间信息
            RegexFilter rf = new RegexFilter(@"\d\d:\d\d");
            for (int i = 0; i < finalNodes.Count; i++) {
                Lexer lexerTmp = new Lexer(finalNodes[i].ToHtml());
                Parser parserTmp = new Parser(lexerTmp);
                NodeList tmp = parserTmp.Parse(rf);
                if (tmp.Count > 0)
                    for (int j = 0; j < tmp.Count; j++) {
                        string temp = tmp[j].ToHtml();
                        ModifyRawText(ref temp);
                        listTime.Add(temp);

                    }

            }
            //提取帖子URL以及帖子标题
            string atagAssist = finalNodes.ToHtml();
            Lexer lex3 = new Lexer(atagAssist);
            Parser par3 = new Parser(lex3);
            NodeFilter filter3 = new TagNameFilter("a");
            NodeList atagNodes = par3.Parse(filter3);
            string urlpart = new Regex(@"http://.*?(?=/)").Match(url).Value;
            for (int i = 0; i < atagNodes.Count; i++) {
                ATag link = (ATag)atagNodes.ElementAt(i);
                string temp1 = link.GetAttribute("href");
                string temp2 = link.StringText;

                if (temp1 != null && !new Regex("http").IsMatch(temp1))//如果提取出的url为相对url,则加上域名补全为绝对url
                {
                    temp1 = urlpart + temp1;//将提取出的url构造完整，形成完整的url
                }
                ModifyRawText(ref temp2);
                listUrl.Add(temp1);
                listTitle.Add(temp2);
            }

        }


        /// <summary>
        /// 函数名称：ItemRetrival_2
        /// 功能说明：用于提取帖子列表页面的url,帖子标题，帖子时间
        /// 参数：string url表示帖子列表url
        /// 参数 ref Encoding encode 用于获取网页字符集编码
        /// 参数： ref List<string> listUrl,listTitle,listTime用于存放提取出的各项信息
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encode"></param>
        /// <param name="listurl"></param>
        /// <param name="listtitle"></param>
        /// <param name="listtime"></param>
        public static void ItemRetrival_2(string url, ref Encoding encode, ref List<string> listUrl, ref List<string> listTitle,
                                            ref List<string> listTime) {

            //获取网页源码；
            string rawtext = GetDataFromUrl(url);
            string reg1 = @"<style[\s\S]+?/style>|<select[\s\S]+?/select>|<script[\s\S]+?/script>|<\!\-\-[\s\S]*?\-\->";
            rawtext = new Regex(reg1, RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(rawtext, "");
            //将无关的style，script等标签去掉；
            //以下操作用于提取帖子页面的发帖时间、帖子URL，帖子标题等信息
            //用htmlparser获取目标li元素
            Lexer lexer = new Lexer(rawtext);
            Parser parser = new Parser(lexer);
            NodeFilter filter = new TagNameFilter("li");//解析出其中的li元素
            NodeList htmlNodes = parser.Parse(filter);
            //去掉其中不含有时间的条目
            Regex f2 = new Regex(@"\d\d:\d\d");
            for (int i = htmlNodes.Count - 1; i >= 0; i--) {
                if (!f2.IsMatch(htmlNodes[i].ToHtml()))
                    htmlNodes.Remove(i);

            }
            RegexFilter rf = new RegexFilter(@"\d\d:\d\d");
            string final = htmlNodes.ToHtml();
            for (int i = 0; i < htmlNodes.Count; i++) {
                Lexer lexerTmp = new Lexer(htmlNodes[i].ToHtml());
                Parser parserTmp = new Parser(lexerTmp);
                NodeList tmp = parserTmp.Parse(rf);
                if (tmp.Count > 0)
                    for (int j = 0; j < tmp.Count; j++) {
                        string temp = tmp[j].ToHtml();
                        ModifyRawText(ref temp);
                        listTime.Add(temp);

                    }
            }


            //提取帖子url和标题
            string atagAssist = htmlNodes.ToHtml();
            Lexer lex3 = new Lexer(atagAssist);
            Parser par3 = new Parser(lex3);
            NodeFilter filter3 = new TagNameFilter("a");
            NodeList atagNodes = par3.Parse(filter3);
            for (int i = 0; i < atagNodes.Count; i++) {
                string urlpart = new Regex(@"http://.*?(?=/)").Match(url).Value;
                ATag link = (ATag)atagNodes.ElementAt(i);
                string temp1 = link.GetAttribute("href");
                string temp2 = link.StringText;

                if (temp1 != null && !new Regex("http").IsMatch(temp1))//如果提取出的url为相对url,则加上域名补全为绝对url
                {
                    temp1 = urlpart + temp1;//将提取出的url构造完整，形成完整的url
                }
                ModifyRawText(ref temp2);
                listUrl.Add(temp1);
                listTitle.Add(temp2);
            }



        }


        /// <summary>
        /// 函数名称：GetPattern
        /// 功能说明：用于判定索引页正文是储存在Li中还是table中
        /// 参数：string rawtext 去掉style 等无关标签之后的网页源码
        /// 返回值 bool  true表明是table型；false表明是li型
        /// </summary>
        /// <param name="rawtext"></param>
        /// <returns></returns>
        public static bool GetPattern(string rawtext) {
            Lexer lexer = new Lexer(rawtext);
            Parser parser = new Parser(lexer);
            NodeFilter filter = new TagNameFilter("li");//解析出其中的li元素
            NodeList htmlNodes = parser.Parse(filter);
            if (htmlNodes.Count == 0) {
                return true;//如果源码中不含有li元素则该索引页属于table型。
            } else {
                //去掉其中不含有时间的条目
                Regex f2 = new Regex(@"\d\d:\d\d");
                for (int i = htmlNodes.Count - 1; i >= 0; i--) {
                    if (!f2.IsMatch(htmlNodes[i].ToHtml()))
                        htmlNodes.Remove(i);

                }

                if (htmlNodes.Count == 0)//如果网页源码中含有li元素，但是li元素中不含有带发布时间的连接，则该索引页属于table型
                    return true;
                else//否则为li型
                    return false;
            }
        }


        /// <summary>
        /// 函数名称：CompareDinosByChineseLength
        /// 函数功能：判断两段儿文本里哪个中文占的比例高
        /// 函数参数：待比较的两个文本块x,y
        /// 函数返回值：1表明文本块x中中文比例高于y,0则反之
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>

        public static int CompareDinosByChineseLength(string x, string y) {
            if (x == null) {
                if (y == null) {
                    return 0;
                } else {
                    return -1;
                }
            } else {
                if (y == null) {
                    return 1;
                } else {
                    Regex r = new Regex("[\u4e00-\u9fa5]");//UTF8 中中文汉字的范围
                    float xCount = (float)(r.Matches(x).Count) / (float)x.Length;
                    float yCount = (float)(r.Matches(y).Count) / (float)y.Length;

                    int retval = xCount.CompareTo(yCount);

                    if (retval != 0) {
                        return retval;
                    } else//如果从百分比上看不出差距，则从汉字数量上来看
                    {
                        return x.CompareTo(y);
                    }
                }
            }
        }

        /// <summary>
        /// 函数名称：GetTags
        /// 函数功能：获取一个网页源码中的标签列表，支持嵌套，一般或去div，td等容器
        /// 函数参数：input网页源码，tag指定要获取列表的表签
        /// 函数返回值：源码中某一标签的标签列表
        /// </summary>
        /// <param name="inputHtml"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static List<string> GetTags(string inputHtml, string tag) {
            StringReader strReader = new StringReader(inputHtml);
            int lowerThanCharCounter = 0;//标记是否出现<,如果出现了，其值为1，否则其值为0
            int lowerThanCharPos = 0;//记录标签<...>的开始位置
            Stack<int> tagPos = new Stack<int>();//
            List<string> taglist = new List<string>();
            int i = 0;//下标记录当前char在整个字符串中的位置
            while (true) {
                try {
                    int intCharacter = strReader.Read();
                    if (intCharacter == -1)
                        break;//退出循环条件

                    char convertedCharacter = Convert.ToChar(intCharacter);

                    if (lowerThanCharCounter > 0) {
                        if (convertedCharacter == '>') {
                            lowerThanCharCounter--;

                            string biaoqian = inputHtml.Substring(lowerThanCharPos, i - lowerThanCharPos + 1);//取出整个标签（标签包括开始标签和结束标签）
                            if (biaoqian.StartsWith(string.Format("<{0}", tag)))//如果是开始标签
                            {
                                tagPos.Push(lowerThanCharPos);//将"<"对应的位置压栈
                            }
                            if (biaoqian.StartsWith(string.Format("</{0}", tag)))//如果是结束标签
                            {
                                if (tagPos.Count < 1)
                                    continue;//
                                int tempTagPos = tagPos.Pop();//弹栈
                                string strdiv = inputHtml.Substring(tempTagPos, i - tempTagPos + 1);//将整个元素<...>...<...>压入保持表
                                taglist.Add(strdiv);//taglist[0],taglist[1],taglist[2]...分别是从内层到外层的元素
                            }
                        }
                    }

                    if (convertedCharacter == '<') {
                        lowerThanCharCounter++;//标记是否找到了"<"标签
                        lowerThanCharPos = i;//记录<的位置
                    }
                } finally {
                    i++;//下标指针后移
                }
            }
            return taglist;
        }


        /// <summary>
        /// 下一页的集合。
        /// </summary>
        private static ArrayList nextPages;
        /// <summary>
        /// 函数名称：GetNextPageUrl
        /// 函数功能：从网页源码中提取下一页URL，并且如果下一页URL不完整，补全下一页URL
        /// 函数参数：rawtext,当前网页源码，url 当前网页的绝对url
        /// 函数返回值：下一页的绝对url，若无下一页url则返回空字符串
        /// </summary>
        /// <param name="rawtext"></param>
        /// <returns></returns>
        public static string GetNextPageUrl(string rawtext, string url) {
            string urlpart = "";//存放提取出的URL
            try {
                NodeList htmlNodes = new Parser(new Lexer(rawtext)).Parse(new TagNameFilter("a"));
                for (int i = htmlNodes.Count - 1; i >= 0; i--) {
                    ATag link = (ATag)htmlNodes.ElementAt(i);
                    string temp = link.StringText;
                    Regex fetchNextPage = new Regex(@"下一页");
                    if (string.IsNullOrEmpty(temp) || !fetchNextPage.IsMatch(temp)) {
                        htmlNodes.Remove(i);
                    }
                }
                //如果<a>标签链表不空，则说明当前页不是最后一页，含有下一页URL，并提取出下一页URL
                if (htmlNodes.Count > 0) {
                    ATag link = (ATag)htmlNodes.ElementAt(0);
                    urlpart = link.GetAttribute("href");

                    //判断从源码中提取出的URL是否需要补全
                    if (!string.IsNullOrEmpty(urlpart) && new Regex(@"^http:").IsMatch(urlpart)) {
                    } else if (!string.IsNullOrEmpty(urlpart)) {
                        urlpart = new xkHttp().getDealUrl(url, urlpart);
                    }
                }
                if (nextPages.Contains(urlpart)) {//若下一页已经存在。
                    urlpart = "";
                } else {
                    nextPages.Add(urlpart);//加入到集合中。
                }
            } catch {
            }
            if (!string.IsNullOrEmpty(urlpart)) {
                urlpart = urlpart.Trim();
            }
            return urlpart;
        }

        /// <summary>
        /// 函数名称：PercentageOfATag
        /// 函数功能：计算某文本段内链接文字（汉字）的浓度
        /// 函数参数：text某文本段文本
        /// 函数返回值：当前文本段内链接文字个数/文本段内总的汉字个数
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static float PercentageOfATag(string text) {  //找到文本段中的所有链接元素形成链接元素表
            NodeList htmlNodes = new Parser(new Lexer(text)).Parse(new TagNameFilter("a"));
            //取出所有链接文本
            string aTagText = "";
            for (int i = 0; i < htmlNodes.Count; i++) {
                aTagText += htmlNodes.ElementAt(i).ToPlainTextString();
            }
            //计算链接文字与文本块中总汉字的比例
            Regex r = new Regex("[\u4e00-\u9fa5]");
            float precise = (float)r.Matches(aTagText).Count / (float)r.Matches(text).Count;
            return precise;

        }

        /// <summary>
        /// 函数名称：GetMainContent
        /// 函数功能：从内容页源码中获取正文
        /// 函数参数：html内容页源码
        /// 函数返回值：提纯后的文本
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string GetMainContent(string html) {
            EchoHelper.Echo("哇，好大的页面，很努力的为你寻找正文内容ing，请稍等...", null, EchoHelper.EchoType.普通信息);
            string reg1 = @"<(p|br)[^<]*>";
            //string reg2 = @"(\[([^=]*)(=[^\]]*)?\][\s\S]*?\[/\1\])|(?<lj>(?<=[^\u4E00-\u9FA5\uFE30-\uFFA0,."");《])<a\s+[^>]*>[^<]{2,}</a>(?=[^\u4E00-\u9FA5\uFE30-\uFFA0,."");》]))|(?<Style><style[\s\S]+?/style>)|(?<select><select[\s\S]+?/select>)|(?<Script><script[\s\S]*?/script>)|(?<Explein><\!\-\-[\s\S]*?\-\->)|(?<li><li(\s+[^>]+)?>[\s\S]*?/li>)|(?<Html></?\s*[^> ]+(\s*[^=>]+?=['""]?[^""']+?['""]?)*?[^\[<]*>)|(?<Other>&[a-zA-Z]+;)|(?<Other2>\#[a-z0-9]{6})|(?<Space>\s+)|(\&\#\d+\;)";

            //1、获取网页的所有div标签
            List<string> list = GetTags(html, "div");

            List<string> needToRemove = new List<string>();
            foreach (string s in list) {
                Regex r = new Regex("[\u4e00-\u9fa5]");
                if (r.Matches(s).Count < 100)//2、去除汉字少于200字的div
                {
                    needToRemove.Add(s);
                }
                if (PercentageOfATag(s) > 0.6)//3、去除链接文字浓度过高（超过60%）的div段
                    needToRemove.Add(s);
            }
            //将非目标div从div链中摘除
            foreach (string s in needToRemove) {
                list.Remove(s);
            }

            //5、把剩下的div按汉字比例多少倒序排列,
            list.Sort(CompareDinosByChineseLength);//
            html = list[list.Count - 1];
            //6、把p和br替换成特殊的占位符[p][br]
            html = new Regex(reg1, RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(html, "[$1]");

            //7、去掉HTML标签，保留汉字
            //html = new Regex(reg2, RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(html, "");

            //8、把特殊占维护替换成回车和换行
            html = new Regex(@"\[p\]", RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(html, "\r\n　　");
            html = new Regex(@"\[br\]", RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(html, "\r\n");
            html = new Regex(@"</p>", RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(html, "\r\n");
            string[] aryReg ={         
                              @"&(quot|#34)",
                              @"&(amp|#38)",
                              @"&(lt|#60)",
                              @"&(gt|#62)", 
                              @"&(nbsp|#160)", 
                              @"&(iexcl|#161)",
                              @"&(cent|#162)",
                              @"&(pound|#163)",
                              @"&(copy|#169)",
                              @"&#(\d+)",
                             };

            string[] aryRep = {
                               "\"",
                               "&",
                               "<",
                               ">",
                               "  ",
                               "\xa1",//chr(161),
                               "\xa2",//chr(162),
                               "\xa3",//chr(163),
                               "\xa9",//chr(169),
                               "",
                              };
            for (int i = 0; i < aryReg.Length; i++) {
                Regex regexfinal = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                html = regexfinal.Replace(html, aryRep[i]);
            }
            //针对腾讯新闻内容页打的小补丁
            html = new Regex(@"(·[\s\S]+?\r\n|\[\])", RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(html, "");
            html = RegexHelper.regReplace(html, "2.*?年.*?月.*?日<br /><br />", "<br /><br />");

            EchoHelper.Echo("真不容易哇，提取成功了。...", "正文提取", EchoHelper.EchoType.普通信息);
            return html;
        }


        public void test() {
            string t1 = "<br/>";
            string t2 = RegexHelper.regReplace(t1, "<.*?>", "");

        }

    }
}
