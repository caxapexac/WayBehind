using System;
using Client.Scripts.OBSOLETE.Components;
using Client.Scripts.OBSOLETE.Misc;
using LeopotamGroup.Math;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client.Scripts.OBSOLETE.Map
{
    public static class MapGenHexPerlinNoise
    {
        public static HexaList3D<HexComponent> GenerateMap(int saeed, int octaves, int radius, float smooth,
            float persistance, int depth, int enemyCount, bool useTextures, float hexSize)
        {
            if (saeed != 0) Random.InitState(saeed);
            float size = hexSize / smooth;
            HexaList3D<HexComponent> map = new HexaList3D<HexComponent>(radius, depth);
            float[,] noise = FractalNoise.Get((int) (Random.value * 10000),
                Mathf.FloorToInt(HexMath.Hexel2Pixel(radius * 2, radius * 2, size).y + 1), octaves, persistance);
            for (int i = -radius; i < radius; i++)
            {
                for (int k = -radius; k < radius; k++)
                {
                    if (MathFast.Abs(HexMath.GetZ(i, k)) <= radius)
                    {
                        Vector2 hexPos = HexMath.Hexel2Pixel(i + radius, k + radius, size);
                        int topLeftX = Mathf.FloorToInt(hexPos.x);
                        int topLeftY = Mathf.FloorToInt(hexPos.y);
                        float top = Mathf.Lerp(noise[topLeftX, topLeftY], noise[topLeftX + 1, topLeftY],
                            QunticCurve(hexPos.x % 1));
                        float bottom = Mathf.Lerp(noise[topLeftX, topLeftY + 1], noise[topLeftX + 1, topLeftY + 1],
                            QunticCurve(hexPos.x % 1));
                        float height = Mathf.Lerp(top, bottom, QunticCurve(hexPos.y % 1));
                        float
                            addictive =
                                1; //todo //1 - Mathf.Abs((float)Mathf.Max(Mathf.Abs(i), Mathf.Abs(k), Mathf.Abs(HexMath.GetZ(i, k))) / (10 * radius));
                        HexTypes type;
                        if (!useTextures)
                        {
                            type = HexTypes.Grass;
                        }
                        else if (height > 0.85f * addictive)
                        {
                            type = HexTypes.Obstacle; //типа горы
                        }
                        else if (height > 0.7f * addictive)
                        {
                            type = HexTypes.Forest; //типа склон горы
                        }
                        else if (height > 0.25f * addictive)
                        {
                            type = HexTypes.Grass; //типа луга
                        }
                        else if (height > 0.1f * addictive)
                        {
                            type = HexTypes.Water; //типа берег
                        }
                        else
                        {
                            type = HexTypes.Swamp; //типа дно мира
                        }

                        map[i, k, 0] = new HexComponent
                        {
                            HexType = type,
                            Color = height
                        };
                    }
                }
            }

            for (int i = -radius; i < radius; i++)
            {
                for (int k = -radius; k < radius; k++)
                {
                    if (MathFast.Abs(HexMath.GetZ(i, k)) <= radius)
                    {
                        if (Random.value > 0.99)
                        {
                            map[i, k, 1] = new HexComponent() {HexType = HexTypes.Diamond};
                        }
                    }
                }
            }

            GenerateEnemy(map, 1, HexTypes.Enemy, HexTypes.Empty, enemyCount);
            map[0, 0, 1] = new HexComponent() {HexType = HexTypes.Spawn};
            return map;
        }

        [Obsolete]
        private static void GenerateEnemy(HexaList3D<HexComponent> map, int depth, HexTypes type, HexTypes badType,
            int count)
        {
            HexaCoords zero = new HexaCoords(0, 0);
            for (int i = 0; i < count; i++)
            {
                HexaCoords coords;
                do
                {
                    coords = HexMath.RandomPosition(map.Radius, depth);
                } while (map[coords].HexType != badType || HexMath.HexDistance(coords, zero) < count * 0.03f);

                map[coords] = new HexComponent()
                {
                    HexType = type,
                };
                int hp = Mathf.RoundToInt(Random.value * 3) + 1;
                map[coords].Properties[HexProperties.HP] = hp;
                map[coords].Properties[HexProperties.MaxHP] = hp;
                map[coords].Properties[HexProperties.IQ] = 100;
                map[coords].Properties[HexProperties.Speed] = 2;
                map[coords].Properties[HexProperties.AgroSpeed] = 3;
                map[coords].Properties[HexProperties.JumpSpeed] = 5;
                map[coords].Properties[HexProperties.AgroRadius] = (int) (Random.value * 6) + 1;
                map[coords].Properties[HexProperties.Flying] = Random.value > 0.5 ? 1 : 0;
            }
        }

        private static float QunticCurve(float t)
        {
            //кто придумал этот пиздец?)
            return t * t * t * (t * (t * 6 - 15) + 10);
        }
    }
}