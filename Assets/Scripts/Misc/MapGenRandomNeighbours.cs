using System;
using System.Reflection;
using Systems;
using Components;
using LeopotamGroup.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Misc
{
    public static class MapGenRandomNeighbours
    {
        public static void GenerateMap(out HexList4D<HexBackgroundComponent> background,
            out HexList4D<HexForegroundComponent> foreground, int radius, int seed)
        {
            Random.InitState(seed);
            background = new HexList4D<HexBackgroundComponent>();
            foreground = new HexList4D<HexForegroundComponent>();
            background.Fill(radius);
            foreground.Fill(radius);
            foreground.Add(0, 0, new HexForegroundComponent()
            {
                ForegroundType = ForegroundTypes.Spawn,
                IsNew = false
            });
            GenerateBackStruct(background, radius, BackroundTypes.Water, BackroundTypes.Grass, 70, 2, 1f, 0.2f);
            GenerateBackStruct(background, radius, BackroundTypes.Swamp, BackroundTypes.Grass, 50, 2, 1f, 0.4f);
            GenerateBackStruct(background, radius, BackroundTypes.Forest, BackroundTypes.Grass, 30, 2, 1f, 0.1f);
            GenerateForeStruct(foreground, radius, ForegroundTypes.Obstacle, ForegroundTypes.Empty, 10, 1, 100, 1f, 0.3f);
            GenerateForeStruct(foreground, radius, ForegroundTypes.Obstacle, ForegroundTypes.Empty, 20, 1, 70, 0.5f, 0.2f);
            GenerateForeStruct(foreground, radius, ForegroundTypes.Obstacle, ForegroundTypes.Empty, 100, 2, 70, 0.1f, 0.1f);
            GenerateForeStruct(foreground, radius, ForegroundTypes.Diamond, ForegroundTypes.Empty, 10, 1, 100, 0f, 0f);
            GenerateForeStruct(foreground, radius, ForegroundTypes.Diamond, ForegroundTypes.Obstacle, 10, 1, 100, 0.6f, 0.3f);
            GenerateForeStruct(foreground, radius, ForegroundTypes.Enemy, ForegroundTypes.Empty, 100, 1, 100, 0.01f, 0.01f);
        }

        private static void GenerateBackStruct(HexList4D<HexBackgroundComponent> background, int radius,
            BackroundTypes type, BackroundTypes badType, int count, int near, float chance, float diffChance)
        {
            for (int i = 0; i < count; i++)
            {
                GenerateRecursiveB(background, type, badType, HexMath.RandomPosition(radius), near, chance, diffChance);
            }
        }

        private static void GenerateRecursiveB(HexList4D<HexBackgroundComponent> background,
            BackroundTypes type, BackroundTypes badType, CubeCoords coords, int near, float chance, float diffChange)
        {
            background.Add(coords, new HexBackgroundComponent()
            {
                BackgroundType = type,
                IsNew = false
            });
            FastList<CubeCoords> neighbours = background.NeighboursOf(coords, near);
            for (int j = 0; j < neighbours.Count; j++)
            {
                HexBackgroundComponent hexComponent = background[neighbours[j].x, neighbours[j].y];
                if (hexComponent.BackgroundType == badType && Random.value < chance)
                {
                    GenerateRecursiveB(background, type, badType, neighbours[j], near, chance - diffChange, diffChange);
                }
            }
        }
        
        private static void GenerateForeStruct(HexList4D<HexForegroundComponent> foreground, int radius, ForegroundTypes type,
            ForegroundTypes badType, int value, int near, int count, float chance, float diffChance)
        {
            for (int i = 0; i < count; i++)
            {
                GenerateRecursiveF(foreground, type, badType, HexMath.RandomPosition(radius), value, near, chance, diffChance);
            }
        }

        private static void GenerateRecursiveF(HexList4D<HexForegroundComponent> background, ForegroundTypes type,
            ForegroundTypes badType, CubeCoords coords, int value, int near, float chance, float diffChange)
        {
            background.Add(coords, new HexForegroundComponent()
            {
                ForegroundType = type,
                IsNew = false,
                Value = value
            });
            FastList<CubeCoords> neighbours = background.NeighboursOf(coords, near);
            for (int j = 0; j < neighbours.Count; j++)
            {
                HexForegroundComponent hexComponent = background[neighbours[j].x, neighbours[j].y];
                if (hexComponent.ForegroundType == badType && Random.value < chance)
                {
                    GenerateRecursiveF(background, type, badType, neighbours[j], value, near, chance - diffChange, diffChange);
                }
            }
        }
        
        public static void GenerateHex(CubeCoords coords,
            HexList4D<HexBackgroundComponent> background, HexList4D<HexForegroundComponent> foreground)
        {
            //island-type generation:
            
            //background
            FastList<CubeCoords> neighbours = background.NeighboursOf(coords, 2);
            int water = 0;
            for (int i = 0; i < neighbours.Count; i++)
            {
                CubeCoords neighbour = neighbours[i];
                if (background[neighbour.x, neighbour.y].BackgroundType == BackroundTypes.Water) water++;
            }
            BackroundTypes TypeB;
            if (water > 9)
            {
                TypeB = BackroundTypes.Water;
            }
            else if (water > 6)
            {
                TypeB = Random.value > 0.9999f ? BackroundTypes.Grass : BackroundTypes.Water;
            }
            else if (water > 3)
            {
                TypeB = Random.value > 0.999f ? BackroundTypes.Grass : BackroundTypes.Water;
            }
            else if (water > 2)
            {
                TypeB = Random.value > 0.99f ? BackroundTypes.Grass : BackroundTypes.Water;
            }
            else
            {
                TypeB = Random.value > 0.05f ? BackroundTypes.Grass : BackroundTypes.Water;
            }
            background.Add(coords, new HexBackgroundComponent()
            {
                BackgroundType = TypeB,
                Parent = null,
                SpeedDown = 1f,
                IsNew = false
            });
            
            //foreground
            
            neighbours = foreground.NeighboursOf(coords, 2);
            int notEmpty = 0;
            for (int i = 0; i < neighbours.Count; i++)
            {
                CubeCoords neighbour = neighbours[i];
                if (foreground[neighbour.x, neighbour.y].ForegroundType != ForegroundTypes.Empty) notEmpty++;
            }
            ForegroundTypes typeF;
            if (notEmpty == 0 && TypeB != BackroundTypes.Water)
            {
                float r = Random.value;
                if (r > 0.99f)
                {
                    typeF = ForegroundTypes.Obstacle;
                }
                else if (r > 0.98f)
                {
                    typeF = ForegroundTypes.Enemy;
                }
                else if (r > 0.95f)
                {
                    typeF = ForegroundTypes.Diamond;
                }
                else
                {
                    typeF = ForegroundTypes.Empty;
                }
            }
            else
            {
                typeF = ForegroundTypes.Empty;
            }
            foreground.Add(coords, new HexForegroundComponent()
            {
                ForegroundType = typeF,
                Parent = null,
                IsNew = false
            });
            
        }
    }
}