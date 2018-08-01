﻿using LeopotamGroup.Math;
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

        public static int[,] Directions =
        {
            {1, -1},
            {0, -1},
            {-1, 0},
            {-1, 1},
            {0, 1},
            {1, 0}
        }; //for rings

        public static int GetZ(int x, int y)
        {
            return -x - y;
        }

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

        public static HexaCoords Pix2Hex(Vector2 vector, float hexSize)
        {
            return Pix2Hex(vector.x, vector.y, hexSize);
        }

        public static HexaCoords Pix2Hex(float xPos, float yPos, float hexSize)
        {
            int x;
            int y;
            Pix2Hex(xPos, yPos, hexSize, out x, out y);
            return new HexaCoords(x, y);
        }

        public static void Pix2Hex(float xPos, float yPos, float hexSize, out int x, out int y)
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

        public static Vector2 Hex2Pix(HexaCoords coords, float hexSize)
        {
            return Hex2Pix(coords.X, coords.Y, hexSize);
        }

        public static Vector2 Hex2Pix(int xPos, int yPos, float hexSize)
        {
            return new Vector2
            {
                x = hexSize * 1.5f * xPos,
                y = hexSize * (Sqrt3Two * xPos + Sqrt3 * yPos)
            };
        }

        public static void Hex2Pix(int xPos, int yPos, float hexSize, out float x, out float y)
        {
            x = hexSize * 1.5f * xPos;
            y = hexSize * (Sqrt3Two * xPos + Sqrt3 * yPos);
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
                y = Mathf.RoundToInt(Random.Range(-radius, radius - x));
            }
            else
            {
                y = Mathf.RoundToInt(Random.Range(-radius - x, radius));
            }
        }
    }
}