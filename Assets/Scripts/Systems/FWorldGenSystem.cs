using System;
using Components;
using Events;
using Leopotam.Ecs;
using LeopotamGroup.Pooling;
using Misc;
using UnityEngine;

namespace Systems
{
    public enum BackroundTypes
    {
        Grass,
        Water,
        Forest,
        Swamp
    }

    public enum ForegroundTypes
    {
        Empty,
        Spawn,
        Enemy,
        Diamond,
        Obstacle
    }

    [EcsInject]
    public class FWorldGenSystem : IEcsInitSystem, IEcsRunSystem
    {
        public float HexSize;
        public int MapSize;
        public int MapSeed;
        public int Fow;
        public GameObject PlayerPrefab;
        public Sprite Spawn;
        public Sprite Enemy;
        public Sprite Diamond;
        public Sprite Obstacle;
        public Sprite Water;
        public Sprite Grass;
        public Sprite Swamp;
        public Sprite Forest;

        private MapComponent _map;
        private PlayerComponent _player;
        private EcsWorld _world = null;
        private EcsFilter<CollisionEvent> _collisionEvents = null;
        private EcsFilter<TriggerForegroundEvent> _triggerForegroundEvents = null;

        public void Initialize()
        {
            _map = _world.CreateEntityWith<MapComponent>();
            _player = _world.CreateEntityWith<PlayerComponent>();
            _player.Transform = GameObject.Instantiate(PlayerPrefab).transform;
            _player.Exp = 0;
            _player.Hp = 100;
            _player.Force = new Force();
            _map.LastCoords = new CubeCoords(0, 0);
            _map.PoolB = PoolContainer.CreatePool(Utils.BackPrefabPath);
            _map.PoolF = PoolContainer.CreatePool(Utils.ForePrefabPath);
            MapGenRandomNeighbours.GenerateMap(out _map.MapB, out _map.MapF, MapSize, MapSeed);
            RenderFull(new CubeCoords(0, 0), Fow);
        }

        public void Run()
        {
            //todo move into IEnumerator
            CubeCoords playerCoords = HexMath.Pix2Hex(_player.Transform.localPosition.x,
                _player.Transform.localPosition.y, HexSize);
            if (_map.LastCoords.x != playerCoords.x || _map.LastCoords.y != playerCoords.y)
            {
                UnrenderRing(_map.LastCoords, Fow + 1);
                RenderRing(playerCoords, Fow);
                _map.LastCoords = playerCoords;
            }

            for (int i = 0; i < _collisionEvents.EntitiesCount; i++)
            {
                CubeCoords coords =
                    HexMath.Pix2Hex(_collisionEvents.Components1[i].ObstacleTransform.position, HexSize);
            }

            for (int i = 0; i < _triggerForegroundEvents.EntitiesCount; i++)
            {
                CubeCoords coords = HexMath.Pix2Hex(_triggerForegroundEvents.Components1[i].ObstacleTransform.position,
                    HexSize);
                HexForegroundComponent hexComponent = _map.MapF[coords.x, coords.y];
                switch (hexComponent.ForegroundType)
                {
                    case ForegroundTypes.Diamond:
                        _player.Exp += hexComponent.Value;
                        RemoveFore(hexComponent, coords);
                        break;
                    default:
                        break;
                }
            }
        }

        private void RenderFull(CubeCoords playerCoords, int radius)
        {
            RenderHexBackground(playerCoords);
            RenderHexForeground(playerCoords);
            for (int i = 1; i <= radius; i++)
            {
                RenderRing(playerCoords, i);
            }
        }

        private void RenderRing(CubeCoords playerCoords, int radius)
        {
            CubeCoords coords = new CubeCoords(playerCoords.x, playerCoords.y + radius);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    coords.x += HexMath.Directions[i, 0];
                    coords.y += HexMath.Directions[i, 1];
                    RenderHexBackground(coords);
                    RenderHexForeground(coords);
                }
            }
        }

        private void UnrenderRing(CubeCoords playerCoords, int radius)
        {
            CubeCoords coords = new CubeCoords(playerCoords.x, playerCoords.y + radius);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    coords.x += HexMath.Directions[i, 0];
                    coords.y += HexMath.Directions[i, 1];
                    if (HexMath.HexDistance(playerCoords.x, playerCoords.y, coords.x, coords.y) >= Fow)
                    {
                        HideHex(coords);
                    }
                }
            }
        }

        private void RenderHexBackground(CubeCoords coords)
        {
            if (!_map.MapB.ExistAt(coords) || _map.MapB[coords.x, coords.y].IsNew)
            {
                MapGenRandomNeighbours.GenerateHex(coords, _map.MapB, _map.MapF);
            }

            HexBackgroundComponent hexComponent = _map.MapB[coords.x, coords.y];
            if (hexComponent.Parent != null) return; //_poolBackground.Recycle(hexComponent.Parent);
            hexComponent.Parent = _map.PoolB.Get();
            hexComponent.Parent.PoolTransform.localPosition = HexMath.Hex2Pix(coords, HexSize);
            switch (hexComponent.BackgroundType)
            {
                case BackroundTypes.Grass:
                    hexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Grass;
                    hexComponent.SpeedDown = 1;
                    break;
                case BackroundTypes.Water:
                    hexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Water;
                    hexComponent.SpeedDown = 0.1f;
                    break;
                case BackroundTypes.Swamp:
                    hexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Swamp;
                    hexComponent.SpeedDown = 0.02f;
                    break;
                case BackroundTypes.Forest:
                    hexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Forest;
                    hexComponent.SpeedDown = 0.5f;
                    break;
                default:
                    throw new Exception("Null ground type");
            }

            hexComponent.Parent.PoolTransform.gameObject.SetActive(true);
        }

        private void RenderHexForeground(CubeCoords coords)
        {
            if (!_map.MapF.ExistAt(coords) || _map.MapF[coords.x, coords.y].IsNew) return;
            HexForegroundComponent foregroundHexComponent = _map.MapF[coords.x, coords.y];
            if (foregroundHexComponent.ForegroundType == ForegroundTypes.Empty) return;
            if (foregroundHexComponent.Parent != null) return; //_poolForeground.Recycle(foregroundHexComponent.Parent);
            foregroundHexComponent.Parent = _map.PoolF.Get();
            foregroundHexComponent.Parent.PoolTransform.localPosition = HexMath.Hex2Pix(coords, HexSize);
            switch (foregroundHexComponent.ForegroundType)
            {
                case ForegroundTypes.Empty:
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = null;
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().enabled = false;
                    return;
                case ForegroundTypes.Enemy:
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Enemy;
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().enabled = true;
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().isTrigger = false;
                    break;
                case ForegroundTypes.Obstacle:
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Obstacle;
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().enabled = true;
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().isTrigger = false;
                    break;
                case ForegroundTypes.Diamond:
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Diamond;
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().enabled = true;
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().isTrigger = true;
                    break;
                case ForegroundTypes.Spawn:
                    //spawn point, dunno why
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = null;
                    foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().enabled = false;
                    break;
                default:
                    throw new Exception("Null object type");
            }

            foregroundHexComponent.Parent.PoolTransform.gameObject.SetActive(true);
        }

        private void HideHex(CubeCoords coords)
        {
            HideBack(coords);
            HideFore(coords);
        }

        private void HideBack(CubeCoords coords)
        {
            if (!_map.MapB.ExistAt(coords)) return;
            HexBackgroundComponent hexComponent = _map.MapB[coords.x, coords.y];
            if (hexComponent.Parent == null) return;
            _map.PoolB.Recycle(hexComponent.Parent);
            hexComponent.Parent = null;
        }

        private void HideFore(CubeCoords coords)
        {
            if (!_map.MapF.ExistAt(coords)) return;
            HexForegroundComponent foregroundHexComponent = _map.MapF[coords.x, coords.y];
            if (foregroundHexComponent.Parent == null) return;
            _map.PoolF.Recycle(foregroundHexComponent.Parent);
            foregroundHexComponent.Parent = null;
        }

        private void RemoveFore(HexForegroundComponent hexComponent, CubeCoords coords)
        {
            _map.PoolF.Recycle(hexComponent.Parent);
            _map.MapF.ClearAt(coords);
        }

        public void Destroy()
        {
            _map = null;
        }
    }
}