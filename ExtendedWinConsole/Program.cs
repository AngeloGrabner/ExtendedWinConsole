using ExtendedWinConsole;
using System.Drawing;
class Testing
{
    static void Main()
    {
        for( int i = 0; i < 10; i++)
        {
            ExtendedConsole.MoveWindowPos(i*10,i*10);
        }
        //ExtendedConsole.SetBufferSize(200, 200);
        //ExtendedConsole.setColor(15, Color.RoyalBlue);
        //ExtendedConsole.setFont(2,2);
        //ExtendedConsole.WriteLine("abcdeqbgiujqoligujhboilskugjhbaolsrjkghbfajglakjbvoruzqouijsaobvwgbkjebgkjqbgksjgbkegbkjvajsljgvjsn aj jh");
        //ExtendedConsole.updateBuffer();
        Console.ReadLine();
    }
}