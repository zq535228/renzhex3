using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace X_Service.Web {

    /// <summary>
    /// 这里是操作WebBrowser的一些常见的方法,
    /// </summary>
    public class WebBrowserHelper {

        public static void Write_value(WebBrowser wb, string name, string value) {
            try {
                foreach (HtmlElement f in wb.Document.GetElementsByTagName("option")) {
                    if (f.OuterHtml.Contains("value=" + value)) {
                        f.SetAttribute("selected", "selected");
                    } else {
                        f.SetAttribute("selected", "");
                    }
                }
                wb.Document.All[name].RaiseEvent("onchange");

                foreach (HtmlElement f in wb.Document.All.GetElementsByName(name)) {
                    if (f.OuterHtml.Contains("type=radio") || f.OuterHtml.Contains("type=checkbox")) {
                        if (f.OuterHtml.Contains("value=" + value)) {
                            f.InvokeMember("click");
                        }
                    } else {
                        f.SetAttribute("value", value);
                    }
                }
                HtmlElement fbyid = wb.Document.GetElementById(name);
                fbyid.SetAttribute("value", value);

            } catch {

            }
        }

        public static void Write_replace(WebBrowser wb, string rep, string value) {
            wb.Document.Body.InnerHtml = wb.Document.Body.InnerHtml.Replace(rep, value);
        }

        public static void Write_value(WebBrowser wb, Point p, string value) {
            try {
                //HtmlElement fbyid = wb.Document.GetElementFromPoint(p);
                //string ss = "<DIV id=login-box>\r\n<DIV class=login-top><A title=返回网站主页 href=\"../index.php\" target=_self>返回网站主页2</A></DIV>\r\n<DIV class=safe-tips>您的管理目录的名称中包含默认名称dede，建议在FTP里把它修改为其它名称，那样会更安全！</DIV>\r\n<DIV class=login-main>\r\n<FORM method=post name=form1 action=login.php target=_self><INPUT style=\"ZOOM: 1\" name=gotopage value=/dede/article_add.php?channelid=1 type=hidden> <INPUT style=\"ZOOM: 1\" name=dopost value=login type=hidden> <INPUT style=\"ZOOM: 1\" name=adminstyle value=newdedecms type=hidden> \r\n<DL>\r\n<DT>用户名： </DT>\r\n<DD><INPUT style=\"ZOOM: 1\" class=alltxt name=userid value=admin> </DD>\r\n<DT>密&nbsp;&nbsp;码： </DT>\r\n<DD><INPUT style=\"ZOOM: 1\" class=alltxt name=pwd value=\"\" type=password> </DD>\r\n<DT>&nbsp; </DT>\r\n<DD><BUTTON class=login-btn onclick=this.form.submit(); name=sm1 type=submit>登录</BUTTON> </DD></DL></FORM></DIV>\r\n<DIV class=login-power>Powered by<A title=DedeCMS官网 href=\"http://www.dedecms.com\" target=_self><STRONG>DedeCMSV57_UTF8_SP1</STRONG></A>© 2004-2011 <A href=\"http://www.desdev.cn\" target=_self>DesDev</A> Inc.</DIV></DIV>\r\n<DIV class=dede-iframe><IFRAME id=loginad marginHeight=0 src=\"login.php?dopost=showad\" frameBorder=0 width=\"100%\" name=loginad marginWidth=0 scrolling=no></IFRAME></DIV>";
                //wb.Document.Write(ss);
                wb.Document.Body.InnerHtml = wb.Document.Body.InnerHtml.Replace("class=alltxt name=userid>", "class=alltxt name=userid value=admin>");
                wb.Document.Body.InnerHtml = wb.Document.Body.InnerHtml.Replace("class=alltxt name=userid>", "class=alltxt name=userid value=admin>");


                //fbyid.SetAttribute("value", value);

            } catch {

            }
        }
        //获取元素value属性的值                                                                                                     
        public static string Get_value(HtmlElement e) {
            return e.GetAttribute("value");
        }

        //执行元素的方法，如：click，submit(需Form表单名)等      相当于点击表单按钮,提交                                                                   
        public static void Btn_click(WebBrowser wb) {
            foreach (HtmlElement f in wb.Document.GetElementsByTagName("input")) {
                if (f.OuterHtml.Contains("type=submit")) {
                    f.InvokeMember("click");
                }
            }

            foreach (HtmlElement f in wb.Document.GetElementsByTagName("button")) {
                f.InvokeMember("click");
            }

        }


    }
}
