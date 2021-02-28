using System;
using System.Collections.Generic;
using System.Text;

namespace X_Service.Db {
    public static class DbConvert<T> {
        public static T getObj(string filePath, string key) {
            T t = default(T);
            object obj = new DbTools().Read(filePath, key);
            if (obj != null) {
                try {
                    t = (T)obj;
                } catch (Exception) {
                    new DbTools().debugVdb();
                }
            }
            return t;
        }
    }
}
