using System;
using System.Collections.Generic;
using System.Text;

namespace X_Service.Util {
    public class ProcessHelper {

        /// <summary>
        /// 使用默认的浏览器,打开路径,可以是本地文件夹
        /// </summary>
        /// <param name="pathUrl"></param>
        public static void openUrl( string pathUrl ) {
            if (!string.IsNullOrEmpty(pathUrl)) {
                try {
                    System.Diagnostics.Process.Start(pathUrl);
                } catch { }
            }

        }

    }
}
