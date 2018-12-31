using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.ComponentsOneFrame
{
    [EcsOneFrame]
    sealed class InputEvent
    {
        public Vector2 Direction;
    }
}