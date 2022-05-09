using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles; 

namespace ExtendedWinConsole
{
    public static class ExtendedConsole
    {
        private static Logger _logger = new();
        private static SMALL_RECT _writtenRegion = new();
        private static SafeFileHandle _outputHandle, _inputHandle;
        private static COORD _cursor = new COORD(0,0);
        private static CHAR_INFO[] _outputBuffer;
        private static int _width = 0, _height = 0;
        static ExtendedConsole()
        {
            _outputHandle = NativeFunc.GetStdHandle(HandleType.output);
            _inputHandle = NativeFunc.GetStdHandle(HandleType.input);
#pragma warning disable
            _width =Console.WindowWidth;
            _height=Console.WindowHeight;
#pragma warning enable
              Console.BufferWidth=_width;
              Console.BufferHeight=_height;
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
            COORD startPos = new COORD(0, 0);
            COORD maxsize = new COORD((short)_width, (short)_height);
            if (!NativeFunc.WriteConsoleOutput(_outputHandle, _outputBuffer, maxsize, startPos, ref _writtenRegion))
            {
                string error = Marshal.GetLastWin32Error().ToString();
                _logger.addError(error);
                Console.WriteLine(error);
            }
            Console.WriteLine(_writtenRegion.ToString());
        }
        public static void WriteLine(string text, COORD? startPos = null)
        {
            for (int i = 0; i < text.Length; i++)
            {
                _outputBuffer[i].UnicodeChar = text[i];
                _outputBuffer[i].Attributes = 1;
            }
        }
        public static void Write(string text, COORD? startPos = null)
        {

        }
        public static void WriteRect(string text, SMALL_RECT textArea)
        {

        }

        private static int Convert2dTo1d(int x, int y)
        {
            return y * _width + x; 
        }
    }
}