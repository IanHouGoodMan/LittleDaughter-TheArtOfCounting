namespace CountingArt.Core.TriangleGrid;

/// <summary>
/// A figure drawn on the triangular grid, made of unit cells (small triangles).
/// A triangle "is in the figure" when every unit segment of its three sides is drawn.
/// </summary>
public sealed class TriangleGridFigure
{
    private readonly HashSet<GridTriangle> _cells;
    private readonly HashSet<UnitEdge> _edges;
    private IReadOnlyList<GridTriangle>? _allTriangles;

    public TriangleGridFigure(IEnumerable<GridTriangle> cells)
    {
        ArgumentNullException.ThrowIfNull(cells);
        _cells = [];
        _edges = [];
        foreach (var cell in cells)
        {
            if (cell.Size != 1)
            {
                throw new ArgumentException("A figure is built from unit cells (Size = 1).", nameof(cells));
            }

            if (_cells.Add(cell))
            {
                _edges.UnionWith(cell.SideEdges());
            }
        }

        if (_cells.Count == 0)
        {
            throw new ArgumentException("A figure needs at least one cell.", nameof(cells));
        }

        Points = _edges.SelectMany(e => new[] { e.P, e.Q }).Distinct().Order().ToArray();
    }

    public IReadOnlySet<GridTriangle> Cells => _cells;

    public IReadOnlySet<UnitEdge> Edges => _edges;

    /// <summary>All grid points that are corners of at least one cell, sorted bottom row first.</summary>
    public IReadOnlyList<LatticePoint> Points { get; }

    public bool IsDrawn(GridTriangle triangle) => triangle.SideEdges().All(_edges.Contains);

    /// <summary>Enumerates every triangle in the figure by brute force, sorted by size, direction, then position.</summary>
    public IReadOnlyList<GridTriangle> FindAllTriangles() => _allTriangles ??= Enumerate();

    private GridTriangle[] Enumerate()
    {
        var minA = Points.Min(p => p.A);
        var maxA = Points.Max(p => p.A);
        var minB = Points.Min(p => p.B);
        var maxB = Points.Max(p => p.B);

        var found = new List<GridTriangle>();
        for (var size = 1; size <= maxB - minB; size++)
        {
            for (var b = minB; b + size <= maxB; b++)
            {
                for (var a = minA - size; a <= maxA; a++)
                {
                    foreach (var candidate in new[] { GridTriangle.Up(a, b, size), GridTriangle.Down(a, b, size) })
                    {
                        if (IsDrawn(candidate))
                        {
                            found.Add(candidate);
                        }
                    }
                }
            }
        }

        found.Sort();
        return [.. found];
    }

    /// <summary>
    /// A parallelogram strip: <paramref name="rows"/> rows, each <paramref name="width"/> units long,
    /// with both slanted sides leaning the same way (like the worksheet figure).
    /// </summary>
    public static TriangleGridFigure Parallelogram(int width, int rows)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(rows, 1);

        var cells = new List<GridTriangle>();
        for (var b = 0; b < rows; b++)
        {
            for (var a = 0; a < width; a++)
            {
                cells.Add(GridTriangle.Up(a, b));
                cells.Add(GridTriangle.Down(a, b));
            }
        }

        return new TriangleGridFigure(cells);
    }

    /// <summary>
    /// A trapezoid whose bottom side has length <paramref name="bottomWidth"/> and whose sides lean inwards;
    /// each row is one unit shorter than the row below it.
    /// </summary>
    public static TriangleGridFigure Trapezoid(int bottomWidth, int rows)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(rows, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(bottomWidth, rows);

        var cells = new List<GridTriangle>();
        for (var b = 0; b < rows; b++)
        {
            var rowLength = bottomWidth - b;
            for (var a = 0; a < rowLength; a++)
            {
                cells.Add(GridTriangle.Up(a, b));
                if (a < rowLength - 1)
                {
                    cells.Add(GridTriangle.Down(a, b));
                }
            }
        }

        return new TriangleGridFigure(cells);
    }

    /// <summary>A big triangle of side <paramref name="size"/> cut into unit triangles.</summary>
    public static TriangleGridFigure Triangle(int size) => Trapezoid(size, size);
}
