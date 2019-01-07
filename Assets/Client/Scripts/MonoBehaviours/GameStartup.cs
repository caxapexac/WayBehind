using System;
using Client.Scripts.Miscellaneous;
using Client.Scripts.Scriptable;
using Client.Scripts.Systems;
using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Client.Scripts.MonoBehaviours
{
    /// Существует несколько систем координат
    /// HexCoords - кубичские координаты x,y,z относительно центра мира (60°)
    /// OffsetCoords - координаты x,y относительно центра мира (90° со сдвигом на четных гексах)
    /// Int2 + index - положение внутри чанка по координатам x,y относительно центра мира
    /// Pixel (Vector2) - координаты гекса относительно центра мира unity (90°)

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
    [DisallowMultipleComponent]
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private SettingsObject _settings;
        [SerializeField] private MapNoiseObject _noiseSettings;
        [SerializeField] private EcsUiEmitter _uiEmitter;

        private Variables _variables;
        private EcsWorld _world;
        private EcsSystems _updateSystems;

        private void OnEnable()
        {
            _variables = new Variables();
            _world = new EcsWorld();
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
#endif
            _updateSystems = new EcsSystems(_world)
                .Add(_uiEmitter)
                .Add(_settings.IsTouchScreen ? (IEcsSystem)new InputStickSystem() : new InputKeyboardSystem())
                .Add(new WorldInitSystem())
                .Add(new PlayerInitSystem())
                .Add(new CameraSystem())

                //.Add(new SpiritRenderSystem())
                .Add(new PlayerMoveSystem())

                //.Add(new PlayerAttackSystem())
                //.Add(new PlayerPickSystem())

                //.Add(new EnemyMoveSystem())
                //.Add(new EnemyAttackSystem())
                
                .Add(new WorldRenderSystem())
                .Add(new DebugUiSystem())
                .Inject(_settings)
                .Inject(_noiseSettings)
                .Inject(_variables)
                .Inject(Camera.main)
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