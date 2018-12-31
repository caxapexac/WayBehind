using UnityEngine;


namespace Client.Scripts.Miscellaneous
{
    public struct HexaCoords
    {
        public HexaCoords(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;

        public int Z
        {
            get { return -X - Y; }
        }
    }


    public struct SimpleVector
    {
        public SimpleVector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void Normalize()
        {
            float magnitude = Mathf.Sqrt(X * X + Y * Y);
            X /= magnitude;
            Y /= magnitude;
        }

        public float X;
        public float Y;
    }


    public struct Int2
    {
        public Int2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;
    }
}