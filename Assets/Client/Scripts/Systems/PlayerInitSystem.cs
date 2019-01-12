using Client.Scripts.Algorithms;
using Client.Scripts.Components;
using Client.Scripts.Miscellaneous;
using Client.Scripts.MonoBehaviours;
using Client.Scripts.Scriptable;
using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.Systems
{
    [EcsInject]
    public class PlayerInitSystem : IEcsInitSystem
    {
        private EcsWorld _world = null;
        private SettingsObject _settings = null;
        private Variables _variables = null;
        private EcsFilter<MapComponent<HexComponent>> _map = null;

        public void Initialize()
        {
            PlayerComponent player = _world.CreateEntityWith<PlayerComponent>();
            MonoPlayer mono = Object.Instantiate(Resources.Load<MonoPlayer>(Prefabs.Player),
                HexMath.Offset2Pixel(_map.Components1[0].PlayerPosition, _variables.HexSize), Quaternion.identity);
            player.Parent = mono;
            player.Hp = _settings.StartHp;
        }

        public void Destroy()
        { }
    }
}