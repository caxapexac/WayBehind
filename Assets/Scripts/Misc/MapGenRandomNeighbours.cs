using System;
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
            HexBackgroundComponent backgroundHex = new HexBackgroundComponent();
            HexForegroundComponent foregroundHex = new HexForegroundComponent();
            background.Fill(radius);
            foreground.Fill(radius);
            foreground.Add(0, 0, new HexForegroundComponent()
            {
                ForegroundType = ForegroundTypes.Spawn,
                IsNew = false
            });
            GenerateBackgroundStructure(background, radius, BackroundTypes.Water, BackroundTypes.Grass, 70, 1f, 0.1f);
            GenerateBackgroundStructure(background, radius, BackroundTypes.Swamp, BackroundTypes.Grass, 50, 1f, 0.2f);
            GenerateBackgroundStructure(background, radius, BackroundTypes.Forest, BackroundTypes.Grass, 30, 1f, 0.03f);
            GenerateForegroundStructure(foreground, radius, ForegroundTypes.Obstacle, ForegroundTypes.Empty, 50, 100, 1f, 0.2f);
            GenerateForegroundStructure(foreground, radius, ForegroundTypes.Obstacle, ForegroundTypes.Empty, 50, 70, 1f, 0.4f);
            GenerateForegroundStructure(foreground, radius, ForegroundTypes.Diamond, ForegroundTypes.Empty, 10, 100, 0f, 0f);
            GenerateForegroundStructure(foreground, radius, ForegroundTypes.Diamond, ForegroundTypes.Obstacle, 10, 100, 1f, 0.3f);
            GenerateForegroundStructure(foreground, radius, ForegroundTypes.Enemy, ForegroundTypes.Empty, 100, 100, 0.01f, 0.01f);
        }

        private static void GenerateBackgroundStructure(HexList4D<HexBackgroundComponent> background, int radius,
            BackroundTypes type, BackroundTypes badType, int count, float chance, float diffChance)
        {
            for (int i = 0; i < count; i++)
            {
                GenerateRecursiveB(background, type, badType, HexMath.RandomPosition(radius), chance, diffChance);
            }
        }

        private static void GenerateRecursiveB(HexList4D<HexBackgroundComponent> background,
            BackroundTypes type, BackroundTypes badType, CubeCoords coords, float chance, float diffChange)
        {
            background.Add(coords, new HexBackgroundComponent()
            {
                BackgroundType = type,
                IsNew = false
            });
            FastList<CubeCoords> neighbours = background.NeighboursOf(coords);
            for (int j = 0; j < neighbours.Count; j++)
            {
                HexBackgroundComponent hexComponent = background[neighbours[j].x, neighbours[j].y];
                if (hexComponent.BackgroundType == badType && Random.Range(0f, 1f) < chance)
                {
                    GenerateRecursiveB(background, type, badType, neighbours[j], chance - diffChange, diffChange);
                }
            }
        }
        
        private static void GenerateForegroundStructure(HexList4D<HexForegroundComponent> foreground, int radius,
            ForegroundTypes type, ForegroundTypes badType, int value, int count, float chance, float diffChance)
        {
            for (int i = 0; i < count; i++)
            {
                GenerateRecursiveF(foreground, type, badType, HexMath.RandomPosition(radius), value, chance, diffChance);
            }
        }

        private static void GenerateRecursiveF(HexList4D<HexForegroundComponent> background,
            ForegroundTypes type, ForegroundTypes badType, CubeCoords coords, int value, float chance, float diffChange)
        {
            background.Add(coords, new HexForegroundComponent()
            {
                ForegroundType = type,
                IsNew = false,
                Value = value
            });
            FastList<CubeCoords> neighbours = background.NeighboursOf(coords);
            for (int j = 0; j < neighbours.Count; j++)
            {
                HexForegroundComponent hexComponent = background[neighbours[j].x, neighbours[j].y];
                if (hexComponent.ForegroundType == badType && Random.Range(0f, 1f) < chance)
                {
                    GenerateRecursiveF(background, type, badType, neighbours[j], value, chance - diffChange, diffChange);
                }
            }
        }
        
        public static void GenerateHex(CubeCoords coords,
            HexList4D<HexBackgroundComponent> background, HexList4D<HexForegroundComponent> foreground)
        {
            FastList<CubeCoords> neighbours = background.NeighboursOf(coords);
            int grass = 0;
            int water = 0;
            int swamp = 0;
            int forest = 0;
            for (int i = 0; i < neighbours.Count; i++)
            {
                switch (background[neighbours[i].x, neighbours[i].y].BackgroundType)
                {
                    case BackroundTypes.Grass:
                        grass++;
                        break;
                    case BackroundTypes.Water:
                        water++;
                        break;
                    case BackroundTypes.Swamp:
                        swamp++;
                        break;
                    case BackroundTypes.Forest:
                        forest++;
                        break;
                    default:
                        break;
                }
            }
            int max = Mathf.Max(grass, water, swamp, forest);
            BackroundTypes TypeB;
            if (Random.Range(0f, 1f) > 0.3f)
            {
                if (max == grass) TypeB = BackroundTypes.Grass;
                else if (max == water) TypeB = BackroundTypes.Water;
                else if (max == swamp) TypeB = BackroundTypes.Swamp;
                else TypeB = BackroundTypes.Forest;
            }
            else
            {
                Array values = Enum.GetValues(typeof(BackroundTypes));
                TypeB = (BackroundTypes)values.GetValue((int)Mathf.Round(Random.Range(0f, 1f) * (values.Length - 1)));
            }
            background.Add(coords, new HexBackgroundComponent()
            {
                BackgroundType = TypeB,
                Parent = null,
                SpeedDown = 1f,
                IsNew = false
            });
            //foreground
            neighbours = foreground.NeighboursOf(coords);
            int empty = 0;
            int obstacle = 0;
            int enemy = 0;
            int diamond = 0;
            for (int i = 0; i < neighbours.Count; i++)
            {
                switch (foreground[neighbours[i].x, neighbours[i].y].ForegroundType)
                {
                    case ForegroundTypes.Empty:
                        empty++;
                        break;
                    case ForegroundTypes.Obstacle:
                        obstacle++;
                        break;
                    case ForegroundTypes.Enemy:
                        enemy++;
                        break;
                    case ForegroundTypes.Diamond:
                        diamond++;
                        break;
                    default:
                        break;
                }
            }
            max = Mathf.Max(empty, obstacle, enemy, diamond);
            ForegroundTypes typeF;
            if (Random.Range(0f, 1f) > 0.2f)
            {
                if (max == obstacle) typeF = ForegroundTypes.Obstacle;
                else if (max == enemy) typeF = ForegroundTypes.Empty;
                else if (max == diamond) typeF = ForegroundTypes.Empty;
                else typeF = ForegroundTypes.Empty;
            }
            else
            {
                Array values = Enum.GetValues(typeof(ForegroundTypes));
                typeF = (ForegroundTypes)values.GetValue((int)Mathf.Round(Random.Range(0f, 1f) * (values.Length - 1)));
                if (typeF == ForegroundTypes.Spawn) typeF = ForegroundTypes.Diamond;
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