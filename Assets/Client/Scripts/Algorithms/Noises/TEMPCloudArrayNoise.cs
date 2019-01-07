namespace Client.Scripts.Algorithms.Noises
{
    public class CloudyNoiseTest
    {
        /*
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;

    private List<PointyHexPoint> Remainders = new List<PointyHexPoint>
    {
        new PointyHexPoint(0, -1),
        new PointyHexPoint(1, -1),
        new PointyHexPoint(-1, 0),
        new PointyHexPoint(0, 0),
        new PointyHexPoint(1, 0),
        new PointyHexPoint(-1, 1),
        new PointyHexPoint(0, 1)
    };

    public override void InitGrid()
    {
        Remainders = Remainders.OrderBy(p => p.GetColor3_7()).ToList();

        FinalNoise();
    }

    private IGrid<float, PointyHexPoint> MakeCloudyNoise()
    {
        var noise0 = MakeWhiteNoise();
        var noise1 = MakeSmoothNoise(2, Div2);
        var noise2 = MakeSmoothNoise(3, Div4);
        var noise3 = MakeSmoothNoise(4, Div8);

        var cloudyNoise = Grid.CloneStructure<float>();
        
        foreach (var point in cloudyNoise)
        {
            float v = (noise0[point] + noise1[point]*2 + noise2[point]*4 + noise3[point]*8)/15f;
            cloudyNoise[point] = v;
        }

        return cloudyNoise;
    }

    public IGrid<float, PointyHexPoint> MakeSmoothNoise(int k, Func<PointyHexPoint, PointyHexPoint> div)
    {
        var noise = MakeWhiteNoise();
        var sampledNoise = noise.CloneStructure<float>();

        foreach (var point in Grid)
        {
            sampledNoise[point] = noise[div(point)];
        }

        var blurredNoise = Blur(sampledNoise, k);

        return blurredNoise;
    }

    private float Blur(IGrid<float, PointyHexPoint> grid, PointyHexPoint center, IEnumerable<PointyHexPoint> neighbors)
    {
        float sum = grid[center];

        foreach (var neighbor in neighbors)
        {
            sum += grid[neighbor];
        }

        return sum / (neighbors.Count() + 1);
    }

    private void FinalNoise()
    {
        var gridChoice = MakeCloudyNoise();
        var choice0 = MakeCloudyNoise();
        var choice1 = MakeCloudyNoise();

        foreach (var point in Grid)
        {
            int choice = Mathf.FloorToInt(gridChoice[point]*2);
            Grid[point].Color =
                Color.Lerp(
                    Color.Lerp(color1, color2, choice0[point]),
                    Color.Lerp(color3, color4, choice0[point]),
                    choice1[point]);
        }
    }
    
    private PointyHexPoint Div2(PointyHexPoint point)
    {
        int remainderIndex = point.GetColor3_7();
        var evenPoint = point - Remainders[remainderIndex];
        var a = evenPoint.Y;
        var b = Mathi.Div(evenPoint.X - 2*a, 7);

        a = a + 3*b;
        b = -b;

        return new PointyHexPoint(a, b);
    }

    private PointyHexPoint Div4(PointyHexPoint point)
    {
        return Div2(Div2(point));
    }

    private PointyHexPoint Div8(PointyHexPoint point)
    {
        return Div2(Div4(point));
    }

    public IGrid<float, PointyHexPoint> Blur(IGrid<float, PointyHexPoint> grid, int n)
    {
        float patchArea = Mathf.Pow(7, n - 1);
        int side = Mathf.FloorToInt((3 + Mathf.Sqrt(9 - 4*3*(1-patchArea)))/(2*3));
        var blurredGrid = grid.CloneStructure<float>();
        var blurBox = PointyHexGrid<float>.Hexagon(side);

        foreach (var point in grid)
        {
            float sum = 0;
            int sampleCount = 0;

            foreach (var offset in blurBox)
            {
                var samplePoint = point + offset;
                
                if (Grid.Contains(samplePoint))
                {
                    sum += grid[samplePoint];
                    sampleCount++;
                }

                blurredGrid[point] = sum/sampleCount;
            }
        }

        return blurredGrid;
    }
    */
    }
}