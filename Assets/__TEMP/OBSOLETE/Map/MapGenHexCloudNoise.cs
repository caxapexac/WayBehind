using Client.Scripts.OBSOLETE.Components;
using Client.Scripts.OBSOLETE.Misc;
using LeopotamGroup.Math;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client.Scripts.OBSOLETE.Map
{
    public static class MapGenHexCloudNoise
    {
        public static HexaList3D<HexComponent> GenerateMap(int saeed, int octaves, int radius, int depth, float hexSize)
        {
            if (saeed != 0) Random.InitState(saeed);
            HexaList3D<HexComponent> map = new HexaList3D<HexComponent>(radius, depth);
            //Vector2 max = HexMath.Hexel2Pixel(radius, radius, hexSize);
            //float[,] noise = PerlinNoise.Get((int)(Random.value * 10000), Mathf.RoundToInt(max.x * 10 + 1), octaves);
            float[,] noise = HexCloudNoise.Get((int)(Random.value * 10000), radius * 2, octaves);
            for (int i = -radius; i < radius; i++)
            {
                for (int k = -radius; k < radius; k++)
                {
                    if (MathFast.Abs(HexMath.GetZ(i, k)) <= radius)
                    {
                        Vector2 hexPos = HexMath.Hexel2Pixel(i + radius, k + radius, hexSize);
                        float height = noise[i + radius, k + radius];
                        float addictive = Mathf.Abs((float)Mathf.Max(Mathf.Abs(i), Mathf.Abs(k), Mathf.Abs(HexMath.GetZ(i, k))) / 350);
                        if (height > 0.85f + addictive)
                        {
                            map[i, k, 0] = new HexComponent() {HexType = HexTypes.Obstacle};
                        }
                        else if (height > 0.6f + addictive)
                        {
                            map[i, k, 0] = new HexComponent() {HexType = HexTypes.Forest};
                        }
                        else if (height > 0.3f + addictive)
                        {
                            map[i, k, 0] = new HexComponent() {HexType = HexTypes.Grass};
                        }
                        else if (height > 0.1f + addictive)
                        {
                            map[i, k, 0] = new HexComponent() {HexType = HexTypes.Water};
                        }
                        else
                        {
                            map[i, k, 0] = new HexComponent() {HexType = HexTypes.Swamp};
                        }

                        map[i, k, 0].Color = height;
                    }
                }
            }
            
            
            
//            FillStructure(map, 0, HexTypes.Grass);
//            FillStructure(map, 1, HexTypes.Empty);
            map[0, 0, 1] = new HexComponent() {HexType = HexTypes.Spawn};
            
            
            
            
            
            
            
//            GenerateStructure(map, 0, HexTypes.Water, HexTypes.Grass, 70, 2, 1f, 0.2f);
//            GenerateStructure(map, 0, HexTypes.Swamp, HexTypes.Grass, 50, 2, 1f, 0.4f);
//            GenerateStructure(map, 0, HexTypes.Forest, HexTypes.Grass, 30, 2, 1f, 0.1f);
//            GenerateStructure(map, 1, HexTypes.Obstacle, HexTypes.Empty, 100, 1, 1f, 0.3f);
//            GenerateStructure(map, 1, HexTypes.Obstacle, HexTypes.Empty, 70, 1, 0.5f, 0.2f);
//            GenerateStructure(map, 1, HexTypes.Obstacle, HexTypes.Empty, 70, 2, 0.1f, 0.1f);
//            GenerateStructure(map, 1, HexTypes.Diamond, HexTypes.Empty, 100, 1, 0f, 1f);
//            GenerateStructure(map, 1, HexTypes.Diamond, HexTypes.Obstacle, 100, 1, 0.6f, 0.3f);
//            GenerateEnemy(map, 1, HexTypes.Enemy, HexTypes.Empty, 200);
            return map;
        }

        
    }
}