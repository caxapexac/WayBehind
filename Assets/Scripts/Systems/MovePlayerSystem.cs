using System.Collections.Generic;
using Components;
using Events;
using Leopotam.Ecs;
using Misc;
using UnityEngine;

namespace Systems
{
    struct Force
    {
        public float X;
        public float Y;
    }

    [EcsInject]
    public class MovePlayerSystem : IEcsInitSystem, IEcsRunSystem
    {
        public float MinSpeed;
        public float MaxSpeed;
        public float SpeedMultipiler;
        public float AccelerationSpeed;

        private GameComponent _game;
        private PlayerComponent _player;
        private Transform _playerChild;
        private Rigidbody2D _playerRigidbody;
        
        private EcsWorld _world = null;
        private EcsFilter<GameComponent> _gameFilter = null;
        private EcsFilter<PlayerComponent> _playerFilter = null;
        private EcsFilter<CollisionEvent> _collisionFilter = null;
        private EcsFilter<TriggerEvent> _triggerFilter = null; 
        
        public void Initialize()
        {
            _game = _gameFilter.Components1[0];
            _player = _playerFilter.Components1[0];
            _playerChild = _player.Transform.GetChild(0);
            _playerRigidbody = _player.Transform.GetComponent<Rigidbody2D>();
        }
        
        //TODO
        public void Run()
        {
            float x = _player.Force.X;
            float y = _player.Force.Y;
            _playerRigidbody.AddForce(Vector2.right * x);
            _playerRigidbody.AddForce(Vector2.up * y);
            HexaCoords coords = HexMath.Pix2Hex(_player.Transform.localPosition, _game.HexSize);
            HexComponent hex = _game.Map[coords];
            
//            Vector2 speedVector = new Vector2(x, y).normalized;
//            float speedForce = (Mathf.Abs(x) + Mathf.Abs(y)) * 0.5f;
//            _player.Transform.Translate(speedVector * speedForce * SpeedMultipiler * Time.deltaTime);
            //USE DELTA TIME
            for (int j = 0; j < _collisionFilter.EntitiesCount; j++)
            {
                _collisionFilter.Components1[j].OtherTransform = null;
                _world.RemoveEntity(_collisionFilter.Entities[j]);
                //Debug.Log(_collisionFilter.Components1[j]);
            }
        }
        
        public void Destroy()
        {
            _game = null;
            _player = null;
            _playerChild = null;
            _playerRigidbody = null;
        }
    }
}