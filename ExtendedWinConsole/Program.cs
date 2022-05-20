﻿using System;
using ExtendedWinConsole;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
class Testing
{
    static void Main()
    {   
        ExtendedConsole.SetMaximumBufferSize(30, 10);
        ExtendedConsole.SetBufferSize(30, 10);
        ExtendedConsole.SetFont(16, 32);
        ExtendedConsole.SetCursorVisiblity(false);
        //ExtendedConsole.SetColor(3, Color.Brown);
        //ExtendedConsole.WriteLine("abc", 3);
        //Thread.Sleep(1000);
        //ExtendedConsole.Clear(false);
        //ExtendedConsole.SetColor(3, Color.BlueViolet);
        //ExtendedConsole.WriteLine("abc", 3);

        //Thread.Sleep(1000);
        Stopwatch sw = Stopwatch.StartNew();
        COLORREF color = new();
        for (byte r = 0; r < 255; r+=5)
        {
            for (byte g = 0; g < 255; g+=5)
            {
                for (byte b = 0; b < 255; b+=5)
                {
                    color = new(r, g, b);
                    ExtendedConsole.SetColor(7, color);
                    ExtendedConsole.Write("ABC");
                    ExtendedConsole.Write("DEFGHIJKLMNOPQRSTUVW", 7);
                    ExtendedConsole.Write("XYZ", 6);
                    ExtendedConsole.Clear(false);
                    //Thread.Sleep(10);
                }
            }
        }
        sw.Stop();
        Console.WriteLine(sw.Elapsed);
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