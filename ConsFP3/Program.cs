using System;

namespace ConsFP3
{
    class Program
    {
        static void Main(string[] args)
        {
            MyConsole.SetupConsole();

            MyConsole.CharBuffer[1,1] = '#';

            MyConsole.DisplayBuffer();
            
            Console.ReadLine();
        }
    }
}
