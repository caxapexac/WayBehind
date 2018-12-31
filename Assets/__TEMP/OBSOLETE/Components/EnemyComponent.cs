using Client.Scripts.OBSOLETE.Misc;
using UnityEngine;

namespace Client.Scripts.OBSOLETE.Components
{
    sealed class EnemyComponent
    {
        public HexaCoords LastCoords; //todo patrol
        public Vector2 Force;
        public Vector2 Target;
        public HexComponent Hex;
        public Transform Head;
        public Transform Body;
    }
}
