using System;
using ExtendedWinConsole;
using System.Threading;
using System.Drawing;
class Testing
{
    static void Main()
    {
        ExtendedConsole.WriteLine("somethingjegjoeighoeigheoig");
        Thread.Sleep(1000);
        if (ExtendedConsole.SetCursorVisiblity(false))
        {
            ExtendedConsole.WriteLine("worked");
        }
        ExtendedConsole.WriteLine("something else");
        //ExtendedConsole.SetColor(15, Color.RoyalBlue);
        //ExtendedConsole.SetFont(10, 20);
        //ExtendedConsole.SetWindowSize(26,5, true);
        //ExtendedConsole.SetBufferSize(26,5);
        //ExtendedConsole.WriteLine("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
        ExtendedConsole.UpdateBuffer();
        string[] log = ExtendedConsole.GetLogs();
        for (int i = 0; i < log.Length; i++)
        {
            Console.WriteLine(log[i]);
        }
        Console.ReadLine();
    }
}