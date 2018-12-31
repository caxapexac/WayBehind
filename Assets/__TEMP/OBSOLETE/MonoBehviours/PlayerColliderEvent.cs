using Client.Scripts.OBSOLETE.Events;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.Scripts.OBSOLETE.MonoBehviours
{
	/// <summary>
	/// Obstacles, Enemies
	/// </summary>
	[DisallowMultipleComponent]
	public class PlayerColliderEvent : MonoBehaviour
	{
		private EcsWorld _world;
	
		void Start()
		{
			_world = EcsWorld.Active;
		}

		void OnCollisionEnter2D(Collision2D other)
		{
			var collisionEvent = _world.CreateEntityWith<CollisionEvent>();
			collisionEvent.Sender = other.transform;
		}

		private void OnCollisionStay2D(Collision2D other)
		{
		
		}

		private void OnCollisionExit2D(Collision2D other)
		{
			
		}
	}
}
