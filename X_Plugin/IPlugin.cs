using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin
{
    public interface  IPlugin
    {
        void load();
        void dispose();
        IApplication Application { get; set; }
    }
}
