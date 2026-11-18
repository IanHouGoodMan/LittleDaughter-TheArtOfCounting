namespace CountingArt.Core.TriangleGrid;

public enum SubmissionResult
{
    /// <summary>A triangle of the figure that had not been found yet.</summary>
    NewTriangle,

    /// <summary>A triangle of the figure that was already found.</summary>
    AlreadyFound,

    /// <summary>The three points are not the corners of an equilateral grid triangle.</summary>
    NotAGridTriangle,

    /// <summary>The points form a grid triangle, but some of its sides are not drawn in the figure.</summary>
    SidesNotDrawn,
}

public sealed record CategoryProgress(TriangleCategory Category, int Found, int Total)
{
    public int Missing => Total - Found;

    public bool IsComplete => Found == Total;
}

/// <summary>
/// One attempt at "how many triangles are there?": the child submits triangles one by one and can ask
/// which kinds (size × direction) still have missing triangles, without being told the answer.
/// </summary>
public sealed class CountingSession
{
    private readonly HashSet<GridTriangle> _all;
    private readonly List<GridTriangle> _foundInOrder = [];
    private readonly HashSet<GridTriangle> _found = [];

    public CountingSession(TriangleGridFigure figure)
    {
        Figure = figure ?? throw new ArgumentNullException(nameof(figure));
        AllTriangles = figure.FindAllTriangles();
        _all = [.. AllTriangles];
    }

    public TriangleGridFigure Figure { get; }

    public IReadOnlyList<GridTriangle> AllTriangles { get; }

    /// <summary>Found triangles in the order the child found them.</summary>
    public IReadOnlyList<GridTriangle> Found => _foundInOrder;

    public bool IsComplete => _found.Count == _all.Count;

    public bool HasFound(GridTriangle triangle) => _found.Contains(triangle);

    public SubmissionResult Submit(LatticePoint p, LatticePoint q, LatticePoint r, out GridTriangle triangle)
    {
        if (!GridTriangle.TryFromVertices(p, q, r, out triangle))
        {
            return SubmissionResult.NotAGridTriangle;
        }

        return Submit(triangle);
    }

    public SubmissionResult Submit(GridTriangle triangle)
    {
        if (!_all.Contains(triangle))
        {
            return SubmissionResult.SidesNotDrawn;
        }

        if (!_found.Add(triangle))
        {
            return SubmissionResult.AlreadyFound;
        }

        _foundInOrder.Add(triangle);
        return SubmissionResult.NewTriangle;
    }

    /// <summary>Progress for every kind of triangle that occurs in the figure, smallest first.</summary>
    public IReadOnlyList<CategoryProgress> Progress() =>
        AllTriangles
            .GroupBy(t => t.Category)
            .OrderBy(g => g.Key)
            .Select(g => new CategoryProgress(g.Key, g.Count(_found.Contains), g.Count()))
            .ToArray();
}
