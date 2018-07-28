using LeopotamGroup.Pooling;
using Misc;

namespace Components
{
    sealed class MapComponent
    {
        public HexList4D<HexBackgroundComponent> MapB;
        public HexList4D<HexForegroundComponent> MapF;
        public PoolContainer PoolB;
        public PoolContainer PoolF;
        public CubeCoords LastCoords;
        public CubeCoords PlayerCoords;
        
    }
}
