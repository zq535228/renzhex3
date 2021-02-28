using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace Plugin
{
    public interface IApplication : IServiceContainer
    {
        //MenuStrip MyMenuStrip { get; }
        //ToolStrip MyToolStrip { get; }
        //TreeView MyTreeView { get; }
        //RichTextBox MyRichTextBox { get; }
        //GroupBox MyGroupBox { get; }
        FlowLayoutPanel MyFLP { get; }
    }
}
