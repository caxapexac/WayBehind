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
        private EcsFilterSingle<PlayerComponent> _playerFilter = null;

        public void Initialize()
        {
            _player = _playerFilter.Data;
            GameObject.FindGameObjectWithTag(Tags.JoystickTag).SetActive(false);
        }

        public void Run()
        {
            _player.Force.x = Input.GetAxis("Horizontal");;
            _player.Force.y = Input.GetAxis("Vertical");;
        }

        public void Destroy()
        {
            _player = null;
        }
    }
}