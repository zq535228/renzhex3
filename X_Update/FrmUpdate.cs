using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;

namespace X_Update {
    public partial class FrmUpdate : Form {
        public FrmUpdate() {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;    //������̰߳�ȫ������

        }

        XmlFiles updaterXmlFiles = null;
        private string updateUrl = string.Empty;
        private string tempUpdatePath = string.Empty;
        private int availableUpdate = 0;
        string mainAppExe = "";

        private void MainForm_Load(object sender, EventArgs e) {
            try {
                //�ӱ��ض�ȡ���������ļ���Ϣ
                string xmlFile = Application.StartupPath + @"\UpdateList.xml";
                txtUpdateInfo.Text = new XmlFiles(xmlFile).GetNodeValue("//description");
                txtUpdateInfo.Text = txtUpdateInfo.Text.Replace("	", "").Replace(" ", "");

            } catch {
                MessageBox.Show("�����ļ�����!", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
        }

        private void tmStart_Tick(object sender, EventArgs e) {
            this.tmStart.Stop();

            this.Text = "���ڻ�ȡ������Ϣ�����Ե�....";

            this.btnNext.Enabled = false;

            string xmlFile = Application.StartupPath + @"\UpdateList.xml";
            string serverXmlFile = string.Empty;

            try {
                //�ӱ��ض�ȡ���������ļ���Ϣ
                updaterXmlFiles = new XmlFiles(xmlFile);
            } catch {
                MessageBox.Show("�����ļ�����!", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            //��ȡ��������ַ
            updateUrl = updaterXmlFiles.GetNodeValue("//Url");

            AppUpdater appUpdater = new AppUpdater();

            appUpdater.UpdaterUrl = updateUrl + "/UpdateList.xml";

            //�����������,���ظ��������ļ�
            try {
                tempUpdatePath = Application.StartupPath + "\\Temp\\";
                appUpdater.DownAutoUpdateFile(tempUpdatePath);
            } catch {
                MessageBox.Show("�����������ʧ��,������ʱ!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            //��ȡ�����ļ��б�
            Hashtable htUpdateFile = new Hashtable();

            serverXmlFile = tempUpdatePath + "UpdateList.xml";
            if (File.Exists(serverXmlFile)) {
                availableUpdate = appUpdater.CheckForUpdate(serverXmlFile, xmlFile, out htUpdateFile);
                if (availableUpdate > 0) {
                    for (int i = 0; i < htUpdateFile.Count; i++) {
                        string[] fileArray = (string[])htUpdateFile[i];
                        lvUpdateList.Items.Add(new ListViewItem(fileArray));
                    }
                    this.btnNext.Enabled = true;
                    this.Text = "������Ϣ��ȡ��ϣ�����Ϊ���θ��µ����ݣ�";
                } else {
                    this.btnNext.Enabled = true;
                    this.Text = "���İ汾������,���޸��£�";
                }
            }
        }

        /// <summary>
        /// Create a zip archive.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="directory">The directory to zip.</param> 
        public void Zip(string filename, string directory) {
            try {
                _createDirtory(filename);
                FastZip fz = new FastZip();
                fz.CreateEmptyDirectories = true;
                fz.CreateZip(filename, directory, true, "");
                fz = null;
            } catch { }
        }


        private void btnNext_Click(object sender, EventArgs e) {
            btnNext.Enabled = false;
            //������ݡ�
            Zip(Application.StartupPath + "\\Temp\\" + DateTime.Now.ToLongDateString() + "\\bak_config.zip", Application.StartupPath + @"\Config\");
            Thread threadDown = new Thread(new ThreadStart(DonwLoadFiles));
            threadDown.IsBackground = true;
            threadDown.Start();


        }

        //������Ҫ���µ��ļ�
        private void DonwLoadFiles() {
            this.Cursor = Cursors.WaitCursor;
            mainAppExe = updaterXmlFiles.GetNodeValue("//EntryPoint");
            Process[] allProcess = Process.GetProcesses();
            //��ֹ��ǰ���ڽ��еĽ���
            foreach (Process p in allProcess) {
                if (p.ProcessName.ToLower().Contains(mainAppExe.ToLower())) {
                    for (int i = 0; i < p.Threads.Count; i++)
                        p.Threads[i].Dispose();
                    p.Kill();
                }
            }

            WebClient wcClient = new WebClient();
            for (int i = 0; i < this.lvUpdateList.Items.Count; i++) {
                string UpdateFile = lvUpdateList.Items[i].Text.Trim();
                string updateFileUrl = updateUrl + lvUpdateList.Items[i].Text.Trim();
                long fileLength = 0;

                WebRequest webReq = WebRequest.Create(updateFileUrl);
                WebResponse webRes = webReq.GetResponse();
                fileLength = webRes.ContentLength;

                pbDownFile.Value = 0;
                pbDownFile.Maximum = (int)fileLength;

                try {
                    Stream srm = webRes.GetResponseStream();
                    StreamReader srmReader = new StreamReader(srm);
                    byte[] bufferbyte = new byte[fileLength];
                    int allByte = (int)bufferbyte.Length;
                    int startByte = 0;
                    while (fileLength > 0) {
                        Application.DoEvents();
                        int downByte = srm.Read(bufferbyte, startByte, allByte);
                        if (downByte == 0) {
                            break;
                        };
                        startByte += downByte;
                        allByte -= downByte;
                        pbDownFile.Value += downByte;

                        float part = (float)startByte / 1024;
                        float total = (float)bufferbyte.Length / 1024;
                        int percent = Convert.ToInt32((part / total) * 100);

                        this.lvUpdateList.Items[i].SubItems[2].Text = percent.ToString() + "%";

                    }

                    string tempPath = tempUpdatePath + DateTime.Now.ToLongDateString() + "\\" + UpdateFile;
                    _createDirtory(tempPath);
                    FileStream fs = new FileStream(tempPath, FileMode.OpenOrCreate, FileAccess.Write);
                    fs.Write(bufferbyte, 0, bufferbyte.Length);
                    srm.Close();
                    srmReader.Close();
                    fs.Close();


                } catch (WebException ex) {
                    MessageBox.Show("�����ļ�����ʧ�ܣ�" + ex.Message.ToString(), "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            try {
                _copyfiles(tempUpdatePath + DateTime.Now.ToLongDateString() + "\\", Directory.GetCurrentDirectory());
                File.Delete(Application.StartupPath + "\\bak_config.zip");
                File.Copy(tempUpdatePath + "UpdateList.xml", Directory.GetCurrentDirectory() + "\\UpdateList.xml", true);
                //System.IO.Directory.Delete(tempUpdatePath, true);
                //MessageBox.Show("�����ļ�������TempĿ¼��,�����ĳ���û�и���,�������ֶ����ǵ�ǰ�ļ�!");
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString());
            }

            try {
                Process.Start(mainAppExe);
            } catch {
                MessageBox.Show("���ֶ�����" + mainAppExe + "����!");
            }

            this.Close();
            this.Dispose();

        }

        private void btnClose_Click(object sender, EventArgs e) {
            try {
                mainAppExe = updaterXmlFiles.GetNodeValue("//EntryPoint");
                Process.Start(mainAppExe);
            } catch {
                MessageBox.Show("���ֶ�����" + mainAppExe + "����!");
            }

            base.Close();
            Application.ExitThread();
            Application.Exit();
        }

        //�����ļ�;
        private void _copyfiles(string sourcePath, string objPath) {
            if (!Directory.Exists(objPath)) {
                Directory.CreateDirectory(objPath);
            }
            string[] files = Directory.GetFiles(sourcePath);
            for (int i = 0; i < files.Length; i++) {
                string[] childfile = files[i].Split('\\');
                File.Copy(files[i], objPath + @"\" + childfile[childfile.Length - 1], true);
            }
            string[] dirs = Directory.GetDirectories(sourcePath);
            for (int i = 0; i < dirs.Length; i++) {
                string[] childdir = dirs[i].Split('\\');
                _copyfiles(dirs[i], objPath + @"\" + childdir[childdir.Length - 1]);
            }
        }
        //����Ŀ¼
        private void _createDirtory(string path) {
            if (!File.Exists(path)) {
                string[] dirArray = path.Split('\\');
                string temp = string.Empty;
                for (int i = 0; i < dirArray.Length - 1; i++) {
                    temp += dirArray[i].Trim() + "\\";
                    if (!Directory.Exists(temp))
                        Directory.CreateDirectory(temp);
                }
            }
        }

    }
}