using Components;
using Leopotam.Ecs;
using Misc;
using TouchControlsKit;
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
            GameObject.FindGameObjectWithTag(Tags.JoystickTag).SetActive(false);
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