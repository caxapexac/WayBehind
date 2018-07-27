using Systems;
using LeopotamGroup.Pooling;
using UnityEngine;

namespace Components
{
    public sealed class HexBackgroundComponent
    {
        public bool IsNew = true;
        public IPoolObject Parent = null;
        public BackroundTypes GroundType;
        public Collider2D Collider;
        public float SpeedDown;//TODO

    }
}
