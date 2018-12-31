using LeopotamGroup.Collections;

namespace Client.Scripts.OBSOLETE.Map
{
    public class HexMap<T> where T : class, new()
    {
        private class HexChunk
        {
            public T Zero;
            public T[][][] Directions;
            public HexChunk(int size, int depth)
            {
                Directions = new T[6][][];
                int length = (1 + size) * size / 2;
                for (int i = 0; i < 6; i++) Directions[i] = new T[length][];
            }
        }

        public int ChunkSize;
        public FastList<FastList<T>> Chunks;

        public HexMap(int chunkSize, int depth)
        {
            //todo 
        }
    }
}