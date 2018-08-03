using Components;
using Events;
using Leopotam.Ecs;

namespace Systems
{
	[EcsInject]
	// ReSharper disable once InconsistentNaming
	public class AISystem : IEcsInitSystem, IEcsRunSystem 
	{
		private PlayerComponent _player;
		private EcsWorld _world = null;
		private EcsFilter<PlayerComponent> _playerFilter = null;
		private EcsFilter<EnemyComponent> _enemyFilter = null;
		
		public void Initialize()
		{
			_player = _playerFilter.Components1[0];
		}

		public void Run()
		{
		
		
//		if (Time.time < _nextUpdateTime) {
//			return;
//		}
//		_nextUpdateTime = Time.time + _delay;

		}

		public void Destroy()
		{
			_player = null;
		}
	
	}
}
