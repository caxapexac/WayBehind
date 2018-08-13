using Components;
using Leopotam.Ecs;
using Misc;
using TouchControlsKit;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class InputStickSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PlayerComponent _player;
        private EcsFilterSingle<PlayerComponent> _playerFilter = null;

        public void Initialize()
        {
            _player = _playerFilter.Data;
        }

        public void Run()
        {
            _player.Force = TCKInput.GetAxis("Joystick");
        }

        public void Destroy()
        {
            _player = null;
        }
    }
}