using System;
using System.Security.Cryptography;
using Components;
using LeopotamGroup.Collections;
using LeopotamGroup.Math;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Misc
{
    public static class MapGenRandomNeighbours
    {
        public static HexaList3D<HexComponent> GenerateMap(int saeed, int radius, int depth)
        {
            if (saeed != 0) Random.InitState(saeed);
            HexaList3D<HexComponent> map = new HexaList3D<HexComponent>(radius, depth);
            FillStructure(map, 0, HexTypes.Grass);
            FillStructure(map, 1, HexTypes.Empty);
            map[0, 0, 1] = new HexComponent() {HexType = HexTypes.Spawn};
            GenerateStructure(map, 0, HexTypes.Water, HexTypes.Grass, 70, 2, 1f, 0.2f);
            GenerateStructure(map, 0, HexTypes.Swamp, HexTypes.Grass, 50, 2, 1f, 0.4f);
            GenerateStructure(map, 0, HexTypes.Forest, HexTypes.Grass, 30, 2, 1f, 0.1f);
            GenerateStructure(map, 1, HexTypes.Obstacle, HexTypes.Empty, 100, 1, 1f, 0.3f);
            GenerateStructure(map, 1, HexTypes.Obstacle, HexTypes.Empty, 70, 1, 0.5f, 0.2f);
            GenerateStructure(map, 1, HexTypes.Obstacle, HexTypes.Empty, 70, 2, 0.1f, 0.1f);
            GenerateStructure(map, 1, HexTypes.Diamond, HexTypes.Empty, 100, 1, 0f, 1f);
            GenerateStructure(map, 1, HexTypes.Diamond, HexTypes.Obstacle, 100, 1, 0.6f, 0.3f);
            GenerateEnemy(map, 1, HexTypes.Enemy, HexTypes.Empty, 200);
            return map;
        }

        private static void FillStructure(HexaList3D<HexComponent> map, int depth, HexTypes type)
        {
            int radius = map.Radius;
            for (int i = -radius; i < radius; i++)
            {
                for (int j = -radius; j < radius; j++)
                {
                    if (MathFast.Abs(HexMath.GetZ(i, j)) <= radius)
                        map[i, j, depth] = new HexComponent() {HexType = type};
                }
            }
        }

        private static void GenerateStructure(HexaList3D<HexComponent> map, int depth,
            HexTypes type, HexTypes badType, int count, int near, float chance, float diffChance)
        {
            for (int i = 0; i < count; i++)
            {
                GenerateRecursiveStructure(map,
                    type, badType, HexMath.RandomPosition(map.Radius, depth), near, chance, diffChance);
            }
        }

        private static void GenerateRecursiveStructure(HexaList3D<HexComponent> map,
            HexTypes type, HexTypes badType, HexaCoords coords, int near, float chance, float diffChange)
        {
            map[coords] = new HexComponent() {HexType = type};
            FastList<HexaCoords> neighbours = map.NeighboursOf(coords, near);
            for (int j = 0; j < neighbours.Count; j++)
            {
                HexComponent hexComponent = map[neighbours[j]];
                if (hexComponent.HexType == badType && Random.value < chance)
                {
                    GenerateRecursiveStructure(map, type, badType, neighbours[j], near, chance - diffChange,
                        diffChange);
                }
            }
        }

        private static void GenerateEnemy(HexaList3D<HexComponent> map, int depth, HexTypes type, HexTypes badType, int count)
        {
            for (int i = 0; i < count; i++)
            {
                HexaCoords coords;
                do
                {
                    coords = HexMath.RandomPosition(map.Radius, depth);
                } while (map[coords].HexType != badType);
                map[coords] = new HexComponent()
                {
                    HexType = type,
                };
                map[coords].Properties[HexProperties.HP] = 3;
                map[coords].Properties[HexProperties.IQ] = 100;
                map[coords].Properties[HexProperties.Speed] = 2;
                map[coords].Properties[HexProperties.AgroSpeed] = 3;
                map[coords].Properties[HexProperties.JumpSpeed] = 5;
                map[coords].Properties[HexProperties.AgroRadius] = (int)(Random.value * 6) + 1;
                map[coords].Properties[HexProperties.Flying] = Random.value > 0.5 ? 1 : 0;
            }
        }

        [Obsolete]
        public static void GenerateHex(HexaList3D<HexComponent> map, HexaCoords coords)
        {
            map[coords] = new HexComponent()
            {
                HexType = HexTypes.Swamp,
                Parent = null,
            };
            
//            //island-type generation:
//
//            //background
//            FastList<HexaCoords> neighbours = map.NeighboursOf(coords, 2, 0);
//            int water = 0;
//            for (int i = 0; i < neighbours.Count; i++)
//            {
//                HexaCoords neighbour = neighbours[i];
//                if (map[neighbour].HexType == HexTypes.Water) water++;
//            }
//
//            HexTypes typeB;
//            if (water > 9)
//            {
//                typeB = HexTypes.Water;
//            }
//            else if (water > 6)
//            {
//                typeB = Random.value > 0.9999f ? HexTypes.Grass : HexTypes.Water;
//            }
//            else if (water > 3)
//            {
//                typeB = Random.value > 0.999f ? HexTypes.Grass : HexTypes.Water;
//            }
//            else if (water > 2)
//            {
//                typeB = Random.value > 0.99f ? HexTypes.Grass : HexTypes.Water;
//            }
//            else
//            {
//                typeB = Random.value > 0.05f ? HexTypes.Grass : HexTypes.Water;
//            }
//
//            map[coords] = new HexComponent()
//            {
//                HexType = typeB,
//                Parent = null,
//            };
//
//            //foreground
//            coords.W = 1;
//            neighbours = map.NeighboursOf(coords, 2);
//            int notEmpty = 0;
//            for (int i = 0; i < neighbours.Count; i++)
//            {
//                HexaCoords neighbour = neighbours[i];
//                if ((map[neighbour]).HexType != HexTypes.Empty) notEmpty++;
//            }
//
//            HexTypes typeF;
//            if (notEmpty == 0 && typeB != HexTypes.Water)
//            {
//                float r = Random.value;
//                if (r > 0.99f)
//                {
//                    typeF = HexTypes.Obstacle;
//                }
//                else if (r > 0.98f)
//                {
//                    typeF = HexTypes.Diamond;
//                }
//                else if (r > 0.95f)
//                {
//                    typeF = HexTypes.Diamond;
//                }
//                else
//                {
//                    typeF = HexTypes.Empty;
//                }
//            }
//            else
//            {
//                typeF = HexTypes.Empty;
//            }
//
//            map[coords] = new HexComponent()
//            {
//                HexType = typeF,
//                Parent = null,
//            };
        }

        public static void GenerateChunk()
        {
            //TODO
        }
    }
}