using Client.Scripts.Miscellaneous;
using LeopotamGroup.Math;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable MemberCanBePrivate.Global

namespace Client.Scripts.Algorithms
{
    public static class HexMath
    {
        private const float TwoThree = 0.6666667f; // 2/3
        private const float ThreeTwo = 1.5f; // 3/2
        private const float OneThree = 0.333333343f; // 1/3
        private const float Sqrt3 = 1.73205078f; // sqrt(3)
        private const float Sqrt3Two = 0.8660254f; // sqrt(3)/2
        private const float Sqrt3Three = 0.577350259f; // sqrt(3)/3
        
        /// <summary>
        /// Координаты соседних гексов, относительно x и y для кубических координат
        /// </summary>
        public static readonly int[,] HexDirs =
        {
            {1, 0},
            {1, -1},
            {0, -1},
            {-1, 0},
            {-1, 1},
            {0, 1}
        };

        /// <summary>
        /// Координаты соседних гексов, относительно x и y для offset координат
        /// </summary>
        public static readonly int[,,] OffsetDirs =
        {
            {
                {+1, 0}, {+1, -1}, {0, -1},
                {-1, -1}, {-1, 0}, {0, +1}
            },
            {
                {+1, +1}, {+1, 0}, {0, -1},
                {-1, 0}, {-1, +1}, {0, +1}
            },
        };
        
        public static float InnerRadius(float outerRadius)
        {
            return outerRadius * 0.866025404f;
        }

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
        public static int HexDistance(HexCoords first, HexCoords second)
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
        public static float LinearDistance(HexCoords first, HexCoords second)
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


        public static OffsetCoords Hexel2Offset(HexCoords coords)
        {
            var x = coords.X;
            var y = coords.Y + (coords.X - (coords.X & 1)) / 2;
            return new OffsetCoords(y, x); //maybe another
        }

        public static HexCoords Offset2Hexel(OffsetCoords coords)
        {
            int x = coords.Y;
            int z = coords.X - (coords.Y - (coords.Y & 1)) / 2;
            int y = -x - z;
            return new HexCoords(x, y); //maybe another
        }

        public static HexCoords Pixel2Hexel(Vector2 vector, float hexSize)
        {
            int x;
            int y;
            float hexX = TwoThree * vector.x / hexSize;
            float hexY = (-OneThree * vector.x + Sqrt3Three * vector.y) / hexSize;
            float hexZ = -hexX - hexY;
            float rX = Mathf.Round(hexX);
            float rY = Mathf.Round(hexY);
            float rZ = Mathf.Round(hexZ);
            float xDiff = MathFast.Abs(rX - hexX);
            float yDiff = MathFast.Abs(rY - hexY);
            float zDiff = MathFast.Abs(rZ - hexZ);
            if (xDiff > yDiff && xDiff > zDiff)
            {
                x = (int)(-rY - rZ);
                y = (int)rY;
            }
            else if (yDiff > zDiff)
            {
                x = (int)rX;
                y = (int)(-rX - rZ);
            }
            else
            {
                x = (int)rX;
                y = (int)rY;
            }
            return new HexCoords(x, y);
        }

        public static Vector2 Hexel2Pixel(HexCoords coords, float hexSize)
        {
            return new Vector2
            {
                x = hexSize * 1.5f * coords.X,
                y = hexSize * (Sqrt3Two * coords.X + Sqrt3 * coords.Y)
            };
        }

        public static OffsetCoords Pixel2Offset(Vector2 vector, float hexSize)
        {
            return Hexel2Offset(Pixel2Hexel(vector, hexSize));
        }

        public static Vector2 Offset2Pixel(OffsetCoords coords, float hexSize)
        {
            float x = hexSize * ThreeTwo * coords.Y;
            float y = (float)(hexSize * Sqrt3 * (coords.X + 0.5 * (coords.Y & 1)));
            return new Vector2(x, y);
        }
        
        public static Int2 Offset2Chunk(OffsetCoords coords, int chunkSize)
        {
            Int2 raw = new Int2(coords.X / chunkSize, coords.Y / chunkSize);
            if (coords.X < 0)
            {
                raw.X--;
            }
            if (coords.Y < 0)
            {
                raw.Y--;
            }
            return raw;
        }
        
        public static Int2 Offset2Chunk(OffsetCoords coords, int chunkSize, out int index)
        {
            index = MathFast.Abs(coords.X % chunkSize) + MathFast.Abs(coords.Y % chunkSize * chunkSize);
            Int2 raw = new Int2(coords.X / chunkSize, coords.Y / chunkSize);
            if (coords.X < 0)
            {
                raw.X--;
            }
            if (coords.Y < 0)
            {
                raw.Y--;
            }
            return raw;
        }

        public static OffsetCoords Chunk2Offset(Int2 coords, int chunkSize, int index)
        {
            return new OffsetCoords(coords.X * chunkSize + index % chunkSize,
                coords.Y * chunkSize + index / chunkSize);
        }

        
        /// <summary>
        /// Случайные гексагональные координаты в заданном радиусе
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static HexCoords RandomPosition(HexCoords center, int radius)
        {
            //todo fix down-left moving
            HexCoords coords = RandomPosition(radius);
            coords.X += center.X;
            coords.Y += center.Y;
            return coords;
        }

        public static HexCoords RandomPosition(int radius)
        {
            int x = Random.Range(-radius, radius);
            int y = x > 0
                ? Mathf.RoundToInt(Random.Range(-radius, radius - x) + 0.5f)
                : Mathf.RoundToInt(Random.Range(-radius - x, radius) + 0.5f);
            return new HexCoords(x, y);
        }

        public static HexCoords HexNeighbour(HexCoords coords, int dirNum)
        {
            return new HexCoords(coords.X + HexDirs[dirNum, 0], coords.Y + HexDirs[dirNum, 1]);
        }

        public static OffsetCoords OffsetNeighbour(OffsetCoords coords, int dirNum)
        {
            return new OffsetCoords(coords.Y + OffsetDirs[coords.Y & 1, dirNum, 0],
                coords.X + OffsetDirs[coords.Y & 1, dirNum, 1]);
        }
    }
}