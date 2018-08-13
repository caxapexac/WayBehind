using Events;
using Leopotam.Ecs;
using Misc;
using UnityEngine;

namespace MonoBehviours
{
    /// <summary>
    /// Diamonds, Swamps, Water
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerTriggerEvent : MonoBehaviour
    {
        private EcsWorld _world;

        void Start()
        {
            _world = EcsWorld.Active;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEvent triggerEvent = _world.CreateEntityWith<TriggerEvent>();
            triggerEvent.Sender = other.transform;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
        }

        private void OnTriggerExit2D(Collider2D other)
        {
        }
    }
}