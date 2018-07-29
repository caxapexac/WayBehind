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
    public class FMovePlayerSystem : IEcsRunSystem
    {
        public float SpeedMultipiler;

        private EcsWorld _world = null;

        private EcsFilter<PlayerComponent> _playerFilter = null;

        private EcsFilter<CollisionEvent> _collisionFilter = null;
        
        private List<int> _removedEntities = new List<int>();

        public void Run()
        {
            for (int i = 0; i < _playerFilter.EntitiesCount; i++)
            {
                var player = _playerFilter.Components1[i];
                float x = player.Force.X;
                float y = player.Force.Y;
                //refactor
                Vector2 speedVector = new Vector2(player.Force.X, player.Force.Y).normalized;
                float speedForce = (Mathf.Abs(x) + Mathf.Abs(y)) * 0.5f;
                player.Transform.Translate(speedVector * speedForce * SpeedMultipiler * Time.deltaTime);
                //
                for (int j = 0; j < _collisionFilter.EntitiesCount; j++)
                {
                    //исправил баг с множественными ентитями дублирующимися
                    var entity = _collisionFilter.Entities[i];
                    if (_removedEntities.Contains(entity)) return;
                    _removedEntities.Add(entity);
                    _collisionFilter.Components1[i].ObstacleTransform = null;
                    _world.RemoveEntity(_collisionFilter.Entities[i]);
                    //Debug.Log(_collisionFilter.Components1[j]);
                }
                _removedEntities.Clear();
            }
        }
    }
}