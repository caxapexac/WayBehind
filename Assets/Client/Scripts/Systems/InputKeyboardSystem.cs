using Client.Scripts.ComponentsOneFrame;
using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.Systems
{
    [EcsInject]
    public class InputKeyboardSystem : IEcsRunSystem
    {
        private EcsWorld _world = null;

        public void Run()
        {
            InputEvent iEvent = _world.CreateEntityWith<InputEvent>();
            iEvent.Direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}