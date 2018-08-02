using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class InputKeyboardSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PlayerComponent _player;
        private EcsFilter<PlayerComponent> _playerFilter = null;

        public void Initialize()
        {
            _player = _playerFilter.Components1[0];
        }

        public void Run()
        {
            _player.Force.X = Input.GetAxis("Horizontal");;
            _player.Force.Y = Input.GetAxis("Vertical");;
        }

        public void Destroy()
        {
            _player = null;
        }
    }
}