using Client.Scripts.Miscellaneous;
using Client.Scripts.MonoBehaviours;
using LeopotamGroup.Collections;
using UnityEngine;


namespace Client.Scripts.Components
{
    public class HexComponent
    {
        public OffsetCoords Chunk;
        public HexCoords HexCoords;


        public double H; //height
        public double T; //temperature
        public double M; //moisture

        //public float Slowing; //? maybe Matf.Abs(H - 0.5f)

        public readonly Vector2 Position;

        public HexComponent(Vector2 position)
        {
            Position = position;
            Parent = null;

            //HexType = HexTypes.Empty;
            //Data = new FastList<SpiritComponent>();
        }

        public HexTypes HexType;
        public MonoHex Parent;
        public FastList<SpiritComponent> Data;
    }
}