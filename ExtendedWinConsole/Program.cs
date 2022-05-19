using System;
using ExtendedWinConsole;
using System.Threading;
using System.Diagnostics;
class Testing
{
    static void Main()
    {
        //ExtendedConsole.SetBufferSize(30,20);
        //ExtendedConsole.SetWindowSize(30,20, true);
        ExtendedConsole.SetFont(16, 32);
        ExtendedConsole.Write("somthing\n long as text");
        SubWindow sw = new(50, 5, 14, 7);
        sw.WriteLine("somthing");
        sw.Write("line two\n", 4);
        sw.Write("01234567890123456789012345");
        for (int i = 0; i < 10; i++)
        {
            ExtendedConsole.WriteSubWindow(sw);
            ExtendedConsole.UpdateBuffer();
            sw.Move(1,1);
            Thread.Sleep(500);
        }

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