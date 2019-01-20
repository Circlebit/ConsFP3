using System;
using System.IO;

namespace ConsFP4
{

    class Program
    {

        static void Main(string[] args)
        {
            short w = 90;
            short h = 30;

            MyConsole.SetupConsole(w, h);

            MyConsole.PrintBufferToConsole();

            //var cb = new byte[w, h];
            //for (int y = 0; y < h; y++)
            //{
            //    for (int x = 0; x < w; x++)
            //    {
            //        int foo = 0;
            //        if (y % 2 == 0) foo = 1;
            //        if (y % 3 == 0) foo = 2;
            //        if (y % 4 == 0) foo = 43;
            //        if (y % 5 == 0) foo = -176;
            //        cb[x, y] = (byte)(176 + foo);
            //    }
            //}

            //MyConsole.ConsoleBuffer.CharBuffer = cb;

            Console.ReadKey();
        }
    }
}