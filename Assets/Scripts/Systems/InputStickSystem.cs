using Components;
using Leopotam.Ecs;
using TouchControlsKit;
using UnityEngine;

namespace Systems
{
	[EcsInject]
	public class InputStickSystem : IEcsRunSystem 
	{
		EcsFilter<PlayerComponent> _playerFilter = null;
		
		public void Run()
		{
			Vector2 move = TCKInput.GetAxis( "Joystick" ); // NEW func since ver 1.5.5

			//if (new Vector2(x, y).sqrMagnitude > 0.01f)
			//{
			for (int i = 0; i < _playerFilter.EntitiesCount; i++)
			{
				_playerFilter.Components1[i].Force.X = move.x;
				_playerFilter.Components1[i].Force.Y = move.y;
			}
			//}
		}

	}
}
