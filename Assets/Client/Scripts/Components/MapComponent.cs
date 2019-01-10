using System.Collections.Generic;
using Client.Scripts.Algorithms;
using Client.Scripts.Miscellaneous;
using UnityEngine;


namespace Client.Scripts.Components
{
    /// <summary>
    /// Хранилище чанков гексагонов с автоматической отложенной процедурной генерацией
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MapComponent<T> where T : class
    {
        public OffsetCoords PlayerPosition;

        private readonly Dictionary<Int2, T[]> _map;

        private readonly int _capacity = 64;

        public readonly int ChunkSize = 4;

        public readonly int ChunkSizeSqr;

        public MapComponent()
        {
            _map = new Dictionary<Int2, T[]>(_capacity);
            ChunkSizeSqr = ChunkSize * ChunkSize;
        }

        /// <summary>
        /// Возвращает гексагон с данных координат
        /// </summary>
        /// <param name="coords"></param>
        public T this[HexCoords coords]
        {
            get
            {
                int index;
                Int2 chunk = HexMath.Offset2Chunk(HexMath.Hexel2Offset(coords), ChunkSize, out index);
                return _map[chunk][index];
            }
            set
            {
                int index;
                Int2 chunk = HexMath.Offset2Chunk(HexMath.Hexel2Offset(coords), ChunkSize, out index);
                _map[chunk][index] = value;
            }
        }

        public T this[OffsetCoords coords]
        {
            get
            {
                int index;
                Int2 chunk = HexMath.Offset2Chunk(coords, ChunkSize, out index);
                return _map[chunk][index];
            }
            set
            {
                int index;
                Int2 chunk = HexMath.Offset2Chunk(coords, ChunkSize, out index);
                _map[chunk][index] = value;
            }
        }

        public T this[Int2 chunk, int index]
        {
            get { return _map[chunk][index]; }
            set { _map[chunk][index] = value; }
        }

        public void AddChunk(Int2 chunk)
        {
            _map.Add(chunk, new T[ChunkSizeSqr]);
        }

        public bool IsExistChunk(HexCoords coords)
        {
            return _map.ContainsKey(HexMath.Offset2Chunk(HexMath.Hexel2Offset(coords), ChunkSize));
        }

        public bool IsExistChunk(OffsetCoords coords)
        {
            return _map.ContainsKey(HexMath.Offset2Chunk(coords, ChunkSize));
        }

        public bool IsExistChunk(Int2 chunk)
        {
            return _map.ContainsKey(chunk);
        }
    }
}