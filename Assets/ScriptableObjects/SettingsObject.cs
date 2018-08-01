using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu]
    public class SettingsObject : ScriptableObject
    {
        public GameObject PlayerPrefab;
        public Sprite[] Enemies;

        public float CameraSize = 8;
        public int FieldOfView = 8;
        public int MapSize = 100;
        public int MapSaeed = 112263;

        public float HexSize = 2;
        public float SpeedMultipiler = 1f;
        public float CameraDistance = 2f;
        public float CameraSpeed = 0.06f;
    }
}