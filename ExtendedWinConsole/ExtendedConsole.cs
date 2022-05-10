using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles; 

namespace ExtendedWinConsole
{
    public static class ExtendedConsole
    {
        private static Logger _logger = new();
        private static SMALL_RECT _writtenRegion;
        private static SafeFileHandle _outputHandle = new SafeFileHandle(), _inputHandle = new SafeFileHandle();
        private static COORD _cursor = new COORD(0,0);
        private static CHAR_INFO[] _outputBuffer;
        private static int _width = 0, _height = 0;
        static ExtendedConsole()
        {
            _outputHandle = NativeFunc.GetStdHandle(HandleType.output);
            _inputHandle = NativeFunc.GetStdHandle(HandleType.input);
            if (_outputHandle.IsInvalid)
            { 
                _logger.addError("invalid outputhandle");
                throw new Exception("invalid outputhandle");
            }
            if (_inputHandle.IsInvalid)
            {
                _logger.addError("invalid inputhandle");
                throw new Exception("invalid intputhandle");
            }
            SetWindowSize((short)Console.WindowWidth, (short)Console.WindowHeight);
#pragma warning disable CS8602
            for (int i = 0; i < _outputBuffer.Length; i++)
#pragma warning restore CS8602
            {
                _outputBuffer[i] = new CHAR_INFO();
                _outputBuffer[i].Attributes = 15;
                _outputBuffer[i].UnicodeChar = ' ';
            }
        }
        public static void SetWindowSize(short sizeX, short sizeY)
        {
            if (!(sizeX < Console.LargestWindowWidth && sizeX > 0 && sizeY < Console.LargestWindowHeight && sizeY > 0))
            {
                _logger.addError("size to big or to small");
                throw new ArgumentException("size to big or to small");
            }
            _width = sizeX;
            _height = sizeY;
            _writtenRegion = new SMALL_RECT(0, 0, (short)_width, (short)_height);
            _outputBuffer = new CHAR_INFO[_width * _height];
        }
        public static void SetWindowSize(COORD size)
        {
            if (!(size.x < Console.LargestWindowWidth && size.x > 0 && size.y < Console.LargestWindowHeight && size.y > 0))
            {
                _logger.addError("size to big or to small");
                throw new ArgumentException("size to big or to small");
            }
            _width = size.x;
            _height = size.y;
            _writtenRegion = new SMALL_RECT(0, 0, (short)_width, (short)_height);
            _outputBuffer = new CHAR_INFO[_width * _height];
        }
        public static void setColor()
        {

        }
        public static void setFont(int width, int height, string fonstStyle)
        {

        }
        public static void updateBuffer()
        {
            if (!NativeFunc.WriteConsoleOutput(_outputHandle, _outputBuffer, new COORD((short)_width, (short)_height), new COORD(0, 0), ref _writtenRegion))
            {
                string error = Marshal.GetLastWin32Error().ToString();
                _logger.addError($"lastWin32 error: {error}");
                _logger.addInfo(_writtenRegion.ToString());
            }
        }
        public static void WriteLine(string text, COORD? startPos = null)
        {
            for (int i = 0; i < text.Length && i < _outputBuffer.Length-1; i++)
            {
                _outputBuffer[i].UnicodeChar = text[i];
                _outputBuffer[i].Attributes = 15;
            }
            // go to the new line
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