using Client.Scripts.Algorithms;
using Client.Scripts.Algorithms.Legacy;
using Client.Scripts.Algorithms.Noises;
using Client.Scripts.Components;
using Client.Scripts.Miscellaneous;
using Client.Scripts.MonoBehaviours;
using Client.Scripts.Scriptable;
using Leopotam.Ecs;
using LeopotamGroup.Collections;
using UnityEngine;


namespace Client.Scripts.Systems
{
    [EcsInject]
    public class WorldRenderSystem : IEcsInitSystem, IEcsRunSystem
    {
        private delegate void NoiseDelegate(HexComponent hex, MapNoiseObject noise);


        private readonly NoiseDelegate _noiseFunction = PerlinPointNoise.SetupHex;


        private FastList<Int2> _loadedChunks;

        private MapComponent<HexComponent> _map;

        private OffsetCoords _lastPos;

        private EcsWorld _world = null;
        private EcsFilter<MapComponent<HexComponent>> _mapFilter = null;
        private EcsFilter<PoolsComponent> _pools = null;
        private EcsFilter<PlayerComponent> _player = null;
        private SettingsObject _settings = null;
        private MapNoiseObject _noiseSettings = null;
        private Variables _variables = null;

        public void Initialize()
        {
            _loadedChunks = new FastList<Int2>();
            _map = _mapFilter.Components1[0];
            _lastPos = new OffsetCoords(int.MaxValue, int.MaxValue);
        }

        public void Run()
        {
            _variables.DebugChunksCount = _loadedChunks.Count; //todo
            
            for (int i = 0; i < _player.EntitiesCount; i++)
            {
                OffsetCoords playerPos = _map.PlayerPosition;

                if (_lastPos.X == playerPos.X && _lastPos.Y == playerPos.Y) break;
                _lastPos = playerPos;
                RenderAll(HexMath.Offset2Chunk(_lastPos, _map.ChunkSize));
                break; //Only the first player
            }
            if (_loadedChunks.Count > 1 && !IsVisible(_loadedChunks[0]))
            {
                UnrenderChunk(_loadedChunks[0]);
            }
            if (_loadedChunks.Count > 200) //GC
            {
                while (!IsVisible(_loadedChunks[0]))
                {
                    UnrenderChunk(_loadedChunks[0]);
                }
            }
            if (_settings.RealTime)
            {
                for (int i = 0; i < _loadedChunks.Count; i++)
                {
                    for (int k = 0; k < _map.ChunkSizeSqr; k++)
                    {
                        HexComponent hex = _map[_loadedChunks[i], k];
                        _noiseFunction(hex, _noiseSettings);
                        hex.Parent.Draw();
                    }
                }
            }
        }

        private void RenderAll(Int2 playerPos)
        {
            int x = 0;
            int y = 0;
            int dx = 0;
            int dy = -1;
            int circle = 0;
            while (true)
            {
                Int2 ch = new Int2(playerPos.X + x, playerPos.Y + y);
                EnsureChunk(ch);
                if (IsVisible(ch))
                {
                    RenderChunk(ch);
                }
                else
                {
                    /*Debug.DrawLine(
                        HexMath.Offset2Pixel(HexMath.Chunk2Offset(ch, _map.ChunkSize, 0), _variables.HexSize),
                        HexMath.Offset2Pixel(_map.PlayerPosition, _variables.HexSize), Color.yellow, 5);*/
                    if (_loadedChunks.Contains(ch))
                    {
                        UnrenderChunk(ch);
                    }
                    else if ((circle - 2) % 4 == 0)
                    {
                        break;
                    }
                    
                }
                if (x == y || (x < 0 && x == -y) || (x > 0 && x == 1 - y))
                {
                    int temp = dx;
                    dx = -dy;
                    dy = temp;
                    circle++;
                }
                x += dx;
                y += dy;
            }
        }

        public void EnsureChunk(Int2 chunk)
        {
            if (!_map.IsExistChunk(chunk))
            {
                //Debug.Log(chunk.X + " " + chunk.Y + " Rendered");
                _map.AddChunk(chunk);
                for (int i = 0; i < _map.ChunkSizeSqr; i++)
                {
                    OffsetCoords coords = HexMath.Chunk2Offset(chunk, _map.ChunkSize, i);
                    HexComponent hex = new HexComponent(HexMath.Offset2Pixel(coords, _variables.HexSize));
                    _noiseFunction(hex, _noiseSettings);
                    _map[chunk, i] = hex;
                }
            }
        }

        private void RenderChunk(Int2 chunk)
        {
            if (!_loadedChunks.Contains(chunk))
            {
                PrefabPool<MonoHex> pool = _pools.Components1[0].HexPool;
                for (int i = 0; i < _map.ChunkSizeSqr; i++)
                {
                    MonoHex mono = pool.Get();
                    mono.Hex = _map[chunk, i];
                    mono.transform.parent = _pools.Components1[0].HexParent;
                    mono.gameObject.SetActive(true);
                }
                _loadedChunks.Add(chunk);
            }
        }

        private bool IsVisible(Int2 chunk)
        {
            Vector2 first = _map[chunk, 0].Position;
            Vector2 last = _map[chunk, _map.ChunkSizeSqr - 1].Position;
            //Vector2 pos = new Vector2((first.x + last.x) / 2, (first.y + last.y) / 2);
            //Debug.DrawLine(first, last, Color.gray, 5);
            //Debug.DrawLine(new Vector2(_variables.CameraMinBound.x, _variables.CameraMinBound.y), new Vector2(_variables.CameraMaxBound.x, _variables.CameraMaxBound.y), Color.green, 5);
            return first.x > _variables.CameraMinBound.x
                && first.x < _variables.CameraMaxBound.x
                && first.y > _variables.CameraMinBound.y
                && first.y < _variables.CameraMaxBound.y
                || last.x > _variables.CameraMinBound.x
                && last.x < _variables.CameraMaxBound.x
                && last.y > _variables.CameraMinBound.y
                && last.y < _variables.CameraMaxBound.y
                || first.x > _variables.CameraMinBound.x
                && first.x < _variables.CameraMaxBound.x
                && last.y > _variables.CameraMinBound.y
                && last.y < _variables.CameraMaxBound.y
                || last.x > _variables.CameraMinBound.x
                && last.x < _variables.CameraMaxBound.x
                && first.y > _variables.CameraMinBound.y
                && first.y < _variables.CameraMaxBound.y;
        }

        private void UnrenderChunk(Int2 chunk)
        {
            PrefabPool<MonoHex> pool = _pools.Components1[0].HexPool;
            for (int i = 0; i < _map.ChunkSizeSqr; i++)
            {
                MonoHex mono = _map[chunk, i].Parent;
                mono.gameObject.SetActive(false);
                pool.Recycle(mono);
            }
            _loadedChunks.Remove(chunk);
        }

        public void Destroy()
        { }
    }
}