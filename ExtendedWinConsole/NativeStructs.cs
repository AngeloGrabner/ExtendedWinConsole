using System.Runtime.InteropServices;
using System.Drawing;

namespace ExtendedWinConsole
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SMALL_RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
        public SMALL_RECT(short left, short top, short right, short bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        public override string ToString()
        {
            return $"Ledt: {Left}, Top: {Top}, Right {Right}, Bottom: {Bottom}";
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        public short x;
        public short y;

        public COORD(short x, short y)
        {
            this.x = x;
            this.y = y;
        }
    }
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public struct CHAR_INFO
    {
        [FieldOffset(0)]
        public char UnicodeChar;
        [FieldOffset(0)]
        public char AsciiChar;
        [FieldOffset(2)] //2 bytes seems to work properly
        public UInt16 Attributes;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct CONSOLE_FONT_INFOEX
    {
        public CONSOLE_FONT_INFOEX()
        {
            cbSize = (uint)Marshal.SizeOf<CONSOLE_FONT_INFOEX>();
        }
        public uint cbSize = 0;
        public uint nFont = 0;
        public COORD dwFontSize = new COORD(0,0);
        public int FontFamily = 0;
        public int FontWeight = 0;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string FaceName = String.Empty;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_SCREEN_BUFFER_INFO_EX
    {
        public CONSOLE_SCREEN_BUFFER_INFO_EX()
        {
            cbSize= (uint)Marshal.SizeOf<CONSOLE_SCREEN_BUFFER_INFO_EX>();
            dwSize = new COORD(0,0);
            dwCursorPosition = new COORD(0,0);
            wAttributes = 0;
            srWindow = new SMALL_RECT(0,0,0,0);
            dwMaximumWindowSize = new COORD(0,0);

            wPopupAttributes = 0;
            bFullscreenSupported = false;
        }
        public uint cbSize;
        public COORD dwSize;
        public COORD dwCursorPosition;
        public short wAttributes;
        public SMALL_RECT srWindow;
        public COORD dwMaximumWindowSize;

        public ushort wPopupAttributes;
        public bool bFullscreenSupported;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public COLORREF[] ColorTable = new COLORREF[16];
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct COLORREF
    {
        internal uint ColorDWORD;

        internal COLORREF(ConsoleColor color)
        {
            ColorDWORD = (uint)color;
        }
        internal COLORREF(Color color)
        {
            ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        }

        internal COLORREF(uint r, uint g, uint b)
        {
            ColorDWORD = r + (g << 8) + (b << 16);
        }

        internal Color GetColor()
        {
            return Color.FromArgb((int)(0x000000FFU & ColorDWORD),
               (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
        }

        internal void SetColor(Color color)
        {
            ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        }
    }

}
