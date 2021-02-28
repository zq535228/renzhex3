using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using X_Service.Db;
using X_Service.Files;
using X_Service.Util;
using X_Service.Web;

namespace X_Service.Login {
    public partial class X_Form_Login : Login_Base {

        #region 构造函数

        protected X_Waiting wait = new X_Waiting();

        public X_Form_Login() {
            InitializeComponent();
            Form.CheckForIllegalCrossThreadCalls = false;
#if !DEBUG
            if (!new xkHttp().httpStatus()) {
                EchoHelper.Echo("链接网络出错，请检查您的网络是否通畅！", "网络故障", EchoHelper.EchoType.错误信息);
            }
#endif
        }


        #endregion

        #region 事件

        #region 窗体加载时
        /// <summary>
        /// 窗体加载时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void X_Form_Login_Load(object sender, EventArgs e) {
            wait.ShowMsg("1/10 加载登录窗口中...");
            TabC_Login_SelectedIndexChanged(sender, e);
            EchoHelper.Echo("窗体加载完成！请输入忍者X2的论坛账户，论坛密码，然后登录！", null, 0);
            txtHardInfo.Text = HardWare.getHardCode();
            FileInfo file = new FileInfo(Application.ExecutablePath);

            cVer.Text = file.LastWriteTime.ToLocalTime().ToString();
            Text_PID.Text = ini.re("登录账户密码", "PID");
            Text_PWD.Text = ini.re("登录账户密码", "PWD");
            btn_Login.Enabled = false;
            btn_Login.Text = "补丁探测...";
            try {
                ck提示更新.Checked = Convert.ToBoolean(ini.re("软件更新", "提示更新"));
                if (ck提示更新.Checked) {
                    EchoHelper.Echo("系统正在检查是否有新版本，请您耐心等候...", "软件更新", EchoHelper.EchoType.任务信息);
                    //软件更新.
                    Thread th = SearchThread("th_update");
                    if (th == null) {
                        th = new Thread(new ThreadStart(checkUpdate));
                        th.Name = "th_update";
                        th.IsBackground = true;
                    }
                    th.Start();
                }
            } catch (Exception ex) {
                EchoHelper.Echo(ex.Message, "加载INI出错", EchoHelper.EchoType.异常信息);
            }
            Text_PID.Focus();
            //wait.CloseMsg();
            Version v = System.Environment.Version;
            cVer.Text = v.ToString();
        }
        #endregion

        #region 单击登陆按钮时
        /// <summary>
        /// 单击登陆按钮时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Login_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Text_PID.Text) || string.IsNullOrEmpty(Text_PWD.Text)) {
                EchoHelper.Echo("用户名，密码不能为空，请填写！", "", EchoHelper.EchoType.普通信息);
                return;
            }
            wait.ShowMsg("2/10 准备连接服务端验证，请稍候...");
            if (ck提示更新.Checked) {
                ini.up("软件更新", "提示更新", ck提示更新.Checked.ToString());
            }
            //使用方法如下.
            Thread th = SearchThread("th_login");
            if (th == null) {
                th = new Thread(new ThreadStart(Login));
                th.Name = "th_login";
                th.IsBackground = true;
            }
            th.Start();
        }
        #region 登陆方法
        /// <summary>
        /// 登陆
        /// </summary>
        private void Login() {
            wait.ShowMsg("3/10 连接【忍者软件】服务端，通信中...");

            TabC_Login.Enabled = false;
            btn_Login.Enabled = false;
            Text_PWD.Enabled = true;
            string userInfo = string.Empty;

            btn_Login.Text = "验证..";
            //保存用户名密码,默认的.
            if (Ch_IsSave.Checked) {
                ini.up("登录账户密码", "PWD", Text_PWD.Text);
                ini.up("登录账户密码", "PID", Text_PID.Text);
            }

            /* 这个是老的验证模式。
             * 之后预计要用webservice的方式来进行数据的验证。
            member = getUser(Text_PID.Text, Text_PWD.Text, HardWare.getHardCode());

            if (member.IS_X_PostKing) {
                
                wait.ShowMsg("10/10 恭喜您，你与服务端通信成功！");

#if !DEBUG
                //进入连接服务端Socket线程。
                Thread threcive = new Thread(new ThreadStart(StartSycReceive));
                threcive.IsBackground = true;
                threcive.Start();

                Thread.Sleep(500);
#endif
            } else {
                wait.CloseMsg();
                EchoHelper.Show("非常遗憾，登录失败了，详细原因请查看日志黑窗！", EchoHelper.MessageType.警告);
                btn_Login.Text = "登陆";
                TabC_Login.Enabled = true;
                btn_Login.Enabled = true;
            }
            */
            this.DialogResult = DialogResult.OK;
            wait.CloseMsg();


        }

        private void StartSycReceive() {
            byte[] result = new byte[1024];//这个缓冲区,够大吗?

            if ((clientSocket = SocketHelper.GetSocket()) == null)
                return;

            //通过clientSocket接收数据  
            int receiveLength = clientSocket.Receive(result);
            EchoHelper.Echo("接收服务端消息成功：" + Encoding.ASCII.GetString(result, 0, receiveLength), "系统登录", EchoHelper.EchoType.任务信息);
            //通过 clientSocket 发送数据  
            try {
                member.strCommand = "login";
                byte[] bufs = db.ClasstoByte(member, "VCDS");
                SocketHelper.SendVarData(clientSocket, bufs);

                while (true) {
                    byte[] rets = SocketHelper.ReceiveVarData(clientSocket);
                    ModelMember remember = (ModelMember)db.BytetoClass(rets, "VCDS");

                    switch (remember.strCommand) {
                        case "login":
                            if (remember.bLoginSuccess) {
                                //MessageBox.Show("登录成功, 过期时间:" + ret.expire);//+ " 登录名:" + ret.netname + " 密码:" + ret.netpass);
                            } else {
                                MessageBox.Show(remember.strMessage);
                            }
                            break;
                        case "exit":
                            clientSocket.Shutdown(SocketShutdown.Both);
                            clientSocket.Close();
                            EchoHelper.Echo(remember.strMessage, "系统登录", EchoHelper.EchoType.错误信息);
                            Thread.Sleep(5000);
                            Environment.Exit(0);
                            break;
                        case "quit":
                            clientSocket.Shutdown(SocketShutdown.Both);
                            clientSocket.Close();
                            Environment.Exit(0);
                            return;
                    }
                }


            } catch (Exception ex) {
                //EchoHelper.EchoException(ex);
            } finally {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }

        #endregion

        Socket clientSocket = null;
        static DbTools db = new DbTools();


        /// <summary>
        /// 验证用户,通过网络
        /// </summary>
        /// <param name="netname"></param>
        /// <param name="netpass"></param>
        /// <param name="hdinfo"></param>
        /// <returns></returns>
        protected ModelMember getUser(string netname, string netpass, string hdinfo) {

#if DEBUG
            member.netname = "调试模式";
            member.group = "商业授权用户";
            member.sitenum = 9999;
            member.IS_X_WordPressBuild = true;
            member.IS_X_PostKing = true;
            member.userMoney = 9999;
            return Login_Base.member;
#endif
            string path = Application.StartupPath + "\\Config\\RenZheMember.txt";
            string html = "";
            DbTools db = new DbTools();
            CookieCollection cookies = new CookieCollection();

            try {
                object obj = db.Read(path, "VCDS");
                if (obj != null) {
                    member = (ModelMember)obj;
                }

                EchoHelper.Echo("连接【忍者软件】用户服务端，进行通信。此过程稍慢，请稍候...", "系统登录", EchoHelper.EchoType.任务信息);

                //发现有不符的情况，将进行登录验证。
                if (member.logintime.Date < DateTime.Now.Date || member.netpass != netpass || member.netname != netname || member.hdinfo != HardWare.getHardCode()) {
                    string purl = "http://www.renzhe.org/member.php?mod=logging&action=login&loginsubmit=yes&infloat=yes&lssubmit=yes&inajax=1";
                    string pdata = "fastloginfield=username&username=" + netname + "&password=" + netpass + "&quickforward=yes&handlekey=ls";
                    html = new xkHttp().httpPost(purl, pdata, ref cookies, purl, Encoding.GetEncoding("utf-8"));

                    if (html.Contains(">window.location.href='")) {
                        wait.ShowMsg("4/10 您的账户、密码验证成功！");
                        EchoHelper.Echo("论坛账户、密码验证成功！", "系统登录", EchoHelper.EchoType.任务信息);
                        purl = "http://www.renzhe.org/home.php?mod=spacecp&ac=credit";
                        html = new xkHttp().httpGET(purl, ref cookies);
                        if (html.Contains("[ 点击这里返回上一页 ]")) {
                            EchoHelper.Echo("忍者服务端维护，暂时关闭，请稍后再试...", "系统登录", EchoHelper.EchoType.错误信息);
                            return member;
                        }
                        if (html.Contains("您需要先登录才能继续本操作")) {
                            EchoHelper.Echo("您的账号异常，请手工登录论坛检查账户问题！", "系统登录", EchoHelper.EchoType.错误信息);
                            return member;
                        }
                        if (html.Contains("抱歉，您的 IP 地址不在被允许，或您的账号被禁用，无法访问本站点")) {
                            EchoHelper.Echo("抱歉，您的 IP 地址不在被允许，或您的账号被禁用，无法访问本站点！", "系统登录", EchoHelper.EchoType.错误信息);
                            return member;
                        }

                        wait.ShowMsg("5/10 您的用户基本信息，获取成功！");
                        EchoHelper.Echo("您的用户基本信息，获取成功！", "系统登录", EchoHelper.EchoType.任务信息);
                        member.UID = RegexHelper.getHtmlRegexText(html, "{discuz_uid = '(.*?)'}");
                        member.netname = RegexHelper.getHtmlRegexText(html, "{title=\"访问我的空间\">(.*?)</a>}");
                        member.sitenum = Convert.ToInt32(RegexHelper.getHtmlRegexText(html, "{站点数:</em>(.*?) </li>}"));
                        member.group = RegexHelper.getHtmlRegexText(html, "{showUpgradeinfo\\)\">(.*?)</a>}");
                        member.userMoney = Convert.ToInt32(RegexHelper.getHtmlRegexText(html, "{金币:</em>(.*?)  &nbsp;}"));
                        member.formhash = RegexHelper.getHtmlRegexText(html, "{formhash=(.*?)\">退出</a>}");
                        member.cookies = cookies;
                        member.netpass = netpass;
                        member.logintime = DateTime.Now;
                        member.hdinfo = HardWare.getHardCode();
                        member.IS_X_PostKing = true;
                        EchoHelper.Echo("链接服务端，判断应用授权状态...", "系统登录", EchoHelper.EchoType.任务信息);
                        wait.ShowMsg("6/10 链接服务端，判断应用授权状态！");

                    } else {
                        wait.ShowMsg("用户验证失败...");
                        string result = "未知，请重试，登录论坛联系管理员。www.renzhe.org";
                        if (html.Contains("登录失败")) {
                            result = "请核对您的用户名密码！登录论坛联系管理员。www.renzhe.org";
                        }
                        if (html.Contains("showWindow('login', 'member.php?mod=logging&action=")) {
                            result = "发现安全问题，清除您的安全问题后，再尝试！www.renzhe.org";
                        }
                        if (html.Contains("密码错误次数过多")) {
                            result = "密码错误次数过多，稍后再试！登录论坛联系管理员。www.renzhe.org";
                        }
                        if (html.Contains("无法解析此远程名称")) {
                            result = "无法解析www.renzhe.org，请检查您的网络，稍后再试";
                        }

                        member.IS_X_PostKing = false;
                        member.IS_X_WordPressBuild = false;

                        EchoHelper.Echo("登录失败：" + result, "系统登录", EchoHelper.EchoType.错误信息);
                        return member;
                    }
                } else {
                    wait.ShowMsg("7/10 发现本地密钥，进行快捷登录...");
                    EchoHelper.Echo("发现本地登录密钥文件，进行验证，请稍后...", "系统登录", EchoHelper.EchoType.任务信息);
                }

#if !DEBUG
                //向服务端提交member序列化的类，然后验证是否为登录成功的状态。
                ValidateUser(ref member);
#else
                //ValidateUser(ref member);
#endif

                ini.up("登录账户密码", "INFO", member.netname);
                if (member.IS_X_PostKing == false) {
                    member.logintime = DateTime.Now.AddDays(-111);
                    FilesHelper.DeleteFile(path);
                } else {
                    wait.ShowMsg("8/10 恭喜，您的密钥经服务端验证成功！");
                    EchoHelper.Echo("恭喜，您的本地密钥，经服务端验证成功，通信一切正常！", "系统登录", EchoHelper.EchoType.任务信息);
                }

            } catch (Exception ex) {
                FilesHelper.DeleteFile(path);
                EchoHelper.Echo("与服务端通信失败！" + ex.Message, "系统登录", EchoHelper.EchoType.异常信息);
            } finally {
                if (member.group.Contains("商业")) {
                    member.IS_X_WordPressBuild = true;
                }

                if (member.IS_X_PostKing == true) {
                    wait.ShowMsg("9/10 密钥已保存，下次可快捷登录！");
                    Thread.Sleep(1200);
                    EchoHelper.Echo("您的本地密钥已保存，下次可快捷登录！", "系统登录", EchoHelper.EchoType.任务信息);
                    db.Save(path, "VCDS", member);
                }

            }
            return Login_Base.member;


        }

        private bool ValidateUser(ref ModelMember member) {
            member.IS_X_PostKing = false;

            //验证是否具有发帖权限
            string testpostUrl = "http://www.renzhe.org/forum.php?mod=post&action=newthread&fid=36";

            string html = new xkHttp().httpGET(testpostUrl, ref member.cookies);
            if (html.Contains("发表帖子 - 忍者X2站群 -  忍者软件 -  RenZhe.org!")) {
                EchoHelper.Echo("恭喜您，权限核对成功，您已被授权使用忍者X2站群！", "用户验证", EchoHelper.EchoType.任务信息);
                member.strMessage = "恭喜您，权限核对成功，您已被授权使用忍者X2站群！";
                member.IS_X_PostKing = true;
            } else if (html.Contains("抱歉，您需要设置自己的头像后才能进行本操作")) {
                EchoHelper.Echo("请完善您的（基本资料、头像），即在论坛上发个帖子激活一下。授权论坛：www.renzhe.org！", "用户验证", EchoHelper.EchoType.错误信息);
                member.strMessage = "请完善您的（基本资料、头像），即在论坛上发个帖子激活一下。授权论坛：www.renzhe.org！";
            } else if (html.Contains("请先绑定手机号码")) {
                EchoHelper.Echo("请先绑定手机号码，授权论坛：www.renzhe.org！", "用户验证", EchoHelper.EchoType.错误信息);
                member.strMessage = "请先绑定手机号码，授权论坛：www.renzhe.org！";
            } else if (html.Contains("<s>商业授权用户</s>")) {
                EchoHelper.Echo("您的账户已过期，请到论坛充值续费！", "用户验证", EchoHelper.EchoType.错误信息);
                member.strMessage = "您的账户已过期，请到论坛充值续费！";
            } else if (html.Contains("超时")) {
                EchoHelper.Echo("链接服务超时！", "用户验证", EchoHelper.EchoType.错误信息);
                member.strMessage = "链接服务超时！";
            } else if (html.Contains("没有权限在该版块发帖")) {
                EchoHelper.Echo("用户登录验证失败，请重新登录！", "用户验证", EchoHelper.EchoType.错误信息);
                member.strMessage = "用户登录验证失败，请重新登录！";
            } else if (html.Contains("无法解析此远程名称")) {
                EchoHelper.Echo("域名解析出现问题，请检查您的网络设置！", "用户验证", EchoHelper.EchoType.错误信息);
                member.strMessage = "域名解析出现问题，请检查您的网络设置！";
            } else {
                EchoHelper.Echo("验证失败，原因未知！", "用户验证", EchoHelper.EchoType.错误信息);
                member.strMessage = "验证失败，原因未知！";
            }

            if (member.group.Contains("商业授权用户")) {
                member.IS_X_WordPressBuild = true;
            } else {
                member.IS_X_WordPressBuild = false;
            }
            return member.IS_X_PostKing;
        }


        private void saveUserModelToTxt() {
            string path = Application.StartupPath + "\\Config\\RenZheMember.txt";
            string content = string.Format("用户名：{0}\r\n用户组：{1}\r\n站点数：{2}\r\n拥有金币：{3}",
                                            member.netname,
                                            member.group,
                                            member.sitenum,
                                            member.userMoney
                                            );
            FilesHelper.Write_File(path, content);
        }

        #endregion

        #region 标签更改时
        private void TabC_Login_SelectedIndexChanged(object sender, EventArgs e) {
            switch (TabC_Login.SelectedIndex) {
                case 0:
                    TabC_Login.Size = new Size(TabC_Login.Size.Width, groupBox_Login.Size.Height + 37);
                    this.Size = new Size(this.Size.Width, 127 + TabC_Login.Size.Height);
                    break;
                case 1:
                    TabC_Login.Size = new Size(TabC_Login.Size.Width, groupBox_Info.Size.Height + 37);
                    this.Size = new Size(this.Size.Width, 127 + TabC_Login.Size.Height);
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void txtHardInfo_Click(object sender, EventArgs e) {
            StringHelper.stringCopy(HardWare.getHardCode());
            EchoHelper.Show("复制成功：" + HardWare.getHardCode(), EchoHelper.MessageType.提示);
        }
        #endregion

        #region 检查更新方法
        /// <summary>
        /// 检查更新,运行softupdate.exe
        /// </summary>
        protected void checkUpdate() {

            if (CheckForUpdate() > 0) {
                btn_Login.Enabled = true;
                btn_Login.Text = "是否升级？";
                btn_Login.Enabled = false;

                EchoHelper.Echo("发现新内容,马上更新吗？", "软件更新", EchoHelper.EchoType.任务信息);
                string xmlFile = Application.StartupPath + @"\Temp\UpdateList.xml";
                string upStr = new XmlFiles(xmlFile).GetNodeValue("//description");
                if (upStr.Contains("\n")) {
                    upStr = upStr.Split('\n')[2].ToString();
                    upStr = upStr.Trim();
                }
                DialogResult dre = MessageBox.Show("发现新内容,马上更新吗?\n" + upStr, "更新程序", MessageBoxButtons.OKCancel);
                if (dre == DialogResult.OK) {
                    string exe_path = Application.StartupPath;
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = "X_Update.exe";
                    process.StartInfo.WorkingDirectory = exe_path;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    Application.Exit();
                }
            } else {
                EchoHelper.Echo("恭喜您，您的版本已经是最新！", "软件更新", EchoHelper.EchoType.任务信息);
            }
            btn_Login.Text = "登陆";
            btn_Login.Enabled = true;
        }


        #endregion

        private void btnReg_Click(object sender, EventArgs e) {
            ProcessHelper.openUrl("http://www.renzhe.org/forum.php");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ProcessHelper.openUrl("http://www.renzhe.org/forum.php");
        }

        private void linkGetPs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ProcessHelper.openUrl("http://www.renzhe.org/member.php?mod=register");
        }

        private void btnTxtPWD_Click(object sender, EventArgs e) {
            Text_PWD.Text = string.Empty;
        }

        private void btnTxtPid_Click(object sender, EventArgs e) {
            Text_PID.Text = string.Empty;
        }



        private string UpdaterUrl;
        /// <summary>
        /// 检查更新文件，返回需要更新的个数。
        /// </summary>
        /// <param name="serverXmlFile"></param>
        /// <param name="localXmlFile"></param>
        /// <param name="updateFileList"></param>
        /// <returns></returns>
        public int CheckForUpdate() {
#if !DEBUG
            wait.ShowMsg("2/10 检查是否存在更新补丁，请稍后...");
            string localXmlFile = Application.StartupPath + "\\UpdateList.xml";
            if (!File.Exists(localXmlFile)) {
                return -1;
            }
            XmlFiles updaterXmlFiles = new XmlFiles(localXmlFile);

            string tempUpdatePath = Application.StartupPath + "\\Temp\\";
            this.UpdaterUrl = updaterXmlFiles.GetNodeValue("//Url") + "UpdateList.xml";
            this.DownAutoUpdateFile(tempUpdatePath);

            string serverXmlFile = tempUpdatePath + "UpdateList.xml";

            if (!File.Exists(serverXmlFile)) {
                return -1;
            }

            XmlFiles serverXmlFiles = new XmlFiles(serverXmlFile);
            XmlFiles localXmlFiles = new XmlFiles(localXmlFile);

            XmlNodeList newNodeList = serverXmlFiles.GetNodeList("AutoUpdater/Files");
            XmlNodeList oldNodeList = localXmlFiles.GetNodeList("AutoUpdater/Files");

            int k = 0;
            for (int i = 0; i < newNodeList.Count; i++) {
                string[] fileList = new string[3];

                string newFileName = newNodeList.Item(i).Attributes["Name"].Value.Trim();
                string newVer = newNodeList.Item(i).Attributes["Ver"].Value.Trim();

                ArrayList oldFileAl = new ArrayList();
                for (int j = 0; j < oldNodeList.Count; j++) {
                    string oldFileName = oldNodeList.Item(j).Attributes["Name"].Value.Trim();
                    string oldVer = oldNodeList.Item(j).Attributes["Ver"].Value.Trim();

                    oldFileAl.Add(oldFileName);
                    oldFileAl.Add(oldVer);

                }
                int pos = oldFileAl.IndexOf(newFileName);
                if (pos == -1) {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    k++;
                } else if (pos > -1 && newVer.CompareTo(oldFileAl[pos + 1].ToString()) > 0) {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    k++;
                }

            }
            wait.CloseMsg();
            return k;
#else
            return 0;
#endif
        }

        /// <summary>
        /// 下载更新文件到临时目录
        /// </summary>
        /// <returns></returns>
        protected void DownAutoUpdateFile(string downpath) {
            if (!System.IO.Directory.Exists(downpath))
                System.IO.Directory.CreateDirectory(downpath);
            string serverXmlFile = downpath + @"UpdateList.xml";

            try {
                WebRequest req = WebRequest.Create(this.UpdaterUrl);
                WebResponse res = req.GetResponse();
                if (res.ContentLength > 0) {
                    try {
                        WebClient wClient = new WebClient();
                        wClient.DownloadFile(this.UpdaterUrl, serverXmlFile);
                    } catch (Exception ex) {
                        EchoHelper.EchoException(ex);
                    }
                }
            } catch {
                return;
            }
        }

    }
}
