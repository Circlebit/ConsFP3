using System;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsFP3
{
    public static class MyConsole
    {
        private static readonly IntPtr stdInputHandle = WinAPI.GetStdHandle(-10);
        private static readonly IntPtr stdOutputHandle = WinAPI.GetStdHandle(-11);
        private static readonly IntPtr stdErrorHandle = WinAPI.GetStdHandle(-12);
        private static readonly IntPtr consoleHandle = WinAPI.GetConsoleWindow();

        public static int Width { get; set; }
        public static int Height { get; set; }

        private static ConsoleBuffer ConsoleBuffer { get; set; }
        public static char[,] CharBuffer { get; set; }
        private static int[,] ColorBuffer { get; set; }
        private static int Background { get; set; }

        public static void SetupConsole()
        {
            Width = 140;
            Height = 40;
            Console.Title = "FP3";
            Console.CursorVisible = false;

            Console.SetWindowSize(Width, Height);
            Console.SetBufferSize(Width, Height);

            ConsoleBuffer = new ConsoleBuffer(Width, Height);
            CharBuffer = new char[Width, Height];

            WinAPI.SetConsoleMode(stdInputHandle, 0x0080);

            CharBuffer = new char[Width, Height];
            ColorBuffer = new int[Width, Height];
        }

        public static void DisplayBuffer()
        {
            ConsoleBuffer.SetBuffer(CharBuffer, ColorBuffer, Background);
            ConsoleBuffer.WriteBufferToConsole();
        }
    }

    public class ConsoleBuffer
    {
        public SafeFileHandle Handle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


        private WinAPI.CharInfo[] CharInfoBuffer { get; set; }

        public ConsoleBuffer(int width, int height)
        {
            Width = width;
            Height = height;

            Handle = WinAPI.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            if (!Handle.IsInvalid)
            {
                CharInfoBuffer = new WinAPI.CharInfo[Width * Height];
            }
        }

        internal void SetBuffer(char[,] charBuffer, int[,] colorBuffer, int background)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int i = (y * Width) + x;
                    CharInfoBuffer[i].Attributes = (short)(colorBuffer[x, y] | (background << 4));
                    CharInfoBuffer[i].UnicodeChar = charBuffer[x, y];
                }
            }
        }

        public void WriteBufferToConsole()
        {
            WinAPI.SmallRect rect = new WinAPI.SmallRect() { Left = 0, Top = 0, Right = (short)Width, Bottom = (short)Height };
            WinAPI.WriteConsoleOutputW(
                Handle, CharInfoBuffer, 
                new WinAPI.Coord() { X = (short)Width, Y = (short)Height },
                new WinAPI.Coord() { X = 0, Y = 0 }, 
                ref rect);
            
        }
    }
}
