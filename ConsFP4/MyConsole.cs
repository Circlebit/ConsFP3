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

        private static SafeFileHandle Handle;
        private static CharInfo[] CharBuffer;
        private static SmallRect Rectangle;

        public static void SetupConsole(short width, short height)
        {
            Width = width;
            Height = height;

            Console.SetWindowSize(Width, Height);
            Console.SetBufferSize(Width, Height);

            Console.CursorVisible = false;

            Handle = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            if (!Handle.IsInvalid)
            {
                CharBuffer = new CharInfo[Width * Height];
                Rectangle = new SmallRect() { Left = 0, Top = 0, Right = Width, Bottom = Height };

                for (byte character = 65; character < 65 + 26; ++character)
                {
                    for (short attribute = 0; attribute < 15; ++attribute)
                    {
                        for (int i = 0; i < CharBuffer.Length; ++i)
                        {
                            CharBuffer[i].Attributes = attribute;
                            CharBuffer[i].Char.AsciiChar = 176;
                        }

                        WriteCharBufferToConsole();
                    }
                }
            }
        }

        private static void WriteCharBufferToConsole()
        {
            WriteConsoleOutput(Handle, CharBuffer,
              new Coord() { X = Width, Y = Height },
              new Coord() { X = 0, Y = 0 },
              ref Rectangle);
        }
    }
}
