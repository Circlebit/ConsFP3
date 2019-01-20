using System;
using System.IO;
using static ConsFP4.Native;

namespace ConsFP4
{

    class Program
    {

        static void Main(string[] args)
        {
            short w = 180;
            short h = 60;
            MyConsole.SetupConsole(w, h);

            var rc = new RayCaster();
            double[] distanceToWallPerCol = rc.CastAllRays();
            //int[] ceilingSize = new int[distanceToWallPerCol.Length];
            //int[] floorSize = new int[distanceToWallPerCol.Length];

            byte[,] newBuf = new byte[MyConsole.Width, MyConsole.Height];

            for (int x = 0; x < distanceToWallPerCol.Length; x++)
            {
                //ceilingSize[x] = GetCeilingSize(distanceToWallPerCol[x]);
                //floorSize[x] = GetFloorSize(distanceToWallPerCol[x]);

                double distToWall = distanceToWallPerCol[x];

                int ceilingSize = GetCeilingSize(distToWall);
                int floorSize = GetFloorSize(distToWall);

                for (int y = 0; y < MyConsole.Height; y++)
                {
                    if (y < ceilingSize) newBuf[x, y] = 126; // ceiling
                    else if (y >= ceilingSize && y <= floorSize) newBuf[x, y] = GetWallChar(distToWall, rc.Depth); // wall
                    else newBuf[x, y] = 61; // floor
                }
            }

            MyConsole.Buffer = newBuf;
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

        private static int GetCeilingSize(double distToWall)
        {
            double doubleHeight = Convert.ToDouble(MyConsole.Height);
            double ceilingSize = (doubleHeight / 2.0) - (doubleHeight / distToWall);
            return Convert.ToInt32(ceilingSize);
        }

        private static int GetFloorSize(double distToWall)
        {
            return MyConsole.Height - GetCeilingSize(distToWall);
        }

        private static byte GetWallChar(double distToWall, double depth)
        {
            // Shader walls based on distance
            byte shade = 32;
            if (distToWall <= depth / 4.0f) shade = 176;  // Very close	
            else if (distToWall < depth / 3.0f) shade = 177;
            else if (distToWall < depth / 2.0f) shade = 178;
            else if (distToWall < depth) shade = 219;
            else shade = 32;      // Too far away
            return shade;
        }
    }

    class RayCaster
    {
        public Player Player { get; private set; }

        public Map Map { get; set; }

        /// <summary>
        /// Field of View
        /// </summary>
        public int FoV { get; set; }

        /// <summary>
        /// Angle between subsequent rays (FoV / Width)
        /// </summary>
        public double AngleBetweenRays
        {
            get => FoV / MyConsole.Width;
        }

        public double StepSize { get; private set; }
        public double Depth { get; private set; }

        public double DistanceToProjectionPlane
        {
            get => MyConsole.Width/2.0 / Math.Tan(DegrToRad(FoV));
        }

        private double DegrToRad(double degr)
        {
            return degr * (Math.PI / 180.0);
        }

        public RayCaster()
        {
            Player = new Player(8.0, 8.0, 0.0);
            FoV = 60;
            StepSize = 0.1;
            Depth = 16.0;
            Map = new Map();
        }

        private double CastRay(double angle)
        {
            double EyeX = Math.Sin(DegrToRad(angle));
            double EyeY = Math.Cos(DegrToRad(angle));

            double distanceToWall = 0;
            bool hitWall = false;

            while (!hitWall && distanceToWall < Depth)
            {
                distanceToWall += StepSize;
                int testX = Convert.ToInt32(Player.X + EyeX * distanceToWall);
                int testY = Convert.ToInt32(Player.Y + EyeY * distanceToWall);
                
                if(testX < 0 || testX >= Map.Width || testY < 0 || testY >= Map.Height)
                {
                    hitWall = true;
                    distanceToWall = Depth;
                }
                else
                {
                    if(Map.String[testX * Map.Width + testY] == '#')
                    {
                        hitWall = true;
                    }
                }
            }

            return distanceToWall;
        }

        public double[] CastAllRays()
        {
            double[] distanceToWallPerColumn = new double[MyConsole.Width];

            for (int x = 0; x < MyConsole.Width; x++)
            {
                double rayAngle = (Player.A - FoV / 2.0) + ( Convert.ToDouble(x) / Convert.ToDouble(MyConsole.Width)) * FoV;
                distanceToWallPerColumn[x] = CastRay(rayAngle);
            }

            return distanceToWallPerColumn;
        }
    }

    class Player
    {
        public double X { get; set; }
        public double Y { get; set; }
        
        /// <summary>
        /// Angle the Player is Looking into
        /// </summary>
        public double A { get; set; }

        public Player(double x, double y, double angle)
        {
            X = x;
            Y = y;
            A = angle;
        }
    }

    class Map
    {
        public string String { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


        public Map()
        {
            Width = 16;
            Height = 16;

            String = String.Empty;
            String += "################";
            String += "#              #";
            String += "#              #";
            String += "#              #";
            String += "#              #";
            String += "#              #";
            String += "#              #";
            String += "#              #";
            String += "#              #";
            String += "#              #";
            String += "#              #";
            String += "#              #";
            String += "#              #";
            String += "#              #";
            String += "#              #";
            String += "################";
        }
    }
}