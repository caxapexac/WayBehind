using Client.Scripts.ComponentsOneFrame;
using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.MonoBehaviours
{
    /// <summary>
    /// Diamonds, Swamps, Water
    /// </summary>
    [DisallowMultipleComponent]
    public class MonoTriggerListener : MonoBehaviour
    {
        private EcsWorld _world;

        void Start()
        {
            _world = EcsWorld.Active;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEvent triggerEvent = _world.CreateEntityWith<TriggerEvent>();
            triggerEvent.Sender = transform;
            triggerEvent.Other = other.transform;
        }

        private void OnTriggerStay2D(Collider2D other)
        { }

        private void OnTriggerExit2D(Collider2D other)
        { }
    }
}