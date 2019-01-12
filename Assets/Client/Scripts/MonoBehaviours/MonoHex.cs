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
            int col = (int)(Hex.M * Biomes.Array[row].Row.Length);
            BiomeObject biome = Biomes.Array[row].Row[col];
            int index = (int)(Hex.H * biome.Heights.Length);
/*            Sprite sprite = biome.Heights.Length > 0
                ? biome.Heights[biome.Heights.Length - 1 - index] //reversed
                : Biomes.MissingSprite;
            Body.sprite = sprite;*///todo
        }
    }
}