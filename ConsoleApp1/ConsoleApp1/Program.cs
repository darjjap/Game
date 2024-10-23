using System;
using System.Reflection.Metadata;

namespace gameInISS
{
    internal class Program
    {
        //размер окна
        private const int ScreenWidth = 150;
        private const int ScreenHeight = 50;

        //размер карты
        private const int MapWidth = 32;
        private const int MapHeight = 32;

        private const double Fov = Math.PI / 3; //обзор
        private const double Depth = 16; //точка невозврата, предел

        //игрок
        private static double _playerX;
        private static double _playerY;
        private static double _playerA = 0; //угол (angle) игрока

        private static string _map = "";

        private static readonly char[] Screen = new char[ScreenWidth * ScreenHeight];

        static void Main(string[] args)
        {
            //задаем размеры окна
            Console.SetWindowSize(ScreenHeight, ScreenWidth);
            Console.SetBufferSize(ScreenHeight, ScreenWidth);
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            _map += "################################";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "################################";
        }

        private static void RenderFrame()
        {
            for (int x = 0; x < ScreenHeight; x++)
            {
                double rayAngle = _playerA + Fov / 2 - x * Fov / ScreenWidth;

                double rayX = Math.Sin(rayAngle);
                double rayY = Math.Cos(rayAngle);

                double distanceToWall = 0;
                bool hitWall = false;

                while (!hitWall && distanceToWall < Depth)
                {
                    distanceToWall += 0.1;

                    int testX = (int)(_playerX + rayX * distanceToWall);
                    int testY = (int)(_playerY + rayY * distanceToWall);

                    if (testX < 0 || testX >= MapWidth || testY < 0 || testY >= MapHeight)
                    {
                        hitWall = true;
                        distanceToWall = Depth;
                    }
                    else
                    {
                        char testCell = _map[testY * MapWidth + testX];

                        if (testCell == '#')
                        {
                            hitWall = true;
                        }
                    }
                }

                int ceiling = (int)(ScreenHeight / 2d - ScreenHeight * Fov / distanceToWall); //длина отрисованного объекта
                                                                                              //в нужном столбике (близко - большой,
                                                                                              //далеко - маленький)
                int floor = ScreenHeight - ceiling; //отрисовка этажа

                char wallshade;

                if (distanceToWall < Depth / 4d)
                {
                    wallshade = '\u2588';
                }
                else if (distanceToWall < Depth / 3d)
                {
                    wallshade = '\u2593';
                }
                else if (distanceToWall < Depth / 2d)
                {
                    wallshade = '\u2592';
                }
                else if (distanceToWall < Depth)
                {
                    wallshade = '\u2591';
                }
                else
                {
                    wallshade = ' ';
                }

                for (int y = 0; y < ScreenHeight; y++)
                {
                    if (y <= ceiling)
                    {
                        Screen[y * ScreenWidth + x] = ' '; //потолок
                    }
                    else if (y > ceiling && y <= floor)
                    {
                        Screen[y * ScreenWidth + x] = wallshade; //стена
                    }
                    else
                    {
                        Screen[y * ScreenWidth + x] = '.'; //пол
                    }
                }
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(Screen);
        }
    }
}