using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using X_Service.Util;
using Mozilla.NUniversalCharDet;

namespace X_Service.Files {
    public class FilesHelper {

        public FilesHelper() {
        }

        public static string Read_File(FileInfo file) {
            string tmp_result = "";
            Stream mystream = file.OpenRead();
            MemoryStream msTemp = new MemoryStream();
            int len = 0;
            byte[] buff = new byte[512];

            while ((len = mystream.Read(buff, 0, 512)) > 0) {
                msTemp.Write(buff, 0, len);
            }

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
                    EchoHelper.Echo("编码识别失败，请手工转码为UTF8保存到任务文件夹。文件：" + file.Name.ToLower(), "编码识别", EchoHelper.EchoType.任务信息);
                }
            }
            return tmp_result;
        }


        public static byte[] FileToByte(string fileName) {
            FileStream pFileStream = null;
            byte[] pReadByte = new byte[0];
            try {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(pFileStream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                pReadByte = r.ReadBytes((int)r.BaseStream.Length);
                return pReadByte;
            } catch {
                return pReadByte;
            } finally {
                if (pFileStream != null)
                    pFileStream.Close();
            }

        }

        //写byte[]到fileName
        public static bool ByteToFile(byte[] pReadByte, string fileName) {
            FileStream pFileStream = null;
            try {
                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                pFileStream.Write(pReadByte, 0, pReadByte.Length);
            } catch {
                return false;
            } finally {
                if (pFileStream != null)
                    pFileStream.Close();
            }
            return true;
        }


        #region

        /// <summary>
        /// 把文件内容写入到物理位置
        /// </summary>
        /// <param name="as_filename"></param>
        /// <param name="a_FileData"></param>
        /// <returns></returns>
        public static string WriteFile(string as_filename, string as_FileData, Encoding encode) {
            return Write_File(as_filename, as_FileData).ToString();
        }

        public static bool Write_File(string path, string neirong) {
            try {
                new FileInfo(path).Directory.Create();
                FileStream file = new FileStream(path, FileMode.Create);
                byte[] bt = Encoding.UTF8.GetBytes(neirong);
                file.Write(bt, 0, bt.Length);
                file.Flush();
                file.Close();
                GC.Collect();
            } catch {
                return false;
            }
            return true;
        }

        public static string ReadFile(string as_filename, Encoding encode) {
            return Read_File(as_filename);
        }

        public static string Read_File(string path) {
            string str = "";
            try {
                FileStream file = new FileStream(path, FileMode.Open);
                byte[] bt = new byte[file.Length];
                file.Read(bt, 0, bt.Length);
                str = Encoding.UTF8.GetString(bt);
                file.Close();
            } catch {
                return "";
            }
            GC.Collect();
            return str;
        }

        public static void DeleteFile(string path) {
            File.Delete(path); //删除文件
        }

        public static IList<FileInfo> ReadDirectoryList(string path, string ext) {
            if (!Directory.Exists(path)) {
                FilesHelper.CreateDirectory(path);
            }

            IList<FileInfo> al = new List<FileInfo>();
            //所有子文件
            try {
                foreach (string item in Directory.GetFiles(path)) {
                    FileInfo fileinfo = new FileInfo(item);
                    if (ext.ToLower().Contains(fileinfo.Extension.ToLower())) {
                        al.Add(fileinfo);
                    }
                }
            } catch {
            }

            return al;
        }

        public static void CreateDirectory(string path) {
            try {
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
            } catch {
            }
        }

        public static ArrayList ReadDirectory(string path) {
            if (!Directory.Exists(path)) {
                FilesHelper.CreateDirectory(path);
            }
            ArrayList al = new ArrayList();
            //所有子文件
            try {
                foreach (string item in Directory.GetFiles(path)) {
                    FileInfo fileinfo = new FileInfo(item);
                    al.Add(fileinfo);
                }
            } catch {
            }

            return al;
        }

        /// <summary>
        /// 读取目录中的所有扩展名文件。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ext">.html|.txt|.exe</param>
        /// <returns></returns>
        public static ArrayList ReadDirectory(string path, string ext) {
            if (!Directory.Exists(path)) {
                FilesHelper.CreateDirectory(path);
            }

            ArrayList al = new ArrayList();
            //所有子文件
            try {
                foreach (string item in Directory.GetFiles(path)) {
                    FileInfo fileinfo = new FileInfo(item);
                    if (ext.ToLower().Contains(fileinfo.Extension.ToLower())) {
                        al.Add(fileinfo);
                    }
                }
            } catch {
            }

            return al;
        }

        /// <summary>
        /// 删除目录里的文件，并删除这个目录。
        /// </summary>
        /// <param name="dirPath"></param>
        public static void DeleteInDir(string dirPath) {
            if (dirPath.Trim() == "" || !Directory.Exists(dirPath))
                return;
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);

            FileInfo[] fileInfos = dirInfo.GetFiles();
            if (fileInfos != null && fileInfos.Length > 0) {
                foreach (FileInfo fileInfo in fileInfos) {
                    File.Delete(fileInfo.FullName); //删除文件
                }
            }

            DirectoryInfo[] dirInfos = dirInfo.GetDirectories();
            if (dirInfos != null && dirInfos.Length > 0) {
                foreach (DirectoryInfo childDirInfo in dirInfos) {
                    //DeleteInDir(childDirInfo.); //递归
                }
            }
            Directory.Delete(dirInfo.FullName, true); //删除目录
        }

        #endregion

        #region 加密数据
        private static byte[] Encryption(string key, byte[] data) {
            try {
                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加解密类对象
                byte[] KEY = Encoding.Unicode.GetBytes(key);                        //定义字节数组，用来存储密钥
                MemoryStream MStream = new MemoryStream();                          //实例化内存流对象
                //使用内存流实例化加密流对象 
                CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(KEY, KEY), CryptoStreamMode.Write);
                CStream.Write(data, 0, data.Length);                                //向加密流中写入数据
                CStream.FlushFinalBlock();                                          //释放加密流
                return MStream.ToArray();                                           //返回加密后的数组
            } catch {
                return null;
            }
        }
        #endregion

        #region 解密数据
        private static byte[] Decryption(string key, byte[] data) {
            try {
                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加解密类对象
                byte[] KEY = Encoding.Unicode.GetBytes(key);                        //定义字节数组，用来存储密钥
                MemoryStream MStream = new MemoryStream();                          //实例化内存流对象
                //使用内存流实例化加密流对象 
                CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(KEY, KEY), CryptoStreamMode.Write);
                CStream.Write(data, 0, data.Length);                                //向加密流中写入数据
                CStream.FlushFinalBlock();                                          //释放加密流
                return MStream.ToArray();                                           //返回加密后的数组
            } catch {
                return null;
            }
        }
        #endregion

    }
}
