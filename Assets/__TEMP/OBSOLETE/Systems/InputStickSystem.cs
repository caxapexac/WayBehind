using Client.Scripts.OBSOLETE.Components;
using Leopotam.Ecs;
using TouchControlsKit;

namespace Client.Scripts.OBSOLETE.Systems
{
    [EcsInject]
    public class InputStickSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PlayerComponent _player;
        private EcsFilter<PlayerComponent> _playerFilter = null;

        public void Initialize()
        {
            _player = _playerFilter.Components1[0];
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