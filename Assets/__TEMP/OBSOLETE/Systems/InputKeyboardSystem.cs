using Client.Scripts.OBSOLETE.Components;
using Client.Scripts.OBSOLETE.Misc;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.Scripts.OBSOLETE.Systems
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
            _player.Force.x = Input.GetAxis("Horizontal");;
            _player.Force.y = Input.GetAxis("Vertical");;
        }

        public void Destroy()
        {
            _player = null;
        }
    }
}