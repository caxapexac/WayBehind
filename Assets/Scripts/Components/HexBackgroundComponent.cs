using Systems;
using LeopotamGroup.Pooling;
using UnityEngine;

namespace Components
{
    public sealed class HexBackgroundComponent
    {
        public HexBackgroundComponent()
        {
            BackgroundType = BackroundTypes.Grass;
            SpeedDown = 1f;
            Parent = null;
            IsNew = false;
        }
        public HexBackgroundComponent(HexBackgroundComponent original)
        {
            IsNew = original.IsNew;
            Parent = original.Parent;
            BackgroundType = original.BackgroundType;
            Collider = original.Collider;
            SpeedDown = original.SpeedDown;
        }
        
        public bool IsNew = true;
        public IPoolObject Parent = null;
        public BackroundTypes BackgroundType;
        public Collider2D Collider;
        public float SpeedDown;//TODO

    }
}
