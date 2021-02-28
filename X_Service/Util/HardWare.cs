using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Management;
using System.ComponentModel;


namespace X_Service.Util {
    public class HardWare {

        /// <summary>
        /// 返回硬盘的编码信息
        /// </summary>
        /// <returns></returns>
        public static string getHardCode ( ) {
            string code = new HardWare().GetCpuID() + new HardWare().GetHardID();
            code = StringHelper.CreateMd5(code , 32);
            return code;
        }

        public string GetHardID ( ) {
            String HDid = "";
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach ( ManagementObject mo in moc ) {
                HDid = (string)mo.Properties["Model"].Value;
            }
            return HDid;
        }

        public String GetCpuID ( ) {
            try {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                String strCpuID = null;
                foreach ( ManagementObject mo in moc ) {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            } catch {
                return "";
            }

        }


    }
}
