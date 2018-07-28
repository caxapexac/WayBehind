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
            //Debug.Log("TOUCH");
            if (other.CompareTag(Utils.BackgroundTag))
            {
                TriggerBackgroundEvent triggerEvent = _world.CreateEntityWith<TriggerBackgroundEvent>();
                triggerEvent.ObstacleTransform = other.transform;
            }
            else
            {
                TriggerForegroundEvent triggerEvent = _world.CreateEntityWith<TriggerForegroundEvent>();
                triggerEvent.ObstacleTransform = other.transform;
            }
            
        }


        private void OnCollisionStay2D(Collision2D other)
        {
        }

        private void OnCollisionExit2D(Collision2D other)
        {
        }
    }
}