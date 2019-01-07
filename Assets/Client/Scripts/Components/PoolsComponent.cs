using Client.Scripts.Algorithms.Legacy;
using Client.Scripts.MonoBehaviours;
using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.Components
{
    public class PoolsComponent : IEcsAutoResetComponent
    {
        public Transform HexParent;
        public Transform SpiritParent;
        public PrefabPool<MonoHex> HexPool;
        public PrefabPool<MonoSpirit> SpiritPool;

        public void Reset()
        {
            HexParent = null;
            SpiritParent = null;
            HexPool.Dispose();
            SpiritPool.Dispose();
            HexPool = null;
            SpiritPool = null;
        }
    }
}