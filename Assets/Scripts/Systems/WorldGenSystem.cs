using System;
using Components;
using Events;
using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using LeopotamGroup.Collections;
using LeopotamGroup.Pooling;
using Misc;
using ScriptableObjects;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class WorldGenSystem : IEcsInitSystem, IEcsRunSystem
    {
        private HexaCoords _lastCoords;
        private SettingsObject _settings;
        private EcsUiEmitter _uiEmitter;
        private GameComponent _game;
        private PlayerComponent _player;
        private EcsWorld _world = null;
        private EcsFilter<HexDisposeEvent> _hexDisposeFilter = null;

        public WorldGenSystem(SettingsObject settings, EcsUiEmitter uiEmitter)
        {
            _settings = settings;
            _uiEmitter = uiEmitter;
        }

        public void Initialize()
        {
            _game = _world.CreateEntityWith<GameComponent>();
            _game.S = _settings;
            _game.UI = _uiEmitter;
            _game.Pools = new FastList<PoolContainer>(Prefabs.Count)
            {
                PoolContainer.CreatePool(Prefabs.Grass),
                PoolContainer.CreatePool(Prefabs.Water),
                PoolContainer.CreatePool(Prefabs.Forest),
                PoolContainer.CreatePool(Prefabs.Swamp),
                PoolContainer.CreatePool(Prefabs.Obstacle),
                PoolContainer.CreatePool(Prefabs.Diamond),
                PoolContainer.CreatePool(Prefabs.Enemy),
            };
            _game.Map = MapGenRandomNeighbours.GenerateMap(_game.S.UseSeed ? _game.S.MapSaeed : 0, _game.S.MapSize, 2);
            _player = _world.CreateEntityWith<PlayerComponent>();
            _player.Transform = GameObject.Instantiate(_game.S.PlayerPrefab).transform;
            _player.Hp = _game.S.Hp;
            _lastCoords = new HexaCoords(0, 0);
            RenderFull(new HexaCoords(0, 0), _game.S.FieldOfView);
        }

        public void Run()
        {
            _game.PlayerCoords = HexMath.Pixel2Hexel(_player.Transform.localPosition, _game.S.HexSize);
            if (_lastCoords.X != _game.PlayerCoords.X || _lastCoords.Y != _game.PlayerCoords.Y)
            {
                UnrenderRing(_lastCoords, _game.S.FieldOfView + 1);
                RenderRing(_game.PlayerCoords, _game.S.FieldOfView);
                _lastCoords = _game.PlayerCoords;
            }

            for (int i = 0; i < _hexDisposeFilter.EntitiesCount; i++)
            {
                RemoveHex(_hexDisposeFilter.Components1[i].Coords);
                _world.RemoveEntity(_hexDisposeFilter.Entities[i]);
            }
        }

        private void RenderFull(HexaCoords coords, int radius)
        {
            RenderHex(coords);
            for (int i = 1; i <= radius; i++)
            {
                RenderRing(coords, i);
            }

            //   Работай без багов, аминь
            //             ______
            //            / ____ \   
            //           | / || \ |
            //           | | || | |
            //       ____| | || | |____         
            //      / _____| || |_____ \
            //     / /__B__U_||_G__S__\ \                      
            //     \ \_____  ||  _____/ /                  
            //      \____  | || |  ____/                    
            //           | | || | |                     
            //           | | || | |                     
            //           | | || | |                     
            //           | | || | |          
            //    \ \    | | || | |    |/      
            //   =========================        
            //   =========================          
        }

        private void RenderRing(HexaCoords coords, int radius)
        {
            coords.Y += radius;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    coords.X += HexMath.Directions[i, 0];
                    coords.Y += HexMath.Directions[i, 1];
                    RenderHex(coords);
                }
            }
        }

        private void UnrenderRing(HexaCoords playerCoords, int radius)
        {
            HexaCoords coords = new HexaCoords(playerCoords.X, playerCoords.Y + radius);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    coords.X += HexMath.Directions[i, 0];
                    coords.Y += HexMath.Directions[i, 1];
                    if (HexMath.HexDistance(playerCoords, coords) >= _game.S.FieldOfView)
                    {
                        HideHex(coords);
                    }
                }
            }
        }

        private void RenderHex(HexaCoords coords)
        {
            if (!_game.Map.ExistAt(coords))
            {
                MapGenRandomNeighbours.GenerateHex(_game.Map, coords);
            }

            HexComponent[] layers = _game.Map.Layers(coords);
            for (int i = 0; i < layers.Length; i++)
            {
                coords.W = i;
                if (layers[i] != null) RenderLayer(coords, layers[i]);
            }
        }

        private void RenderLayer(HexaCoords coords, HexComponent hex)
        {
            if (hex.Parent != null) return;
            switch (hex.HexType)
            {
                case HexTypes.Grass:
                    hex.Parent = _game.Pools[(int) Pool.Grass].Get();
                    //speed 1
                    break;
                case HexTypes.Water:
                    hex.Parent = _game.Pools[(int) Pool.Water].Get();
                    //speed 0.1f
                    break;
                case HexTypes.Forest:
                    hex.Parent = _game.Pools[(int) Pool.Forest].Get();
                    //speed 0.5f
                    break;
                case HexTypes.Swamp:
                    hex.Parent = _game.Pools[(int) Pool.Swamp].Get();
                    //speed 0.02f
                    break;
                case HexTypes.Obstacle:
                    hex.Parent = _game.Pools[(int) Pool.Obstacle].Get();
                    //todo value
                    break;
                case HexTypes.Diamond:
                    hex.Parent = _game.Pools[(int) Pool.Diamond].Get();
                    //todo value
                    break;
                case HexTypes.Enemy:
                    if (hex.Parent != null) throw new Exception();
                    hex.Parent = _game.Pools[(int) Pool.Enemy].Get();
                    hex.Parent.PoolTransform.GetComponentInChildren<SpriteRenderer>().sprite = _game.S.Enemies[0];//todo
                    _game.Map[coords] = new HexComponent() { HexType = HexTypes.Empty};
                    EnemyComponent enemy = _world.CreateEntityWith<EnemyComponent>();
                    enemy.Hex = hex;
                    enemy.LastCoords = coords;
                    enemy.Head = hex.Parent.PoolTransform;
                    enemy.Body = hex.Parent.PoolTransform.GetChild(0);
                    enemy.Target = HexMath.Hexel2Pixel(coords, _game.S.HexSize);
                    break;
                case HexTypes.Empty:
                    //Debug.Log(coords.X + " " + coords.Y + " " + coords.W + " empty");
                    return;
                case HexTypes.Spawn: //?
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            hex.Parent.PoolTransform.localPosition = HexMath.Hexel2Pixel(coords, _game.S.HexSize);
            hex.Parent.PoolTransform.gameObject.SetActive(true);
        }

        private void HideHex(HexaCoords coords)
        {
            HexComponent[] layers = _game.Map.Layers(coords);
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i] != null) HideLayer(coords, layers[i]);
            }
        }

        private void HideLayer(HexaCoords coords, HexComponent hex)
        {
            if (hex.Parent == null || hex.HexType == HexTypes.Enemy) return;
            _game.Pools[(int) hex.HexType].Recycle(hex.Parent);
            hex.Parent = null;
        }

        private void RemoveHex(HexaCoords coords)
        {
            //todo fading coroutines
            HexComponent hex = _game.Map[coords];
            _game.Pools[(int) hex.HexType].Recycle(hex.Parent);
            hex.Parent = null;
            _game.Map.ClearAt(coords);
        }

        public void Destroy()
        {
            _game = null;
        }
    }
}