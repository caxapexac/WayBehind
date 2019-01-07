using UnityEngine;


namespace Client.Scripts.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/BiomeStorage")]
    public class BiomeStorageObject : ScriptableObject
    {
        [System.Serializable]
        public struct RowStruct
        {
            public BiomeObject[] Row;
        }
        public RowStruct[] Array;

        public Sprite MissingSprite;
    }
}