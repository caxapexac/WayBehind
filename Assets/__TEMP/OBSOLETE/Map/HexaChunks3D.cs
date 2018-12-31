using System.Collections.Generic;
using Client.Scripts.OBSOLETE.Misc;
using LeopotamGroup.Collections;
using LeopotamGroup.Math;

namespace Client.Scripts.OBSOLETE.Map
{
    /// <summary>
    /// Хранилище гексагонов, расширяется во все стороны, имеет заданую глубину и размер чанка
    /// Граница мира = -2147483647..2147483647 чанков во все стороны
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HexaChunks3D<T> where T : class, new()
    {
        private Dictionary<int, FastList<T>> _map;
        public readonly int Depth;
        public int Radius;
        public readonly int ChunkSize;

        public HexaChunks3D(int radius, int depth, int chunkSize = 16, int capacity = 64)
        {
            _map = new Dictionary<int, FastList<T>>();
            Radius = radius;
            Depth = depth;
            ChunkSize = chunkSize;
        }

        /// <summary>
        /// Возвращает гексагон с данных координат с определенной глубины
        /// </summary>
        /// <param name="coords"></param>
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
                    layers[w] = new T();
                }

                return layers[w];
            }
            set
            {
                T[] layers = Layers(x, y);
                if (layers[w] == null)
                {
                    layers[w] = new T();
                }

                layers[w] = value;
            }
        }

        /// <summary>
        /// Возвращает все гексагоны с данных координат
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
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

            //while (_map[d].Count <= x) _map[d].Add(new FastList<T[]>());
            //while (_map[d][x].Count <= y) _map[d][x].Add(new T[Depth]);
            //return _map[d][x][y];]
            return null;
        }

        /// <summary>
        /// Заменяет гексагон в коорднинатах на дефолтный
        /// </summary>
        /// <param name="coords"></param>
        public void ClearAt(HexaCoords coords)
        {
            ClearAt(coords.X, coords.Y, coords.W);
        }

        public void ClearAt(int x, int y, int w)
        {
            //Debug.Log(x + " " + y + " " + w + " clearing");
            this[x, y, w] = new T();
        }

        /// <summary>
        /// Проверяет на null ячейку по данным координатам
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
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
            /*
            if (_map[d].Count > x && _map[d][x] != null && _map[d][x].Count > y && _map[d][x][y] != null &&
                _map[d][x][y][w] != null)
            {
                return true;
            }
            */

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

        /// <summary>
        /// Возвращает координаты соседей гексагона в заданном радиусе
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="radius"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public FastList<HexaCoords> NeighboursOf(HexaCoords coords, int radius = 1, int depth = -1)
        {
            if (depth != -1) coords.W = depth;
            return NeighboursOf(coords.X, coords.Y, coords.W, radius);
        }

        public FastList<HexaCoords> NeighboursOf(int x, int y, int w, int radius = 1)
        {
            return NextNeighbours(x, y, x, y, w, radius);
        }

        private FastList<HexaCoords> NextNeighbours(int xo, int yo, int x, int y, int w, int radius = 1)
        {
            if (radius == 0) return new FastList<HexaCoords>() {new HexaCoords(x, y, w)};
            FastList<HexaCoords> neighbours = new FastList<HexaCoords>(6);
            FastList<HexaCoords> nextNeighbours;
            if (ExistAt(x + 1, y, w))
            {
                nextNeighbours = NextNeighbours(xo, yo, x + 1, y, w, radius - 1);
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
                nextNeighbours = NextNeighbours(xo, yo, x + 1, y - 1, w, radius - 1);
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
                nextNeighbours = NextNeighbours(xo, yo, x, y + 1, w, radius - 1);
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
                nextNeighbours = NextNeighbours(xo, yo, x, y - 1, w, radius - 1);
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
                nextNeighbours = NextNeighbours(xo, yo, x - 1, y + 1, w, radius - 1);
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
                nextNeighbours = NextNeighbours(xo, yo, x - 1, y, w, radius - 1);
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

        public long Hash(int x, int y)
        {
            
            return (long)y + (((long)x) << 32);
        }
    }
}