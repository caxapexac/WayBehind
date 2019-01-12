using UnityEngine;


namespace Client.Scripts.Miscellaneous
{
    /// <summary>
    /// Ущербный класс, из которого в конце концов должны исчезнуть все поля, т.к. сейчас
    /// я не смог их никуда запихнуть
    /// </summary>
    public class Variables
    {
        public float HexSize = 0.32f; //GetComponent<SpriteRenderer>.bounds.size.x,y
        public Vector2 CameraMinBound;
        public Vector2 CameraMaxBound;

        public int DebugChunksCount = 0;
    }
}