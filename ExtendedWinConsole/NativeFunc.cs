using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace ExtendedWinConsole
{
    public class NativeFunc
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle GetStdHandle(HandleType nStdHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool WriteConsoleOutput(
            SafeFileHandle hConsoleOutput,
            CHAR_INFO[] lpBuffer,
            COORD dwBufferSize,
            COORD dwBufferCoord,
            ref SMALL_RECT lpWriteRegion);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern COORD GetLargestConsoleWindowSize(SafeFileHandle hConsoleOutput);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetCurrentConsoleFontEx(SafeFileHandle hConsoleOutput, bool bMaximumWindow, ref CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetCurrentConsoleFontEx(SafeFileHandle hConsoleOutput, bool bMaximumWindow, ref CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleScreenBufferInfoEx(SafeFileHandle hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX lpConsoleScreenBufferInfo);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleScreenBufferInfoEx(SafeFileHandle hConsoleOuput, ref CONSOLE_SCREEN_BUFFER_INFO_EX lpConsoleScreenBufferInfo);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(SafeFileHandle hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetWindowRect(SafeFileHandle hWnd, ref SMALL_RECT rect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleScreenBufferSize(SafeFileHandle hConsoleOutput, COORD dwSize);


    }
    public enum HandleType
    {
        error = -12,
        output = -11,
        input = -10
    }
}
