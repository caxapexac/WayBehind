using Client.Scripts.Miscellaneous;
using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.Components
{
    sealed class EnemyComponent : IEcsAutoResetComponent
    {
        public HexCoords LastCoords; //todo patrol
        public Vector2 Force;
        public Vector2 Target;

        public HexComponent Hex;
        public Transform Head;
        public Transform Body;

        public void Reset()
        {
            Hex = null;
            Head = null;
            Body = null;
        }
    }
}