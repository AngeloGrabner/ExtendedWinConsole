using System;
using ExtendedWinConsole;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
class Testing
{
    static void Main()
    {
        ExtendedConsole.SetColor(3, Color.Brown);
        ExtendedConsole.WriteLine("abc", 3);
        Thread.Sleep(1000);
        //ExtendedConsole.SetMaximumBufferSize(10, 100);
        ExtendedConsole.SetColor(3, Color.BlueViolet);
        //ExtendedConsole.WriteLine("abc", 3);

        Thread.Sleep(1000);
        //COLORREF color = new();
        //for (byte r = 0; r < 255; r++)
        //{
        //    for (byte g = 0; g < 255; g++)
        //    {
        //        for (byte b = 0; b < 255; b++)
        //        {
        //            color = new(r, g, b);
        //            ExtendedConsole.SetColor(7, color);
        //            ExtendedConsole.Write("test text", 7);
        //            ExtendedConsole.Clear(false);
        //            Thread.Sleep(500);
        //        }
        //    }
        //}
        Console.ReadLine();
    }

    public static void exTest()
    {
        ExtendedConsole.SetFont(10, 20);
        ExtendedConsole.SetCursorVisiblity(false);
        ExtendedConsole.SetBufferSize(50, 1);
        Thread.Sleep(100);
        Stopwatch ex = new();
        ex.Start();
        for (int i = 0; i < 10000; i++)
        {
            ExtendedConsole.WriteLine(i);
            ExtendedConsole.Clear(false);
        }
        ex.Stop();
        ExtendedConsole.WriteLine($"time elapsed: {ex.Elapsed}");
        Console.ReadKey();
    }

    public static void normal()
    {
        Console.WriteLine("in normal");
        Thread.Sleep(1000);
        Stopwatch ex = new();
        ex.Start();
        for (int i = 0; i < 10000; i++)
        {
            Console.WriteLine(i);
            //Thread.Sleep(10);
            Console.Clear();
        }
        ex.Stop();
        Console.WriteLine($"time elapsed: {ex.Elapsed}");
        Console.ReadKey();
    }
}