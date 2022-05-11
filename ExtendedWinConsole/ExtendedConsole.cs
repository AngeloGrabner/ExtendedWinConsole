using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Drawing;
using System.Diagnostics;

namespace ExtendedWinConsole
{
    public static class ExtendedConsole
    {
        private static Logger _logger = new();
        private static SMALL_RECT _writtenRegion = new(),_windowPos = new();
        private static SafeFileHandle _outputHandle = new SafeFileHandle(), _inputHandle = new SafeFileHandle(), _windowHandle = new SafeFileHandle();
        private static COORD _cursor = new(0,0);
        private static CHAR_INFO[] _outputBuffer;
        private static int _width = 0, _height = 0;
#pragma warning disable 
        static ExtendedConsole()
        {
            //Console.WriteLine(NativeFunc.GetConsoleWindow());
            _windowHandle = NativeFunc.GetConsoleWindow();
            if (_windowHandle.IsInvalid)
            {
                _logger.addError("invalid windowHandle");
                throw new Exception("invalid windowHandle id: " + _windowHandle.DangerousGetHandle());
            }


            _outputHandle = NativeFunc.GetStdHandle(HandleType.output);
            _inputHandle = NativeFunc.GetStdHandle(HandleType.input);
            if (_outputHandle.IsInvalid)
            { 
                _logger.addError("invalid outputHandle");
                throw new Exception("invalid outputHandle");
            }
            if (_inputHandle.IsInvalid)
            {
                _logger.addError("invalid inputHandle");
                throw new Exception("invalid intputHandle");
            }
            SetBufferSize((short)Console.WindowWidth, (short)Console.WindowHeight);

            if (!NativeFunc.GetWindowRect(_windowHandle, ref _windowPos))
            {
                _logger.addError("in GetWindowRect: win32error: " + Marshal.GetLastWin32Error());
                throw new Exception("error while getting rect of window");
            }
            //ResizeWindow(20, 20);
        }
        public static bool ResizeWindow(int width, int height)
        {
            CONSOLE_FONT_INFOEX? CFIX = new CONSOLE_FONT_INFOEX?();
            CFIX = GetFont();
            if (CFIX == null)
            { 
                //return false;
                throw new Exception("getting error");
            }
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("value must be greater 0");
            }
            if (!NativeFunc.MoveWindow(_windowHandle, _windowPos.Left, _windowPos.Top, width * CFIX.Value.dwFontSize.x,height* CFIX.Value.dwFontSize.y, true))
            {
                _logger.addError("in ResizeWindow: in MoveWindow: " + Marshal.GetLastWin32Error());
                //return false;
                throw new ArgumentException(_logger.getLatest());
            }

            return true;
        }
        public static void MoveWindowPos(int ofsetX, int ofsetY) // win32 error: 1400 (ERROR_INVALID_WINDOW_HANDLE)
        {
            try
            {
                if (!NativeFunc.MoveWindow(_windowHandle, _windowPos.Left + ofsetX, _windowPos.Top + ofsetY, _windowPos.Right, _windowPos.Bottom, true))
                {
                    _logger.addError("in MoveWindowPos: in MoveWindow: " + Marshal.GetLastWin32Error());
                    throw new ArgumentException(_logger.getLatest());
                }
            }
            catch (SEHException e)
            {
                _logger.addError("in moveWindowPos: win32 error: "+ Marshal.GetLastWin32Error().ToString()+" throw: "+e);
                throw new Exception("something with win32 and window position");
            }
        }
        public static void SetBufferSize(short sizeX, short sizeY) 
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
#pragma warning restore
            for (int i = 0; i < _outputBuffer.Length; i++)
            {
                _outputBuffer[i] = new CHAR_INFO();
                _outputBuffer[i].Attributes = 15;
                _outputBuffer[i].UnicodeChar = ' ';
            }
#pragma warning disable
            Console.BufferHeight = Console.WindowHeight = _height;
            Console.BufferWidth = Console.WindowWidth = _width;
#pragma warning restore
           
        }
        public static void SetBufferSize(COORD size)
        {
           SetBufferSize(size.x, size.y);
        }
        public static void SetColor(int index, Color c)
        {
            CONSOLE_SCREEN_BUFFER_INFO_EX conscreenbufinex = new CONSOLE_SCREEN_BUFFER_INFO_EX();
            if (!NativeFunc.GetConsoleScreenBufferInfoEx(_outputHandle, ref conscreenbufinex))
            {

                throw new Exception("error while getting buffer info "+Marshal.GetLastWin32Error());
            }
            if (!(index >= 0 && index < 16))
                throw new ArgumentException("index must be between 0 and 15");
            conscreenbufinex.ColorTable[index] = new COLORREF(c);

            if (!NativeFunc.SetConsoleScreenBufferInfoEx(_outputHandle,ref conscreenbufinex))
            {

                throw new Exception("error while setting buffer info "+Marshal.GetLastWin32Error());
            }
        }
        public static CONSOLE_FONT_INFOEX? GetFont()
        {
            CONSOLE_FONT_INFOEX confoninfex = new CONSOLE_FONT_INFOEX();
            if (!NativeFunc.GetCurrentConsoleFontEx(_outputHandle, false, ref confoninfex))
            {
                _logger.addInfo("getFont failed: win32error"+Marshal.GetLastWin32Error().ToString());
                return null;
            }
            return confoninfex;
        }
        public static void SetFont(int width, int height, string fontStyle = " ")
        {
            CONSOLE_FONT_INFOEX confoninfex = new CONSOLE_FONT_INFOEX();
            if (!NativeFunc.GetCurrentConsoleFontEx(_outputHandle, false, ref confoninfex))
            {

                throw new Exception("error while getting font info " + Marshal.GetLastWin32Error());
            }
            if (!(width > 1 && height > 1))
            {
                throw new ArgumentException("must be greater 1");
            }
            confoninfex.dwFontSize = new COORD((short)width, (short)height);
            if (fontStyle != " ")
                confoninfex.FaceName = fontStyle;
            if (!NativeFunc.SetCurrentConsoleFontEx(_outputHandle, false, ref confoninfex))
            {
                throw new Exception("error while setting font info " + Marshal.GetLastWin32Error());
            }

        }
        public static void UpdateBuffer()
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