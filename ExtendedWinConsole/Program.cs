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
        ExtendedConsole.SetMaximumBufferSize(100, 20);
        ExtendedConsole.SetBufferSize(100, 20);
        ExtendedConsole.SetFont(12, 24);
        ExtendedConsole.SetCursorVisiblity(false);
        ExtendedConsole.WriteLine("ABCDEFGHIJKLMNOPQRSTUVW", 7);
        ExtendedConsole.Write("DEFGHIJKLMNOPQRSTUVW", 8);
        //Thread.Sleep(1000);
        string a = ExtendedConsole.ReadLine();
        a = a.Substring(0, a.Length - 1);
        //if (File.Exists(a))
        //    ExtendedConsole.WriteLine("worked");
        //else
        //    ExtendedConsole.WriteLine("didnt "+a);
        ExtendedConsole.WriteLine("wow: "+a);

        Console.ReadLine();
    }
  
}