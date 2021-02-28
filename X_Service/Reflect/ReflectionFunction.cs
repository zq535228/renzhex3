using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using X_Service.Util;

namespace X_Service.Reflect {
    /// <summary>
    /// 反射调用功能组件
    /// </summary>
    public class ReflectionFunction {

        /// <summary>
        /// 反射调用Mdi窗体
        /// </summary>
        /// <param name="FunctionAssemblyFileName">功能程序集文件名称</param>
        /// <param name="AssemblyNamespaceAndClass">[功能程序集命名空间].[类名（窗体名）]</param>
        /// <param name="MdiParentForm">Mdi父窗体</param>
        /// <param name="functionParameter">功能模块传递参数组件HR.Win.FunctionParameter</param>
        /// <returns></returns>
        public Form LoadMdiForm(string FunctionAssemblyFileName, string AssemblyNamespaceAndClass, Form MdiParentForm) {
            return LoadForm(FunctionAssemblyFileName, AssemblyNamespaceAndClass, MdiParentForm);
        }
        /// <summary>
        /// 反射调用模式Dialog窗体
        /// </summary>
        /// <param name="FunctionAssemblyFileName">功能程序集文件名称</param>
        /// <param name="AssemblyNamespaceAndClass">[功能程序集命名空间].[类名（窗体名）]</param>
        /// <param name="functionParameter">功能模块传递参数组件HR.Win.FunctionParameter</param>
        /// <returns></returns>
        public Form LoadDialogForm(string FunctionAssemblyFileName, string AssemblyNamespaceAndClass) {
            return LoadForm(FunctionAssemblyFileName, AssemblyNamespaceAndClass, null);
        }
        /// <summary>
        /// 反射调用Mdi窗体
        /// </summary>
        /// <param name="FunctionAssemblyFileName">功能程序集文件名称</param>
        /// <param name="AssemblyNamespaceAndClass">[功能程序集命名空间].[类名（窗体名）]</param>
        /// <param name="MdiParentForm">Mdi父窗体</param>
        /// <param name="functionParameter">功能模块传递参数组件HR.Win.FunctionParameter</param>
        /// <returns></returns>
        private Form LoadForm(string FunctionAssemblyFileName, string AssemblyNamespaceAndClass, Form MdiParentForm) {
            string AssemblyFilePath = Application.StartupPath + "\\" + FunctionAssemblyFileName;
            if (File.Exists(AssemblyFilePath) == true) {
                System.Reflection.Assembly assembly = Assembly.LoadFile(AssemblyFilePath);
                Type FormType = assembly.GetType(AssemblyNamespaceAndClass, true, true);
                Form frm = FormType.InvokeMember(null,
                                 System.Reflection.BindingFlags.DeclaredOnly
                                | System.Reflection.BindingFlags.Public
                                | System.Reflection.BindingFlags.NonPublic
                                | System.Reflection.BindingFlags.Instance
                                | System.Reflection.BindingFlags.CreateInstance,
                                null, null, null) as Form;
                if (frm.MaximizeBox == true) {
                    frm.WindowState = FormWindowState.Maximized;
                } else {
                    frm.WindowState = FormWindowState.Normal;
                }
                Application.DoEvents();
                return frm;
            }

            return null;
        }


    }
}
