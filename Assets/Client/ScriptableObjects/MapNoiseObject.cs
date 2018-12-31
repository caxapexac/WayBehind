using Client.Scripts.Miscellaneous;
using UnityEngine;


namespace Client.ScriptableObjects
{
    [CreateAssetMenu]
    public class MapNoiseObject : ScriptableObject
    {
        [Header("Main")]
        [ContextMenuItem("Change seed", "RandomizeSeed")]
        public int Saeed;

        public float Size;
        public int PixelPerUnit;

        [Space]
        [Header("Height")]
        public NoiseTypes HNoiseType;

        public float HScale;

        [Range(0, 10)]
        public int HOctaves;

        [Range(0, 1)]
        public float HPersistance;

        [Range(0, 1)]
        public float HLacunarity;

        [Space]
        [Header("Temperatures")]
        public NoiseTypes TNoiseType;

        public float TScale;

        [Range(0, 10)]
        public int TOctaves;

        [Range(0, 1)]
        public float TPersistance;

        [Range(0, 1)]
        public float TLacunarity;

        [Space]
        [Header("Moisture")]
        public NoiseTypes MNoiseType;

        public float MScale;

        [Range(0, 10)]
        public int MOctaves;

        [Range(0, 1)]
        public float MPersistance;

        [Range(0, 1)]
        public float MLacunarity;

        private void RandomizeSeed()
        {
            Saeed = (int)(Random.value * 1000000);
        }
    }
}