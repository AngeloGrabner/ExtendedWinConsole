using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Drawing;
using System.Diagnostics;

namespace ExtendedWinConsole // to be added flush _ouputbuffer[] (and display it)
{
    public static class ExtendedConsole
    {
        private static Logger _logger = new();
        private static SMALL_RECT _writtenRegion = new(),_windowPos = new();
        private static SafeFileHandle _outputHandle, _inputHandle, _windowHandle;
        private static COORD _cursor = new(0,0);
        private static CHAR_INFO[] _outputBuffer;
        private static int _width = 0, _height = 0;
        private static ushort _baseColor = 15;
        public static ushort BasColor
        { get { return _baseColor; } set { if (value < 16) { _baseColor = value; } } }


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
        }
        public static void SetMaximumBufferSize(short width, short heith)
        {

            CONSOLE_SCREEN_BUFFER_INFO_EX conscreenbufinex = new CONSOLE_SCREEN_BUFFER_INFO_EX();
            conscreenbufinex.cbSize = (uint)Marshal.SizeOf<CONSOLE_SCREEN_BUFFER_INFO_EX>();
            if (!NativeFunc.GetConsoleScreenBufferInfoEx(_outputHandle, ref conscreenbufinex))
            {

                throw new Exception("error while getting buffer info " + Marshal.GetLastWin32Error());
            }
            if (width <= 0 || heith <= 0)
            {
                throw new ArgumentException();
            }
            conscreenbufinex.dwSize = new COORD(width, heith);
            if (!NativeFunc.SetConsoleScreenBufferInfoEx(_outputHandle, ref conscreenbufinex))
            {

                throw new Exception("error while setting buffer info " + Marshal.GetLastWin32Error());
            }
        }
        public static bool SetCursorVisiblity(bool visible, int? cursorSize = null)
        {
            CONSOLE_CURSOR_INFO CCI = new CONSOLE_CURSOR_INFO();
            if (!NativeFunc.GetConsoleCursorInfo(_outputHandle, out CCI))
            {
                _logger.addError("in SetCursorVisibility: error while getting: win32error: "+Marshal.GetLastWin32Error());
                throw new Exception(_logger.getLatest());
                //return false;
            }
            CCI.bVisible = visible;
            if (cursorSize.HasValue)
            {
                if (cursorSize.Value <= 0 || cursorSize.Value > 100)
                {
                    throw new ArgumentException("cursorSize must be between 1 an 100");
                }
                CCI.dwSize = (uint)cursorSize.Value;
            }
            if (!NativeFunc.SetConsoleCursorInfo(_outputHandle, ref CCI))
            {
                _logger.addError("in SetCursorVisibility: error while setting: win32error: " + Marshal.GetLastWin32Error());
                throw new Exception(_logger.getLatest());
                //return false;
            }
            return true;

        }
        public static bool SetWindowSize(int width, int height, bool valueIsInCharaters = false) // value in character = true isnt working properly
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
            if (valueIsInCharaters)
            {
                if (!NativeFunc.MoveWindow(_windowHandle, _windowPos.Left, _windowPos.Top, width * CFIX.Value.dwFontSize.x + 40, height * CFIX.Value.dwFontSize.y + 60, true))
                {
                    _logger.addError("in ResizeWindow: in MoveWindow: " + Marshal.GetLastWin32Error());
                    return false;
                    //throw new ArgumentException(_logger.getLatest());
                }
            }
            else
            {
                if (!NativeFunc.MoveWindow(_windowHandle, _windowPos.Left, _windowPos.Top, width,  height, true))
                {
                    _logger.addError("in ResizeWindow: in MoveWindow: " + Marshal.GetLastWin32Error());
                    return false;
                    //throw new ArgumentException(_logger.getLatest());
                }
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
            if (sizeX <=  0 || sizeY <= 0)
            {
                _logger.addError("in SetBufferSize: size is to small");
                throw new ArgumentException("size is to small");
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
            NativeFunc.SetConsoleScreenBufferSize(_outputHandle, new COORD ((short)_width, (short)_height));
        }
        public static void SetBufferSize(COORD size)
        {
           SetBufferSize(size.x, size.y);
        }
        public static void SetColor(int index, Color c)
        {
            CONSOLE_SCREEN_BUFFER_INFO_EX conscreenbufinex = new CONSOLE_SCREEN_BUFFER_INFO_EX();
            conscreenbufinex.cbSize = (uint)Marshal.SizeOf<CONSOLE_SCREEN_BUFFER_INFO_EX>();
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
            confoninfex.cbSize = (uint)Marshal.SizeOf<CONSOLE_FONT_INFOEX>();
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
            confoninfex.cbSize = (uint)Marshal.SizeOf<CONSOLE_FONT_INFOEX>();
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
        public static void UpdateBuffer(bool flushBuffer = true)
        {
            if (!NativeFunc.WriteConsoleOutput(_outputHandle, _outputBuffer, new COORD((short)_width, (short)_height), new COORD(0, 0), ref _writtenRegion))
            {
                string error = Marshal.GetLastWin32Error().ToString();
                _logger.addError($"lastWin32 error: {error}");
                _logger.addInfo(_writtenRegion.ToString());
            }
            if (flushBuffer)
            {
                FlushBuffer();
            }
        }
        public static void Clear(bool updateBuffer = true)
        {
            FlushBuffer();
            if (updateBuffer)
            {
                UpdateBuffer(false);
            }
        }
        private static void FlushBuffer()
        {
            for (int i = 0; i < _outputBuffer.Length; i++)
            {
                _outputBuffer[i].UnicodeChar = ' ';
                _outputBuffer[i].Attributes = _baseColor; 
            }
        }
        public static void WriteLine(string text, ushort? color = null)
        {
            COORD tempCursorPos = _cursor;  // if this breaks im done
            int startPos = Convert2dTo1d(tempCursorPos.x, tempCursorPos.y);

            if (color.HasValue)
            {

                for (int i = startPos, j = 0; j < text.Length && i < _outputBuffer.Length; i++, j++)
                {
                    if (text[j] == '\n')
                    {
                        tempCursorPos.y++;
                        tempCursorPos.x = 0;
                        i = Convert2dTo1d(tempCursorPos.x, tempCursorPos.y);
                    }
                    _outputBuffer[i].UnicodeChar = text[j];
                    _outputBuffer[i].Attributes = color.Value;
                }
            }
            else
            {
                for (int i = startPos, j = 0; j < text.Length && i < _outputBuffer.Length; i++, j++)
                {
                    if (text[j] == '\n')
                    {
                        tempCursorPos.y++;
                        tempCursorPos.x = 0;
                        i = Convert2dTo1d(tempCursorPos.x, tempCursorPos.y);
                    }
                    _outputBuffer[i].UnicodeChar = text[j];
                    _outputBuffer[i].Attributes = _baseColor;
                }
            }
            UpdateBuffer(false);
        }
        public static void Write(string text, COORD? startPos = null)
        {

        }
        public static void WriteRect(string text, SMALL_RECT textArea)
        {

        }
        public static string[] GetLogs()
        {
            return _logger.getAll();
        }
        public static int Convert2dTo1d(int x, int y)
        {
            return y * _width + x;
        }
        public static COORD Convert1dTo2d(short pos)
        {
            short y = 0, x = 0, pos1 = pos;
            while (pos1 > _width)
            {
                pos1 -= (short)_width;
                y++;
            }
            x = (short)(pos - (y * _width)); 
            return new COORD(x, y);
        }
        
    }
}