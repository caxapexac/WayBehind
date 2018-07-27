using Components;
using Leopotam.Ecs;
using Misc;
using UnityEngine;

namespace Systems
{
	[EcsInject]
	public class PlayerInitSystem : IEcsInitSystem
	{
		EcsWorld _world = null;
		
		public void Initialize()
		{
			foreach (var unityObject in GameObject.FindGameObjectsWithTag(Utils.PlayerTag))
			{
				var component = _world.CreateEntityWith<PlayerComponent>();
				component.Transform = unityObject.transform;
				component.Collider = unityObject.GetComponent<Collider2D>();
				component.Force = new Force(){ X = 0f, Y = 0f };
			}
		}
	
		public void Destroy()
		{
			
		}
	}
}
