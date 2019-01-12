using UnityEngine;


namespace Client.Scripts.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/Hex")]
    public class HexObject : ScriptableObject
    {
        public string Name;
        public float Speed;
        [Range(0, 1)]
        public float HeadChance;
        public Sprite Head;
        public Sprite Body;
        public Sprite Feet;
    }
}