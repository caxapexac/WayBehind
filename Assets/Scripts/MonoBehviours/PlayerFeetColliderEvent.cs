using Events;
using Leopotam.Ecs;
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
            CollisionEvent collisionEvent = new CollisionEvent()
            {
                ObstacleTransform = other.transform
            };
            _world.CreateEntityWith(out collisionEvent);
        }


        private void OnCollisionStay2D(Collision2D other)
        {
        }

        private void OnCollisionExit2D(Collision2D other)
        {
        }
    }
}