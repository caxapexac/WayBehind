using UnityEngine;

namespace Client.ScriptableObjects
{   
    [CreateAssetMenu]
    public class SettingsObject : ScriptableObject
    {
        [Header("Map")]
        [ContextMenuItem("Change seed", "RandomizeSeed")]
        public bool UseSeed;
        public bool UseTextures;
        public int MapSaeed;
        [Range(1, 10)]
        public int Octaves;
        [Range(0.1f, 1f)]
        public float Smoothing;
        [Range(0.1f, 1f)]
        public float Persistance;
        [Range(1, 250)]
        public int MapSize;
        [Range(1, 250)]
        public int FieldOfView;
        [Range(0, 250)] 
        public int EnemyCount;
        [HideInInspector]
        public float HexSize = 1f; //GetComponent<SpriteRenderer>.bounds.size.x,y
        
        [Space]
        [Header("Camera")]
        [Range(1, 50)]
        public float CameraSize = 8;
        [Range(0.1f, 10f)]
        public float CameraDistance;
        [Range(0.1f, 10f)]
        public float CameraSpeed;
        [Range(0, 10f)]
        public float CameraBorders;
        
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

        [Space]
        [Header("Prefabs")]
        public GameObject HexPrefab;
        public GameObject SpiritPrefab;

        private void RandomizeSeed()
        {
            MapSaeed = (int) (Random.value * 1000000);
        }
    }
}