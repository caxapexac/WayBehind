using Client.Scripts.Algorithms;
using Client.Scripts.Components;
using Client.Scripts.Miscellaneous;
using Client.Scripts.Scriptable;
using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using UnityEngine;
using UnityEngine.UI;


namespace Client.Scripts.Systems
{
    [EcsInject]
    public class DebugUiSystem : IEcsInitSystem, IEcsRunSystem
    {
        private Text _playerPosVector;
        private Text _playerPosChunk;
        private Text _playerPosOffset;
        private Text _playerPosHexel;
        private Text _loadedChunks;
        private MapComponent<HexComponent> _map;
        
        private EcsWorld _world = null;
        private EcsFilter<MapComponent<HexComponent>> _mapFilter = null;
        private EcsFilter<PoolsComponent> _pools = null;
        private EcsFilter<PlayerComponent> _player = null;
        private SettingsObject _settings = null;
        private MapNoiseObject _noiseSettings = null;
        private EcsUiEmitter _uiEmitter = null;
        private Variables _variables = null;
        
        
        
        public void Initialize()
        {
            _playerPosVector = _uiEmitter.GetNamedObject(Names.PlayerPosVector).GetComponent<Text>();
            _playerPosChunk = _uiEmitter.GetNamedObject(Names.PlayerPosChunk).GetComponent<Text>();
            _playerPosOffset = _uiEmitter.GetNamedObject(Names.PlayerPosOffset).GetComponent<Text>();
            _playerPosHexel = _uiEmitter.GetNamedObject(Names.PlayerPosHexel).GetComponent<Text>();
            _loadedChunks = _uiEmitter.GetNamedObject(Names.LoadedChunks).GetComponent<Text>();
            _map = _mapFilter.Components1[0];
        }
        
        public void Run()
        {
            for (int i = 0; i < _player.EntitiesCount; i++)
            {
                Vector2 pos = _player.Components1[i].Parent.transform.position;
                OffsetCoords offset = HexMath.Pixel2Offset(pos, _variables.HexSize);
                Int2 chunk = HexMath.Offset2Chunk(offset, _map.ChunkSize);
                HexCoords hex = HexMath.Pixel2Hexel(pos, _variables.HexSize);
                _playerPosVector.text = pos.ToString();
                _playerPosChunk.text = chunk.ToString();
                _playerPosOffset.text = offset.ToString();
                _playerPosHexel.text = hex.ToString();
            }
            _loadedChunks.text = "Chunks loaded " + _variables.DebugChunksCount;
        }

        public void Destroy()
        {
            _playerPosVector = null;
            _playerPosChunk = null;
            _playerPosOffset = null;
            _playerPosHexel = null;
            _map = null;
        }

        
    }
}