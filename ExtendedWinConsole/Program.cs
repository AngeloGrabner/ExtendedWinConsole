using System;
using ExtendedWinConsole;
using System.Drawing;
class Testing
{
    static void Main()
    {
        Console.CursorVisible = false;
        //ExtendedConsole.SetColor(15, Color.RoyalBlue);
        ExtendedConsole.SetFont(10, 20);
        ExtendedConsole.SetWindowSize(26,5, true);
        ExtendedConsole.SetBufferSize(26,5);
        ExtendedConsole.WriteLine("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
        ExtendedConsole.UpdateBuffer();
        string[] log = ExtendedConsole.GetLogs();
        for (int i = 0; i < log.Length; i++)
        {
            Console.WriteLine(log[i]);
        }
        Console.ReadLine();
    }
}