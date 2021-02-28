using System;
using System.Threading;
using HttpWatch;

namespace X_Service.HttpWatch {

    /// <summary>
    /// HttpWatch类（抓包并返回详细数据）
    /// 
    /// </summary>
    public class HttpWatchTool {

        #region 构造函数
        private static Controller controld = null;
        private Plugin plugin = null;
        private Thread jianceT;
        private int KeyCount = 0;
        private int Count = 0;
        private bool IsRun = false;
        public string Url = "http://www.renzhe.org/";
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="url"></param>
        public HttpWatchTool( string url ) {
            Url = url;
            controld = new Controller();
        }
        #endregion

        #region 事件
        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="Leixing">类型</param>
        /// <param name="Data">数据包Data，若类型为Count++ 或CloseBrowser 时 值为null</param>
        /// <param name="Count">捕获总数据包</param>
        /// <param name="KeyCount">捕获关键数据包</param>
        public delegate void PageChangeHandler( String Leixing , Run_WebData Data , int Count , int KeyCount );
        /// <summary>
        /// 事件
        /// </summary>
        public event PageChangeHandler Page;
        /// <summary>
        /// 事件触发方法
        /// </summary>
        /// <param name="Leixing">类型</param>
        /// <param name="Data">网络数据包</param>
        private void Event( String Leixing , Run_WebData Data ) {
            if (Page != null) {
                if (Leixing == "Count++") {
                    Page(Leixing , null , Count , KeyCount);
                } else if (Leixing == "KeyCount++") {
                    Page(Leixing , Data , Count , KeyCount);
                } else if (Leixing == "Post++") {
                    Page(Leixing , Data , Count , KeyCount);
                } else if (Leixing == "CloseBrowser") {
                    Page(Leixing , null , 0 , 0);
                }
            }

        }
        #endregion

        #region 开始监听方法
        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="i">参数 1为IE 2为火狐</param>
        public void Open( int i ) {
            if (i == 1) {
                plugin = controld.IE.New();
            } else if (i == 2) {
                plugin = controld.Firefox.New();
            } else {
                Exception Exception = new Exception("参数必须是1或2！");
                throw Exception;
            }
            plugin.Log.EnableFilter(false);
            plugin.Record();
            plugin.GotoURL(Url);
            IsRun = true;
            jianceT = new Thread(new ThreadStart(jiance));
            jianceT.IsBackground = true;
            jianceT.Start();
            //controld.WaitEx(plugin, -1, true, true, 1000000);
        }
        #endregion

        #region 检测方法
        /// <summary>
        /// 线程体
        /// </summary>
        private void jiance() {
            while (true) {
                try {
                    if (plugin.Log.Entries.Count != Count) {
                        Jiance(plugin.Log.Entries.Count);
                    } else {

                    }
                } catch {
                    if (IsRun) {
                        Event("CloseBrowser" , null);
                    }
                    Close();
                    Thread.CurrentThread.Abort();
                }
            }
        }
        private void Jiance( int b ) {
            for (int i = Count; i < b; i++) {
                #region 处理Get方法
                if (plugin.Log.Entries[i].Method == "GET") {
                    if (plugin.Log.Entries[i].URL.ToString().ToLower().Contains("?renzhe")) {
                        KeyCount++;
                        Event("KeyCount++" , CreatWebData(i));
                    }
                } else if (plugin.Log.Entries[i].Method == "POST") {
                    if (plugin.Log.Entries[i].URL.ToString().ToLower().Contains("?renzhe")) {
                        KeyCount++;
                        Event("KeyCount++" , CreatWebData(i));
                    } else {
                        string Name = string.Empty;
                        string Value = string.Empty;
                        for (int ii = 0; ii < plugin.Log.Entries[i].Request.POSTParameters.Count; ii++) {
                            Name = plugin.Log.Entries[i].Request.POSTParameters[ii].Name;
                            Value = plugin.Log.Entries[i].Request.POSTParameters[ii].Value;
                            if (Value.ToLower().Contains("?renzhe") | Value.ToLower().Contains("?renzhe")) {
                                Event("KeyCount++" , CreatWebData(i));
                                Count++;
                                Event("Count++" , null);
                                return;
                            }
                        }
                        Event("Post++" , CreatWebData(i));
                    }
                }
                #endregion

                Count++;
                Event("Count++" , null);
            }

        }

        #endregion

        #region 销毁方法
        /// <summary>
        /// 销毁（必须）
        /// </summary>
        public void Close() {
            try {
                IsRun = false;
                KeyCount = 0;
                Count = 0;
                jianceT.Abort();
                Thread.Sleep(10);
                plugin.Stop();
                plugin.CloseBrowser();
                plugin = null;
            } catch {

            }
        }
        #endregion

        #region 创建网络数据包详细信息
        /// <summary>
        /// 创建网络数据包详细信息
        /// </summary>
        /// <param name="Count"></param>
        /// <returns></returns>
        private Run_WebData CreatWebData( int Count ) {
            Run_WebData Data = new Run_WebData();
            string Name = string.Empty;
            string Value = string.Empty;
            Data.Url = plugin.Log.Entries[Count].URL.ToString();
            Data.Method = plugin.Log.Entries[Count].Method.ToString();
            if (Data.Method == "GET") {
                for (int i = 0; i < plugin.Log.Entries[Count].Request.QueryStringValues.Count; i++) {
                    Name = plugin.Log.Entries[Count].Request.QueryStringValues[i].Name;
                    Value = plugin.Log.Entries[Count].Request.QueryStringValues[i].Value;
                    Data.Parameters += Name + "=" + Value + "\r\n";
                }
            } else if (Data.Method == "POST") {
                for (int i = 0; i < plugin.Log.Entries[Count].Request.POSTParameters.Count; i++) {
                    Name = plugin.Log.Entries[Count].Request.POSTParameters[i].Name;
                    Value = plugin.Log.Entries[Count].Request.POSTParameters[i].Value;
                    Data.Parameters += Name + "→" + Value + "\r\n";
                }
            }
            for (int i = 0; i < plugin.Log.Entries[Count].Request.Headers.Count; i++) {
                Name = plugin.Log.Entries[Count].Request.Headers[i].Name;
                Value = plugin.Log.Entries[Count].Request.Headers[i].Value;
                if (Name == "Cookie") {
                    Data.Cookie = Value;
                }
                if (Name == "Referer") {
                    Data.Referer = Value;
                }
                if (Name == "User-Agent") {
                    Data.UserAgent = Value;
                }
                Data.Head += Name + ":" + Value + "\r\n";
            }
            return Data;
        }
        #endregion

    }
}
