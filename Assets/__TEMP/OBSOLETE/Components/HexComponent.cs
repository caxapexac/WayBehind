using System.Collections.Generic;
using Client.Scripts.OBSOLETE.Misc;
using LeopotamGroup.Pooling;

namespace Client.Scripts.OBSOLETE.Components
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
            Properties = new Dictionary<HexProperties, int>();
        }

        public float Color;
        public HexTypes HexType;
        public IPoolObject Parent;
        public Dictionary<HexProperties, int> Properties;
    }
}