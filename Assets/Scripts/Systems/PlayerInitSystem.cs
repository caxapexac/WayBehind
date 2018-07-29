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
			//?
		}
	
		public void Destroy()
		{
			
		}
	}
}
