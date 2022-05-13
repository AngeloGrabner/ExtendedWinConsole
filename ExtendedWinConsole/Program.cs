using System;
using ExtendedWinConsole;
using System.Threading;
using System.Drawing;
class Testing
{
    static void Main()
    {
        //ExtendedConsole.SetWindowSize(30, 10, true);
        ExtendedConsole.SetBufferSize(30, 10);
        ExtendedConsole.WriteLine("something \n neue zeile");
        //CHAR_INFO[] test = new CHAR_INFO[10*30];
        //for (int i = 0; i < test.Length; i++)
        //{
        //    test[i] = new CHAR_INFO();
        //    test[i].Attributes = 2;
        //    test[i].UnicodeChar = 'X';
        //}
        //ExtendedConsole.SetBuffer(test);
        //ExtendedConsole.UpdateBuffer();
        Console.ReadKey();
        //Console.WriteLine("something \n neue zeile");
    }
}