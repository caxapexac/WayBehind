using UnityEngine;


namespace Client.Scripts.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/Noise")]
    public class NoiseObject : ScriptableObject
    {
        [ContextMenuItem("Change seed", "RandomizeSeed")]
        public int Saeed;

        [Range(0, 16)] public int Octaves;
        [Range(0, 2)] public double Frequency;
        [Range(0, 2)] public double Lacunarity;
        [Range(0, 1)] public double Amplitude;
        [Range(0, 1)] public double Persistance;

        private bool _isNew = true;
        private float _xOffset;
        private float _yOffset;

        public void RandomizeSeed()
        {
            Saeed = (int)(Random.value * 1000000);
            _isNew = true;
        }

        public void GetOffset(out float x, out float y)
        {
            if (_isNew)
            {
                Random.InitState(Saeed);
                _xOffset = Random.value * 1000000;
                _yOffset = Random.value * 1000000;
                _isNew = false;
            }
            x = _xOffset;
            y = _yOffset;
        }
    }
}