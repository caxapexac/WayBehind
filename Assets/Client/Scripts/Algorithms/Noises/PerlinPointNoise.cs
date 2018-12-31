using System;
using Client.Scripts.Algorithms.Legacy;


namespace Client.Scripts.Algorithms.Noises
{
    public class PerlinPointNoise
    {
        private static double GetNoise(int x, int y)
        {
            int n = x + y * 57;
            n = (n << 13) ^ n;
            return 1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0;
        }

        private static double SmoothNoise(double x, double y)
        {
            int ix = (int)x, iy = (int)y;

            double corners = (GetNoise(ix - 1, iy - 1)
                    + GetNoise(ix + 1, iy - 1)
                    + GetNoise(ix - 1, iy + 1)
                    + GetNoise(ix + 1, iy + 1))
                / 16;
            double sides = (GetNoise(ix - 1, iy)
                    + GetNoise(ix + 1, iy)
                    + GetNoise(ix, iy - 1)
                    + GetNoise(ix, iy + 1))
                / 8;
            double center = GetNoise(ix, iy) / 4;
            return corners + sides + center;
        }


        private static double CompileNoise(double x, double y)
        {
            int integerX = (int)x;
            double fractionalX = x - integerX;

            int integerY = (int)y;
            double fractionalY = y - integerY;

            double v1 = SmoothNoise(integerX, integerY);
            double v2 = SmoothNoise(integerX + 1, integerY);
            double v3 = SmoothNoise(integerX, integerY + 1);
            double v4 = SmoothNoise(integerX + 1, integerY + 1);

            double i1 = LerpCurves.CosLerp(v1, v2, fractionalX);
            double i2 = LerpCurves.CosLerp(v3, v4, fractionalX);

            return LerpCurves.CosLerp(i1, i2, fractionalY);
        }

        public double GenerateNoise(double x, double y, double xOffset)
        {
            double total = 0;

            // это число может иметь и другие значения хоть cosf(sqrtf(2))*3.14f
            // главное чтобы было красиво и результат вас устраивал
            double persistence = 0.5f;

            // экспериментируйте с этими значениями, попробуйте ставить
            // например sqrtf(3.14f)*0.25f или что-то потяжелее для понимания J)
            double frequency = 0.1f;
            double amplitude = 0.5; //амплитуда, в прямой зависимости от значения настойчивости

            // рандомизация
            x += (xOffset);
            y += (xOffset);

            // NUM_OCTAVES - переменная, которая обозначает число октав,
            // чем больше октав, тем лучше получается шум
            int NUM_OCTAVES = 3;
            for (int i = 0; i < NUM_OCTAVES; i++)
            {
                total += CompileNoise(x * frequency, y * frequency) * amplitude;
                amplitude *= persistence;
                frequency *= 2;
            }

            total = Math.Abs(total);
            return total;
        }

        public double GenerateNoise(double x, double y, double xOffset, double yOffset, double octaves,
            double persistence, double frequency, double amplitude)
        {
            double total = 0;

            // рандомизация
            x += (xOffset);
            y += (yOffset);

            for (int i = 0; i < octaves; i++)
            {
                total += CompileNoise(x * frequency, y * frequency) * amplitude;
                amplitude *= persistence;
                frequency *= 2;
            }

            total = Math.Abs(total);
            return total;
        }
    }
}