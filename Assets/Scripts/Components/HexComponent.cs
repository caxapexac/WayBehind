using System.Collections.Generic;
using Systems;
using LeopotamGroup.Pooling;
using Misc;
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
            Properties = new Dictionary<HexProperties, int>();
        }

        public HexTypes HexType;
        public IPoolObject Parent;
        public Dictionary<HexProperties, int> Properties;
    }
}