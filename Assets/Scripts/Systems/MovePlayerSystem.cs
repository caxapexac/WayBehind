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
        public float SpeedMultipiler;
        
        private PlayerComponent _player;
        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _playerFilter = null;
        private EcsFilter<CollisionEvent> _collisionFilter = null;
        
        public void Initialize()
        {
            _player = _playerFilter.Components1[0];
        }

        
        //TODO
        public void Run()
        {
            float x = _player.Force.X;
            float y = _player.Force.Y;
            Vector2 speedVector = new Vector2(x, y).normalized;
            float speedForce = (Mathf.Abs(x) + Mathf.Abs(y)) * 0.5f;
            _player.Transform.Translate(speedVector * speedForce * SpeedMultipiler * Time.deltaTime);
            for (int j = 0; j < _collisionFilter.EntitiesCount; j++)
            {
                _collisionFilter.Components1[j].OtherTransform = null;
                _world.RemoveEntity(_collisionFilter.Entities[j]);
                //Debug.Log(_collisionFilter.Components1[j]);
            }
        }
        
        public void Destroy()
        {
            _player = null;
        }
    }
}