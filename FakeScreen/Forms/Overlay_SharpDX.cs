using System;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DirectX_Renderer
{
        public partial class Overlay_SharpDX : Form
        {


        private IntPtr handle;
        //DllImports

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetClientRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("dwmapi.dll")]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref int[] pMargins);

        [DllImport("user32.dll")]
        private static extern IntPtr SetActiveWindow(IntPtr handle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public static Rectangle GetScreen(Control referenceControl)
        {
            return Screen.FromControl(referenceControl).Bounds;
        }

        public static Color FromHealth(float percent)
        {
            if (percent < 0 || percent > 1) return Color.Black;

            int red, green;

            red = percent < 0.5 ? 255 : 255 - (int)(255 * (percent - 0.5) / 0.5);
            green = percent < 0.5 ? (int)(255 * percent) : 255;

            return Color.FromArgb(255, red, green, 0);
        }

        public Overlay_SharpDX()
        {
            this.handle = Handle;
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            SetWindowPos(this.Handle, new IntPtr(-1), 0, 0, 0, 0, 0x0001 | 0x0002);
            OnResize(null);

            InitializeComponent();

        }

        private void Overlay_SharpDX_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Visible = true;
            timer1.Start();

            Activate();
        }


        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        public static IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const int WS_EX_NOACTIVATE = 0x08000000;
        private const int WS_EX_TOPMOST = 0x00000008;
        private const int WM_ACTIVATE = 6;
        private const int WA_INACTIVE = 0;
        private const int WM_MOUSEACTIVATE = 0x0021;
        private const int MA_NOACTIVATEANDEAT = 0x0004;




        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams pm = base.CreateParams;
                pm.ExStyle |= 0x80;
                pm.ExStyle |= WS_EX_TOPMOST; // make the form topmost
                pm.ExStyle |= WS_EX_NOACTIVATE; // prevent the form from being activated
                return pm;
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTTRANSPARENT = -1;
            if (m.Msg == WM_NCHITTEST) m.Result = new IntPtr(HTTRANSPARENT);
            else base.WndProc(ref m);
        }


  
        private void Overlay_SharpDX_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }
    }
}

