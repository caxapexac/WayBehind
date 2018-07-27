using Systems;
using Components;

namespace Misc
{
    public static class MapGenRandomNeighbours
    {
        public static HexList4D<HexBackgroundComponent> GenerateBackground(int capacity)
        {
            HexList4D<HexBackgroundComponent> back = new HexList4D<HexBackgroundComponent>(capacity);
            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    back.Add(i, j, new HexBackgroundComponent()
                    {
                        GroundType = BackroundTypes.Grass,
                        IsNew = false
                    });
                }
            }
            return back;
        }

        public static HexList4D<HexForegroundComponent> GenerateForeground(HexList4D<HexBackgroundComponent> back)
        {
            HexList4D<HexForegroundComponent> fore = new HexList4D<HexForegroundComponent>(back.CapacityMin());
            fore.Add(0, 0, new HexForegroundComponent()
            {
                ObjectType = ForegroundTypes.Player,
                IsNew = false
            });
            fore.Add(2, 1, new HexForegroundComponent()
            {
                ObjectType = ForegroundTypes.Obstacle,
                IsNew = false
            });
            return fore;
        }

        public static void GenerateHex(CubeCoords coords,
            HexList4D<HexBackgroundComponent> back, HexList4D<HexForegroundComponent> fore)
        {
            back.Add(coords.x, coords.y, new HexBackgroundComponent()
            {
                GroundType = BackroundTypes.Grass,
                IsNew = false
            });
        }
    }
}