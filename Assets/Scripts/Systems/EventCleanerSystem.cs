using Events;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class EventCleanerSystem : IEcsRunSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<CollisionEvent> _collisionEvents = null;
        private EcsFilter<TriggerEvent> _triggerEvents = null;

        public void Run()
        {
            for (var i = 0; i < _collisionEvents.EntitiesCount; i++)
            {
                _collisionEvents.Components1[i].Sender = null;
                _world.RemoveEntity(_collisionEvents.Entities[i]);
            }

            for (var i = 0; i < _triggerEvents.EntitiesCount; i++)
            {
                _triggerEvents.Components1[i].Sender = null;
                _world.RemoveEntity(_triggerEvents.Entities[i]);
            }
        }
    }
}