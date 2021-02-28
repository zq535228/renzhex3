/*
 * 类库名称：有关TreeView控件操作的公用类
 * 版本号：1.0.0.1
 * 开发语言：.NET2008 C#
 * 作者：邓泽波
 * E-Mail：dengzebo@163.com；dengzebo@cdce.cn  
 * 编制日期：2008年11月08日
 * 修订日期：
 */

using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.IO;
using System.Windows.Forms;

namespace X_Service.Ctrl {

    /// <summary>
    /// 有关TreeView控件操作的公用类
    /// </summary>
    public class TreeViewC {

        #region "保存树结点到 DataTable"
        /// <summary>
        /// 保存树结点到 DataTable
        /// 当节点为顶级时，它的 父编号='';节点的Tag值将作为编号值；节点父节点的Tag值将作为父编号的值
        /// </summary>
        /// <param name="TreeView1">要保存到DataTable的TreeView控件</param>
        /// <param name="strParentNumberFieldText">要保存到DataTable中的树节点父编号字段名称</param>
        /// <param name="strNumberFieldText">要保存到DataTable中的树节点编号字段名称</param>
        /// <param name="strNameFieldText">要保存到DataTable中的树节点Text字段名称</param>
        /// <returns>DataTable</returns>
        public DataTable SaveToDataTable ( TreeView TreeView1 , string strParentNumberFieldText , string strNumberFieldText , string strNameFieldText ) {
            try {
                DataTable DT = new DataTable("HRTreeView");
                DataColumn ParentNumber = new DataColumn(strParentNumberFieldText , System.Type.GetType("System.String"));
                ParentNumber.MaxLength = 20;
                ParentNumber.AllowDBNull = true;
                DataColumn Number = new DataColumn(strNumberFieldText , System.Type.GetType("System.String"));
                Number.MaxLength = 20;
                Number.AllowDBNull = false;
                DataColumn Name = new DataColumn(strNameFieldText , System.Type.GetType("System.String"));
                Name.MaxLength = 500;
                Name.AllowDBNull = false;

                DT.Columns.Add(ParentNumber);
                DT.Columns.Add(Number);
                DT.Columns.Add(Name);

                FindTreeView(TreeView1 , DT);
                DT.AcceptChanges();
                return DT;
            } catch {
                return null;
            }

        }

        /// <summary>
        /// 循环查找TreeView树的一级节点
        /// </summary>
        /// <param name="TreeView1">要查找的TreeView控件</param>
        /// <param name="DT">用来保存TreeView控件结点结构的DataTable</param>
        private void FindTreeView ( TreeView TreeView1 , DataTable DT ) {
            foreach ( TreeNode TempNode in TreeView1.Nodes ) {
                ForTreeNodeToDataTable(TempNode , DT);
            }
        }

        /// <summary>
        /// 遍历传入的一级树节点的所有子节点并将其值保存到DataRow，进而添加到DataTable中
        /// </summary>
        /// <param name="TempNode">传入的一级节点</param>
        /// <param name="DT">用来保存TreeView控件结点结构的DataTable</param>
        private void ForTreeNodeToDataTable ( TreeNode TempNode , DataTable DT ) {
            DataRow DR;
            DR = DT.NewRow();
            if ( TempNode.Parent == null )//TempNode.Level==0
            {
                DR[0] = "";
            } else {
                DR[0] = TempNode.Parent.Tag;
            }
            DR[1] = TempNode.Tag.ToString();
            DR[2] = TempNode.Text;

            DT.Rows.Add(DR);
            foreach ( TreeNode aNode in TempNode.Nodes ) {
                ForTreeNodeToDataTable(aNode , DT);
            }
        }
        #endregion

        #region "保存树结点到 Xml 文件"
        /// <summary>
        /// 保存树结点到 Xml 文件
        /// 当节点为顶级时，它的 父编号='';节点的Tag值将作为编号值；节点父节点的Tag值将作为父编号的值
        /// </summary>
        /// <param name="TreeView1">要保存到 Xml 文件的TreeView控件</param>
        /// <param name="strParentNumberFieldText">要保存到 Xml 文件中的树节点父编号字段名称</param>
        /// <param name="strNumberFieldText">要保存到 Xml 文件中的树节点编号字段名称</param>
        /// <param name="strNameFieldText">要保存到 Xml 文件中的树节点Text字段名称</param>
        /// <param name="XmlFilePath">要保存到 Xml 文件的存放路径</param>
        /// <returns>True/False</returns>
        public bool SaveToXmlFile ( TreeView TreeView1 , string strParentNumberFieldText , string strNumberFieldText , string strNameFieldText , string XmlFilePath ) {

            DataTable DT = SaveToDataTable(TreeView1 , strParentNumberFieldText , strNumberFieldText , strNameFieldText);
            if ( DT != null ) {
                DT.WriteXml(XmlFilePath);
                return true;
            } else {
                return false;
            }
        }
        #endregion

        #region "读取树结点从Datatable"
        /// <summary>
        /// 读取树结点从Datatable"
        /// </summary>
        /// <param name="TreeView1">在填充的TreeView控件</param>
        /// <param name="DT">数据源DataTable</param>
        /// <param name="IsAppendNode">是在现有TreeView控件上添加结点，还是清空再添加</param>
        /// <param name="ParentNumberColumnIndex">在DataTable中，代表父节点编号的列索引</param>
        /// <param name="NumberColumnIndex">在DataTable中，代表当前节点编号的列索引</param>
        /// <param name="NameColumnIndex">在DataTable中，代表当前节点名称的列索引</param>
        /// <returns>True/False</returns>
        public bool ReadNodesFromDataTable ( TreeView TreeView1 , DataTable DT , bool IsAppendNode , int ParentNumberColumnIndex , int NumberColumnIndex , int NameColumnIndex ) {
            try {
                if ( IsAppendNode == false ) {
                    TreeView1.Nodes.Clear();
                }
                if ( DT != null && DT.Rows.Count > 0 ) {
                    DataRow[] DR = null;
                    DR = DT.Select(DT.Columns[ParentNumberColumnIndex].ColumnName + "='' or " + DT.Columns[ParentNumberColumnIndex].ColumnName + " is null");//先将顶级的查出来
                    for ( int I = 0; I <= DR.Length - 1; I++ )//先将顶级的加入到TreeView中
                    {
                        TreeNode TNode = new TreeNode(DR[I][DT.Columns[NameColumnIndex].ColumnName].ToString());
                        TNode.Tag = DR[I][DT.Columns[NumberColumnIndex].ColumnName].ToString();
                        TNode.Name = DR[I][DT.Columns[NameColumnIndex].ColumnName].ToString();
                        TreeView1.Nodes.Add(TNode);
                    }
                    for ( int I = 0; I <= TreeView1.Nodes.Count - 1; I++ )//再递归遍历结点
                    {
                        ForTreeNodeFormDT(TreeView1.Nodes[I] , DT , ParentNumberColumnIndex , NumberColumnIndex , NameColumnIndex);
                    }
                }
                return false;
            } catch {
                return true;
            }
        }

        /// <summary>
        /// 从DT中递归遍历出结点
        /// </summary>
        /// <param name="TempNode">传入的顶级结点</param>
        /// <param name="DT">保存TreeView结构的DataTable</param>
        /// <param name="ParentNumberColumnIndex">在DataTable中，代表父节点编号的列索引</param>
        /// <param name="NumberColumnIndex">在DataTable中，代表当前节点编号的列索引</param>
        /// <param name="NameColumnIndex">在DataTable中，代表当前节点名称的列索引</param>
        private void ForTreeNodeFormDT ( TreeNode TempNode , DataTable DT , int ParentNumberColumnIndex , int NumberColumnIndex , int NameColumnIndex ) {
            string TTag = null;
            TTag = TempNode.Tag.ToString();
            DataRow[] DR = null;
            DR = DT.Select(DT.Columns[ParentNumberColumnIndex].ColumnName + "='" + TTag + "'");

            for ( int I = 0; I <= DR.Length - 1; I++ ) {
                TreeNode TNode = new TreeNode(DR[I][DT.Columns[NameColumnIndex].ColumnName].ToString());
                TNode.Tag = DR[I][DT.Columns[NumberColumnIndex].ColumnName].ToString();
                TNode.Name = DR[I][DT.Columns[NameColumnIndex].ColumnName].ToString();
                TempNode.Nodes.Add(TNode);
            }

            foreach ( TreeNode aNode in TempNode.Nodes ) {
                ForTreeNodeFormDT(aNode , DT , ParentNumberColumnIndex , NumberColumnIndex , NameColumnIndex);
            }
        }
        #endregion

        #region "读取树结点从 Xml 文件"
        /// <summary>
        /// 读取树结点从 Xml 文件
        /// </summary>
        /// <param name="TreeView1">在填充的TreeView控件</param>
        /// <param name="XmlFilePath">数据源 Xml 文件全路径</param>
        /// <param name="IsAppendNode">是在现有TreeView控件上添加结点，还是清空再添加</param>
        /// <param name="ParentNumberXmlNodeIndex">在Xml文件中，代表父节点编号的列索引</param>
        /// <param name="NumberXmlNodeIndex">在Xml文件中，代表当前节点编号的列索引</param>
        /// <param name="NameXmlNodeIndex">在Xml文件中，代表当前节点名称的列索引</param>
        /// <returns>True/False</returns>
        public bool ReadNodesFromXmlFile ( TreeView TreeView1 , string XmlFilePath , bool IsAppendNode , int ParentNumberXmlNodeIndex , int NumberXmlNodeIndex , int NameXmlNodeIndex ) {
            if ( System.IO.File.Exists(XmlFilePath) == true ) {
                DataSet DS = new DataSet();
                DS.ReadXml(XmlFilePath);
                if ( ReadNodesFromDataTable(TreeView1 , DS.Tables[0] , IsAppendNode , ParentNumberXmlNodeIndex , NumberXmlNodeIndex , NameXmlNodeIndex) == true ) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }
        #endregion

        #region "设置树结点的TAG"

        /// <summary>
        /// 设置树结点的Tag，其TreeView控件的Tag将已级次编号自动设置，
        /// 如：01 0101 02 一般用于保存到DataTable或Xml前使用
        /// </summary>
        /// <param name="TreeView1">要设置的TreeView控件</param>
        /// <returns>是否成功设置</returns>
        public bool SetTreeViewTag ( TreeView TreeView1 ) {
            try {
                foreach ( TreeNode TempNode in TreeView1.Nodes ) {
                    ForTreeNodeSetTag(TempNode);
                }
                return true;
            } catch {
                return false;
            }
        }
        /// <summary>
        /// 循环遍历TreeView的节点
        /// </summary>
        /// <param name="TempNode">传入的一级节点</param>
        private void ForTreeNodeSetTag ( TreeNode TempNode ) {
            SetTagNumber(TempNode);
            foreach ( TreeNode aNode in TempNode.Nodes ) {
                ForTreeNodeSetTag(aNode);
            }
        }

        /// <summary>
        /// 设置当前节点的Tag值，设置它的级次编号,节点的编号规则为：2,4,6,8....二位编码
        /// </summary>
        /// <param name="TempNode">需要设置的节点</param>
        private void SetTagNumber ( TreeNode TempNode ) {
            string TTag = "";
            if ( TempNode.Parent == null ) {
                if ( TempNode.PrevNode == null )//上一个同级结点为空
                {
                    TTag = "01";//第一个节点
                } else {
                    TTag = ( ( System.Convert.ToInt64(TempNode.PrevNode.Tag.ToString()) + 1 ).ToString().PadLeft(TempNode.PrevNode.Tag.ToString().Length , '0') );
                }
            } else {
                if ( TempNode.PrevNode == null ) {
                    TTag = TempNode.Parent.Tag.ToString() + "01";
                } else {
                    TTag = ( ( System.Convert.ToInt64(TempNode.PrevNode.Tag.ToString()) + 1 ).ToString().PadLeft(TempNode.PrevNode.Tag.ToString().Length , '0') );
                }
            }
            TempNode.Tag = TTag;
        }
        #endregion

        #region "控制树节点移动,向左右下上"
        /// <summary>
        /// 通过Ctrl+键盘移动选定的树节点
        /// </summary>
        /// <param name="TreeView1">要编辑的TreeView控件</param>
        /// <param name="eKeys">The <see cref="System.Windows.Forms.KeyEventArgs"/>KeyEventArgs为按键事件提供数据</param>
        public void MoveSelectNode ( TreeView TreeView1 , KeyEventArgs eKeys ) {
            try {
                if ( TreeView1.SelectedNode == null ) {
                    return;
                }
                try {
                    if ( eKeys.KeyCode == Keys.Up && eKeys.Control == true ) {
                        TreeNode TN = new TreeNode();
                        TN = TreeView1.SelectedNode;
                        TreeNode TempNode = (TreeNode)TreeView1.SelectedNode.Clone();
                        if ( TreeView1.SelectedNode.PrevNode == null ) {
                            return;
                        }
                        if ( TN.Parent == null ) {
                            TreeView1.Nodes.Insert(TN.Index - 1 , TempNode);
                        } else {
                            TN.Parent.Nodes.Insert(TN.Index - 1 , TempNode);
                        }
                        TreeView1.SelectedNode.Remove();
                        TreeView1.SelectedNode = TempNode;
                    } else if ( eKeys.KeyCode == Keys.Down && eKeys.Control == true ) {
                        TreeNode TN = new TreeNode();
                        TN = TreeView1.SelectedNode;
                        TreeNode TempNode = (TreeNode)TreeView1.SelectedNode.Clone();
                        if ( TreeView1.SelectedNode.NextNode == null ) {
                            return;
                        }
                        if ( TN.Parent == null ) {
                            TreeView1.Nodes.Insert(TN.Index + 2 , TempNode);
                        } else {
                            TN.Parent.Nodes.Insert(TN.Index + 2 , TempNode);
                        }
                        TreeView1.SelectedNode.Remove();
                        TreeView1.SelectedNode = TempNode;
                    } else if ( eKeys.KeyCode == Keys.Left && eKeys.Control == true ) {
                        TreeNode TN = new TreeNode();
                        TN = TreeView1.SelectedNode;
                        TreeNode TempNode = (TreeNode)TreeView1.SelectedNode.Clone();
                        if ( TreeView1.SelectedNode.Parent == null ) {
                            return;
                        } else {
                            if ( TreeView1.SelectedNode.Parent.Parent == null ) {
                                TreeView1.Nodes.Add(TempNode);
                            } else {
                                TN.Parent.Parent.Nodes.Add(TempNode);
                            }
                        }
                        TN.Remove();
                        TreeView1.SelectedNode = TempNode;
                    } else if ( eKeys.KeyCode == Keys.Right && eKeys.Control == true ) {
                        TreeNode TN = new TreeNode();
                        TN = TreeView1.SelectedNode;
                        TreeNode TempNode = (TreeNode)TreeView1.SelectedNode.Clone();
                        if ( TreeView1.SelectedNode.NextNode == null ) {
                            return;
                        }
                        TN.NextNode.Nodes.Insert(0 , TempNode);
                        TN.Remove();
                        TreeView1.SelectedNode = TempNode;
                    }
                } catch {
                }
            } catch { }
        }
        #endregion

        #region "查找遍历树节点"
        /// <summary>
        /// 查找遍历树的节点，根据节点名称
        /// </summary>
        /// <param name="TreeView1">要查找的TreeView</param>
        /// <param name="FindNodeName">要查找的树的节点的名称(Name)</param>
        /// <returns>查找到的TreeNode[]数组</returns>
        public TreeNode[] FindTreeNodeFromNodeText ( TreeView TreeView1 , string FindNodeName ) {
            return TreeView1.Nodes.Find(FindNodeName , true);

        }
        /// <summary>
        /// 查找遍历树的节点，根据节点全路径
        /// </summary>
        /// <param name="TreeView1">要查找的TreeView</param>
        /// <param name="FindFullPath">要查找的树的节点的全路径</param>
        /// <returns>查找到的TreeNode</returns>
        public TreeNode FindTreeNodeFromFullPath ( TreeView TreeView1 , string FindFullPath ) {
            string NodeText = "";
            if ( FindFullPath.LastIndexOf("\\") > 0 ) {
                NodeText = FindFullPath.Substring(FindFullPath.LastIndexOf("\\") + 1);
            } else {
                NodeText = FindFullPath;
            }
            TreeNode[] TNS = FindTreeNodeFromNodeText(TreeView1 , NodeText);
            foreach ( TreeNode TN in TNS ) {
                if ( TN.FullPath == FindFullPath ) {
                    return TN;
                }
            }
            return null;
        }
        /// <summary>
        ///查找遍历树的节点，根据节点Tag
        /// </summary>
        /// <param name="TreeView1">要查找的TreeView</param>
        /// <param name="FindTag">要查找的树的节点的Tag</param>
        /// <returns>查找到的TreeNode[]</returns>
        public TreeNode[] FindTreeNodeFromTag ( TreeView TreeView1 , string FindTag ) {
            TreeNode[] TNList = null;
            if ( TreeView1.Nodes == null ) {
                return null;
            }
            System.Collections.ArrayList FindNodeList = new System.Collections.ArrayList();
            foreach ( TreeNode TempNode in TreeView1.Nodes ) {
                // ref 关键字使参数按引用传递。其效果是,当控制权传递回调用方法时,
                //在方法中对参数所做的任何更改都将反映在该变量中。
                //out与ref完全一样，除了out的参数不用先付初使值外
                ForTreeNode(TempNode , FindTag , ref FindNodeList);
            }
            if ( FindNodeList.Count > 0 ) {
                TNList = new TreeNode[FindNodeList.Count];
                for ( int i = 0; i < FindNodeList.Count; i++ ) {
                    TNList[i] = (TreeNode)FindNodeList[i];
                }
            }
            return TNList;
        }

        /// <summary>
        /// 要遍历的传入的结点
        /// </summary>
        /// <param name="TempNode">遍历的树节点</param>
        /// <param name="FindTagString">要查找的Tag值</param>
        /// <param name="FindNodeList">通过ref标识的动态保存结果数组</param>
        private void ForTreeNode ( TreeNode TempNode , string FindTagString , ref System.Collections.ArrayList FindNodeList ) {
            if ( TempNode.Tag != null && TempNode.Tag.ToString() == FindTagString ) {
                FindNodeList.Add(TempNode);
            }
            foreach ( TreeNode aNode in TempNode.Nodes ) {
                ForTreeNode(aNode , FindTagString , ref FindNodeList);
            }
        }
        #endregion

        #region "加载文件到树"
        /// <summary>
        /// 加载文件夹文件到树
        /// 并包含ImageList文件类型图标，注意：在ImageList控件的图标列表中，图标的Name值按下面规则命名
        /// 文件夹默认：close；文件夹打开：open；其它按文件扩展名命名，如：doc
        /// </summary>
        /// <param name="TV">要加载文件列表的TreeView控件</param>
        /// <param name="DirPath">文件夹全路径</param>
        /// <param name="ImgList">TreeView对应的ImageList</param>
        /// <returns></returns>
        public bool LoadFileToTreeView ( TreeView TV , string DirPath , ImageList ImgList ) {
            TV.Nodes.Clear();
            TV.ImageList = ImgList;
            foreach ( DirectoryInfo TD in new DirectoryInfo(DirPath).GetDirectories() ) {
                TreeNode TN = new TreeNode(TD.Name);
                TN.Tag = TD.FullName;
                SetTreeNodeIcon(TN , "dir" , ImgList);
                TV.Nodes.Add(TN);
                LoadDirAndFile(TN , TD , ImgList);
                foreach ( FileInfo FI in new DirectoryInfo(TD.FullName).GetFiles() ) {
                    if ( FI.Extension != "" ) {
                        string nodeText = "";
                        if ( FI.Extension != "" ) {
                            nodeText = FI.Name.Replace(FI.Extension , "");
                        } else {
                            nodeText = FI.Name;
                        }

                        TreeNode TNN = new TreeNode(nodeText);
                        TNN.Tag = FI.FullName;
                        SetTreeNodeIcon(TNN , FI.Extension , ImgList);
                        TN.Nodes.Add(TNN);
                    } else {
                        TreeNode TNN = new TreeNode(FI.Name);
                        TNN.Tag = FI.FullName;
                        SetTreeNodeIcon(TNN , FI.Extension , ImgList);
                        TN.Nodes.Add(TNN);
                    }
                }
            }
            foreach ( FileInfo FI in new DirectoryInfo(DirPath).GetFiles() ) {
                string nodeText = "";
                if ( FI.Extension != "" ) {
                    nodeText = FI.Name.Replace(FI.Extension , "");
                } else {
                    nodeText = FI.Name;
                }
                TreeNode TN = new TreeNode(nodeText);
                TN.Tag = FI.FullName;
                SetTreeNodeIcon(TN , FI.Extension , ImgList);
                TV.Nodes.Add(TN);
            }
            return true;
        }
        /// <summary>
        /// 加载传入的文件夹文件列表
        /// </summary>
        /// <param name="CurTN">当前TreeNode</param>
        /// <param name="CurDir">当前文件夹</param>
        /// <param name="ImgList">TreeView对应的ImageList</param>
        private void LoadDirAndFile ( TreeNode CurTN , DirectoryInfo CurDir , ImageList ImgList ) {
            foreach ( DirectoryInfo TD in new DirectoryInfo(CurDir.FullName).GetDirectories() ) {
                TreeNode TN = new TreeNode(TD.Name);
                TN.Tag = TD.FullName;
                SetTreeNodeIcon(TN , "dir" , ImgList);
                CurTN.Nodes.Add(TN);
                foreach ( FileInfo FI in new DirectoryInfo(TD.FullName).GetFiles() ) {
                    string nodeText = "";
                    if ( FI.Extension != "" ) {
                        nodeText = FI.Name.Replace(FI.Extension , "");
                    } else {
                        nodeText = FI.Name;
                    }

                    TreeNode TNN = new TreeNode(nodeText);
                    TNN.Tag = FI.FullName;
                    SetTreeNodeIcon(TNN , FI.Extension , ImgList);
                    TN.Nodes.Add(TNN);
                }

                LoadDirAndFile(TN , TD , ImgList);
            }

        }

        /// <summary>
        /// 设置树节点的图标
        /// </summary>
        /// <param name="CurTN">当前TreeNode</param>
        /// <param name="strLX">TreeNode类型，文件夹为：dir；其它的为文件扩展名</param>
        /// <param name="ImgList">TreeView对应的ImageList</param>
        private void SetTreeNodeIcon ( TreeNode CurTN , string strLX , ImageList ImgList ) {
            strLX = strLX.Replace("." , "");
            if ( strLX.ToLower() == "dir" ) {
                CurTN.ImageIndex = ImgList.Images.IndexOfKey("close");
                CurTN.SelectedImageIndex = ImgList.Images.IndexOfKey("open");
            } else {
                CurTN.ImageIndex = ImgList.Images.IndexOfKey(strLX);
                CurTN.SelectedImageIndex = ImgList.Images.IndexOfKey(strLX);
            }
        }
        #endregion

        #region "加载文件夹到树"
        /// <summary>
        /// 加载文件夹到树
        /// </summary>
        /// <param name="TV">加载文件夹到TreeView控件</param>
        /// <param name="DirPath">文件夹全路径</param>
        /// <returns></returns>
        public bool LoadDirToTreeView ( TreeView TV , string DirPath ) {
            TV.Nodes.Clear();
            foreach ( DirectoryInfo TD in new DirectoryInfo(DirPath).GetDirectories() ) {
                TreeNode TN = new TreeNode(TD.Name);
                TN.Tag = TD.FullName;
                TV.Nodes.Add(TN);
                LoadDirOnly(TN , TD);
            }
            return true;
        }
        /// <summary>
        /// 加载传入的文件夹文件列表
        /// </summary>
        /// <param name="CurTN">当前TreeNode</param>
        /// <param name="CurDir">当前文件夹</param>
        private void LoadDirOnly ( TreeNode CurTN , DirectoryInfo CurDir ) {
            foreach ( DirectoryInfo TD in new DirectoryInfo(CurDir.FullName).GetDirectories() ) {
                TreeNode TN = new TreeNode(TD.Name);
                TN.Tag = TD.FullName;
                CurTN.Nodes.Add(TN);
                LoadDirOnly(TN , TD);
            }
        }

        #endregion

    }
}
