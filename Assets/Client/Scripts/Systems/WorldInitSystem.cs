using Client.ScriptableObjects;
using Client.Scripts.Components;
using Leopotam.Ecs;


namespace Client.Scripts.Systems
{
    [EcsInject]
    public class WorldInitSystem : IEcsInitSystem
    {
        private EcsWorld _world = null;
        private SettingsObject _settings = null;

        public void Initialize()
        {
            MapComponent<HexComponent> map = _world.CreateEntityWith<MapComponent<HexComponent>>();


            /*PoolsComponent pools = _world.CreateEntityWith<PoolsComponent>();
            pools.EnemyPool = PoolContainer.CreatePool<>();
            pools.HexPool = PoolContainer.CreatePool<>();
            _game.HexPool = new FastList<PoolContainer>(Prefabs.Count)

            //_game.Map = MapGenRandomNeighbours.GenerateMap(_game.S.UseSeed ? _game.S.MapSaeed : 0, _game.S.MapSize, 2);
            //_game.Map = MapGenSquareFractalNoise.GenerateMap(_game.S.UseSeed ? _game.S.MapSaeed : 0, _game.S.Octaves, _game.S.MapSize, 2, _game.S.HexSize);
            //_game.Map = MapGenHexCloudNoise.GenerateMap(_game.S.UseSeed ? _game.S.MapSaeed : 0, _game.S.Octaves, _game.S.MapSize, 2, _game.S.HexSize);
            _game.Map = MapGenHexPerlinNoise.GenerateMap(_game.S.UseSeed ? _game.S.MapSaeed : 0, _game.S.Octaves,
                _game.S.MapSize, _game.S.Smoothing, _game.S.Persistance, 2, _game.S.EnemyCount, _game.S.UseTextures,
                _game.S.HexSize);
            _player = _world.CreateEntityWith<PlayerComponent>();
            _player.Transform = GameObject.Instantiate(_game.S.PlayerPrefab).transform;
            _player.Hp = _game.S.Hp;
            _lastCoords = new HexaCoords(0, 0);
            RenderFull(new HexaCoords(0, 0), _game.S.FieldOfView);*/
        }


        public void Destroy()
        { }
    }
}