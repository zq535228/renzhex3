using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security;
using System.IO;
using System.ComponentModel;

namespace X_Service.Ctrl {
    public sealed class FolderBrowserDialogEx : CommonDialog {
        [DllImport("user32.dll" , CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage ( HandleRef hWnd , int msg , int wParam , string lParam );
        [DllImport("user32.dll" , CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage ( HandleRef hWnd , int msg , int wParam , int lParam );

        [DllImport("shell32.dll" , CharSet = CharSet.Auto)]
        private static extern bool SHGetPathFromIDList ( IntPtr pidl , IntPtr pszPath );
        [DllImport("shell32.dll")]
        private static extern int SHGetMalloc ( [Out , MarshalAs(UnmanagedType.LPArray)] IMalloc[] ppMalloc );
        [DllImport("shell32.dll")]
        private static extern int SHGetSpecialFolderLocation ( IntPtr hWnd , int csidl , ref IntPtr ppidl );
        [DllImport("shell32.dll" , CharSet = CharSet.Unicode)]
        private static extern int SHParseDisplayName ( string pszName , IntPtr pbc , ref IntPtr ppidl , ulong sfGaoIn , IntPtr psfGaoOut );
        [DllImport("shell32.dll" , CharSet = CharSet.Auto)]
        private static extern IntPtr SHBrowseForFolder ( [In] BROWSEINFO lpbi );

        public delegate bool FolderSelectableDelegate ( string folder );
        private delegate int FolderBrowseCallbackProc ( IntPtr hwnd , int msg , IntPtr lParam , IntPtr lpData );

        private static readonly int S_OK = 0;
        private const int MAX_PATH = 260;
        private const int BFFM_INITIALIZED = 1;
        private const int BFFM_SELCHANGED = 2;
        private const int WM_USER = 1024;
        private const int BFFM_ENABLEOK = WM_USER + 101;
        private const int ENABLE = 1;
        private const int DISABLE = 0;
        private static readonly int BFFM_SETSELECTION = 0x467;

        private FolderSelectableDelegate _folderSelectable;
        private FolderBrowseCallbackProc _callback;
        private string _description;
        private System.Environment.SpecialFolder _rootFolder;
        private string _rootFolderPath;
        private string _selectedPath;
        private bool _selectedPathNeedCheck;
        private bool _showNewFolderButton;

        public FolderBrowserDialogEx ( )
            : this(null) {
        }

        public FolderBrowserDialogEx ( FolderSelectableDelegate folderSelectable )
            : base() {
            _folderSelectable = folderSelectable;

            if ( null == _folderSelectable ) {
                _folderSelectable = delegate { return true; };
            }

            Reset();
        }

        public string Description {
            get {
                return _description;
            }
            set {
                _description = ( null == value ? string.Empty : value );
            }
        }

        public System.Environment.SpecialFolder RootFolder {
            get {
                return _rootFolder;
            }
            set {
                if ( Enum.IsDefined(typeof(System.Environment.SpecialFolder) , value) ) {
                    _rootFolder = value;
                }
            }
        }

        public string RootFolderPath {
            get {
                return _rootFolderPath;
            }
            set {
                if ( null != value && 0 < value.Length ) {
                    if ( !Directory.Exists(value) ) {
                        throw new Exception("The root folder must exist");
                    }

                    new FileIOPermission(FileIOPermissionAccess.PathDiscovery , value).Demand();
                }

                _rootFolderPath = ( null == value ? string.Empty : value );
            }
        }

        public string SelectedPath {
            get {
                if ( null != _selectedPath && 0 < _selectedPath.Length && _selectedPathNeedCheck ) {
                    new FileIOPermission(FileIOPermissionAccess.PathDiscovery , _selectedPath).Demand();
                }

                return _selectedPath;
            }
            set {
                _selectedPath = ( null == value ? string.Empty : value );
                _selectedPathNeedCheck = false;
            }
        }

        public bool ShowNewFolderButton {
            get {
                return _showNewFolderButton;
            }
            set {
                _showNewFolderButton = value;
            }
        }

        [Browsable(false) , EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler HelpRequest {
            add {
                base.HelpRequest += value;
            }
            remove {
                base.HelpRequest -= value;
            }
        }

        public override void Reset ( ) {
            _rootFolder = Environment.SpecialFolder.Desktop;
            _rootFolderPath = string.Empty;
            _description = string.Empty;
            _selectedPath = string.Empty;
            _selectedPathNeedCheck = false;
            _showNewFolderButton = false;
        }

        protected override bool RunDialog ( IntPtr hwndOwner ) {
            if ( Control.CheckForIllegalCrossThreadCalls && ( Application.OleRequired() != System.Threading.ApartmentState.STA ) ) {
                throw new Exception("Thread must be STA");
            }

            IntPtr pidlRoot = IntPtr.Zero;
            bool success = false;

            SHGetSpecialFolderLocation(hwndOwner , (int)_rootFolder , ref pidlRoot);
            if ( pidlRoot == IntPtr.Zero ) {
                SHGetSpecialFolderLocation(hwndOwner , (int)System.Environment.SpecialFolder.Desktop , ref pidlRoot);
                if ( pidlRoot == IntPtr.Zero ) {
                    throw new Exception("No root folder");
                }
            }

            if ( null != _rootFolderPath && 0 < _rootFolderPath.Length ) {
                IntPtr pidlTmp = IntPtr.Zero;
                IntPtr psfGaoOut = IntPtr.Zero;

                if ( S_OK == SHParseDisplayName(_rootFolderPath , IntPtr.Zero , ref pidlTmp , 0 , psfGaoOut) ) {
                    pidlRoot = pidlTmp;
                }
            }

            int num = 0x40;
            if ( !_showNewFolderButton ) {
                num += 0x200;
            }

            IntPtr pidl = IntPtr.Zero;
            IntPtr hGlobal = IntPtr.Zero;
            IntPtr pszPath = IntPtr.Zero;
            try {
                BROWSEINFO lpbi = new BROWSEINFO();
                hGlobal = Marshal.AllocHGlobal((int)( MAX_PATH * Marshal.SystemDefaultCharSize ));
                pszPath = Marshal.AllocHGlobal((int)( MAX_PATH * Marshal.SystemDefaultCharSize ));
                _callback = new FolderBrowseCallbackProc(this.CallbackProc);
                lpbi.pidlRoot = pidlRoot;
                lpbi.hwndOwner = hwndOwner;
                lpbi.pszDisplayName = hGlobal;
                lpbi.lpszTitle = _description;
                lpbi.ulFlags = num;
                lpbi.lpfn = _callback;
                lpbi.lParam = IntPtr.Zero;
                lpbi.iImage = 0;
                pidl = SHBrowseForFolder(lpbi);

                if ( pidl != IntPtr.Zero ) {
                    SHGetPathFromIDList(pidl , pszPath);
                    this._selectedPathNeedCheck = true;
                    this._selectedPath = Marshal.PtrToStringAuto(pszPath);
                    success = true;
                }
            } finally {
                IMalloc shMalloc = GetSHMalloc();
                shMalloc.Free(pidlRoot);
                if ( pidl != IntPtr.Zero ) {
                    shMalloc.Free(pidl);
                }
                if ( pszPath != IntPtr.Zero ) {
                    Marshal.FreeHGlobal(pszPath);
                }
                if ( hGlobal != IntPtr.Zero ) {
                    Marshal.FreeHGlobal(hGlobal);
                }
                _callback = null;
            }

            return success;
        }

        private int CallbackProc ( IntPtr hwnd , int msg , IntPtr lParam , IntPtr lpData ) {
            switch ( msg ) {
                case BFFM_INITIALIZED:
                    if ( _selectedPath.Length != 0 ) {
                        SendMessage(new HandleRef(null , hwnd) , BFFM_SETSELECTION , ENABLE , _selectedPath);
                    }
                    break;
                case BFFM_SELCHANGED:
                    IntPtr pidl = lParam;
                    if ( pidl != IntPtr.Zero ) {
                        IntPtr pszPath = Marshal.AllocHGlobal((int)( MAX_PATH * Marshal.SystemDefaultCharSize ));
                        bool canSelectPath = SHGetPathFromIDList(pidl , pszPath);
                        if ( canSelectPath ) {
                            string selectedPath = Marshal.PtrToStringAuto(pszPath);
                            canSelectPath = _folderSelectable(selectedPath);
                        }
                        Marshal.FreeHGlobal(pszPath);
                        SendMessage(new HandleRef(null , hwnd) , BFFM_ENABLEOK , 0 , canSelectPath ? ENABLE : DISABLE);
                    }
                    break;
            }

            return 0;
        }

        private static IMalloc GetSHMalloc ( ) {
            IMalloc[] ppMalloc = new IMalloc[1];
            SHGetMalloc(ppMalloc);
            return ppMalloc[0];
        }

        [ComImport , Guid("00000002-0000-0000-c000-000000000046") , SuppressUnmanagedCodeSecurity , InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMalloc {
            [PreserveSig]
            IntPtr Alloc ( int cb );
            [PreserveSig]
            IntPtr Realloc ( IntPtr pv , int cb );
            [PreserveSig]
            void Free ( IntPtr pv );
            [PreserveSig]
            int GetSize ( IntPtr pv );
            [PreserveSig]
            int DidAlloc ( IntPtr pv );
            [PreserveSig]
            void HeapMinimize ( );
        }

        [StructLayout(LayoutKind.Sequential , CharSet = CharSet.Auto)]
        private class BROWSEINFO {
            public IntPtr hwndOwner;
            public IntPtr pidlRoot;
            public IntPtr pszDisplayName;
            public string lpszTitle;
            public int ulFlags;
            public FolderBrowseCallbackProc lpfn;
            public IntPtr lParam;
            public int iImage;
        }
    }
}
