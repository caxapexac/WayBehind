using Systems;
using LeopotamGroup.Pooling;
using UnityEngine;

namespace Components
{
    public enum HexTypes
    {
        Grass,
        Water,
        Forest,
        Swamp,
        Obstacle,
        Diamond,
        Enemy,
        Empty,
        Spawn,
    }

    public class HexComponent
    {
        public HexComponent()
        {
            Parent = null;
            HexType = HexTypes.Empty;
        }

        public HexComponent(HexComponent original)
        {
            Parent = original.Parent;
            HexType = original.HexType;
        }

        public HexTypes HexType;
        public IPoolObject Parent;
        public object Properties; //TODO
    }
}