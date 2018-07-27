using Systems;
using LeopotamGroup.Pooling;
using UnityEngine;

namespace Components
{
    public sealed class HexForegroundComponent
    {
        public bool IsNew = true;
        public IPoolObject Parent;
        public ForegroundTypes ObjectType;
        public Collider2D Collider;
        public int Hp;
    }
}
