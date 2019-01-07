using System;
using Client.Scripts.Algorithms.Legacy;
using Client.Scripts.Components;
using Client.Scripts.Scriptable;
using UnityEngine;


namespace Client.Scripts.Algorithms.Noises
{
    public static class PerlinPointNoise
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

            double i1 = Curves.CosLerp(v1, v2, fractionalX);
            double i2 = Curves.CosLerp(v3, v4, fractionalX);

            return Curves.CosLerp(i1, i2, fractionalY);
        }

        public static double GenerateNoise(double x, double y, double xOffset)
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

        public static double GenerateNoise(float x, float y, double octaves,
            double frequency, double amplitude, double persistance, double lacunarity)
        {
            double total = 0;
            for (int i = 0; i < octaves; i++)
            {
                total += CompileNoise(x * frequency, y * frequency) * amplitude;
                amplitude *= persistance;
                frequency *= lacunarity;
            }
            total = Math.Abs(total);
            if (total > 1d)
            {
                return 1d;
            }
            if (total < 0)
            {
                return 0;
            }
            return total;
        }

        public static void SetupHex(HexComponent hex, MapNoiseObject settings)
        {
            float x, y;
            settings.H.GetOffset(out x, out y);
            hex.H = GenerateNoise(hex.Position.x + x,
                hex.Position.y + y,
                settings.H.Octaves,
                settings.H.Frequency,
                settings.H.Amplitude,
                settings.H.Persistance,
                settings.H.Lacunarity);
            settings.T.GetOffset(out x, out y);
            hex.T = GenerateNoise(hex.Position.x + x,
                hex.Position.y + y,
                settings.T.Octaves,
                settings.T.Frequency,
                settings.T.Amplitude,
                settings.T.Persistance,
                settings.T.Lacunarity);
            settings.M.GetOffset(out x, out y);
            hex.M = GenerateNoise(hex.Position.x + x,
                hex.Position.y + y,
                settings.M.Octaves,
                settings.M.Frequency,
                settings.M.Amplitude,
                settings.M.Persistance,
                settings.M.Lacunarity);
        }
    }
}