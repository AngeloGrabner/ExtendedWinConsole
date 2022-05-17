using System;
using ExtendedWinConsole;
using System.Threading;
using System.Diagnostics;
class Testing
{
    static void Main()
    {
        ExtendedConsole.SetBufferSize(15,2);
        ExtendedConsole.WriteLine("12345678901234567890sgfjhiahgiasogpiahgofejfjf");
        ExtendedConsole.Write("somthing");
        //SubWindow sw = new(2, 2, 12, 6);
        //sw.WriteLine("somthing");
        //sw.Write("line two\n", 4);
        //sw.Write("0123456789012345");
        //ExtendedConsole.WriteSubWindow(sw);
        //ExtendedConsole.UpdateBuffer();
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