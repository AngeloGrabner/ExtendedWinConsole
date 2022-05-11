using ExtendedWinConsole;
using System.Drawing;
class Testing
{
    static void Main()
    {
        Console.CursorVisible = false;
        //ExtendedConsole.SetBufferSize(200, 200);
        //ExtendedConsole.SetColor(15, Color.RoyalBlue);
        //ExtendedConsole.SetFont(2, 2);
        ExtendedConsole.WriteLine("abcdeqbgiujqoligujhboilskugjhbaolsrjkghbfajglakjbvoruzqouijsaobvwgbkjebgkjqbgksjgbkegbkjvajsljgvjsn aj jh");
        ExtendedConsole.UpdateBuffer();
        Console.ReadLine();
    }
}