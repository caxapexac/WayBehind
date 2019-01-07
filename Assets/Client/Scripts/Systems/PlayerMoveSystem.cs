using Client.Scripts.Algorithms;
using Client.Scripts.Algorithms.Legacy;
using Client.Scripts.Components;
using Client.Scripts.ComponentsOneFrame;
using Client.Scripts.Miscellaneous;
using Client.Scripts.Scriptable;
using Leopotam.Ecs;
using LeopotamGroup.Math;
using UnityEngine;


namespace Client.Scripts.Systems
{
    [EcsInject]
    public class PlayerMoveSystem : IEcsRunSystem
    {
        //private PlayerComponent _player;
        //private EcsFilter<TriggerEvent> _triggerFilter = null;

        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _playerFilter = null;
        private EcsFilter<MapComponent<HexComponent>> _mapFilter = null;
        private EcsFilter<InputEvent> _inputEventFilter = null;
        private SettingsObject _settings = null;
        private Variables _variables = null;

        public void Run()
        {
            for (int i = 0; i < _playerFilter.EntitiesCount; i++)
            {
                _mapFilter.Components1[i].PlayerPosition = HexMath.Pixel2Offset(
                    _playerFilter.Components1[i].Parent.transform.position,
                    _variables.HexSize);
                PlayerComponent player = _playerFilter.Components1[i];

                //                //HexComponent hex = _game.Map[coords];
                //                //player.Slowing = hex.Slowing;
                //
                //                if (player.Hp <= 0) player.CurrentSlowing *= 0.1f;
                //
                //                player.CurrentForce =
                //                    Vector2.Lerp(player.CurrentForce, player.Force, _settings.LerpSpeed * Time.deltaTime);
                //                player.CurrentSlowing = MathFast.Lerp(player.CurrentSlowing, player.Slowing,
                //                    _settings.LerpSlowing * Time.deltaTime);
                Vector2 moveVector = player.Parent.transform.localPosition;
                for (int k = 0; k < _inputEventFilter.EntitiesCount; k++)
                {
                    if (_inputEventFilter.Components1[k].Direction.magnitude >= 0.5f)
                    {
                        moveVector += _inputEventFilter.Components1[k].Direction
                            * _settings.SpeedMultipiler
                            * Time.deltaTime;
                    }
                }
                //player.Transform.GetChild(0).transform.LookAt2D(moveVector);
                player.Parent.transform.GetComponent<Rigidbody2D>().MovePosition(moveVector);
            }
        }
    }
}