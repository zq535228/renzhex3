using System;

namespace X_Service.HttpWatch {

    [Serializable]
    public class Run_WebData {
        /// <summary>
        /// 地址
        /// </summary>
        public string Url = string.Empty;
        /// <summary>
        /// 操作方法
        /// </summary>
        public string Method = string.Empty;
        /// <summary>
        /// 参数
        /// </summary>
        public string Parameters = string.Empty;
        /// <summary>
        /// Cookie
        /// </summary>
        public string Cookie = string.Empty;
        /// <summary>
        /// 头部数据
        /// </summary>
        public string Head = string.Empty;
        /// <summary>
        /// 来路（2011年1月25日12:13:39新增）
        /// </summary>
        public string Referer = string.Empty;
        /// <summary>
        /// User-Agent
        /// </summary>
        public string UserAgent = string.Empty;
    }
}
