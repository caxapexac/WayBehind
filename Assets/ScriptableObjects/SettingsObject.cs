using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu]
    public class SettingsObject : ScriptableObject
    {
        [Header("Map")]
        [ContextMenuItem("Change seed", "RandomizeSeed")]
        public int MapSaeed;
        [Range(1, 500)]
        public int MapSize;
        [Range(1, 500)]
        public int FieldOfView;
        [Range(0.1f, 10f)]
        public float HexSize;
        
        
        [Space]
        [Header("UI")]
        [Range(1, 50)]
        public float CameraSize = 8;
        [Range(0.1f, 10f)]
        public float CameraDistance;
        [Range(0.1f, 10f)]
        public float CameraSpeed;
        public bool IsTouchScreen;
        
        
        [Space]
        [Header("Player")]
        public GameObject PlayerPrefab;
        [Range(0.1f, 10f)]
        public float SpeedMultipiler;
        
        
        [Space]
        [Header("Enemies")]
        public Sprite[] EnemiesSprites;


        private void RandomizeSeed()
        {
            MapSaeed = (int) (Random.value * 1000000);
        }
        
        
        
        
        

        
        
        
    }
}