using Client.Scripts.Miscellaneous;
using LeopotamGroup.Collections;
using LeopotamGroup.Pooling;


namespace Client.Scripts.Components
{
    public class HexComponent
    {
        public float H; //height
        public float T; //temperature
        public float M; //moisture
        public float Slowing; //? maybe Matf.Abs(H - 0.5f)

        public HexComponent()
        {
            Parent = null;
            HexType = HexTypes.Empty;
            Data = new FastList<SpiritComponent>();
        }

        public HexTypes HexType;
        public IPoolObject Parent;
        public FastList<SpiritComponent> Data;
    }
}