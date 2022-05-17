using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWinConsole
{
    public class SubWindow // no optimaziations in this class, it was not made for super high performence anyways
    {
        private char[] _borderTiles = new char[6] { '╔', '╗', '╚', '╝', '║', '═' };
        private ushort _baseColor = 15;
        private COORD _cursor = new COORD(0,0);
        private SMALL_RECT _rect;
        private Utility _utility;
        public SMALL_RECT rect
        {
            get { return _rect; }
            private set { _rect = value; }
        }
        private CHAR_INFO[] _buffer;
        public CHAR_INFO[] buffer
        {
            get { return _buffer; }
            private set { _buffer = value; }
        }
        public SubWindow(short x, short y, short width, short height)
        {
            _rect = new SMALL_RECT(x, y, width, height);
            _buffer = new CHAR_INFO[(x - width) * (y - height)];
            _utility = new(width);
            FillBuffer();
        }
        public SubWindow(SMALL_RECT rect)
        {
            _rect = rect;
            _buffer = new CHAR_INFO[(_rect.Left - _rect.Right) * (_rect.Top - _rect.Bottom)];
            _utility = new(rect.Right);
            FillBuffer();
        }
        public void WriteLine(string text)
        {
            Write(text);
            _cursor.x = 0;
            _cursor.y++;
        }
        public void WriteLine(object obj)
        {
            Write(obj);
            _cursor.x = 0;
            _cursor.y++;
        }
        public void WriteLine(string text, ushort color)
        {
            Write(text, color);
            _cursor.x = 0;
            _cursor.y++;
        }
        public void WriteLine(object obj, ushort color)
        {
            Write(obj, color);
            _cursor.x = 0;
            _cursor.y++;
        }
        public void Write(object obj, ushort color)
        {
            Write(obj.ToString(), color);
        }
        public void Write(string text, ushort color)
        {
            if (color > 15)
            {
                color = 15;
            }
            COORD tempCursorPos = _cursor;
            int i = _utility.Convert2dTo1d(tempCursorPos.x, tempCursorPos.y);
            for (int j = 0; j < text.Length && i < _buffer.Length; i++, j++)
            {
                if (text[j] == '\n')
                {
                    tempCursorPos.y++;
                    tempCursorPos.x = 0;
                    i = _utility.Convert2dTo1d(tempCursorPos.x, tempCursorPos.y);
                    j++;
                }
                tempCursorPos.x++;
                _buffer[i].UnicodeChar = text[j];
                _buffer[i].Attributes = color;
            }
            _cursor = tempCursorPos;
        }
        public void Write(object obj)
        {
            Write(obj.ToString());
        }
        public void Write(string text)
        {
            COORD tempCursorPos = _cursor;
            int i = _utility.Convert2dTo1d(tempCursorPos.x, tempCursorPos.y);
            for (int j = 0; j < text.Length && i < _buffer.Length; i++, j++)
            {
                if (text[j] == '\n')
                {
                    tempCursorPos.y++;
                    tempCursorPos.x = 0;
                    i = _utility.Convert2dTo1d(tempCursorPos.x, tempCursorPos.y);
                    j++;
                }
                tempCursorPos.x++;
                _buffer[i].UnicodeChar = text[j];
                _buffer[i].Attributes = _baseColor;
            }
            _cursor = tempCursorPos;
        }
        public void DrawBorder()
        {
            DrawBorder(15);
        }
        public void DrawBorder(ushort color)
        {
            if (color > 15)
            {
                color = 15;
            }
            for (int i = 1; i < _rect.Right-1; i++) // top line
            {
                _buffer[i].UnicodeChar = _borderTiles[(int)BorderType.HoriziontalLine];
                _buffer[i].Attributes = color;
            }
            for (int i = _utility.Convert2dTo1d(1, _rect.Bottom); i < _utility.Convert2dTo1d(_rect.Right, _rect.Bottom)-1; i++) // bottom line
            {
                _buffer[i].UnicodeChar = _borderTiles[(int)BorderType.HoriziontalLine];
                _buffer[i].Attributes = color;
            }
            for (int i = _utility.Convert2dTo1d(0, 1); i < _utility.Convert2dTo1d(0, _rect.Bottom-1); i+= _utility.Convert2dTo1d(0,1)) // left line
            {
                _buffer[i].UnicodeChar = _borderTiles[(int)BorderType.VerticalLine];
                _buffer[i].Attributes = color;
            }
            for (int i = _utility.Convert2dTo1d(_rect.Right, 1); i < _utility.Convert2dTo1d(_rect.Right-1, _rect.Bottom); i+= _utility.Convert2dTo1d(0, 1)) // right line
            {
                _buffer[i].UnicodeChar = _borderTiles[(int)BorderType.VerticalLine];
                _buffer[i].Attributes = color;
            }
            // corners
            _buffer[0].UnicodeChar = _borderTiles[(int)BorderType.TopLeft];
            _buffer[0].Attributes = color;

            _buffer[_rect.Right].UnicodeChar = _borderTiles[(int)BorderType.TopRight];
            _buffer[_rect.Right].Attributes = color;

            _buffer[_utility.Convert2dTo1d(0, _rect.Bottom)].UnicodeChar = _borderTiles[(int)BorderType.BottomRight];
            _buffer[_utility.Convert2dTo1d(0, _rect.Bottom)].Attributes = color;

            _buffer[(_rect.Bottom * _rect.Right) - 1].UnicodeChar = _borderTiles[(int)BorderType.BottomRight];
            _buffer[(_rect.Bottom * _rect.Right) - 1].Attributes = color;
        }
        public void Clear()
        {
            FillBuffer();
        }
        public void Resize(short newX, short newY,short newWidth, short newHeight)
        {
            _rect = new SMALL_RECT(newX,newY,newWidth,newHeight);
            _buffer = new CHAR_INFO[(_rect.Left - _rect.Right) * (_rect.Top - _rect.Bottom)];
            _utility = new(newWidth);
            FillBuffer();
        }
        public void Resize(short newWidth, short newHeight)
        {
            Resize(_rect.Left, _rect.Top, newWidth, newHeight);
        }
        public void Move(short Xoffset, short Yoffset)
        {
            _rect.Right += Xoffset;
            _rect.Left += Xoffset;

            _rect.Bottom += Yoffset;
            _rect.Top += Yoffset;
        }
        private void FillBuffer(char defaultChar = ' ', ushort defaultColor = 15) 
        {
            if (defaultColor > 15)
            {
                throw new ArgumentException("unknown color number");
            }
            for (int i = 0; i < _buffer.Length; i++)
            {
                _buffer[i].UnicodeChar = defaultChar;
                _buffer[i].Attributes = defaultColor;
            }
        }
        public void SetBorder(BorderType BT, char c)
        {
            _borderTiles[(int)BT] = c;
        }
    }
}
