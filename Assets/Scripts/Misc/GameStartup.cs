﻿using Systems;
using Leopotam.Ecs;
using Scriptable;
using Temporary;
using UnityEngine;

namespace Misc
{
    public class GameStartup : MonoBehaviour
    {
        public SettingsObject Settings;

        private EcsWorld _world;
        private EcsSystems _updateSystems;

        void OnEnable()
        {
            _world = new EcsWorld();
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
#endif
            _updateSystems = new EcsSystems(_world)
                .Add(new FWorldGenSystem()
                {
                    PlayerPrefab = Settings.PlayerPrefab,
                    Fow = Settings.FieldOfView,
                    HexSize = Settings.HexSize,
                    MapSize = Settings.MapSize,
                    MapSeed = Settings.MapSeed,
                    Spawn = Settings.Spawn,
                    Enemy = Settings.Enemy,
                    Diamond = Settings.Diamond,
                    Obstacle = Settings.Obstacle,
                    Water = Settings.Water,
                    Grass = Settings.Grass,
                    Swamp = Settings.Swamp,
                    Forest = Settings.Forest
                })
                .Add(new FMovePlayerSystem()
                {
                    SpeedMultipiler = Settings.SpeedMultipiler
                })
                .Add(new CameraSystem()
                {
                    CameraDistance = Settings.CameraDistance,
                    CameraSpeed = Settings.CameraSpeed
                })
#if UNITY_EDITOR
                .Add(new InputKeyboardSystem())
#else
                .Add(new InputStickSystem())
#endif
                .Add(new FAISystem());
            //
            _updateSystems.Initialize();
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_updateSystems);
#endif
        }

        void Update()
        {
            _updateSystems.Run();
        }

        private void OnDisable()
        {
            _updateSystems.Destroy();
        }
    }
}