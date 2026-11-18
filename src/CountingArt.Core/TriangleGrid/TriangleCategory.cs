namespace CountingArt.Core.TriangleGrid;

/// <summary>The "kind" of a triangle used for counting by cases: its side length and its direction.</summary>
public readonly record struct TriangleCategory(Orientation Orientation, int Size) : IComparable<TriangleCategory>
{
    public int CompareTo(TriangleCategory other) =>
        Size != other.Size ? Size.CompareTo(other.Size) : Orientation.CompareTo(other.Orientation);
}
