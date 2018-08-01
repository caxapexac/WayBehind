using Events;
using Leopotam.Ecs;
using Misc;
using UnityEngine;

namespace MonoBehviours
{
    public class PlayerFeetColliderEvent : MonoBehaviour
    {
        private EcsWorld _world;

        void Start()
        {
            _world = EcsWorld.Active;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEvent triggerEvent = _world.CreateEntityWith<TriggerEvent>();
            triggerEvent.OtherTransform = other.transform;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
        }

        private void OnCollisionExit2D(Collision2D other)
        {
        }
    }
}