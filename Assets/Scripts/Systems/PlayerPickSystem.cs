using Components;
using Events;
using Leopotam.Ecs;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
	[EcsInject]
	public class PlayerPickSystem : IEcsInitSystem, IEcsRunSystem 
	{
		private GameComponent _game;
		private PlayerComponent _player;
		private Text _diamondsText;
		private EcsWorld _world = null;
		private EcsFilterSingle<GameComponent> _gameFilter = null;
		private EcsFilterSingle<PlayerComponent> _playerFilter = null;
		private EcsFilter<TriggerEvent> _triggerEvents = null;
		
		public void Initialize()
		{
			_game = _gameFilter.Data;
			_player = _playerFilter.Data;
			_diamondsText = _game.UI.GetNamedObject(Names.Diamonds).GetComponent<Text>();
		}
	
		public void Run()
		{
			for (int i = 0; i < _triggerEvents.EntitiesCount; i++)
			{
				int depth = _triggerEvents.Components1[i].Sender.CompareTag(Tags.ForegroundTag) ? 1 : 0;
                HexaCoords coords =
                    HexMath.Pixel2Hexel(_triggerEvents.Components1[i].Sender.localPosition, _game.S.HexSize, depth);
                HexComponent hex = _game.Map[coords];
				switch (hex.HexType)
				{
					case HexTypes.Diamond:
						_player.Exp += 1;
						_diamondsText.text = _player.Exp.ToString();
						HexDisposeEvent hexDispose = _world.CreateEntityWith<HexDisposeEvent>();
						hexDispose.Coords = coords;
						break;
					default:
						break;
				}
			}
		}
		
		public void Destroy()
		{
			_game = null;
			_player = null;
		}
	}
}
