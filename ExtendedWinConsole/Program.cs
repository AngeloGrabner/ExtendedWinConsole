using System;
using ExtendedWinConsole;
using System.Threading;
using System.Diagnostics;
class Testing
{
    static void Main()
    {
        ExtendedConsole.SetFont(8,16);
        Thread.Sleep(1000);
        ExtendedConsole.SetMaximumBufferSize(20, 20);
        ExtendedConsole.SetWindowSize(22, 22, true);
        Thread.Sleep(1000);
        ExtendedConsole.SetBufferSize(20, 20);
        ExtendedConsole.UpdateBuffer(false);
        Console.ReadLine();
    }

    //public static void exTest()
    //{
    //    ExtendedConsole.SetFont(10, 20);
    //    ExtendedConsole.SetCursorVisiblity(false);
    //    ExtendedConsole.SetBufferSize(10, 50);
    //    Thread.Sleep(100);
    //    Stopwatch ex = new();
    //    ex.Start();
    //    for (int i = 0; i < 10000; i++)
    //    {
    //        ExtendedConsole.WriteLine(i);
    //        //ExtendedConsole.Clear(false);
    //    }
    //    ex.Stop();
    //    ExtendedConsole.WriteLine($"time elapsed: {ex.Elapsed}");
    //    Console.ReadKey();
    //}

    //public static void normal()
    //{
    //    Console.WriteLine("in normal");
    //    Thread.Sleep(1000);
    //    Stopwatch ex = new();
    //    ex.Start();
    //    for (int i = 0; i < 10000; i++)
    //    {
    //        Console.WriteLine(i);
    //        //Thread.Sleep(10);
    //        //Console.Clear();
    //    }
    //    ex.Stop();
    //    Console.WriteLine($"time elapsed: {ex.Elapsed}");
    //    Console.ReadKey();
    //}
}