using Systems;
using LeopotamGroup.Pooling;
using UnityEngine;

namespace Components
{
    public sealed class HexForegroundComponent
    {
        public HexForegroundComponent()
        {
            ForegroundType = ForegroundTypes.Empty;
            Parent = null;
            IsNew = false;
            Value = 0;
        }
        public HexForegroundComponent(HexForegroundComponent original)
        {
            IsNew = original.IsNew;
            Parent = original.Parent;
            ForegroundType = original.ForegroundType;
            Collider = original.Collider;
            Value = original.Value;
        }
        public bool IsNew = true;
        public IPoolObject Parent;
        public ForegroundTypes ForegroundType;
        public Collider2D Collider;
        public int Value;
    }
}
