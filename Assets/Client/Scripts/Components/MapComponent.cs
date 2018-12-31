using System.Collections.Generic;
using Client.Scripts.Algorithms;
using Client.Scripts.Miscellaneous;


namespace Client.Scripts.Components
{
    /// <summary>
    /// Хранилище гексагонов, расширяется во все стороны, имеет заданую глубину и размер чанка
    /// Граница мира = -2147483647..2147483647 чанков во все стороны
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MapComponent<T> where T : class, new()
    {
        private readonly Dictionary<Int2, T[]> _map;

        public readonly int ChunkSize = 16;

        private const int Capacity = 64;

        public MapComponent()
        {
            _map = new Dictionary<Int2, T[]>(Capacity);
        }

        /// <summary>
        /// Возвращает гексагон с данных координат
        /// </summary>
        /// <param name="coords"></param>
        public T this[HexaCoords coords]
        {
            get
            {
                int index;
                Int2 chunk = HexMath.HexToChunk(ChunkSize, coords, out index);
                return this[chunk, index];
            }
            set
            {
                int index;
                Int2 chunk = HexMath.HexToChunk(ChunkSize, coords, out index);
                this[chunk, index] = value;
            }
        }

        public T this[Int2 chunk, int index]
        {
            get { return _map[chunk][index]; }
            set { _map[chunk][index] = value; }
        }

        public void Add(Int2 pos)
        {
            _map.Add(pos, new T[ChunkSize]);
        }

        /// <summary>
        /// Заменяет гексагон в коорднинатах на дефолтный
        /// </summary>
        /// <param name="coords"></param>
        public void ClearAt(HexaCoords coords)
        {
            this[coords] = new T();
        }

        /// <summary>
        /// Проверяет на null ячейку по данным координатам
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public bool IsExistChunk(HexaCoords coords)
        {
            int cx, cy, index;
            return IsExistChunk(HexMath.HexToChunk(ChunkSize, coords));
        }

        public bool IsExistChunk(Int2 coords)
        {
            return _map.ContainsKey(coords);
        }
    }
}