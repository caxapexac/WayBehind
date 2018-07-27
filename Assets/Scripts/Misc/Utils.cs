using LeopotamGroup.Math;
using UnityEngine;

namespace Misc
{
    public static class Utils
    {
        public static string CameraTag = "MainCamera";
        public static string PlayerTag = "Player";
        public static string EnemyTag = "Enemy";
        public static string GroundTag = "Ground";
        public static string BackPrefabPath = "Prefabs/BackgroundPrefab";
        public static string ForePrefabPath = "Prefabs/ForegroundPrefab";
    }

    public struct CubeCoords
    {
        public CubeCoords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x;
        public int y;

        public int z
        {
            get { return -x - y; }
        }
    }
    
    

    public static class HexMath
    {
        private static float TwoThree = 2f / 3f;
        private static float OneThree = 1f / 3f;
        private static float Sqrt3 = Mathf.Sqrt(3);
        private static float Sqrt3Two = Mathf.Sqrt(3) / 2;
        private static float Sqrt3Three = Mathf.Sqrt(3) / 3;

        public static int[,] Directions =
        {
            {1,-1},
            {0,-1},
            {-1,0},
            {-1,1},
            {0,1},
            {1,0}
        };//for rings
        
        public static int GetZ(int x, int y)
        {
            return -x - y;
        }

        public static int HexDistance(int x1, int y1, int x2, int y2)
        {
            int x = MathFast.Abs(x2 - x1);
            int y = MathFast.Abs(y2 - y1);
            int z = MathFast.Abs(-x - y);
            return MathFast.Max(x, MathFast.Max(y, z));
        }

        public static float LinearDistance(int x1, int y1, int x2, int y2)
        {
            int x = x2 - x1;
            int y = y2 - y1;
            int z = -x - y;
            return Mathf.Sqrt(x ^ 2 + y ^ 2 + z ^ 2);
        }

        public static CubeCoords Pix2Hex(float x, float y, float hexSize)
        {
            float hexX = TwoThree * x / hexSize;
            float hexY = (-OneThree * x + Sqrt3Three * y) / hexSize;
            float hexZ = -hexX - hexY;
            float rX = Mathf.Round(hexX);
            float rY = Mathf.Round(hexY);
            float rZ = Mathf.Round(hexZ);
            float xDiff = Mathf.Abs(rX - hexX);
            float yDiff = Mathf.Abs(rY - hexY);
            float zDiff = Mathf.Abs(rZ - hexZ);
            //Debug.Log(rX + " " + rY + " " + rZ);
            if (xDiff > yDiff && xDiff > zDiff) return new CubeCoords((int)(-rY - rZ), (int) rY);
            if (yDiff > zDiff) return new CubeCoords((int) rX, (int) (-rX - rZ));
            return new CubeCoords((int) rX, (int) rY);
        }

        public static CubeCoords Pix2Hex(Vector2 coords, float hexSize)
        {
            return Pix2Hex(coords.x, coords.y, hexSize);
        }

        public static Vector2 Hex2Pix(int x, int y, float hexSize)
        {
            Vector2 result = new Vector2();
            result.x = hexSize * 1.5f * x;
            result.y = hexSize * (Sqrt3Two * x + Sqrt3 * y);
            return result;
        }

        public static Vector2 Hex2Pix(CubeCoords coords, float hexSize)
        {
            return Hex2Pix(coords.x, coords.y, hexSize);
        }
    }
}