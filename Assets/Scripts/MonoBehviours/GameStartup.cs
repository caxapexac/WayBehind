using Systems;
using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehviours
{
    [DisallowMultipleComponent]
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private SettingsObject _settings;
        [SerializeField] private EcsUiEmitter _uiEmitter;
        private EcsWorld _world;
        private EcsSystems _updateSystems;

        void OnEnable()
        {
            _world = new EcsWorld();
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
#endif
            _updateSystems = new EcsSystems(_world)
                .Add(_uiEmitter)
                .Add(new WorldGenSystem(_settings, _uiEmitter))
                .Add(new PlayerMoveSystem())
                .Add(new PlayerAttackSystem())
                .Add(new PlayerPickSystem())
                .Add(new CameraSystem())
                .Add(new EnemyMoveSystem())
                .Add(new EnemyAttackSystem())
                .Add(_settings.IsTouchScreen ? (IEcsSystem) new InputStickSystem() : new InputKeyboardSystem())
                .Add(new EventCleanerSystem())
                .Add(new EcsUiCleaner());
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
            _updateSystems.Dispose();
        }
    }
}