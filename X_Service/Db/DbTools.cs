using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using X_Service.Files;
using X_Service.Util;
using System.Windows.Forms;

namespace X_Service.Db {
    /// <summary>
    /// 忍者X2专用数据库
    /// </summary>
    public class DbTools {
        #region 保存数据

        public void Save(string filePath, string key, object obj) {
            try {
                if (filePath.ToLower().EndsWith(".vdb")) {
                    //                     string _bak = FilesHelper.ReadFile(filePath + "_bak", Encoding.UTF8);
                    //                     string _cache = FilesHelper.ReadFile(filePath + "_cache", Encoding.UTF8);
                    string _buff = FilesHelper.ReadFile(filePath + "_cache", Encoding.UTF8);
                    string _vdb = FilesHelper.ReadFile(filePath, Encoding.UTF8);

                    //                     if (File.Exists(filePath + "_cache") && _cache != _bak) {
                    //                         File.Copy(filePath + "_cache", filePath + "_bak", true);
                    //                     }
                    // 
                    //                     if (File.Exists(filePath + "_buff") && _buff != _cache) {
                    //                         File.Copy(filePath + "_buff", filePath + "_cache", true);
                    //                     }

                    if (File.Exists(filePath) && _buff != _vdb) {
                        File.Copy(filePath, filePath + "_buff", true);
                    }
                }
            } catch {
            }

            FilesHelper.Write_File(filePath, ClasstoString(obj, key));
        }
        public object Read(string filePath, string key) {
            if (!string.IsNullOrEmpty(filePath) && !File.Exists(filePath)) {
                EchoHelper.Echo("系统未发现数据库密钥文件，自动为您生成...", "", EchoHelper.EchoType.普通信息);
                FilesHelper.Write_File(filePath, "");
            }
            try {
                object obj = StringtoClass(FilesHelper.ReadFile(filePath, Encoding.UTF8), key);
                return obj;
            } catch {
                return null;
            }
        }

        public void debugVdb() {
            string bugs = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\把此文件发送给Qin.zip";
            ZipHelper.Zip(bugs, Application.StartupPath + @"\Config\");
            EchoHelper.Echo("数据加载出现了异常，为了分析异常数据，请上交给Qin处理！", "请提交异常数据", EchoHelper.EchoType.异常信息);
            EchoHelper.Echo("建议把桌面上的【把此文件发送给Qin.zip】文件，发送给Qin，让他来分析此数据包的问题！", "请提交异常数据", EchoHelper.EchoType.异常信息);

        }
        public object StringtoClass(string str, string key) {
            object result = null;
            byte[] dBytes = Convert.FromBase64String(str);
            dBytes = Decryption(dBytes, key);
            using (MemoryStream ms = new MemoryStream(dBytes)) {
                IFormatter formatter = new BinaryFormatter();
                result = formatter.Deserialize(ms);
            }
            return result;
        }


        public string ClasstoString(object obj, string key) {
            byte[] dBytes = null;
            using (MemoryStream ms = new MemoryStream()) {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                dBytes = ms.GetBuffer();
            }
            string Base64Str;
            dBytes = Encryption(dBytes, key);
            Base64Str = Convert.ToBase64String(dBytes);
            return Base64Str;
        }

        public byte[] ClasstoByte(object obj, string key) {
            byte[] dBytes = null;
            using (MemoryStream ms = new MemoryStream()) {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                dBytes = ms.GetBuffer();
            }
            dBytes = Encryption(dBytes, key);
            return dBytes;
        }


        public object BytetoClass(byte[] dBytes, string key) {
            object result = null;
            try {
                dBytes = Decryption(dBytes, key);
                using (MemoryStream ms = new MemoryStream(dBytes)) {
                    IFormatter formatter = new BinaryFormatter();
                    result = formatter.Deserialize(ms);
                }
            } catch {
            }
            return result;
        }

        #endregion


        #region 加密数据
        public byte[] Encryption(byte[] data, string key) {
            byte[] by = new byte[0];
            if (data.Length > 0) {
                try {
                    DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加解密类对象
                    byte[] KEY = Encoding.Unicode.GetBytes(key);                        //定义字节数组，用来存储密钥
                    MemoryStream MStream = new MemoryStream();                          //实例化内存流对象
                    //使用内存流实例化加密流对象 
                    CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(KEY, KEY), CryptoStreamMode.Write);
                    CStream.Write(data, 0, data.Length);                                //向加密流中写入数据
                    CStream.FlushFinalBlock();                                          //释放加密流
                    by = MStream.ToArray();                                                        //返回加密后的数组
                } catch (Exception ex) {
                    EchoHelper.EchoException(ex);
                }
            }
            return by;
        }
        #endregion

        #region 解密数据
        public byte[] Decryption(byte[] data, string key) {
            byte[] by = new byte[0];
            if (data.Length > 0) {
                try {
                    DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加解密类对象
                    byte[] KEY = Encoding.Unicode.GetBytes(key);                        //定义字节数组，用来存储密钥
                    MemoryStream MStream = new MemoryStream();                          //实例化内存流对象
                    //使用内存流实例化加密流对象 
                    CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(KEY, KEY), CryptoStreamMode.Write);
                    CStream.Write(data, 0, data.Length);                                //向加密流中写入数据
                    CStream.FlushFinalBlock();                                          //释放加密流
                    by = MStream.ToArray();                                           //返回加密后的数组
                } catch (Exception ex) {
                    EchoHelper.EchoException(ex);
                }
            }
            return by;
        }
        #endregion

    }

}
