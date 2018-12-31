using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.ComponentsOneFrame
{
    [EcsOneFrame]
    sealed class CollisionEvent
    {
        public Transform Sender;
        public Transform Other;
    }
}