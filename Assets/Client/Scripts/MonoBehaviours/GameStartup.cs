using System;
using Client.ScriptableObjects;
using Client.Scripts.Systems;
using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


namespace Client.Scripts.MonoBehaviours
{
    /// Существует несколько систем координат
    /// HexaCoords - uint координаты гексов x,y,z относительно центра мира 
    
    //  Работай без багов, аминь
    //            ______
    //           / ____ \   
    //          | / || \ |
    //          | | || | |
    //      ____| | || | |____         
    //     / _____| || |_____ \
    //    / /__B__U_||_G__S__\ \                      
    //    \ \_____  ||  _____/ /                  
    //     \____  | || |  ____/                    
    //          | | || | |                     
    //          | | || | |                     
    //          | | || | |                     
    //          | | || | |          
    //   \ \    | | || | |    |/      
    //  =========================        
    //  =========================  

    #region EcsWorldsTips

    //Юзаем EcsSystem.Inject(); вместо кастомного мира

    //Все компоненты наследуем от IEcsAutoResetComponent

    //Используем [EcsOneFrame] вместо ручного удаления в update

    //Все передачи данных через фильтры

    //!    PlayerInitSystem
    //!    WorldInitSystem
    //!    CommandSystem
    //!    EnemySystems
    //?    MoveSystem 2 или 1
    //!    DeathSystem
    //!    GuiSystem
    //!    GameStateSystem
    //!    SerializeSystem
    //!    SettingsSystem
    //!    UserInputSystem
    //!    PickPlayerSystem
    //!    ScoreSystem

    //оффтоп
    //EventSystem.current.IsPointerOverGameObject() - проверить кнопка в гуе или нажатие на гект

    //public const float outerRadius = 10f;

    //public const float innerRadius = outerRadius * 0.866025404f;

    //position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);


    public enum GameStates
    {
        Start,
        Pause,
        Restart,
        Exit
    }

    #endregion


    [DisallowMultipleComponent]
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private SettingsObject _settings;
        [SerializeField] private EcsUiEmitter _uiEmitter;

        private EcsWorld _world;
        private EcsSystems _updateSystems;

        private void OnEnable()
        {
            _world = new EcsWorld();
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
#endif
            _updateSystems = new EcsSystems(_world)
                .Add(_uiEmitter)
                .Add(_settings.IsTouchScreen ? (IEcsSystem)new InputStickSystem() : new InputKeyboardSystem())
                .Add(new CameraSystem())
                .Add(new WorldInitSystem())
                .Add(new PlayerInitSystem())
                .Add(new WorldRenderSystem())

                //.Add(new SpiritRenderSystem())
                .Add(new PlayerMoveSystem())

                //.Add(new PlayerAttackSystem())
                //.Add(new PlayerPickSystem())

                //.Add(new EnemyMoveSystem())
                //.Add(new EnemyAttackSystem())
                .Inject(_settings)
                .Inject(_uiEmitter);
            _updateSystems.Initialize();
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_updateSystems);
#endif
        }

        private void Update()
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