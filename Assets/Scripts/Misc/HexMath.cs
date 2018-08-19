using LeopotamGroup.Math;
using UnityEngine;

namespace Misc
{
    public class HexMath
    {
        private static float TwoThree = 2f / 3f;
        private static float OneThree = 1f / 3f;
        private static float Sqrt3 = Mathf.Sqrt(3);
        private static float Sqrt3Two = Mathf.Sqrt(3) / 2;
        private static float Sqrt3Three = Mathf.Sqrt(3) / 3;

        /// <summary>
        /// Координаты соседних гексов, относительно центрального
        /// </summary>
        public static int[,] Directions =
        {
            {1, -1},
            {0, -1},
            {-1, 0},
            {-1, 1},
            {0, 1},
            {1, 0}
        };

        public static int GetZ(int x, int y)
        {
            return -x - y;
        }

        /// <summary>
        /// Расстояние в гексах между двумя гексами
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static int HexDistance(HexaCoords first, HexaCoords second)
        {
            return HexDistance(first.X, second.X, first.Y, second.Y);
        }
        public static int HexDistance(int x1, int x2, int y1, int y2)
        {
            int x = x2 - x1;
            int y = y2 - y1;
            int z = -x - y;
            return MathFast.Max(MathFast.Abs(x), MathFast.Max(MathFast.Abs(y), MathFast.Abs(z)));
        }

        /// <summary>
        /// Расстояние по прямой между двумя гексами
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static float LinearDistance(HexaCoords first, HexaCoords second)
        {
            return LinearDistance(first.X, second.X, first.Y, second.Y);
        }
        public static float LinearDistance(int x1, int x2, int y1, int y2)
        {
            int x = x2 - x1;
            int y = y2 - y1;
            int z = -x - y;
            return Mathf.Sqrt(x ^ 2 + y ^ 2 + z ^ 2);
        }

        /// <summary>
        /// Преобразование мировых координат в гексагональные
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="hexSize"></param>
        /// <param name="wPos"></param>
        /// <returns></returns>
        public static HexaCoords Pixel2Hexel(Vector2 vector, float hexSize, int wPos = 0)
        {
            return Pixel2Hexel(vector.x, vector.y, hexSize, wPos);
        }
        public static HexaCoords Pixel2Hexel(float xPos, float yPos, float hexSize, int wPos = 0)
        {
            int x;
            int y;
            Pixel2Hexel(xPos, yPos, hexSize, out x, out y);
            return new HexaCoords(x, y, wPos);
        }
        public static void Pixel2Hexel(float xPos, float yPos, float hexSize, out int x, out int y)
        {
            float hexX = TwoThree * xPos / hexSize;
            float hexY = (-OneThree * xPos + Sqrt3Three * yPos) / hexSize;
            float hexZ = -hexX - hexY;
            float rX = Mathf.Round(hexX);
            float rY = Mathf.Round(hexY);
            float rZ = Mathf.Round(hexZ);
            float xDiff = MathFast.Abs(rX - hexX);
            float yDiff = MathFast.Abs(rY - hexY);
            float zDiff = MathFast.Abs(rZ - hexZ);
            if (xDiff > yDiff && xDiff > zDiff)
            {
                x = (int) (-rY - rZ);
                y = (int) rY;
            }
            else if (yDiff > zDiff)
            {
                x = (int) rX;
                y = (int) (-rX - rZ);
            }
            else
            {
                x = (int) rX;
                y = (int) rY;
            }
        }

        /// <summary>
        /// Преобразование гексагональных координат в мировые
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="hexSize"></param>
        /// <returns></returns>
        public static Vector2 Hexel2Pixel(HexaCoords coords, float hexSize)
        {
            return Hexel2Pixel(coords.X, coords.Y, hexSize);
        }
        public static Vector2 Hexel2Pixel(int xPos, int yPos, float hexSize)
        {
            return new Vector2
            {
                x = hexSize * 1.5f * xPos,
                y = hexSize * (Sqrt3Two * xPos + Sqrt3 * yPos)
            };
        }
        //YAGNI
        public static void Hexel2Pixel(int xPos, int yPos, float hexSize, out float x, out float y)
        {
            x = hexSize * 1.5f * xPos;
            y = hexSize * (Sqrt3Two * xPos + Sqrt3 * yPos);
        }

        /// <summary>
        /// Случайные гексагональные координаты в заданном радиусе
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static HexaCoords RandomPosition(HexaCoords center, int radius, int depth = 0)
        {
            //todo fix down-left moving
            HexaCoords coords = RandomPosition(radius, depth);
            coords.X += center.X;
            coords.Y += center.Y;
            return coords;
        }
        public static HexaCoords RandomPosition(int radius, int depth = 0)
        {
            int x;
            int y;
            RandomPosition(radius, out x, out y);
            return new HexaCoords(x, y, depth);
        }
        public static void RandomPosition(int radius, out int x, out int y)
        {
            x = Random.Range(-radius, radius);
            if (x > 0)
            {
                y = Mathf.RoundToInt(Random.Range(-radius, radius - x) + 0.5f);
            }
            else
            {
                y = Mathf.RoundToInt(Random.Range(-radius - x, radius) + 0.5f);
            }
        }
    }
}