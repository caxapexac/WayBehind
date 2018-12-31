using System;
using Client.Scripts.OBSOLETE.Components;
using Client.Scripts.OBSOLETE.Events;
using Client.Scripts.OBSOLETE.Misc;
using Leopotam.Ecs;
using LeopotamGroup.Math;
using UnityEngine;

namespace Client.Scripts.OBSOLETE.Systems
{
    [EcsInject]
    public class PlayerMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private Vector2 _lastPosition;
        private Transform _playerBody; //todo for rotation
        private Rigidbody2D _playerRigidbody;
        private GameComponent _game;
        private PlayerComponent _player;
        private EcsWorld _world = null;
        private EcsFilter<GameComponent> _gameFilter = null;
        private EcsFilter<PlayerComponent> _playerFilter = null;
        private EcsFilter<TriggerEvent> _triggerFilter = null;

        public void Initialize()
        {
            _game = _gameFilter.Components1[0];
            _player = _playerFilter.Components1[0];
            _playerBody = _player.Transform.GetChild(0);
            _playerRigidbody = _player.Transform.GetComponent<Rigidbody2D>();
            _lastPosition = _player.Transform.position;
            _player.CurrentForce = new Vector2();
            _player.CurrentSlowing = 1f;
        }

        public void Run()
        {
            HexaCoords coords = HexMath.Pixel2Hexel(_player.Transform.localPosition, _game.S.HexSize);
            HexComponent hex = _game.Map[coords];
            for (int i = 0; i < _triggerFilter.EntitiesCount; i++)
            {
                if (!_triggerFilter.Components1[i].Sender.CompareTag(Tags.BackgroundTag)) continue;
                switch (hex.HexType)
                {
                    case HexTypes.Grass:
                        _player.Slowing = _game.S.GrassSpeed;
                        break;
                    case HexTypes.Water:
                        _player.Slowing = _game.S.WaterSpeed;
                        break;
                    case HexTypes.Forest:
                        _player.Slowing = _game.S.ForestSpeed;
                        break;
                    case HexTypes.Swamp:
                        _player.Slowing = _game.S.SwampSpeed;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (_player.Hp <= 0) _player.CurrentSlowing *= 0.1f;
            //todo
            _player.CurrentForce = Vector2.Lerp(_player.CurrentForce, _player.Force, _game.S.LerpSpeed * Time.deltaTime);
            _player.CurrentSlowing = MathFast.Lerp(_player.CurrentSlowing, _player.Slowing, _game.S.LerpSlowing * Time.deltaTime);
            Vector2 moveVector = (Vector2) _player.Transform.localPosition +
                                 _player.CurrentForce * _game.S.SpeedMultipiler * _player.CurrentSlowing * Time.deltaTime;
            _playerBody.transform.LookAt2D(moveVector);
            _playerRigidbody.MovePosition(moveVector);
//            Vector2 speedVector = new Vector2(x, y).normalized;
//            float speedForce = (Mathf.Abs(x) + Mathf.Abs(y)) * 0.5f;
        }

        public void Destroy()
        {
            _game = null;
            _player = null;
            _playerBody = null;
            _playerRigidbody = null;
        }
    }
}