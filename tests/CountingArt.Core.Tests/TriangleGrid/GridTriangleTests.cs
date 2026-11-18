using CountingArt.Core.TriangleGrid;

namespace CountingArt.Core.Tests.TriangleGrid;

public class GridTriangleTests
{
    [Theory]
    [InlineData(0, 0, 1)]
    [InlineData(2, 1, 3)]
    [InlineData(-1, 4, 2)]
    public void TryFromVertices_RoundTripsUpAndDownTriangles_InAnyVertexOrder(int a, int b, int size)
    {
        foreach (var expected in new[] { GridTriangle.Up(a, b, size), GridTriangle.Down(a, b, size) })
        {
            var v = expected.Vertices;
            LatticePoint[][] orders =
            [
                [v[0], v[1], v[2]], [v[0], v[2], v[1]], [v[1], v[0], v[2]],
                [v[1], v[2], v[0]], [v[2], v[0], v[1]], [v[2], v[1], v[0]],
            ];

            foreach (var o in orders)
            {
                Assert.True(GridTriangle.TryFromVertices(o[0], o[1], o[2], out var actual));
                Assert.Equal(expected, actual);
            }
        }
    }

    [Fact]
    public void TryFromVertices_RejectsPointsThatAreNotAnEquilateralGridTriangle()
    {
        LatticePoint o = new(0, 0);
        Assert.False(GridTriangle.TryFromVertices(o, new(2, 0), new(0, 1), out _));
        Assert.False(GridTriangle.TryFromVertices(o, new(1, 0), new(2, 0), out _));
        Assert.False(GridTriangle.TryFromVertices(o, o, new(0, 1), out _));
        Assert.False(GridTriangle.TryFromVertices(o, new(1, 0), new(1, 1), out _));
    }

    [Fact]
    public void SideEdges_HasThreeTimesSizeDistinctUnitSegments()
    {
        var t = GridTriangle.Down(1, 2, 3);

        var edges = t.SideEdges().ToList();

        Assert.Equal(9, edges.Count);
        Assert.Equal(9, edges.Distinct().Count());
    }

    [Fact]
    public void Constructor_RejectsSizeBelowOne()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GridTriangle.Up(0, 0, 0));
    }

    [Fact]
    public void UnitEdge_IsTheSameRegardlessOfDirection()
    {
        Assert.Equal(new UnitEdge(new(1, 0), new(0, 1)), new UnitEdge(new(0, 1), new(1, 0)));
        Assert.Throws<ArgumentException>(() => new UnitEdge(new(0, 0), new(1, 1)));
    }
}
