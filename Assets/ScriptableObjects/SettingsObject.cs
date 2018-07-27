using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu]
    public class SettingsObject : ScriptableObject
    {
        public Sprite Hexagon;
        public Sprite Obstacle;
        public Sprite Water;
        public Sprite Grass;
        public Sprite Boloto;
        public GameObject PlayerPrefab;
        public int FieldOfView = 8;
        public float HexSize = 2;
        public float SpeedMultipiler = 1f;
        public float CameraDistance = 2f;
        public float CameraSpeed = 0.06f;
    }
}
