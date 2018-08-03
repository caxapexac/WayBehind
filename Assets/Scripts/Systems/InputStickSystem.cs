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
        private EcsFilter<PlayerComponent> _playerFilter = null;

        public void Initialize()
        {
            _player = _playerFilter.Components1[0];
            GameObject.FindGameObjectWithTag(Tags.JoystickTag).GetComponent<TCKJoystick>().isEnable = true;
        }

        public void Run()
        {
            Vector2 move = TCKInput.GetAxis("Joystick");
            _player.Force.X = move.x;
            _player.Force.Y = move.y;
        }

        public void Destroy()
        {
            _player = null;
        }
    }
}