using System;
using Client.Scripts.Algorithms;
using Client.Scripts.Components;
using Client.Scripts.ComponentsOneFrame;
using Client.Scripts.Miscellaneous;
using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using UnityEngine.UI;


namespace Client.Scripts.Systems
{
    [EcsInject]
    [Obsolete]
    public class PlayerPickSystem : IEcsInitSystem, IEcsRunSystem
    {
        private Text _diamondsText;
        private Slider _hpBar;

        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _playerFilter = null;
        private EcsFilter<TriggerEvent> _triggerEvents = null;
        private EcsUiEmitter _uiEmitter = null;

        public void Initialize()
        {
            _diamondsText = _uiEmitter.GetNamedObject(Names.Diamonds).GetComponent<Text>();
            _hpBar = _uiEmitter.GetNamedObject(Names.HpBar).GetComponent<Slider>();
        }

        public void Run()
        {
            /*for (int i = 0; i < _triggerEvents.EntitiesCount; i++)
            {
                int depth = _triggerEvents.Components1[i].Sender.CompareTag(Tags.ForegroundTag) ? 1 : 0;
                HexaCoords coords =
                    HexMath.Pixel2Hexel(_triggerEvents.Components1[i].Sender.localPosition, _game.S.HexSize, depth);
                HexComponent hex = _game.MapComponent[coords];
                
                switch (hex.HexType)
                {
                    case HexTypes.Diamond:
                        _player.Exp += 1;
                        _player.Hp += 1;
                        _hpBar.value = _player.Hp;
                        _diamondsText.text = _player.Exp.ToString();

                        //HexDisposeEvent hexDispose = _world.CreateEntityWith<HexDisposeEvent>();
                        //hexDispose.Coords = coords;
                        break;
                    default:
                        break;
                }
            }*/
        }


        public void Destroy()
        { }
    }
}