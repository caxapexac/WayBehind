using Systems;
using Leopotam.Ecs;
using Scriptable;
using Temporary;
using UnityEngine;

namespace Misc
{
	public class GameStartup : MonoBehaviour
	{
		public SettingsObject Settings;
		
		private EcsWorld _world;
		private EcsSystems _updateSystems;
		private EcsSystems _fixedUpdateSystems;

		void OnEnable()
		{
			_world = new EcsWorld();
#if UNITY_EDITOR
			Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
#endif  
			_updateSystems = new EcsSystems(_world)
				.Add(new PlayerInitSystem())
				.Add(new InputKeyboardSystem())
				.Add(new InputStickSystem())
				.Add(new CameraSystem()
				{
					CameraDistance = Settings.CameraDistance,
					CameraSpeed = Settings.CameraSpeed
				});
			//
			_fixedUpdateSystems = new EcsSystems(_world)
				.Add(new FMovePlayerSystem()
				{
					SpeedMultipiler = Settings.SpeedMultipiler
				})
				.Add(new FWorldGenSystem()
				{
					Fow = Settings.FieldOfView,
					HexSize = Settings.HexSize,
					Player = Settings.Player,
					Enemy = Settings.Enemy,
					Diamond = Settings.Diamond,
					Obstacle = Settings.Obstacle,
					Water = Settings.Water,
					Grass = Settings.Grass,
					Boloto = Settings.Boloto,
					Forest = Settings.Forest
				})
				.Add(new FAISystem());
			_updateSystems.Initialize();
			_fixedUpdateSystems.Initialize();
#if UNITY_EDITOR
			Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_updateSystems);
#endif
		}
	
		void Update () 
		{
			_updateSystems.Run ();
		}

		private void FixedUpdate()
		{
			_fixedUpdateSystems.Run();
		}

		private void OnDisable()
		{
			_updateSystems.Destroy ();
			_fixedUpdateSystems.Destroy();
		}
	}
}
