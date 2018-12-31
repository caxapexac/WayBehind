using System.Collections;
using Client.ScriptableObjects;
using Client.Scripts.Algorithms;
using Client.Scripts.Algorithms.Noises;
using Client.Scripts.Miscellaneous;
using Client.Scripts.OBSOLETE.Misc;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Client.Scripts.MonoBehaviours
{
    public class MonoNoiseTester : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Renderer _hRenderer;
        [SerializeField] private Renderer _tRenderer;
        [SerializeField] private Renderer _mRenderer;
        [SerializeField] private MapNoiseObject _map;

        private float[,] _hMap;
        private float[,] _tMap;
        private float[,] _mMap;

        public bool AutoUpdate;
        public float AutoDelaySec;
        public BiomesGeneratorObject BiomesGenerator;


        public int PixelSize
        {
            get { return (int)(_map.Size * _map.PixelPerUnit); }
        }

        public void StartDraw()
        {
            Debug.Log("Start");
            StartCoroutine(Draw());
        }

        public void StopDraw()
        {
            Debug.Log("Stop");
            StopAllCoroutines();
        }

        private IEnumerator Draw()
        {
            while (true)
            {
                if (_hMap == null || AutoUpdate)
                {
                    GenerateMap();
                }
                Color[] colors = new Color[PixelSize * PixelSize];
                Color[] hColors = new Color[PixelSize * PixelSize];
                Color[] tColors = new Color[PixelSize * PixelSize];
                Color[] mColors = new Color[PixelSize * PixelSize];
                for (int i = 0; i < PixelSize; i++)
                {
                    for (int k = 0; k < PixelSize; k++)
                    {
                        colors[i * PixelSize + k] = new Color(Mathf.Sqrt(_hMap[i, k]), Mathf.Sqrt(_tMap[i, k]),
                            Mathf.Sqrt(_mMap[i, k]));
                        hColors[i * PixelSize + k] = Color.white * Mathf.Sqrt(_hMap[i, k]);
                        tColors[i * PixelSize + k] = Color.white * Mathf.Sqrt(_tMap[i, k]);
                        mColors[i * PixelSize + k] = Color.white * Mathf.Sqrt(_mMap[i, k]);
                    }
                }
                SetTexture(_renderer, colors);
                SetTexture(_hRenderer, hColors, 0.3f);
                SetTexture(_tRenderer, tColors, 0.3f);
                SetTexture(_mRenderer, mColors, 0.3f);
                yield return new WaitForSeconds(AutoDelaySec);
            }
        }

        private void SetTexture(Renderer curRenderer, Color[] colors, float multiSize = 1)
        {
            Texture2D texture = new Texture2D(PixelSize, PixelSize);
            texture.SetPixels(colors);
            texture.Apply();
            curRenderer.sharedMaterial.mainTexture = texture;
            curRenderer.transform.localScale = new Vector3(_map.Size * multiSize, 1, _map.Size * multiSize);
        }


        public void GenerateMap()
        {
            Random.InitState(_map.Saeed);
            _hMap = FractalArrayNoise.Get((int)(Random.value * 10000000), PixelSize, _map.HOctaves, _map.HPersistance);
            _tMap = FractalArrayNoise.Get((int)(Random.value * 10000000), PixelSize, _map.TOctaves, _map.TPersistance);
            _mMap = FractalArrayNoise.Get((int)(Random.value * 10000000), PixelSize, _map.MOctaves, _map.MPersistance);
        }

        public void RandomizeSeed()
        {
            _map.Saeed = (int)(Random.value * 100000000);
        }

        public void Randomize()
        {
            RandomizeSeed();
            _map.HNoiseType = RandomEnum.Value<NoiseTypes>();
            _map.HScale = Random.value;
            _map.HOctaves = (int)(Random.value * 9) + 1;
            _map.HPersistance = Random.value;
            _map.HLacunarity = Random.value;

            _map.TNoiseType = RandomEnum.Value<NoiseTypes>();
            _map.TScale = Random.value;
            _map.TOctaves = (int)(Random.value * 9) + 1;
            _map.TPersistance = Random.value;
            _map.TLacunarity = Random.value;

            _map.MNoiseType = RandomEnum.Value<NoiseTypes>();
            _map.MScale = Random.value;
            _map.MOctaves = (int)(Random.value * 9) + 1;
            _map.MPersistance = Random.value;
            _map.MLacunarity = Random.value;
            GenerateMap();
            StartDraw();
        }
    }
}