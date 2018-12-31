using Client.Scripts.ComponentsOneFrame;
using Leopotam.Ecs;
using TouchControlsKit;


namespace Client.Scripts.Systems
{
    [EcsInject]
    public class InputStickSystem : IEcsRunSystem
    {
        private EcsWorld _world = null;

        public void Run()
        {
            InputEvent iEvent = _world.CreateEntityWith<InputEvent>();
            iEvent.Direction = TCKInput.GetAxis("Joystick");
        }
    }
}