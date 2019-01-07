using UnityEngine;


namespace Client.Scripts.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/Settings")]
    public class SettingsObject : ScriptableObject
    {
        [Header("Map")] [Range(0, 100)] public int Difficulty;
        public bool RealTime;
        
        
        [Header("Camera")]
        [Range(1, 50)] public float CameraSize;
        //[Range(0, 10f)] public float CameraBorders;
        [Range(0.1f, 10f)] public float CameraDistance;
        [Range(0.1f, 10f)] public float CameraSpeed;
        

        [Header("Player")] [Range(1, 100)] public int StartHp; //+
        [Range(0.1f, 10f)] public float SpeedMultipiler;
        [Range(0.001f, 10f)] public float LerpSpeed;
        [Range(0.001f, 10f)] public float LerpSlowing;

        [Header("Enemies")] public Sprite[] Enemies;

        [Header("UI")] public Sprite HeartSprite;
        public bool IsTouchScreen;
    }
}