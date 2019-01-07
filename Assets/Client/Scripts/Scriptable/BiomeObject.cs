using UnityEngine;


namespace Client.Scripts.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/Biome")]
    public class BiomeObject : ScriptableObject
    {
        public string Name;
        public Sprite[] LandMap; //change to know slowing, enemies, mountains etc
    }
}