using Client.ScriptableObjects;
using Client.Scripts.Components;
using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.Systems
{
    [EcsInject]
    public class CameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _playerFilter = null;
        private SettingsObject _settings = null;

        private Camera _camera;

        public void Initialize()
        {
            _camera = Camera.main;
        }

        public void Run()
        {
            _camera.orthographicSize = _settings.CameraSize;
            for (int i = 0; i < _playerFilter.EntitiesCount; i++)
            {
                float distance = Vector2.Distance(_camera.transform.localPosition,
                    _playerFilter.Components1[i].Transform.localPosition);
                if (distance >= _settings.CameraDistance)
                {
                    _camera.transform.position = Vector2.Lerp(_camera.transform.localPosition,
                        _playerFilter.Components1[i].Transform.localPosition,
                        Time.deltaTime * _settings.CameraSpeed);
                }
                return; //Only the first player
            }
        }

        public void Destroy()
        {
            _camera = null;
        }
    }
}