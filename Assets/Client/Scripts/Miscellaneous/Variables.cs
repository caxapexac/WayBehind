using UnityEngine;


namespace Client.Scripts.Miscellaneous
{
    public class Variables
    {
        public float HexSize = 0.32f; //GetComponent<SpriteRenderer>.bounds.size.x,y
        public Vector2 CameraMinBound;
        public Vector2 CameraMaxBound;

        public int DebugChunksCount = 0;
    }
}