using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles; 

namespace ExtendedWinConsole
{
    public static class ExtendedConsole
    {
        private static SafeFileHandle _outputHandle, _inputHandle;
        private static COORD _cursor = new COORD(0,0);
        private static CHAR_INFO[] _outputBuffer;
        private static int _width, _height;
        static ExtendedConsole()
        {
            _outputHandle = NativeFunc.GetStdHandle(HandleType.output);
            _inputHandle = NativeFunc.GetStdHandle(HandleType.input);
            _width = Console.BufferWidth;
            _height = Console.BufferHeight;
            Console.WindowWidth = _width;
            Console.WindowHeight = _height;
            _outputBuffer = new CHAR_INFO[_width*_height];
        }
        public static void setColor()
        {

        }
        public static void setFont(int width, int height, string fonstStyle)
        {

        }
        public static void updateBuffer()
        {
            NativeFunc.WriteConsoleOutput(_outputHandle,)
        }
        public static void WriteLine(string text, COORD? startPos = null)
        {

        }
        public static void Write(string text, COORD? startPos = null)
        {

        }
        public static void WriteRect(string text, SMALL_RECT textArea)
        {

        }

        private static void convert2d
    }
}