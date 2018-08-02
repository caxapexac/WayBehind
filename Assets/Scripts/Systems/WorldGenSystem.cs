using System;
using Components;
using Events;
using Leopotam.Ecs;
using LeopotamGroup.Collections;
using LeopotamGroup.Pooling;
using Misc;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class WorldGenSystem : IEcsInitSystem, IEcsRunSystem
    {
        public float HexSize;
        public int MapSize;
        public int MapSaeed;
        public int Fow;
        public GameObject PlayerPrefab;
        public Sprite[] Enemies;

        private GameComponent _game;
        private PlayerComponent _player;
        private EcsWorld _world = null;
        private EcsFilter<TriggerEvent> _triggerEvents = null;

        public void Initialize()
        {
            _game = _world.CreateEntityWith<GameComponent>();
            _game.Pools = new FastList<PoolContainer>()
            {
                PoolContainer.CreatePool(Prefabs.Grass),
                PoolContainer.CreatePool(Prefabs.Water),
                PoolContainer.CreatePool(Prefabs.Forest),
                PoolContainer.CreatePool(Prefabs.Swamp),
                PoolContainer.CreatePool(Prefabs.Obstacle),
                PoolContainer.CreatePool(Prefabs.Diamond),
                PoolContainer.CreatePool(Prefabs.Enemy),
            };
            MapGenRandomNeighbours.GenerateMap(MapSaeed, MapSize, 2, out _game.Map);
            _player = _world.CreateEntityWith<PlayerComponent>();
            _player.Transform = GameObject.Instantiate(PlayerPrefab).transform;
            RenderFull(new HexaCoords(0, 0, 0), Fow);
        }

        public void Run()
        {
            _game.PlayerCoords = HexMath.Pix2Hex(_player.Transform.localPosition, HexSize);
            if (_game.LastCoords.X != _game.PlayerCoords.X || _game.LastCoords.Y != _game.PlayerCoords.Y)
            {
                UnrenderRing(_game.LastCoords, Fow + 1);
                RenderRing(_game.PlayerCoords, Fow);
                _game.LastCoords = _game.PlayerCoords;
            }

            //TODO
            for (int i = 0; i < _triggerEvents.EntitiesCount; i++)
            {
                HexaCoords coords =
                    HexMath.Pix2Hex(_triggerEvents.Components1[i].OtherTransform.localPosition, HexSize);
                coords.W = 1;
                var hexComponent = _game.Map[coords];
                if (hexComponent != null)
                {
                    switch (hexComponent.HexType)
                    {
                        case HexTypes.Diamond:
                            _player.Exp += 1;
                            //Debug.Log(coords.X + " " + coords.Y + " " + coords.W);
                            RemoveHex(hexComponent, coords);
                            break;
                        default:
                            break;
                    }
                }

                _triggerEvents.Components1[i].OtherTransform = null;
                _world.RemoveEntity(_triggerEvents.Entities[i]);
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
                    if (HexMath.HexDistance(playerCoords, coords) >= Fow)
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
                    hex.Parent = _game.Pools[(int) Pool.Enemy].Get();
                    hex.Parent.PoolTransform.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Enemies[0];
                    //TODO value
                    break;
                case HexTypes.Empty:
                    //Debug.Log(coords.X + " " + coords.Y + " " + coords.W + " empty");
                    return;
                case HexTypes.Spawn: //?
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            hex.Parent.PoolTransform.localPosition = HexMath.Hex2Pix(coords, HexSize);
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
            if (hex.Parent == null) return;
            _game.Pools[(int) hex.HexType].Recycle(hex.Parent);
            hex.Parent = null;
        }

        private void RemoveHex(HexComponent hexComponent, HexaCoords coords)
        {
            _game.Pools[(int) hexComponent.HexType].Recycle(hexComponent.Parent);
            hexComponent.Parent = null;
            _game.Map.ClearAt(coords);
        }

        public void Destroy()
        {
            _game = null;
        }
    }
}