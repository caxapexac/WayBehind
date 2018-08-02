using Systems;
using Leopotam.Ecs;
using Scriptable;
using Temporary;
using UnityEngine;

namespace Misc
{
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private SettingsObject _settings;
        private EcsWorld _world;
        private EcsSystems _updateSystems;

        void OnEnable()
        {
            _world = new EcsWorld();
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
#endif
            _updateSystems = new EcsSystems(_world)
                .Add(new WorldGenSystem()
                {
                    PlayerPrefab = _settings.PlayerPrefab,
                    Fow = _settings.FieldOfView,
                    HexSize = _settings.HexSize,
                    MapSize = _settings.MapSize,
                    MapSaeed = _settings.MapSaeed,
                    Enemies = _settings.Enemies
                })
                .Add(new MovePlayerSystem()
                {
                    SpeedMultipiler = _settings.SpeedMultipiler
                })
                .Add(new CameraSystem()
                {
                    CameraSize = _settings.CameraSize,
                    CameraDistance = _settings.CameraDistance,
                    CameraSpeed = _settings.CameraSpeed
                })
                .Add(new AISystem());

            if (_settings.IsTouchScreen)
            {
                _updateSystems.Add(new InputStickSystem());
            }
            else
            {
                _updateSystems.Add(new InputKeyboardSystem());
            }

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