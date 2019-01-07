using Client.Scripts.ComponentsOneFrame;
using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.MonoBehaviours
{
    [DisallowMultipleComponent]
    public class MonoColliderListener : MonoBehaviour
    {
        private EcsWorld _world;

        void Start()
        {
            _world = EcsWorld.Active;
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            CollisionEvent collisionEvent = _world.CreateEntityWith<CollisionEvent>();
            collisionEvent.Sender = transform;
            collisionEvent.Other = other.transform;
        }

        private void OnCollisionStay2D(Collision2D other)
        { }

        private void OnCollisionExit2D(Collision2D other)
        { }
    }
}