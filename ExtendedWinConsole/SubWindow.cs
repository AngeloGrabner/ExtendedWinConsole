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
        private ushort _borderColor = 15;
        private SMALL_RECT _rect;
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
            FillBuffer();
        }
        public SubWindow(SMALL_RECT rect)
        {
            _rect = rect;
            _buffer = new CHAR_INFO[(rect.Left - rect.Right) * (rect.Top-rect.Bottom)];
            FillBuffer();
        }
        public void Resize(short newX, short newY,short newWidth, short newHeight)
        {
            _rect = new SMALL_RECT(newX,newY,newWidth,newHeight);
            _buffer = new CHAR_INFO[(_rect.Left - _rect.Right) * (_rect.Top - _rect.Bottom)];
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
        public void SetBorderColor(ushort color)
        {
            if (color > 15)
            {
                color = 15;
            }
            _borderColor = color;
        }
    }
}
