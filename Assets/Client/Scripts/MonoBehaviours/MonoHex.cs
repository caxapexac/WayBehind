using Client.Scripts.Components;
using Client.Scripts.Scriptable;
using UnityEngine;
using UnityEngine.UI;


namespace Client.Scripts.MonoBehaviours
{
    [DisallowMultipleComponent]
    public class MonoHex : MonoBehaviour
    {
        public BiomeStorageObject Biomes;

        public HexComponent Hex;

        public SpriteRenderer Body;

        public Text DebugState; //todo remove

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (Hex != null)
            {
                Hex.Parent = this;
                transform.position = Hex.Position;
                Draw();
            }
        }

        public void Draw()
        {
            //Body.color = new Color((float)Hex.H, (float)Hex.T, (float)Hex.M);
            /*DebugState.text = Mathf.Round((float)(Hex.H * 100))
                + "|"
                + Mathf.Round((float)(Hex.T * 100))
                + "|"
                + Mathf.Round((float)(Hex.M * 100));*/
            int row = (int)(Hex.T * Biomes.Array.Length);
            if (row >= Biomes.Array.Length)
            {
                Debug.Log("T" + Hex.T + " " + Biomes.Array.Length + " " + row);
                Body.sprite = Biomes.MissingSprite;
                return;
            }
            int col = (int)(Hex.M * Biomes.Array[row].Row.Length);
            if (col >= Biomes.Array[row].Row.Length)
            {
                Debug.Log("M" + Hex.M + " " + Biomes.Array[row].Row.Length + " " + col);
                Body.sprite = Biomes.MissingSprite;
                return;
            }
            BiomeObject biome = Biomes.Array[row].Row[col];
            int index = (int)(Hex.H * biome.LandMap.Length);
            if (index >= biome.LandMap.Length)
            {
                Debug.Log("H" + Hex.H + " " + biome.LandMap.Length + " " + index);
                Body.sprite = Biomes.MissingSprite;
                return;
            }
            Sprite sprite = biome.LandMap.Length > 0
                ? biome.LandMap[biome.LandMap.Length - 1 - index]
                : Biomes.MissingSprite;
            Body.sprite = sprite;
        }
    }
}