namespace CountingArt.Core.TriangleGrid;

/// <summary>
/// Closed-form answers, used to check the brute-force enumeration ("counting in two ways").
/// </summary>
public static class TriangleFormulas
{
    /// <summary>
    /// For a parallelogram of the given width and rows, an up (or down) triangle of side k fits in
    /// (width - k + 1)(rows - k + 1) positions, so the total is 2 Σ (width - k + 1)(rows - k + 1).
    /// With 3 rows and width n ≥ 2 this is 12n - 8.
    /// </summary>
    public static long Parallelogram(int width, int rows)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(rows, 1);

        long total = 0;
        for (var k = 1; k <= Math.Min(width, rows); k++)
        {
            total += 2L * (width - k + 1) * (rows - k + 1);
        }

        return total;
    }

    /// <summary>Triangles in a big triangle of side n: ⌊n(n + 2)(2n + 1) / 8⌋ (OEIS A002717).</summary>
    public static long Triangle(int size)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(size, 1);
        long n = size;
        return n * (n + 2) * (2 * n + 1) / 8;
    }
}
