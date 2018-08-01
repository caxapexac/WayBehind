﻿using LeopotamGroup.Collections;
using LeopotamGroup.Math;
using UnityEngine;

namespace Misc
{
    public struct HexaCoords
    {
        /// <summary>
        /// Create coords
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w">0 = background, 1 = foreground</param>
        public HexaCoords(int x, int y, int w = 0)
        {
            X = x;
            Y = y;
            W = w;
        }

        public int X;
        public int Y;
        public int W;

        public int Z
        {
            get { return -X - Y; }
        }
    }

    public class HexaList3D<T> where T : class, new()
    {
        private FastList<FastList<T[]>>[] _map;
        public int Depth;
        public int Radius;

        public HexaList3D(int radius, int depth, int capacity = 64)
        {
            _map = new[]
            {
                new FastList<FastList<T[]>>(capacity), //x>=0 y>=0
                new FastList<FastList<T[]>>(capacity), //x>=0 y<0
                new FastList<FastList<T[]>>(capacity), //x<0 y>=0
                new FastList<FastList<T[]>>(capacity) //x<0 y<0
            };
            Radius = radius;
            Depth = depth;
        }

        public T this[HexaCoords coords]
        {
            get { return this[coords.X, coords.Y, coords.W]; }
            set { this[coords.X, coords.Y, coords.W] = value; }
        }

        public T this[int x, int y, int w]
        {
            get
            {
                T[] layers = Layers(x, y);
                if (layers[w] == null)
                {
                    //Debug.Log(x + " " + y + " " + w + " get");
                    layers[w] = new T();
                }

                return layers[w];
            }
            set
            {
                T[] layers = Layers(x, y);
                if (layers[w] == null)
                {
                    //Debug.Log(x + " " + y + " " + w + " set");
                    layers[w] = new T();
                }

                layers[w] = value;
            }
        }

        public T[] Layers(HexaCoords coords)
        {
            return Layers(coords.X, coords.Y);
        }

        public T[] Layers(int x, int y)
        {
            int d = 0;
            if (x < 0)
            {
                d += 2;
                x = -x - 1;
            }

            if (y < 0)
            {
                d += 1;
                y = -y - 1;
            }

            while (_map[d].Count <= x) _map[d].Add(new FastList<T[]>());
            while (_map[d][x].Count <= y) _map[d][x].Add(new T[Depth]);
            return _map[d][x][y];
        }

        public void ClearAt(HexaCoords coords)
        {
            ClearAt(coords.X, coords.Y, coords.W);
        }

        public void ClearAt(int x, int y, int w)
        {
            //Debug.Log(x + " " + y + " " + w + " clearing");
            this[x, y, w] = new T();
        }

        public bool ExistAt(HexaCoords coords)
        {
            return ExistAt(coords.X, coords.Y, coords.W);
        }

        public bool ExistAt(int x, int y, int w = 0)
        {
            int d = 0;
            if (x < 0)
            {
                d += 2;
                x = -x - 1;
            }

            if (y < 0)
            {
                d += 1;
                y = -y - 1;
            }

            if (_map[d].Count > x && _map[d][x] != null && _map[d][x].Count > y && _map[d][x][y] != null &&
                _map[d][x][y][w] != null)
            {
                return true;
            }

            return false;
        }

        //YAGNI
        public void Fill(int depth = 0)
        {
            for (int i = -Radius; i < Radius; i++)
            {
                for (int j = -Radius; j < Radius; j++)
                {
                    if (MathFast.Abs(HexMath.GetZ(i, j)) <= Radius) this[i, j, depth] = new T();
                }
            }
        }

        public FastList<HexaCoords> NeighboursOf(HexaCoords coords, int radius = 1, int depth = -1)
        {
            if (depth != -1) coords.W = depth;
            return NeighboursOf(coords.X, coords.Y, coords.W, radius);
        }

        public FastList<HexaCoords> NeighboursOf(int x, int y, int w, int radius = 1)
        {
            return NeighboursOf(x, y, x, y, w, radius);
        }

        private FastList<HexaCoords> NeighboursOf(int xo, int yo, int x, int y, int w, int radius = 1)
        {
            if (radius == 0) return new FastList<HexaCoords>() {new HexaCoords(x, y, w)};
            FastList<HexaCoords> neighbours = new FastList<HexaCoords>(6);
            FastList<HexaCoords> nextNeighbours;
            if (ExistAt(x + 1, y, w))
            {
                nextNeighbours = NeighboursOf(xo, yo, x + 1, y, w, radius - 1);
                for (int i = 0; i < nextNeighbours.Count; i++)
                {
                    if (!neighbours.Contains(nextNeighbours[i]) &&
                        (nextNeighbours[i].X != xo || nextNeighbours[i].Y != yo))
                    {
                        neighbours.Add(nextNeighbours[i]);
                    }
                }
            }

            if (ExistAt(x + 1, y - 1, w))
            {
                nextNeighbours = NeighboursOf(xo, yo, x + 1, y - 1, w, radius - 1);
                for (int i = 0; i < nextNeighbours.Count; i++)
                {
                    if (!neighbours.Contains(nextNeighbours[i]) &&
                        (nextNeighbours[i].X != xo || nextNeighbours[i].Y != yo))
                    {
                        neighbours.Add(nextNeighbours[i]);
                    }
                }
            }

            if (ExistAt(x, y + 1, w))
            {
                nextNeighbours = NeighboursOf(xo, yo, x, y + 1, w, radius - 1);
                for (int i = 0; i < nextNeighbours.Count; i++)
                {
                    if (!neighbours.Contains(nextNeighbours[i]) &&
                        (nextNeighbours[i].X != xo || nextNeighbours[i].Y != yo))
                    {
                        neighbours.Add(nextNeighbours[i]);
                    }
                }
            }

            if (ExistAt(x, y - 1, w))
            {
                nextNeighbours = NeighboursOf(xo, yo, x, y - 1, w, radius - 1);
                for (int i = 0; i < nextNeighbours.Count; i++)
                {
                    if (!neighbours.Contains(nextNeighbours[i]) &&
                        (nextNeighbours[i].X != xo || nextNeighbours[i].Y != yo))
                    {
                        neighbours.Add(nextNeighbours[i]);
                    }
                }
            }

            if (ExistAt(x - 1, y + 1, w))
            {
                nextNeighbours = NeighboursOf(xo, yo, x - 1, y + 1, w, radius - 1);
                for (int i = 0; i < nextNeighbours.Count; i++)
                {
                    if (!neighbours.Contains(nextNeighbours[i]) &&
                        (nextNeighbours[i].X != xo || nextNeighbours[i].Y != yo))
                    {
                        neighbours.Add(nextNeighbours[i]);
                    }
                }
            }

            if (ExistAt(x - 1, y, w))
            {
                nextNeighbours = NeighboursOf(xo, yo, x - 1, y, w, radius - 1);
                for (int i = 0; i < nextNeighbours.Count; i++)
                {
                    if (!neighbours.Contains(nextNeighbours[i]) &&
                        (nextNeighbours[i].X != xo || nextNeighbours[i].Y != yo))
                    {
                        neighbours.Add(nextNeighbours[i]);
                    }
                }
            }

            return neighbours;
        }
    }
}