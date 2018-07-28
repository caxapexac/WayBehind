using System;
using LeopotamGroup.Collections;
using LeopotamGroup.Math;
using UnityEditor;
using UnityEngine;

namespace Misc
{
    public class HexList4D<T> where T : class, new()
    {
        private FastList<FastList<T>> PP; //x>=0 y>=0
        private FastList<FastList<T>> PN; //x>=0 y<0
        private FastList<FastList<T>> NP; //x<0 y>=0
        private FastList<FastList<T>> NN; //x<0 y<0

        public HexList4D()
        {
            PP = new FastList<FastList<T>>();
            PN = new FastList<FastList<T>>();
            NP = new FastList<FastList<T>>();
            NN = new FastList<FastList<T>>();
        }

        public HexList4D(int capacity)
        {
            PP = new FastList<FastList<T>>(capacity);
            PN = new FastList<FastList<T>>(capacity);
            NP = new FastList<FastList<T>>(capacity);
            NN = new FastList<FastList<T>>(capacity);
        }

        public T this[int x, int y]
        {
            get
            {
                if (x >= 0)
                {
                    if (y >= 0)
                    {
                        if (PP.Count > x && PP[x].Count > y)
                        {
                            return PP[x][y];
                        }
                    }
                    else
                    {
                        y = -y - 1;
                        if (PN.Count > x && PN[x].Count > y)
                        {
                            return PN[x][y];
                        }
                    }
                }
                else
                {
                    x = -x - 1;
                    if (y >= 0)
                    {
                        if (NP.Count > x && NP[x].Count > y)
                        {
                            return NP[x][y];
                        }
                    }
                    else
                    {
                        y = -y - 1;
                        if (NN.Count > x && NN[x].Count > y)
                        {
                            return NN[x][y];
                        }
                    }
                }

                throw new IndexOutOfRangeException();
            }
            set
            {
                if (x >= 0)
                {
                    if (y >= 0)
                    {
                        if (PP.Count > x && PP[x].Count > y)
                        {
                            PP[x][y] = value;
                        }
                        else
                        {
                            throw new IndexOutOfRangeException();
                        }
                    }
                    else
                    {
                        y = -y - 1;
                        if (PN.Count > x && PN[x].Count > y)
                        {
                            PN[x][y] = value;
                        }
                        else
                        {
                            throw new IndexOutOfRangeException();
                        }
                    }
                }
                else
                {
                    x = -x - 1;
                    if (y >= 0)
                    {
                        if (NP.Count > x && NP[x].Count > y)
                        {
                            NP[x][y] = value;
                        }
                        else
                        {
                            throw new IndexOutOfRangeException();
                        }
                    }
                    else
                    {
                        y = -y - 1;
                        if (NN.Count > x && NN[x].Count > y)
                        {
                            NN[x][y] = value;
                        }
                        else
                        {
                            throw new IndexOutOfRangeException();
                        }
                    }
                }
            }
        }

        public int CapacityMin()
        {
            return Mathf.Min(PP.Capacity, PN.Capacity, NP.Capacity, NN.Capacity);
        }

        //todo hexagonal
        public void Fill(int radius)
        {
            for (int i = -radius; i < radius; i++)
            {
                for (int j = -radius; j < radius; j++)
                {
                    if (MathFast.Abs(HexMath.GetZ(i, j)) <= radius)
                    {
                        Add(i, j, new T());
                    }
                }
            }
        }

        public void Add(int x, int y, T item)
        {
            if (x >= 0)
            {
                if (y >= 0)
                {
                    while (PP.Count <= x)
                    {
                        PP.Add(new FastList<T>());
                    }

                    while (PP[x].Count <= y)
                    {
                        PP[x].Add(new T());
                    }

                    PP[x][y] = item;
                }
                else
                {
                    y = -y - 1;
                    while (PN.Count <= x)
                    {
                        PN.Add(new FastList<T>());
                    }

                    while (PN[x].Count <= y)
                    {
                        PN[x].Add(new T());
                    }

                    PN[x][y] = item;
                }
            }
            else
            {
                x = -x - 1;
                if (y >= 0)
                {
                    while (NP.Count <= x)
                    {
                        NP.Add(new FastList<T>());
                    }

                    while (NP[x].Count <= y)
                    {
                        NP[x].Add(new T());
                    }

                    NP[x][y] = item;
                }
                else
                {
                    y = -y - 1;
                    while (NN.Count <= x)
                    {
                        NN.Add(new FastList<T>());
                    }

                    while (NN[x].Count <= y)
                    {
                        NN[x].Add(new T());
                    }

                    NN[x][y] = item;
                }
            }
        }
        public void Add(CubeCoords coords, T item)
        {
            Add(coords.x, coords.y, item);
        }
        
        public void ClearAt(int x, int y)
        {
            try
            {
                this[x, y] = new T();
            }
            catch (Exception e)
            {
                return;
            }
        }
        public void ClearAt(CubeCoords coords)
        {
            ClearAt(coords.x, coords.y);
        }

        public bool ExistAt(int x, int y)
        {
            if (x >= 0)
            {
                if (y >= 0)
                {
                    if (PP.Count > x && PP[x].Count > y)
                    {
                        return true;
                    }

                    return false;
                }
                else
                {
                    y = -y - 1;
                    if (PN.Count > x && PN[x].Count > y)
                    {
                        return true;
                    }

                    return false;
                }
            }
            else
            {
                x = -x - 1;
                if (y >= 0)
                {
                    if (NP.Count > x && NP[x].Count > y)
                    {
                        return true;
                    }

                    return false;
                }
                else
                {
                    y = -y - 1;
                    if (NN.Count > x && NN[x].Count > y)
                    {
                        return true;
                    }

                    return false;
                }
            }
        }
        public bool ExistAt(CubeCoords coords)
        {
            return ExistAt(coords.x, coords.y);
        }

        public FastList<CubeCoords> NeighboursOf(int x, int y)
        {
            int z = -x - y;
            FastList<CubeCoords> neighbours = new FastList<CubeCoords>(6);
            if (ExistAt(x + 1, y)) neighbours.Add(new CubeCoords(x + 1, y)); //this[x + 1, y]);
            if (ExistAt(x + 1, y - 1)) neighbours.Add(new CubeCoords(x + 1, y - 1)); //this[x + 1, y - 1]);
            if (ExistAt(x, y + 1)) neighbours.Add(new CubeCoords(x, y + 1)); //this[x, y + 1]);
            if (ExistAt(x, y - 1)) neighbours.Add(new CubeCoords(x, y - 1)); //this[x, y - 1]);
            if (ExistAt(x - 1, y + 1)) neighbours.Add(new CubeCoords(x - 1, y + 1)); //this[x - 1, y + 1]);
            if (ExistAt(x - 1, y)) neighbours.Add(new CubeCoords(x - 1, y)); //this[x - 1, y]);
            return neighbours;
        }
        public FastList<CubeCoords> NeighboursOf(CubeCoords coords)
        {
            return NeighboursOf(coords.x, coords.y);
        }
    }
}