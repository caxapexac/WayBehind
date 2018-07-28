using System.Xml.Linq;
using Systems;
using Components;
using LeopotamGroup.Collections;
using UnityEngine;

namespace Misc
{
    public static class MapGenRandomNeighbours
    {
        public static HexList4D<HexBackgroundComponent> GenerateBackground(int capacity)
        {
            HexList4D<HexBackgroundComponent> back = new HexList4D<HexBackgroundComponent>(capacity);
            for (int i = -100; i < 100; i++)
            {
                for (int j = -100; j < 100; j++)
                {
                    back.Add(i, j, new HexBackgroundComponent()
                    {
                        GroundType = BackroundTypes.Grass,
                        IsNew = false
                    });
                }
            }

            int waterCount = Mathf.RoundToInt(Random.Range(0.2f, 1f) * 130);
            int bolotoCount = Mathf.RoundToInt(Random.Range(0.2f, 1f) * 70);
            int forestCount = Mathf.RoundToInt(Random.Range(0.2f, 1f) * 20);
            
            for (int i = 0; i < waterCount; i++)
            {
                CubeCoords coords = new CubeCoords(Mathf.RoundToInt(Random.Range(-1f, 1f) * 100), Mathf.RoundToInt(Random.Range(-1f, 1f) * 100));
                GenerateWater(back, coords, 1f);
            }
            for (int i = 0; i < bolotoCount; i++)
            {
                CubeCoords coords = new CubeCoords(Mathf.RoundToInt(Random.Range(-1f, 1f) * 100), Mathf.RoundToInt(Random.Range(-1f, 1f) * 100));
                GenerateBoloto(back, coords, 1f);
            }
            for (int i = 0; i < forestCount; i++)
            {
                CubeCoords coords = new CubeCoords(Mathf.RoundToInt(Random.Range(-1f, 1f) * 100), Mathf.RoundToInt(Random.Range(-1f, 1f) * 100));
                GenerateForest(back, coords, 1f);
            }
            
            return back;
        }

        private static void GenerateWater(HexList4D<HexBackgroundComponent> back ,CubeCoords coords, float chance)
        {
            back.Add(coords, new HexBackgroundComponent()
            {
                GroundType = BackroundTypes.Water,
                IsNew = false
            });
            FastList<CubeCoords> neighbours = back.NeighboursOf(coords);
            for (int j = 0; j < neighbours.Count; j++)
            {
                HexBackgroundComponent hexComponent = back[neighbours[j].x, neighbours[j].y];
                if (hexComponent.GroundType == BackroundTypes.Grass && Random.Range(0f,1f) < chance)
                {
                    GenerateWater(back, neighbours[j], chance - 0.05f);
                }
            }
        }
        
        private static void GenerateBoloto(HexList4D<HexBackgroundComponent> back ,CubeCoords coords, float chance)
        {
            back.Add(coords, new HexBackgroundComponent()
            {
                GroundType = BackroundTypes.Boloto,
                IsNew = false
            });
            FastList<CubeCoords> neighbours = back.NeighboursOf(coords);
            for (int j = 0; j < neighbours.Count; j++)
            {
                HexBackgroundComponent hexComponent = back[neighbours[j].x, neighbours[j].y];
                if ((hexComponent.GroundType == BackroundTypes.Grass || hexComponent.GroundType == BackroundTypes.Water) 
                    && Random.Range(0f,1f) < chance)
                {
                    GenerateBoloto(back, neighbours[j], chance - 0.1f);
                }
            }
        }
        
        private static void GenerateForest(HexList4D<HexBackgroundComponent> back ,CubeCoords coords, float chance)
        {
            back.Add(coords, new HexBackgroundComponent()
            {
                GroundType = BackroundTypes.Forest,
                IsNew = false
            });
            FastList<CubeCoords> neighbours = back.NeighboursOf(coords);
            for (int j = 0; j < neighbours.Count; j++)
            {
                HexBackgroundComponent hexComponent = back[neighbours[j].x, neighbours[j].y];
                if (hexComponent.GroundType == BackroundTypes.Grass && Random.Range(0f,1f) < chance)
                {
                    GenerateForest(back, neighbours[j], chance - 0.02f);
                }
            }
        }

            
            
            
            
        public static HexList4D<HexForegroundComponent> GenerateForeground(HexList4D<HexBackgroundComponent> back)
        {
            HexList4D<HexForegroundComponent> fore = new HexList4D<HexForegroundComponent>(back.CapacityMin());
            for (int i = -100; i < 100; i++)
            {
                for (int j = -100; j < 100; j++)
                {
                    fore.Add(i, j, new HexForegroundComponent()
                    {
                        ObjectType = ForegroundTypes.Empty,
                        IsNew = false
                    });
                }
            }
            fore.Add(0, 0, new HexForegroundComponent()
            {
                ObjectType = ForegroundTypes.Player,
                IsNew = false
            });
            int obstacleCount = Mathf.RoundToInt(Random.Range(0.2f, 1f) * 100);
            int enemyCount = Mathf.RoundToInt(Random.Range(0.2f, 1f) * 100);
            int diamondCount = Mathf.RoundToInt(Random.Range(0.2f, 1f) * 100);
            for (int i = 0; i < obstacleCount; i++)
            {
                CubeCoords coords = new CubeCoords(Mathf.RoundToInt(Random.Range(-1f, 1f) * 100), Mathf.RoundToInt(Random.Range(-1f, 1f) * 100));
                GenerateObstacle(back, fore, coords, 1f);
                coords = new CubeCoords(Mathf.RoundToInt(Random.Range(-1f, 1f) * 100), Mathf.RoundToInt(Random.Range(-1f, 1f) * 100));
                GenerateSingleObstacle(back, fore, coords);
            }
            for (int i = 0; i < enemyCount; i++)
            {
                //todo
            }
            for (int i = 0; i < diamondCount; i++)
            {
                CubeCoords coords = new CubeCoords(Mathf.RoundToInt(Random.Range(-1f, 1f) * 100), Mathf.RoundToInt(Random.Range(-1f, 1f) * 100));
                GenerateDiamond(back, fore, coords, 0.5f);
            }
            return fore;
        }

        private static void GenerateDiamond(HexList4D<HexBackgroundComponent> back, HexList4D<HexForegroundComponent> fore,
            CubeCoords coords, float chance)
        {
            fore.Add(coords, new HexForegroundComponent()
            {
                ObjectType = ForegroundTypes.Diamond,
                IsNew = false
            });
            FastList<CubeCoords> neighbours = fore.NeighboursOf(coords);
            for (int j = 0; j < neighbours.Count; j++)
            {
                HexForegroundComponent hexComponent = fore[neighbours[j].x, neighbours[j].y];
                if (hexComponent.ObjectType == ForegroundTypes.Empty && Random.Range(0f,1f) < chance)
                {
                    GenerateObstacle(back, fore, neighbours[j], chance - 0.2f);
                }
            }
        }

        private static void GenerateObstacle(HexList4D<HexBackgroundComponent> back, HexList4D<HexForegroundComponent> fore,
            CubeCoords coords, float chance)
        {
            fore.Add(coords, new HexForegroundComponent()
            {
                ObjectType = ForegroundTypes.Obstacle,
                IsNew = false
            });
            FastList<CubeCoords> neighbours = fore.NeighboursOf(coords);
            for (int j = 0; j < neighbours.Count; j++)
            {
                HexForegroundComponent hexComponent = fore[neighbours[j].x, neighbours[j].y];
                if (hexComponent.ObjectType == ForegroundTypes.Empty && Random.Range(0f,1f) < chance)
                {
                    GenerateObstacle(back, fore, neighbours[j], chance - 0.3f);
                }
            }
        }
        
        private static void GenerateSingleObstacle(HexList4D<HexBackgroundComponent> back, HexList4D<HexForegroundComponent> fore,
            CubeCoords coords)
        {
            fore.Add(coords, new HexForegroundComponent()
            {
                ObjectType = ForegroundTypes.Obstacle,
                IsNew = false
            });
        }

        public static void GenerateHex(CubeCoords coords,
            HexList4D<HexBackgroundComponent> back, HexList4D<HexForegroundComponent> fore)
        {
            back.Add(coords.x, coords.y, new HexBackgroundComponent()
            {
                GroundType = BackroundTypes.Boloto,
                IsNew = false
            });
        }
    }
}