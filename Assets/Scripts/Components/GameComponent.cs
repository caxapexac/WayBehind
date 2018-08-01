using System.Collections.Generic;
using LeopotamGroup.Collections;
using LeopotamGroup.Pooling;
using Misc;

namespace Components
{   
    sealed class GameComponent
    {
        public GameComponent()
        {
            LastCoords = new HexaCoords(0, 0, 0);
        }
        public HexaList3D<HexComponent> Map;
        public FastList<PoolContainer> Pools; //use enum Pool to get access
        public HexaCoords LastCoords;
        public HexaCoords PlayerCoords;
    }
}
