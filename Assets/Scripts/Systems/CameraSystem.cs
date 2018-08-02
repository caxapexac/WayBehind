using Components;
using Leopotam.Ecs;
using Misc;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class CameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        public float CameraSize;
        public float CameraDistance;
        public float CameraSpeed;

        private Camera _camera;
        private Transform _cameraTransform;
        private Transform _playerTransform;
        private EcsFilter<PlayerComponent> _playerFilter = null;

        public void Initialize()
        {
            _camera = Camera.main;
            _camera.orthographicSize = CameraSize;
            _cameraTransform = _camera.transform;
            _playerTransform = _playerFilter.Components1[0].Transform;
        }

        public void Run()
        {
            float distance = Vector2.Distance(_cameraTransform.position, _playerTransform.position);
            if (distance > CameraDistance)
            {
                _cameraTransform.Translate(((Vector2)_playerTransform.localPosition - (Vector2)_cameraTransform.localPosition)
                                           * distance * CameraSpeed * Time.deltaTime);
            }
        }

        public void Destroy()
        {
            _camera = null;
            _cameraTransform = null;
            _playerTransform = null;
        }
    }
}