using Client.Scripts.Algorithms.Legacy;
using Client.Scripts.Components;
using Client.Scripts.Miscellaneous;
using Client.Scripts.MonoBehaviours;
using Client.Scripts.Scriptable;
using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.Systems
{
    [EcsInject]
    public class WorldInitSystem : IEcsInitSystem
    {
        private EcsWorld _world = null;
        private SettingsObject _settings = null;
        private MapNoiseObject _noiseSettings = null;

        public void Initialize()
        {
            MapComponent<HexComponent> map = _world.CreateEntityWith<MapComponent<HexComponent>>();
            map.PlayerPosition = new OffsetCoords(0, 0);
            PoolsComponent pools = _world.CreateEntityWith<PoolsComponent>();
            pools.HexParent = Object.Instantiate(Resources.Load<GameObject>(Prefabs.Empty)).transform;
            pools.HexParent.name = "MAP";
            pools.SpiritPool = new PrefabPool<MonoSpirit>(Prefabs.Spirit);
            pools.HexPool = new PrefabPool<MonoHex>(Prefabs.Hex);
        }

        public void Destroy()
        { }
    }
}