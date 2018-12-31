using System;
using Client.ScriptableObjects;
using Client.Scripts.OBSOLETE.Systems;
using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Scripts.OBSOLETE.MonoBehviours
{
    #region EcsWorldsWithSingleton

    class MyData
    {
        
    }
    
    class MyEcsWorld : EcsWorld
    {
        readonly public MyData Data;

        public MyEcsWorld(MyData data) {
            Data = data;
        }
    }
    
    //Либо юзаем EcsSystem.Inject();
    
    
    //Очистка классов
    class MyComponent : IEcsAutoResetComponent {
        public object LinkToAnotherComponent;

        void IEcsAutoResetComponent.Reset() {
            // Cleanup all marshal-by-reference fields here.
            LinkToAnotherComponent = null;
        }
    }
    
    //Эвенты [EcsOneFrame]
    
    //***System ---> ***Procissing?
    
    //player ловим фильтром, Game как можно легче сделать
    
//!    PlayerInitSystem
//    WallInitSystem
//!    CommandSystem
//    GhostSys
//    MoveSystem
//    PortalSystem
//    DeathSystem
//    GuiSystem
//!    GameStateSystem
//    ObstacleProcessing
//!    UserInputProcessing
//    MovementProcessing
//    FoodProcessing
//    DeadProcessing
//    ScoreProcessing 
    
    public enum GameStates
    {
        START,
        PAUSE,
        RESTART,
        EXIT
    }
    
    /*public class CommandComponent
    {
        public int PlayerNum;
        public Directions NewDirection;
    }*/

    #endregion
    
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
                //.Add(new PlayerMoveSystem())
                //.Add(new PlayerAttackSystem())
                //.Add(new PlayerPickSystem())
                //.Add(new CameraSystem())
                //.Add(new EnemyMoveSystem())
                //.Add(new EnemyAttackSystem())
                //.Add(_settings.IsTouchScreen ? (IEcsSystem) new InputStickSystem() : new InputKeyboardSystem())
                //.Add(new EventCleanerSystem())
                //.Add(new EcsUiCleaner())
                
                ;
            _updateSystems.Initialize();
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_updateSystems);
#endif
        }

        void Update()
        {
            _updateSystems.Run();
            _world.RemoveOneFrameComponents();
        }

        private void OnDisable()
        {
            _updateSystems.Dispose();
            _updateSystems = null;
            _world.Dispose();
            _world = null;
        }

        [Obsolete]
        public void Menu()
        {
            SceneManager.LoadScene("SettingsScene");
        }
    }
}