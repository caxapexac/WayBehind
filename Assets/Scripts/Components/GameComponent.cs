using System.Collections.Generic;
using Leopotam.Ecs.Ui.Systems;
using LeopotamGroup.Collections;
using LeopotamGroup.Pooling;
using Misc;
using ScriptableObjects;

namespace Components
{   
    sealed class GameComponent
    {
        public EcsUiEmitter UI;
        public SettingsObject S;
        public HexaList3D<HexComponent> Map;
        public FastList<PoolContainer> Pools; //use enum Pool to get access
        public HexaCoords PlayerCoords;
    }
}
