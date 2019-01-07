using UnityEngine;


namespace Client.Scripts.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/MapNoise")]
    public class MapNoiseObject : ScriptableObject
    {
        [ContextMenuItem("Change seed", "RandomizeSeed")]
        public NoiseObject H;

        [ContextMenuItem("Change seed", "RandomizeSeed")]
        public NoiseObject T;

        [ContextMenuItem("Change seed", "RandomizeSeed")]
        public NoiseObject M;
        
        private void RandomizeSeed()
        {
            H.RandomizeSeed();
            T.RandomizeSeed();
            M.RandomizeSeed();
        }
    }
}