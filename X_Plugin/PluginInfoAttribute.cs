using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginInfoAttribute : System.Attribute
    {
        public PluginInfoAttribute() { }
        public PluginInfoAttribute(string name, string version, string author, string webpage, bool loadWhenStart)
        {
            this._Name = name;
            this._Version = version;
            this._Author = author;
            this._Webpage = webpage;
            this._LoadWhenStart = loadWhenStart;
        }
        public string Name { get { return _Name; } }
        public string Version { get { return _Version; } }
        public string Author { get { return _Author; } }
        public string Webpage { get { return _Webpage; } }
        public bool LoadWhenStart { get { return _LoadWhenStart; } }
        ///
        /// 用来存储一些有用的信息
        ///
        public object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }
        ///
        /// 用来存储序号
        ///
        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }

        private string _Name = "";
        private string _Version = "";
        private string _Author = "";
        private string _Webpage = "";
        private object _Tag = null;
        private int _Index = 0;
        // 暂时不会用
        private bool _LoadWhenStart = true;
    }
}