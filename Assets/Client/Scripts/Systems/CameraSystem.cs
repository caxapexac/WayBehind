using Client.Scripts.Components;
using Client.Scripts.Miscellaneous;
using Client.Scripts.Scriptable;
using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.Systems
{
    [EcsInject]
    public class CameraSystem : IEcsRunSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _playerFilter = null;
        private SettingsObject _settings = null;
        private Variables _variables = null;
        private Camera _camera = null;

        public void Run()
        {
            if (_settings.CameraSize != _camera.orthographicSize)
            {
                _camera.orthographicSize = _settings.CameraSize;
            }
            _variables.CameraMinBound =
                _camera.ViewportToWorldPoint(Vector3.zero) - Vector3.one * 8 * _variables.HexSize;
            _variables.CameraMaxBound =
                _camera.ViewportToWorldPoint(Vector3.one) + Vector3.one * 8 * _variables.HexSize;

            for (int i = 0; i < _playerFilter.EntitiesCount; i++)
            {
                float distance = Vector2.Distance(_camera.transform.localPosition,
                    _playerFilter.Components1[i].Parent.transform.localPosition);
                if (distance >= _settings.CameraDistance)
                {
                    _camera.transform.position = Vector3.Lerp(_camera.transform.localPosition,
                        _playerFilter.Components1[i].Parent.transform.localPosition + Vector3.back * 10,
                        Time.deltaTime * _settings.CameraSpeed);
                }
                return; //Only the first player
            }
        }
    }
}