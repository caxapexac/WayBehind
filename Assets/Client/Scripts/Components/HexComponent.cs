using Client.Scripts.Miscellaneous;
using Client.Scripts.MonoBehaviours;
using LeopotamGroup.Collections;
using UnityEngine;


namespace Client.Scripts.Components
{
    public class HexComponent
    {
        public readonly Vector2 Position;
        public OffsetCoords Chunk;
        public HexCoords HexCoords;


        public double H; //height
        public double T; //temperature
        public double M; //moisture

        public float Speed;
        public float HasHead;

        public MonoHex Parent;
        public FastList<SpiritComponent> Data;

        public HexComponent(Vector2 position)
        {
            Position = position;
            Parent = null;

            //Data = new FastList<SpiritComponent>();
        }
    }
}