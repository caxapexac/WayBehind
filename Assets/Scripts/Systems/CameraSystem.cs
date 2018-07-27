using Components;
using Leopotam.Ecs;
using Misc;
using UnityEngine;

namespace Systems
{
	[EcsInject]
	public class CameraSystem : IEcsInitSystem, IEcsRunSystem
	{
		public float CameraDistance;
		public float CameraSpeed;
		
		private GameObject _camera;
		private Transform _cameraTransform;
		private Transform _playerTransform;
		private EcsFilter<PlayerComponent> _playerFilter = null;
		
		public void Initialize()
		{
			_camera = GameObject.FindGameObjectWithTag(Utils.CameraTag);
			_cameraTransform = _camera.transform;
			_playerTransform = _playerFilter.Components1[0].Transform;
		}
	
		public void Run()
		{
			if (Vector2.Distance(_cameraTransform.position, _playerTransform.position) > CameraDistance)
			{
				_cameraTransform.Translate(new Vector2(_playerTransform.position.x - _cameraTransform.position.x,
					_playerTransform.position.y - _cameraTransform.position.y) * CameraSpeed);
			}
		}
	
		public void Destroy()
		{
			
		}
	}
}
