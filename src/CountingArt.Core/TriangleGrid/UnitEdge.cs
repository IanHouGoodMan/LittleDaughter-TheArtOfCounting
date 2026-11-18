namespace CountingArt.Core.TriangleGrid;

/// <summary>A segment of length 1 between two neighbouring lattice points, stored in a canonical order.</summary>
public readonly record struct UnitEdge
{
    public UnitEdge(LatticePoint p, LatticePoint q)
    {
        if (!AreNeighbours(p, q))
        {
            throw new ArgumentException($"{p} and {q} are not neighbouring lattice points.");
        }

        (P, Q) = p.CompareTo(q) <= 0 ? (p, q) : (q, p);
    }

    public LatticePoint P { get; }

    public LatticePoint Q { get; }

    public static bool AreNeighbours(LatticePoint p, LatticePoint q)
    {
        var (da, db) = (q.A - p.A, q.B - p.B);
        return (da, db) is (1, 0) or (-1, 0) or (0, 1) or (0, -1) or (-1, 1) or (1, -1);
    }
}
