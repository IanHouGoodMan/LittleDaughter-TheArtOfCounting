namespace CountingArt.Core.TriangleGrid;

/// <summary>Converts lattice coordinates into ordinary x/y coordinates (unit side length, y pointing up).</summary>
public static class GridGeometry
{
    public static readonly double RowHeight = Math.Sqrt(3) / 2;

    public static (double X, double Y) ToCartesian(LatticePoint point) =>
        (point.A + point.B / 2.0, point.B * RowHeight);
}
