namespace CountingArt.Core.TriangleGrid;

/// <summary>
/// An equilateral triangle whose sides lie on grid lines.
/// Up:   (A, B), (A + Size, B), (A, B + Size).
/// Down: (A + Size, B), (A, B + Size), (A + Size, B + Size).
/// </summary>
public readonly record struct GridTriangle(Orientation Orientation, int A, int B, int Size)
    : IComparable<GridTriangle>
{
    public int Size { get; init; } = Size >= 1
        ? Size
        : throw new ArgumentOutOfRangeException(nameof(Size), Size, "Size must be at least 1.");

    public TriangleCategory Category => new(Orientation, Size);

    public IReadOnlyList<LatticePoint> Vertices => Orientation == Orientation.Up
        ? [new(A, B), new(A + Size, B), new(A, B + Size)]
        : [new(A + Size, B), new(A, B + Size), new(A + Size, B + Size)];

    public static GridTriangle Up(int a, int b, int size = 1) => new(Orientation.Up, a, b, size);

    public static GridTriangle Down(int a, int b, int size = 1) => new(Orientation.Down, a, b, size);

    /// <summary>All unit segments that make up the three sides.</summary>
    public IEnumerable<UnitEdge> SideEdges()
    {
        var v = Vertices;
        for (var i = 0; i < 3; i++)
        {
            var from = v[i];
            var to = v[(i + 1) % 3];
            var stepA = (to.A - from.A) / Size;
            var stepB = (to.B - from.B) / Size;
            for (var k = 0; k < Size; k++)
            {
                yield return new UnitEdge(from.Offset(k * stepA, k * stepB), from.Offset((k + 1) * stepA, (k + 1) * stepB));
            }
        }
    }

    /// <summary>
    /// Identifies the grid triangle with the given three corners (in any order).
    /// Returns false when the points are not the corners of an up or down equilateral grid triangle.
    /// </summary>
    public static bool TryFromVertices(LatticePoint p, LatticePoint q, LatticePoint r, out GridTriangle triangle)
    {
        triangle = default;
        LatticePoint[] pts = [p, q, r];
        Array.Sort(pts);
        var (v0, v1, v2) = (pts[0], pts[1], pts[2]);

        if (v0.B == v1.B && v2.B > v1.B)
        {
            var size = v1.A - v0.A;
            if (size >= 1 && v2.B - v0.B == size && v2.A == v0.A)
            {
                triangle = Up(v0.A, v0.B, size);
                return true;
            }
        }
        else if (v1.B == v2.B && v1.B > v0.B)
        {
            var size = v2.A - v1.A;
            if (size >= 1 && v1.B - v0.B == size && v0.A == v2.A)
            {
                triangle = Down(v1.A, v0.B, size);
                return true;
            }
        }

        return false;
    }

    public int CompareTo(GridTriangle other)
    {
        var byCategory = Category.CompareTo(other.Category);
        if (byCategory != 0)
        {
            return byCategory;
        }

        return B != other.B ? B.CompareTo(other.B) : A.CompareTo(other.A);
    }
}
