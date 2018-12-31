using Client.Scripts.OBSOLETE.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.Scripts.OBSOLETE.Systems
{
    [EcsInject]
    public class CameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private Camera _camera;
        private GameComponent _game;
        private PlayerComponent _player;
        private EcsWorld _world = null;
        private EcsFilter<GameComponent> _gameFilter = null;
        private EcsFilter<PlayerComponent> _playerFilter = null;
        //private E

        public void Initialize()
        {
            _game = _gameFilter.Components1[0];
            _player = _playerFilter.Components1[0];
            _camera = Camera.main;
            _camera.orthographicSize = _game.S.CameraSize;
        }

        public void Run()
        {
            float distance = Vector2.Distance(_camera.transform.localPosition, _player.Transform.localPosition);
            if (distance > _game.S.CameraDistance)
            {
                Vector2 diff = (Vector2) _player.Transform.localPosition - (Vector2) _camera.transform.localPosition;
                _camera.transform.Translate(diff * distance * _game.S.CameraSpeed * Time.deltaTime);
            }
        }

        public void Destroy()
        {
            _camera = null;
            _game = null;
        }
    }
}