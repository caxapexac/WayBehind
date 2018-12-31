using Client.Scripts.OBSOLETE.Components;
using Client.Scripts.OBSOLETE.Events;
using Client.Scripts.OBSOLETE.Misc;
using Leopotam.Ecs;
using UnityEngine.UI;

namespace Client.Scripts.OBSOLETE.Systems
{
	[EcsInject]
	public class PlayerPickSystem : IEcsInitSystem, IEcsRunSystem 
	{
		private GameComponent _game;
		private PlayerComponent _player;
		private Text _diamondsText;
		private Slider _hpBar;
		private EcsWorld _world = null;
		private EcsFilter<GameComponent> _gameFilter = null;
		private EcsFilter<PlayerComponent> _playerFilter = null;
		private EcsFilter<TriggerEvent> _triggerEvents = null;
		
		public void Initialize()
		{
			_game = _gameFilter.Components1[0];
			_player = _playerFilter.Components1[0];
			_diamondsText = _game.UI.GetNamedObject(Names.Diamonds).GetComponent<Text>();
			_hpBar = _game.UI.GetNamedObject(Names.HpBar).GetComponent<Slider>();
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
						_player.Hp += 1;
						_hpBar.value = _player.Hp;
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
