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

            MyConsole.PrintBuffer();

            Console.ReadKey();


            var cb = new byte[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int foo = 1;
                    cb[x, y] = (byte)(176 + foo);
                }
            }

            MyConsole.Buffer = cb;

            MyConsole.PrintBuffer();

            Console.ReadKey();
        }
    }
}