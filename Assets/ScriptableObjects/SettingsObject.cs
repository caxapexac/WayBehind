using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class SettingsObject : ScriptableObject
    {
        [Header("Map")] [ContextMenuItem("Change seed", "RandomizeSeed")]
        public bool UseSeed;
        public int MapSaeed;
        [Range(1, 500)]
        public int MapSize;
        [Range(1, 500)]
        public int FieldOfView;
        [Range(0.1f, 10f)]
        public float HexSize;
        
        
        [Space]
        [Header("Camera")]
        [Range(1, 50)]
        public float CameraSize = 8;
        [Range(0.1f, 10f)]
        public float CameraDistance;
        [Range(0.1f, 10f)]
        public float CameraSpeed;
        
        [Space]
        [Header("Player")]
        public GameObject PlayerPrefab;
        [Range(1, 100)]
        public int Hp;
        [Range(0.1f, 10f)]
        public float SpeedMultipiler;
        [Range(0.001f, 10f)]
        public float LerpSpeed;
        [Range(0.001f, 10f)]
        public float LerpSlowing;
        
        [Space]
        [Header("Ground")]
        [Range(0.01f, 1f)]
        public float GrassSpeed;
        [Range(0.01f, 1f)]
        public float WaterSpeed;
        [Range(0.01f, 1f)]
        public float ForestSpeed;
        [Range(0.01f, 1f)]
        public float SwampSpeed;

        
        [Space]
        [Header("Enemies")]
        public Sprite[] Enemies;


        [Space]
        [Header("UI")]
        public Sprite HeartSprite;
        public bool IsTouchScreen;

        
        private void RandomizeSeed()
        {
            MapSaeed = (int) (Random.value * 1000000);
        }
        
        
        
        
        

        
        
        
    }
}