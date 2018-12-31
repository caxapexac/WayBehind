﻿using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.ComponentsOneFrame
{
    [EcsOneFrame]
    sealed class TriggerEvent
    {
        public Transform Sender;
        public Transform Other;
    }
}