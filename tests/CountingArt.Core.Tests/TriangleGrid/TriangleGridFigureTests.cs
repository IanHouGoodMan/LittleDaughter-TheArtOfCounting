using CountingArt.Core.TriangleGrid;

namespace CountingArt.Core.Tests.TriangleGrid;

public class TriangleGridFigureTests
{
    [Fact]
    public void WorksheetFigure_ThreeRowsWidthSeven_Has76Triangles_ByCategory()
    {
        var figure = TriangleGridFigure.Parallelogram(width: 7, rows: 3);

        var byCategory = figure.FindAllTriangles()
            .GroupBy(t => t.Category)
            .ToDictionary(g => g.Key, g => g.Count());

        Assert.Equal(42, figure.Cells.Count);
        Assert.Equal(76, figure.FindAllTriangles().Count);
        Assert.Equal(new Dictionary<TriangleCategory, int>
        {
            [new(Orientation.Up, 1)] = 21,
            [new(Orientation.Down, 1)] = 21,
            [new(Orientation.Up, 2)] = 12,
            [new(Orientation.Down, 2)] = 12,
            [new(Orientation.Up, 3)] = 5,
            [new(Orientation.Down, 3)] = 5,
        }, byCategory);
    }

    [Theory]
    [InlineData(1, 6)]
    [InlineData(2, 16)]
    [InlineData(3, 28)]
    [InlineData(7, 76)]
    [InlineData(10, 112)]
    public void Parallelogram_WithThreeRows_Follows12nMinus8_FromWidthTwo(int width, int expected)
    {
        Assert.Equal(expected, TriangleGridFigure.Parallelogram(width, 3).FindAllTriangles().Count);
        if (width >= 2)
        {
            Assert.Equal(12 * width - 8, expected);
        }
    }

    [Fact]
    public void Parallelogram_BruteForceAgreesWithFormula()
    {
        for (var rows = 1; rows <= 6; rows++)
        {
            for (var width = 1; width <= 9; width++)
            {
                var counted = TriangleGridFigure.Parallelogram(width, rows).FindAllTriangles().Count;
                Assert.Equal(TriangleFormulas.Parallelogram(width, rows), counted);
            }
        }
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 5)]
    [InlineData(3, 13)]
    [InlineData(4, 27)]
    [InlineData(5, 48)]
    [InlineData(6, 78)]
    public void BigTriangle_MatchesKnownSequence(int size, int expected)
    {
        Assert.Equal(expected, TriangleGridFigure.Triangle(size).FindAllTriangles().Count);
        Assert.Equal(expected, TriangleFormulas.Triangle(size));
    }

    [Fact]
    public void BigTriangle_OfSideFour_ContainsADownTriangleOfSideTwo_ButNotOfSideThree()
    {
        var all = TriangleGridFigure.Triangle(4).FindAllTriangles();

        Assert.Contains(all, t => t.Category == new TriangleCategory(Orientation.Down, 2));
        Assert.DoesNotContain(all, t => t.Category == new TriangleCategory(Orientation.Down, 3));
    }

    [Fact]
    public void Trapezoid_IsMadeOfRowsThatShrinkByOne()
    {
        var figure = TriangleGridFigure.Trapezoid(bottomWidth: 5, rows: 2);

        // Row 0: 5 up + 4 down, row 1: 4 up + 3 down; side 2: 4 up + 2 down.
        Assert.Equal(16, figure.Cells.Count);
        Assert.Equal(16 + 4 + 2, figure.FindAllTriangles().Count);
    }

    [Fact]
    public void Triangle_WithMissingSide_IsNotCounted()
    {
        // Two up cells side by side: the down triangle between them has no top side.
        var figure = new TriangleGridFigure([GridTriangle.Up(0, 0), GridTriangle.Up(1, 0)]);

        Assert.Equal(2, figure.FindAllTriangles().Count);
        Assert.False(figure.IsDrawn(GridTriangle.Down(0, 0)));
    }

    [Fact]
    public void Constructor_RejectsEmptyFiguresAndNonUnitCells()
    {
        Assert.Throws<ArgumentException>(() => new TriangleGridFigure([]));
        Assert.Throws<ArgumentException>(() => new TriangleGridFigure([GridTriangle.Up(0, 0, 2)]));
    }
}
