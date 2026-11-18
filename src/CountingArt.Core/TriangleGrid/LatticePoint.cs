namespace CountingArt.Core.TriangleGrid;

/// <summary>
/// A point of the triangular lattice, written in the basis e1 = (1, 0) and e2 = (1/2, √3/2).
/// <see cref="B"/> is the row (height) and <see cref="A"/> is the position along the row.
/// </summary>
public readonly record struct LatticePoint(int A, int B) : IComparable<LatticePoint>
{
    public LatticePoint Offset(int da, int db) => new(A + da, B + db);

    public int CompareTo(LatticePoint other) =>
        B != other.B ? B.CompareTo(other.B) : A.CompareTo(other.A);

    public override string ToString() => $"({A},{B})";
}
