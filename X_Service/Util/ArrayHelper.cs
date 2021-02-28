using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace X_Service.Util {
    public class ArrayHelper {

        /// <summary>
        /// 获取数组中最大的一个+1，并返回。用户获取最后一个ID+1值，也就是newID的意思。
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
        public static int getNextID(ArrayList al) {
            int max = 0;
            try {
                for (int i = 0; i < al.Count; i++) {
                    int current = int.Parse(al[i].ToString());
                    if (current > max) {
                        max = current;
                    }
                }
                max += 1;
            } catch {
            }
            return max;
        }

        /// <summary>
        /// 随机返回array中的一个 arrayList是字符串的集合.
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
        public static string getRandOneFromArray(ArrayList al) {
            try {
                Random rd = new Random();
                int i = rd.Next(al.Count);
                return al[i].ToString();
            } catch {

                return "";
            }
        }

        /// <summary>
        /// 随机返回一个字符串数组中的一个,例如 string[] st = {"1111","2","apple"}
        /// 返回的是3个中的1个.
        /// </summary>
        /// <param name="st">string[]</param>
        /// <returns>string</returns>
        public static string getRandOneFromStrs(string[] st) {
            if (st.Length == 0) {
                return "";
            }
            Random rd = new Random();
            int i = rd.Next(st.Length);
            string re = st[i].ToString().Trim();
            return re;
        }

        public static string getFirst(string[] st) {
            string re = "";
            try {
                re = st[0].ToString();
            } catch {
            }
            return re;
        }

        public static string getLast(string[] st) {
            string re = "";
            try {
                re = st[st.Length - 1].ToString();
            } catch {
            }
            return re;
        }


        /// <summary>
        /// 把ArrayList数组，转化为string类型，1,2,3,4,5
        /// </summary>
        /// <param name="al">ArrayList</param>
        /// <returns>1,23,"22","abc"</returns>
        public static string getStrs(ArrayList al) {
            string re = string.Empty;
            for (int i = 0; i < al.Count; i++) {
                re += "," + al[i].ToString();
            }
            if (re.StartsWith(",")) {
                re = re.TrimStart(',');
            }
            return re;
        }

        /// <summary>
        /// 随机返回一个字符串数组中的一个,例如 string[] st = {"1111","2","apple"}
        /// 返回的是3个中的1个.
        /// </summary>
        /// <param name="st">string[]</param>
        /// <returns>string</returns>
        public static string getStrs(string[] st) {
            string re = string.Empty;
            for (int i = 0; i < st.Length; i++) {
                re += "," + st[i].ToString();
            }
            if (re.StartsWith(",")) {
                re = re.TrimStart(',');
            }
            return re;
        }

    }
}
