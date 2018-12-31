using Client.ScriptableObjects;
using Client.Scripts.OBSOLETE.Map;
using Client.Scripts.OBSOLETE.Misc;
using Leopotam.Ecs.Ui.Systems;
using LeopotamGroup.Collections;
using LeopotamGroup.Pooling;

namespace Client.Scripts.OBSOLETE.Components
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
