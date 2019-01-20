using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static ConsFP4.Native;

namespace ConsFP4
{
    class ConsoleBuffer
    {
        public short Width { get; private set; }
        public short Height { get; private set; }

        /// <summary>
        /// 2d map of ASCII Codes to display
        /// </summary>
        public byte[,] CharBuffer { get; set; }

        private SafeFileHandle Handle;
        private CharInfo[] CharInfoBuffer;
        private SmallRect Rectangle;

        public ConsoleBuffer(short width, short height)
        {
            Width = width;
            Height = height;
            Handle = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            Rectangle = new SmallRect() { Left = 0, Top = 0, Right = Width, Bottom = Height };
            CharInfoBuffer = new CharInfo[Width * Height];
            CharBuffer = new byte[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int foo = 0;
                    if (y % 2 == 0) foo = 1;
                    if (y % 3 == 0) foo = 2;
                    if (y % 4 == 0) foo = 43;
                    if (y % 5 == 0) foo = -176;
                    CharBuffer[x, y] = (byte)(176 + foo);
                }
            }

        }

        /// <summary>
        /// Prints the contents of CharBuffer to console
        /// </summary>
        public void PrintToConsole()
        {
            GenerateCharInfoBuffer();
            if (!Handle.IsInvalid)
            {
                WriteConsoleOutput(Handle, CharInfoBuffer,
                  new Coord() { X = Width, Y = Height },
                  new Coord() { X = 0, Y = 0 },
                  ref Rectangle);
            }
        }

        /// <summary>
        /// Generates the CharInfoBuffer from CharBuffer
        /// </summary>
        private void GenerateCharInfoBuffer()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int i = (y * Width) + x;
                    CharInfoBuffer[i].Char.AsciiChar = CharBuffer[x, y];
                    CharInfoBuffer[i].Attributes = 7;
                }
            } 
        }

    }
}
