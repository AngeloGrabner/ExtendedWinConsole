using System;
using ExtendedWinConsole;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
class Testing
{
    static void Main()
    {
        ExConsole.SetMaximumBufferSize(100, 20);
        ExConsole.SetBufferSize(100, 20);
        ExConsole.SetFont(12, 24);
        ExConsole.SetCursorVisiblity(false);
        ExConsole.WriteLine("ABCDEFGHIJKLMNOPQRSTUVW", 7);
        ExConsole.Write("DEFGHIJKLMNOPQRSTUVW", 8);
        Thread.Sleep(1000);
        //for (int i = 0; i < 30; i++)
        //{
        //    ExConsole.Remove();
        //    Thread.Sleep(200);
        //}
        //Thread.Sleep(500);
        //ExConsole.Write("123");
        string a = ExConsole.ReadLine();
        //a = a.Substring(0, a.Length - 1);
        //if (File.Exists(a))
        //    ExConsole.WriteLine("worked");
        //else
        //    ExConsole.WriteLine("didnt "+a);
        ExConsole.WriteLine("...wow: " + a);

        Console.ReadLine();
    }
  
}