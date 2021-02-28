using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace X_Service.Util {
    public class ThreadHelper {
        
        public Thread m_thread;

        public ThreadHelper ( string name , ThreadModel ClassName ) {
            m_thread = new Thread(new ThreadStart(test01));
            m_thread.Name = "t0011";

            Thread.GetNamedDataSlot("t0011");
        }


        public void test01 ( ) {
        }
    }


    public class ThreadModel{
        public string t_name;
        public void t_method ( ) { 
            
        }
        //public string 
    }
}
