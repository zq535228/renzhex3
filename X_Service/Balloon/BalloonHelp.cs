using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace X_Service.Balloon {
    public partial class BalloonHelp : BalloonWindow {
        #region Constants
        const int CLOSEBUTTON_WIDTH = 14;
        const int CLOSEBUTTON_HEIGHT = 14;
        const uint SHOWCLOSEBUTTON = 0x1;
        const uint CLOSEONMOUSECLICK = 0x2;
        const uint CLOSEONKEYPRESS = 0x4;
        const uint CLOSEONMOUSEMOVE = 0x10;
        const uint CLOSEONDEACTIVATE = 0x20;
        const uint ENABLETIMEOUT = 0x40;
        const int WM_ACTIVATEAPP = 0x001C;
        #endregion

        #region Fields
        private uint flags = CLOSEONMOUSECLICK | CLOSEONKEYPRESS | CLOSEONDEACTIVATE;
        private string content = String.Empty;
        private Font captionFont;
        private Size headerSize;
        #endregion

        public BalloonHelp ( ) {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
        }

        #region Utility functions
        private void SetBoolProp ( uint prop , bool b ) {
            if ( b )
                flags |= prop;
            else
                flags &= ~prop;
        }

        private bool GetBoolProp ( uint prop ) {
            return ( flags & prop ) != 0;
        }
        #endregion

        #region Public Properties

        [Category("Balloon")]
        public int Timeout {
            get {
                return timer1.Interval;
            }
            set {
                timer1.Interval = value;
            }
        }


        [Category("Balloon")]
        public bool EnableTimeout {
            get {
                return GetBoolProp(ENABLETIMEOUT);
            }
            set {
                SetBoolProp(ENABLETIMEOUT , value);
            }
        }

        [Category("Balloon")]
        public bool CloseOnMouseClick {
            get {
                return GetBoolProp(CLOSEONMOUSECLICK);
            }
            set {
                SetBoolProp(CLOSEONMOUSECLICK , value);
            }
        }

        [Category("Balloon")]
        public bool CloseOnKeyPress {
            get {
                return GetBoolProp(CLOSEONKEYPRESS);
            }
            set {
                SetBoolProp(CLOSEONKEYPRESS , value);
            }
        }

        [Category("Balloon")]
        public bool CloseOnMouseMove {
            get {
                return GetBoolProp(CLOSEONMOUSEMOVE);
            }
            set {
                SetBoolProp(CLOSEONMOUSEMOVE , value);
            }
        }

        [Category("Balloon")]
        public bool CloseOnDeactivate {
            get {
                return GetBoolProp(CLOSEONDEACTIVATE);
            }
            set {
                SetBoolProp(CLOSEONDEACTIVATE , value);
            }
        }

        [Category("Balloon")]
        public string Content {
            get {
                return content;
            }
            set {
                content = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new string Text {
            get {
                return base.Text;
            }
            set {
                base.Text = value;
            }
        }

        [Category("Balloon")]
        public string Caption {
            get {
                string ret = this.Text;
                return ( ret == null ) ? String.Empty : ret;
            }
            set {
                this.Text = value;
            }
        }

        [Category("Balloon")]
        public Font CaptionFont {
            get {
                if ( captionFont == null )
                    captionFont = new Font(this.Font , FontStyle.Bold);

                return captionFont;
            }
            set {
                captionFont = value;
            }
        }
        #endregion


        #region Calculate size
        protected Size CalcClientSize ( ) {
            System.Drawing.Size size = Size.Empty;

            using ( System.Drawing.Graphics g = this.CreateGraphics() ) {
                if ( this.Icon != null ) {
                    size = this.Icon.Size;
                }


                if ( this.Caption.Length != 0 ) {
                    if ( size.Width != 0 )
                        size.Width += TIPMARGIN;

                    System.Drawing.Size captionSize = Size.Ceiling(g.MeasureString(Caption , this.CaptionFont));
                    size.Width += captionSize.Width;
                    size.Height = Math.Max(captionSize.Height , size.Height);
                }

                headerSize = size;

                string text = this.Content;

                if ( ( text != null ) && ( text.Length != 0 ) ) {
                    size.Height += TIPMARGIN;

                    System.Drawing.Size textSize = Size.Ceiling(g.MeasureString(text , this.Font));
                    size.Height += textSize.Height;
                    size.Width = Math.Max(textSize.Width , headerSize.Width);
                    headerSize.Width = size.Width;
                }
            }

            return size;
        }
        #endregion

        #region Client Area Drawing

        private void Draw ( Graphics g ) {
            Rectangle drawingRect = this.ClientRectangle;

            string content = this.Content;

            if ( content.Length != 0 ) {
                RectangleF textRect = new RectangleF(0 , headerSize.Height + TIPMARGIN , drawingRect.Width , drawingRect.Height - headerSize.Height - TIPMARGIN);
                Brush b = new SolidBrush(this.ForeColor);
                g.DrawString(content , this.Font , b , textRect);
                b.Dispose();
            }

            // calc & draw icon
            if ( this.Icon != null ) {
                g.DrawIcon(this.Icon , 0 , 0);
                drawingRect.X += ( this.Icon.Width );
                drawingRect.Width -= ( this.Icon.Width );
            }

            string caption = this.Caption;

            if ( caption.Length != 0 ) {
                drawingRect.X += TIPMARGIN;
                drawingRect.Width -= TIPMARGIN;

                //StringFormat sf = new StringFormat();
                //sf.Alignment = StringAlignment.Center;

                Brush b = new SolidBrush(this.ForeColor);

                g.DrawString(caption , this.CaptionFont , b , (RectangleF)drawingRect);//, sf); 

                b.Dispose();
                //sf.Dispose();
            }
        }

        protected override void OnPaint ( System.Windows.Forms.PaintEventArgs e ) {
            Draw(e.Graphics);
        }

        #endregion


        protected override void OnLoad ( System.EventArgs e ) {
            this.ClientSize = CalcClientSize();

            if ( ( this.flags != 0 ) && ( this.flags != SHOWCLOSEBUTTON ) ) {
                if ( this.CloseOnKeyPress )
                    this.windowsHook.KeyBoardHook += new X_Service.Balloon.Hooks.KeyBoardHookEventHandler(this.windowsHook_KeyBoardHook);

                if ( ( this.CloseOnMouseClick ) || ( this.CloseOnMouseMove ) )
                    this.windowsHook.MouseHook += new X_Service.Balloon.Hooks.MouseHookEventHandler(this.windowsHook_MouseHook);

                windowsHook.HookCurrentThread();
            }

            if ( this.EnableTimeout )
                timer1.Start();

            base.OnLoad(e);
        }

        private void windowsHook_KeyBoardHook ( object sender , X_Service.Balloon.Hooks.KeyBoardHookEventArgs e ) {
            this.Close();
        }

        private void windowsHook_MouseHook ( object sender , X_Service.Balloon.Hooks.MouseHookEventArgs e ) {
            switch ( e.Message ) {
                case Hooks.MouseMessages.LButtonDblClk:
                case Hooks.MouseMessages.LButtonDown:
                case Hooks.MouseMessages.LButtonUp:
                case Hooks.MouseMessages.MButtonDblClk:
                case Hooks.MouseMessages.MButtonDown:
                case Hooks.MouseMessages.MButtonUp:
                case Hooks.MouseMessages.RButtonDblClk:
                case Hooks.MouseMessages.RButtonDown:
                case Hooks.MouseMessages.RButtonUp:
                case Hooks.MouseMessages.XButtonDblClk:
                case Hooks.MouseMessages.XButtonDown:
                    if ( this.CloseOnMouseClick )
                        Close();
                    break;
                case Hooks.MouseMessages.MouseMove:
                    if ( this.CloseOnMouseMove )
                        Close();
                    break;
            }
        }

        protected virtual void OnDeactivateApp ( System.EventArgs e ) {
            if ( this.CloseOnDeactivate )
                Close();
        }

        protected override void WndProc ( ref Message m ) {
            if ( ( m.Msg == WM_ACTIVATEAPP ) && ( m.WParam == IntPtr.Zero ) ) {
                OnDeactivateApp(EventArgs.Empty);
            } else
                base.WndProc(ref m);
        }

        protected override void OnClosed ( System.EventArgs e ) {
            windowsHook.Dispose();
            base.OnClosed(e);
        }

        private void closeButton_Click ( object sender , System.EventArgs e ) {
            Close();
        }

        private void timer1_Tick ( object sender , System.EventArgs e ) {
            Close();
        }
    }
}
