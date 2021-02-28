using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

/*
 * 
 * NOTE : These classes and logic will work only and only if the
 * following key in the registry is set
 * HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\EnableToolTips\
 * 
*/

namespace BalloonCS
{
//	public enum TooltipIcon : int
//	{
//		None,
//		Info,
//		Warning,
//		Error
//	}

	internal class BalloonTool : NativeWindow
	{
	}

	/// <summary>
	/// A sample class to manipulate ballon tooltips.
	/// Windows XP balloon-tips if used properly can 
	/// be very helpful.
	/// This class creates a balloon tooltip.
	/// This becomes useful for showing important information 
	/// quickly to the user.
	/// For example in a data-entry form full of 
	/// controls the most important
	/// and used control is the Order Placement button.
	/// Guide the user by using this hover balloon on it.
	/// This helps in a shorter learning cycle of the 
	/// application.
	/// </summary>
	public class HoverBalloon : IDisposable
	{
		private BalloonTool m_tool = null;
		
		private int m_maxWidth = 250;
		private string m_displayText = "FMS HoverBalloon Tooltip Control Display text";
		private string m_title = "FMS HoverBalloon";
		private TooltipIcon m_titleIcon = TooltipIcon.None;

		private const string TOOLTIPS_CLASS = "tooltips_class32";
		private const int WS_POPUP = unchecked((int)0x80000000);
		private const int SWP_NOSIZE = 0x0001;
		private const int SWP_NOMOVE = 0x0002;
		private const int SWP_NOACTIVATE = 0x0010;
		private const int WM_USER = 0x0400;
		private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

		[StructLayout(LayoutKind.Sequential)]
		private struct TOOLINFO
		{
			public int cbSize;
			public int uFlags;
			public IntPtr hwnd;
			public IntPtr uId;
			public Rectangle rect;
			public IntPtr hinst;
			[MarshalAs(UnmanagedType.LPTStr)] 
			public string lpszText;
			public uint lParam;
		}
		private const int TTS_ALWAYSTIP = 0x01;
		private const int TTS_NOPREFIX = 0x02;
		private const int TTS_BALLOON = 0x40;
		private const int TTF_SUBCLASS = 0x0010;
		private const int TTF_TRANSPARENT = 0x0100;
		private const int TTM_ADDTOOL = WM_USER + 50;
		private const int TTM_SETMAXTIPWIDTH = WM_USER + 24;
		private const int TTM_SETTITLE = WM_USER + 33;

		[DllImport("User32", SetLastError=true)]
		private static extern bool SetWindowPos(
			IntPtr hWnd,
			IntPtr hWndInsertAfter,
			int X,
			int Y,
			int cx,
			int cy,
			int uFlags);
		[DllImport("User32", SetLastError=true)]
		private static extern int GetClientRect(
			IntPtr hWnd,
			ref Rectangle lpRect);
		[DllImport("User32", SetLastError=true)]
		private static extern int SendMessage(
			IntPtr hWnd,
			int Msg,
			int wParam,
			IntPtr lParam);

		public HoverBalloon()
		{
			m_tool = new BalloonTool();
		}

		private bool disposed = false;
		public void Dispose()
		{
			Dispose(true);
			// Take yourself off the Finalization queue 
			// to prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(!this.disposed)
			{
				if(disposing)
				{
					// release managed resources if any
				}
				
				// release unmanaged resource
				m_tool.DestroyHandle();

				// Note that this is not thread safe.
				// Another thread could start disposing the object
				// after the managed resources are disposed,
				// but before the disposed flag is set to true.
				// If thread safety is necessary, it must be
				// implemented by the client.
			}
			disposed = true;
		}
		
		// Finalizer 
		~HoverBalloon()      
		{
			Dispose(false);
		}

		public void SetToolTip(Control parent, string tipText)
		{
			System.Diagnostics.Debug.Assert(parent.Handle!=IntPtr.Zero, "parent hwnd is null", "SetToolTip");
			
			m_displayText = tipText;
			
			CreateParams cp = new CreateParams();
			cp.ClassName = TOOLTIPS_CLASS;
			cp.Style = WS_POPUP | TTS_BALLOON | TTS_NOPREFIX |	TTS_ALWAYSTIP;

			cp.Parent = parent.Handle;

			// create the tool
			m_tool.CreateHandle(cp);

			// make sure we make it the top level window
			SetWindowPos(
				m_tool.Handle, 
				HWND_TOPMOST, 
				0, 0, 0, 0,
				SWP_NOACTIVATE | 
				SWP_NOMOVE | 
				SWP_NOSIZE);

			// create and fill in the tool tip info
			TOOLINFO ti = new TOOLINFO();
			ti.cbSize = Marshal.SizeOf(ti);
			ti.uFlags = TTF_TRANSPARENT | TTF_SUBCLASS;
			ti.hwnd = parent.Handle;
			ti.lpszText = m_displayText;
			
			// get the display co-ordinates
			GetClientRect(parent.Handle, ref ti.rect);

			// add the tool tip
			IntPtr ptrStruct = Marshal.AllocHGlobal(Marshal.SizeOf(ti));
			Marshal.StructureToPtr(ti, ptrStruct, true);

			SendMessage(
				m_tool.Handle, 
				TTM_ADDTOOL, 
				0, 
				ptrStruct);

			ti = (TOOLINFO)Marshal.PtrToStructure(ptrStruct, typeof(TOOLINFO));

			Marshal.FreeHGlobal(ptrStruct);

			SendMessage(
				m_tool.Handle, 
				TTM_SETMAXTIPWIDTH, 
				0, 
				new IntPtr(m_maxWidth));


			if(m_title != null || m_title!=string.Empty)
			{
				IntPtr ptrTitle = Marshal.StringToHGlobalAuto(m_title);

				SendMessage(
					m_tool.Handle, TTM_SETTITLE, 
					(int)m_titleIcon, ptrTitle);

				Marshal.FreeHGlobal(ptrTitle);
			}


		}



		/// <summary>
		/// Sets the maximum text width of the tooltip text.
		/// At that length the tooltip will start wrapping text 
		/// over to the next line. 
		/// </summary>
		public int MaximumTextWidth
		{
			get
			{
				return m_maxWidth;
			}
			set
			{
				m_maxWidth = value;
			}
		}

		/// <summary>
		/// Sets up the title of the tooltip.
		/// </summary>
		public string Title
		{
			get
			{
				return m_title;
			}
			set
			{
				m_title = value;
			}
		}

		/// <summary>
		/// Sets the icon for the tooltip.
		/// </summary>
		public TooltipIcon TitleIcon
		{
			get
			{
				return m_titleIcon;
			}
			set
			{
				m_titleIcon = value;
			}
		}

	}
}
