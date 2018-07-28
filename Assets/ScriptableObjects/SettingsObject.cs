using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu]
    public class SettingsObject : ScriptableObject
    {
        public GameObject PlayerPrefab;
        public Sprite Spawn;
        public Sprite Enemy;
        public Sprite Diamond;
        public Sprite Obstacle;
        public Sprite Water;
        public Sprite Grass;
        public Sprite Swamp;
        public Sprite Forest;
        
        public int FieldOfView = 8;
        public int MapSize = 100;
        public int MapSeed = 112263;
        
        public float HexSize = 2;
        public float SpeedMultipiler = 1f;
        public float CameraDistance = 2f;
        public float CameraSpeed = 0.06f;
    }
}
