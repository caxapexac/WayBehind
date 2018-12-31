using System.Collections.Generic;
using UnityEngine;


namespace Client.ScriptableObjects
{
    [CreateAssetMenu]
    public class BiomesGeneratorObject : ScriptableObject
    {
        [System.Serializable]
        public struct RowData
        {
            public List<BiomeObject> Row;
        }
        public List<RowData> Data = new List<RowData>();
    }
}