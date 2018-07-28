using Events;
using Leopotam.Ecs;
using UnityEngine;

namespace MonoBehviours
{
	public class PlayerColliderEvent : MonoBehaviour
	{
		private EcsWorld _world;
	
		void Start()
		{
			_world = EcsWorld.Active;
		}

		void OnCollisionEnter2D(Collision2D other)
		{
			//Debug.Log("TOUCH");
			var collisionEvent = _world.CreateEntityWith<CollisionEvent>();
			collisionEvent.ObstacleTransform = other.transform;
		}

		private void OnCollisionStay2D(Collision2D other)
		{
		
		}

		private void OnCollisionExit2D(Collision2D other)
		{
			
		}
	}
}
