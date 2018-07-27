using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
	[EcsInject]
	public class InputKeyboardSystem : IEcsRunSystem
	{
		EcsFilter<PlayerComponent> _playerFilter = null;
	
		public void Run()
		{
			var x = Input.GetAxis ("Horizontal");
			var y = Input.GetAxis ("Vertical");
			//if (new Vector2(x, y).sqrMagnitude > 0.01f)
			//{
				for (int i = 0; i < _playerFilter.EntitiesCount; i++)
				{
					_playerFilter.Components1[i].Force.X = x;
					_playerFilter.Components1[i].Force.Y = y;
				}
			//}
		}
	}
}
