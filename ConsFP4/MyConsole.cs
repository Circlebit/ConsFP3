using System;
using System.IO;
using Microsoft.Win32.SafeHandles;
using static ConsFP4.Native;

namespace ConsFP4
{
    static class MyConsole
    {
        public static short Width { get; set; }
        public static short Height { get; set; }

        private static ConsoleBuffer ConsoleBuffer { get; set; }
        public static byte[,] Buffer
        {
            get { return ConsoleBuffer.CharBuffer; }
            set { ConsoleBuffer.CharBuffer = value; }
        }

        public static void SetupConsole(short width, short height)
        {
            Width = width;
            Height = height;

            Console.SetWindowSize(Width, Height);
            Console.SetBufferSize(Width, Height);

            Console.CursorVisible = false;

            ConsoleBuffer = new ConsoleBuffer(Width, Height);

            

            //if (!ConsoleBuffer.Handle.IsInvalid)
            //{

            //    for (byte character = 65; character < 65 + 26; ++character)
            //    {
            //        for (short attribute = 0; attribute < 15; ++attribute)
            //        {
            //            for (int i = 0; i < ConsoleBuffer.CharInfoBuffer.Length; ++i)
            //            {
            //                ConsoleBuffer.CharInfoBuffer[i].Attributes = attribute;
            //                ConsoleBuffer.CharInfoBuffer[i].Char.AsciiChar = 176;
            //            }

            //            ConsoleBuffer.PrintToConsole();
            //        }
            //    }
            //}
        }

        public static void PrintBuffer()
        {
            ConsoleBuffer.PrintToConsole();
        }
    }
}
